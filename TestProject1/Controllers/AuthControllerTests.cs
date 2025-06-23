using Microsoft.AspNetCore.Mvc;
using Moq;
using MBStream.Controllers;
using MBStream.DTOs;
using MBStream.Services;
using Xunit;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;


namespace MBStream.Tests.Controllers
{
    public class AuthControllerTests
    {
        private readonly Mock<IAuthService> _mockAuthService;
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            _mockAuthService = new Mock<IAuthService>();
            _controller = new AuthController(_mockAuthService.Object);
        }

        [Fact]
        public async Task Login_ValidCredentials_ReturnsOkWithToken()
        {
            // Arrange
            var loginDto = new LoginDTO { UserEmail = "test@example.com", Password = "validPassword" };
            var expectedToken = "fake-jwt-token";

            _mockAuthService.Setup(s => s.LoginAsync(loginDto))
                .ReturnsAsync(expectedToken);

            // Act
            var result = await _controller.Login(loginDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var json = JObject.FromObject(okResult.Value);
            Assert.Equal(expectedToken, json["Token"].Value<string>());
        }

        [Fact]
        public async Task Login_InvalidCredentials_ReturnsUnauthorized()
        {
            // Arrange
            var loginDto = new LoginDTO { UserEmail = "wrong@example.com", Password = "wrongPassword" };
            _mockAuthService.Setup(s => s.LoginAsync(loginDto))
                .ReturnsAsync((string)null);

            // Act
            var result = await _controller.Login(loginDto);

            // Assert
            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task Register_NewUser_ReturnsCreatedUser()
        {
            // Arrange
            var registerDto = new RegisterDTO
            {
                UserName = "newuser",
                UserEmail = "new@example.com",
                Password = "validPassword"
            };

            var expectedUser = new UserResponseDTO
            {
                UserId = 1,
                UserName = "newuser",
                UserEmail = "new@example.com"
            };

            _mockAuthService.Setup(s => s.RegisterAsync(registerDto))
                .ReturnsAsync(expectedUser);

            // Act
            var result = await _controller.Register(registerDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expectedUser, okResult.Value);
        }

        [Fact]
        public async Task Register_ExistingEmail_ReturnsBadRequest()
        {
            // Arrange
            var registerDto = new RegisterDTO
            {
                UserEmail = "exists@example.com",
                Password = "password"
            };

            _mockAuthService.Setup(s => s.RegisterAsync(registerDto))
                .ReturnsAsync((UserResponseDTO)null);

            // Act
            var result = await _controller.Register(registerDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Email already exists", badRequestResult.Value);
        }
    }
}
