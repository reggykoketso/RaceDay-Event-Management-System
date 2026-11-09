

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;
using RaceDayAPI.Controllers;
using RaceDayAPI.Data;
using Xunit;

namespace RaceDayAPI.Tests
{
    public class UnitTest1
    {
        private class TestWebHostEnvironment : IWebHostEnvironment
        {
            public string EnvironmentName { get; set; } = "Development";
            public string ApplicationName { get; set; } = "RaceDayAPI.Tests";
            public string WebRootPath { get; set; } = "";
            public IFileProvider WebRootFileProvider { get; set; } = new NullFileProvider();
            public string ContentRootPath { get; set; } = "";
            public IFileProvider ContentRootFileProvider { get; set; } = new NullFileProvider();
        }

        [Fact]
        public async Task Register_WithValidDetails_ReturnsOk()
        {
            var options = new DbContextOptionsBuilder<RaceDayContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using var context = new RaceDayContext(options);

            var settings = new Dictionary<string, string?>
            {
                ["Jwt:Key"] = "RaceDay_Development_Key_2026_Use_A_Stronger_Secret!"
            };

            IConfiguration configuration =
                new ConfigurationBuilder()
                    .AddInMemoryCollection(settings)
                    .Build();

            var environment = new TestWebHostEnvironment();

            var controller = new AuthController(context, configuration, environment);

            var request = new AuthController.RegisterRequest
            {
                FirstName = "Test",
                LastName = "Participant",
                Email = "test@example.com",
                Password = "TestPassword123!",
                Phone = "0712345678"
            };

            var result = await controller.Register(request);

            Assert.IsType<OkObjectResult>(result);

            var user = await context.Users
                .SingleOrDefaultAsync(u => u.Email == "test@example.com");

            Assert.NotNull(user);
            Assert.Equal("Participant", user.Role);
            Assert.NotEqual("TestPassword123!", user.PasswordHash);
        }

        [Fact]
        public async Task Register_WithDuplicateEmail_ReturnsConflict()
        {
            var options = new DbContextOptionsBuilder<RaceDayContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using var context = new RaceDayContext(options);

            var settings = new Dictionary<string, string?>
            {
                ["Jwt:Key"] = "RaceDay_Development_Key_2026_Use_A_Stronger_Secret!"
            };

            IConfiguration configuration =
                new ConfigurationBuilder()
                    .AddInMemoryCollection(settings)
                    .Build();

            var environment = new TestWebHostEnvironment();

            var controller = new AuthController(context, configuration, environment);

            var request = new AuthController.RegisterRequest
            {
                FirstName = "Test",
                LastName = "Participant",
                Email = "duplicate@example.com",
                Password = "TestPassword123!"
            };

            await controller.Register(request);
            var result = await controller.Register(request);

            Assert.IsType<ConflictObjectResult>(result);
        }
    }
}
