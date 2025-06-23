using Microsoft.AspNetCore.Mvc;
using Moq;
using MBStream.Controllers;
using MBStream.DTOs;
using MBStream.Services;
using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MBStream.Tests.Controllers
{
    public class UsersControllerTests
    {
        private readonly Mock<IUserService> _mockService;
        private readonly UsersController _controller;

        public UsersControllerTests()
        {
            _mockService = new Mock<IUserService>();
            _controller = new UsersController(_mockService.Object);
        }

        [Fact]
        public async Task GetAllUsers_ReturnsOkWithUsers()
        {
            // Arrange
            var users = new List<UserResponseDTO> { new UserResponseDTO() };
            _mockService.Setup(s => s.GetAllUsersAsync()).ReturnsAsync(users);

            // Act
            var result = await _controller.GetAllUsers();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(users, okResult.Value);
        }

        [Fact]
        public async Task GetUser_ValidId_ReturnsUser()
        {
            // Arrange
            var user = new UserResponseDTO { UserId = 1 };
            _mockService.Setup(s => s.GetUserByIdAsync(1)).ReturnsAsync(user);

            // Act
            var result = await _controller.GetUser(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(user, okResult.Value);
        }

        [Fact]
        public async Task GetUser_InvalidId_ReturnsNotFound()
        {
            // Arrange
            _mockService.Setup(s => s.GetUserByIdAsync(1)).ReturnsAsync((UserResponseDTO)null);

            // Act
            var result = await _controller.GetUser(1);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task CreateUser_ValidInput_ReturnsCreated()
        {
            // Arrange
            var registerDto = new RegisterDTO();
            var newUser = new UserResponseDTO { UserId = 1 };
            _mockService.Setup(s => s.CreateUserAsync(registerDto)).ReturnsAsync(newUser);

            // Act
            var result = await _controller.CreateUser(registerDto);

            // Assert
            var createdAtResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal("GetUser", createdAtResult.ActionName);
        }

        [Fact]
        public async Task UpdateUser_ValidId_ReturnsNoContent()
        {
            // Arrange
            var userDto = new UserDTO();
            _mockService.Setup(s => s.UpdateUserAsync(1, userDto)).ReturnsAsync(new UserResponseDTO());

            // Act
            var result = await _controller.UpdateUser(1, userDto);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateUser_InvalidId_ReturnsNotFound()
        {
            // Arrange
            _mockService.Setup(s => s.UpdateUserAsync(1, It.IsAny<UserDTO>())).ReturnsAsync((UserResponseDTO)null);

            // Act
            var result = await _controller.UpdateUser(1, new UserDTO());

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteUser_ValidId_ReturnsNoContent()
        {
            // Arrange
            _mockService.Setup(s => s.DeleteUserAsync(1)).ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteUser(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteUser_InvalidId_ReturnsNotFound()
        {
            // Arrange
            _mockService.Setup(s => s.DeleteUserAsync(1)).ReturnsAsync(false);

            // Act
            var result = await _controller.DeleteUser(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
