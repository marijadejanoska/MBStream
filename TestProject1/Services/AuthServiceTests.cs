using Moq;
using Xunit;
using MBStream.Services;
using MBStream.Repositories;
using MBStream.DTOs;
using MBStream.Models;
using BCrypt.Net;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MBStream.Tests.Services
{
    public class AuthServiceTests
    {
        private readonly Mock<IUserRepository> _mockRepo;
        private readonly Mock<IConfiguration> _mockConfig;
        private readonly AuthService _service;

        public AuthServiceTests()
        {
            _mockRepo = new Mock<IUserRepository>();
            _mockConfig = new Mock<IConfiguration>();
            _mockConfig.Setup(c => c["Jwt:Key"]).Returns("super-secret-key-at-least-32-chars");
            _mockConfig.Setup(c => c["Jwt:Issuer"]).Returns("test-issuer");
            _mockConfig.Setup(c => c["Jwt:Audience"]).Returns("test-audience");
            _mockConfig.Setup(c => c["Jwt:ExpiryInMinutes"]).Returns("60");

            _service = new AuthService(_mockRepo.Object, _mockConfig.Object);
        }

        [Fact]
        public async Task LoginAsync_ValidCredentials_ReturnsToken()
        {
            // Arrange
            var passwordHash = BCrypt.Net.BCrypt.HashPassword("password");
            var user = new User { UserEmail = "test@example.com", PasswordHash = passwordHash };
            _mockRepo.Setup(r => r.GetByEmailAsync("test@example.com")).ReturnsAsync(user);

            // Act
            var result = await _service.LoginAsync(new LoginDTO { UserEmail = "test@example.com", Password = "password" });

            // Assert
            Assert.NotNull(result);
            Assert.IsType<string>(result);
        }

        [Fact]
        public async Task LoginAsync_InvalidCredentials_ReturnsNull()
        {
            // Arrange
            _mockRepo.Setup(r => r.GetByEmailAsync("wrong@example.com")).ReturnsAsync((User)null);

            // Act
            var result = await _service.LoginAsync(new LoginDTO { UserEmail = "wrong@example.com", Password = "wrong" });

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task RegisterAsync_NewUser_ReturnsUserResponse()
        {
            // Arrange
            _mockRepo.Setup(r => r.GetByEmailAsync("new@example.com")).ReturnsAsync((User)null);
            _mockRepo.Setup(r => r.AddAsync(It.IsAny<User>())).ReturnsAsync(new User { UserId = 1 });

            // Act
            var result = await _service.RegisterAsync(new RegisterDTO
            {
                UserEmail = "new@example.com",
                Password = "password"
            });

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.UserId);
        }

        [Fact]
        public async Task RegisterAsync_ExistingEmail_ReturnsNull()
        {
            // Arrange
            _mockRepo.Setup(r => r.GetByEmailAsync("exist@example.com")).ReturnsAsync(new User());

            // Act
            var result = await _service.RegisterAsync(new RegisterDTO
            {
                UserEmail = "exist@example.com",
                Password = "password"
            });

            // Assert
            Assert.Null(result);
        }
    }
}
