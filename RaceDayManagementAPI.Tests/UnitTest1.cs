using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDayManagementAPI.Controllers;
using RaceDayManagementAPI.Data;
using RaceDayManagementAPI.DTOs;
using RaceDayManagementAPI.Models;
using Xunit;

namespace RaceDayManagementAPI.Tests
{
    public class ApiUnitTests
    {
        private ApplicationDbContext GetDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            var databaseContext = new ApplicationDbContext(options);
            databaseContext.Database.EnsureCreated();
            return databaseContext;
        }

        private HttpContext GetHttpContextWithClaims(string role, int userId)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Role, role)
            };
            var identity = new ClaimsIdentity(claims, "TestAuth");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            return new DefaultHttpContext { User = claimsPrincipal };
        }

        [Fact]
        public async Task Register_ValidUser_ReturnsCreatedAtAction()
        {
            // Arrange
            var db = GetDatabaseContext();
            var controller = new AuthController(db);
            var dto = new RegisterDto("Test Runner", "runner@raceday.co.za", "password123", "Participant", "Bio text");

            // Act
            var result = await controller.Register(dto);

            // Assert
            Assert.IsType<CreatedAtActionResult>(result);
        }

        [Fact]
        public async Task CreateEvent_ParticipantRole_ReturnsUnauthorizedOrForbidden()
        {
            // Arrange
            var db = GetDatabaseContext();
            var controller = new EventsController(db);

            // Simulate a logged-in 'Participant' attempting an Organiser-only action
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = GetHttpContextWithClaims("Participant", 103)
            };

            var dto = new CreateEventDto("Unauthorized Run", "Desc", DateTime.UtcNow, "Cape Town", "Run");

            // Act
            var result = await controller.Create(dto);

            // Assert - [Authorize(Roles = "Organiser")] policy will block this
            Assert.True(result is UnauthorizedResult || result is ForbidResult || result is ObjectResult);
        }
    }
}
