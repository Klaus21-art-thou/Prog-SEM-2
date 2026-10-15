using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceManagementAPI.Controllers;
using RaceManagementAPI.Data;
using RaceManagementAPI.DTOs;
using RaceManagementAPI.Models;
using RaceManagementAPI.Services;
using Xunit;

namespace RaceManagementAPI.Tests
{
    public class ApiUnitTests
    {
        // Helper method to create a clean In-Memory database context for each test
        private ApplicationDbContext GetDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            var databaseContext = new ApplicationDbContext(options);
            databaseContext.Database.EnsureCreated();
            return databaseContext;
        }

        // Helper method to simulate an authenticated user with claims
        private DefaultHttpContext GetHttpContextWithClaims(string role, int userId)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Name, "TestUser"),
                new Claim(ClaimTypes.Role, role)
            };
            var identity = new ClaimsIdentity(claims, "TestAuth");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            return new DefaultHttpContext
            {
                User = claimsPrincipal
            };
        }

        [Fact]
        public async Task Register_ValidUser_ReturnsCreated()
        {
            // Arrange
            var db = GetDatabaseContext();
            var passwordService = new PasswordService();
            var controller = new RaceManagementController(db, passwordService);

            var registerDto = new RegisterDto(
                Username: "Test Runner",
                UserEmail: "runner@raceday.co.za",
                Password: "password123",
                UserRole: "Participant",
                UserBio: "Avid runner"
            );

            // Act
            var result = await controller.Register(registerDto);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(201, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task CreateEvent_ParticipantRole_ReturnsForbidOrUnauthorized()
        {
            // Arrange
            var db = GetDatabaseContext();
            var passwordService = new PasswordService();
            var controller = new RaceManagementController(db, passwordService);

            // Simulate a logged-in 'Participant' attempting an Organiser-only action
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = GetHttpContextWithClaims("Participant", 103)
            };

            var eventDto = new CreateEventDto(
                EventName: "Unauthorized Run",
                EventDescription: "Participants cannot create events",
                EventDate: DateTime.UtcNow.AddDays(5),
                EventLocation: "Cape Town",
                EventType: "Run"
            );

            // Act
            var result = await controller.CreateEvent(eventDto);

            // Assert - [Authorize(Roles = "Organiser")] will return Challenge/Forbid (401/403)
            var actionResult = Assert.IsType<ForbidResult>(result);
        }
    }
}