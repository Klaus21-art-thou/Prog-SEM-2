using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceManagementAPI.Data;
using RaceManagementAPI.DTOs;
using RaceManagementAPI.Models;
using RaceManagementAPI.Services;

namespace RaceManagementAPI.Controllers
{
    [ApiController]
    [Route("api/race")]
    public class RaceManagementController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly PasswordService _passwordService;

        public RaceManagementController(ApplicationDbContext context, PasswordService passwordService)
        {
            _context = context;
            _passwordService = passwordService;
        }

# ==========================================
# 1. AUTHENTICATION & USER PROFILE ENDPOINTS
# ==========================================

        [HttpPost("users/register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            if (await _context.Users.AnyAsync(u => u.UserEmail == dto.UserEmail))
                return BadRequest("Email address is already in use.");

            if (dto.UserRole != "Organiser" && dto.UserRole != "Participant")
                return BadRequest("UserRole must be either 'Organiser' or 'Participant'.");

            var user = new User
            {
                Username = dto.Username,
                UserEmail = dto.UserEmail,
                PasswordHash = _passwordService.HashPassword(dto.Password),
                UserRole = dto.UserRole,
                UserBio = dto.UserBio
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return StatusCode(201, new UserProfileDto(user.UserID, user.Username, user.UserEmail, user.UserRole, user.UserBio));
        }

        [HttpPost("users/login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserEmail == dto.UserEmail);
            if (user == null || !_passwordService.VerifyPassword(dto.Password, user.PasswordHash))
                return Unauthorized("Invalid credentials.");

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.UserEmail),
                new Claim(ClaimTypes.Role, user.UserRole)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

            return Ok(new { Message = "Login successful", user.UserID, user.Username, user.UserRole });
        }

        [HttpPost("users/logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok(new { Message = "Logged out successfully" });
        }

        [HttpGet("users/{id}")]
        [Authorize]
        public async Task<IActionResult> GetProfile(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            return Ok(new UserProfileDto(user.UserID, user.Username, user.UserEmail, user.UserRole, user.UserBio));
        }

# ==========================================
# 2. EVENT & TRACK ENDPOINTS
# ==========================================

        [HttpGet("events")]
        [AllowAnonymous]
        public async Task<IActionResult> GetEvents() =>
            Ok(await _context.Events.Include(e => e.Tracks).ThenInclude(t => t.Fees).ToListAsync());

        [HttpPost("events")]
        [Authorize(Roles = "Organiser")]
        public async Task<IActionResult> CreateEvent([FromBody] CreateEventDto dto)
        {
            var organiserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var ev = new Event
            {
                OrganiserID = organiserId,
                EventName = dto.EventName,
                EventDescription = dto.EventDescription,
                EventDate = dto.EventDate,
                EventLocation = dto.EventLocation,
                EventType = dto.EventType
            };

            _context.Events.Add(ev);
            await _context.SaveChangesAsync();
            return StatusCode(201, ev);
        }

        [HttpPost("tracks")]
        [Authorize(Roles = "Organiser")]
        public async Task<IActionResult> CreateTrack([FromBody] CreateTrackDto dto)
        {
            var track = new Track
            {
                EventID = dto.EventID,
                TrackName = dto.TrackName,
                DistanceKM = dto.DistanceKM
            };

            _context.Tracks.Add(track);
            await _context.SaveChangesAsync();
            return StatusCode(201, track);
        }

# ==========================================
# 3. PARTICIPANT ENROLMENT ENDPOINTS
# ==========================================

        [HttpGet("participants")]
        [Authorize(Roles = "Organiser")]
        public async Task<IActionResult> GetParticipants() =>
            Ok(await _context.Participants
                .Include(p => p.User)
                .Include(p => p.Event)
                .Include(p => p.Track)
                .Include(p => p.Fee)
                .ToListAsync());

        [HttpPost("participants")]
        [Authorize(Roles = "Participant")]
        public async Task<IActionResult> EnrollParticipant([FromBody] EnrollParticipantDto dto)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            bool alreadyEnrolled = await _context.Participants
                .AnyAsync(p => p.UserID == userId && p.EventID == dto.EventID);

            if (alreadyEnrolled)
                return Conflict("You are already enrolled in this event.");

            var participant = new Participant
            {
                UserID = userId,
                EventID = dto.EventID,
                TrackID = dto.TrackID,
                FeeID = dto.FeeID,
                TeamID = dto.TeamID,
                RegistrationDate = DateTime.UtcNow
            };

            _context.Participants.Add(participant);
            await _context.SaveChangesAsync();

            return StatusCode(201, participant);
        }

# ==========================================
# 4. RESULTS ENDPOINTS
# ==========================================

        [HttpGet("results")]
        [AllowAnonymous]
        public async Task<IActionResult> GetResults() =>
            Ok(await _context.Results.Include(r => r.Participant).ThenInclude(p => p.User).ToListAsync());

        [HttpPost("results")]
        [Authorize(Roles = "Organiser")]
        public async Task<IActionResult> RecordResult([FromBody] RecordResultDto dto)
        {
            var participant = await _context.Participants.FindAsync(dto.ParticipantID);
            if (participant == null) return BadRequest("Participant not found.");

            var result = new Result
            {
                ParticipantID = dto.ParticipantID,
                FinishTimeSeconds = dto.FinishTimeSeconds,
                FinishingPosition = dto.FinishingPosition
            };

            _context.Results.Add(result);
            await _context.SaveChangesAsync();

            return StatusCode(201, result);

# ==========================================
# 5. TRACKS ENDPOINTS
# ==========================================

            [HttpGet("tracks")]
            [AllowAnonymous]
            public async Task<IActionResult> GetTracks() =>
                Ok(await _context.Tracks.Include(t => t.Fees).ToListAsync());

            [HttpGet("tracks/{id}")]
            [AllowAnonymous]
            public async Task<IActionResult> GetTrackById(int id)
            {
                var track = await _context.Tracks.Include(t => t.Fees).FirstOrDefaultAsync(t => t.TrackID == id);
                return track == null ? NotFound() : Ok(track);
            }

            [HttpPut("tracks/{id}")]
            [Authorize(Roles = "Organiser")]
            public async Task<IActionResult> UpdateTrack(int id, [FromBody] CreateTrackDto dto)
            {
                var track = await _context.Tracks.FindAsync(id);
                if (track == null) return NotFound();

                track.TrackName = dto.TrackName;
                track.DistanceKM = dto.DistanceKM;

                await _context.SaveChangesAsync();
                return Ok(track);
            }

            [HttpDelete("tracks/{id}")]
            [Authorize(Roles = "Organiser")]
            public async Task<IActionResult> DeleteTrack(int id)
            {
                var track = await _context.Tracks.FindAsync(id);
                if (track == null) return NotFound();

                _context.Tracks.Remove(track);
                await _context.SaveChangesAsync();
                return NoContent();
            }

# ==========================================
# 6. FEES ENDPOINTS 
# ==========================================

            [HttpGet("fees")]
            [AllowAnonymous]
            public async Task<IActionResult> GetFees() =>
                Ok(await _context.Fees.ToListAsync());

            [HttpPost("fees")]
            [Authorize(Roles = "Organiser")]
            public async Task<IActionResult> CreateFee([FromBody] CreateFeeDto dto)
            {
                var fee = new Fee
                {
                    TrackID = dto.TrackID,
                    FeeName = dto.FeeName,
                    FeeAmount = dto.FeeAmount,
                    FeeCurrency = dto.FeeCurrency
                };

                _context.Fees.Add(fee);
                await _context.SaveChangesAsync();
                return StatusCode(201, fee);
            }

            [HttpDelete("fees/{id}")]
            [Authorize(Roles = "Organiser")]
            public async Task<IActionResult> DeleteFee(int id)
            {
                var fee = await _context.Fees.FindAsync(id);
                if (fee == null) return NotFound();

                _context.Fees.Remove(fee);
                await _context.SaveChangesAsync();
                return NoContent();
            }

# ==========================================
# 7. TEAMS ENDPOINTS 
# ==========================================

            [HttpGet("teams")]
            [AllowAnonymous]
            public async Task<IActionResult> GetTeams() =>
                Ok(await _context.Teams.ToListAsync());

            [HttpPost("teams")]
            [Authorize(Roles = "Participant")]
            public async Task<IActionResult> CreateTeam([FromBody] CreateTeamDto dto)
            {
                if (await _context.Teams.AnyAsync(t => t.TeamName == dto.TeamName))
                    return Conflict("Team name already exists.");

                var team = new Team
                {
                    TeamName = dto.TeamName,
                    TeamDescription = dto.TeamDescription
                };

                _context.Teams.Add(team);
                await _context.SaveChangesAsync();
                return StatusCode(201, team);
            }

# ==========================================
# 8. USER PROFILE UPDATE ENDPOINT
# ==========================================

            [HttpPut("users/{id}")]
            [Authorize]
            public async Task<IActionResult> UpdateProfile(int id, [FromBody] UpdateProfileDto dto)
            {
                var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                if (currentUserId != id) return Forbid();

                var user = await _context.Users.FindAsync(id);
                if (user == null) return NotFound();

                user.Username = dto.Username;
                user.UserEmail = dto.UserEmail;
                user.UserBio = dto.UserBio;

                await _context.SaveChangesAsync();
                return Ok(new UserProfileDto(user.UserID, user.Username, user.UserEmail, user.UserRole, user.UserBio));
            }
        }
    }
}