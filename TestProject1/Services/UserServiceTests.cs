using Moq;
using Xunit;
using MBStream.Services;
using MBStream.Repositories;
using MBStream.DTOs;
using MBStream.Models;
using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MBStream.Tests.Services
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly UserService _service;

        public UserServiceTests()
        {
            _mockRepo = new Mock<IUserRepository>();
            _mockMapper = new Mock<IMapper>();
            _service = new UserService(_mockRepo.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task GetAllUsersAsync_ReturnsMappedUsers()
        {
            // Arrange
            var users = new List<User> { new User() };
            var userDtos = new List<UserResponseDTO> { new UserResponseDTO() };
            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(users);
            _mockMapper.Setup(m => m.Map<List<UserResponseDTO>>(users)).Returns(userDtos);

            // Act
            var result = await _service.GetAllUsersAsync();

            // Assert
            Assert.Single(result);
        }

        [Fact]
        public async Task CreateUserAsync_HashesPassword()
        {
            // Arrange
            var registerDto = new RegisterDTO { Password = "password" };
            _mockRepo.Setup(r => r.AddAsync(It.IsAny<User>()))
                .Callback<User>(u => Assert.True(BCrypt.Net.BCrypt.Verify("password", u.PasswordHash)))
                .ReturnsAsync(new User());

            // Act
            await _service.CreateUserAsync(registerDto);
        }

        [Fact]
        public async Task UpdateUserAsync_ValidId_UpdatesUser()
        {
            // Arrange
            var user = new User { UserId = 1 };
            var userDto = new UserDTO { UserName = "Updated Name" };

            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(user);
            _mockRepo.Setup(r => r.UpdateAsync(user)).ReturnsAsync(user);

            // Add this line to mock the response mapping
            _mockMapper.Setup(m => m.Map<UserResponseDTO>(user))
                      .Returns(new UserResponseDTO { UserId = 1, UserName = "Updated Name" });

            // Act
            var result = await _service.UpdateUserAsync(1, userDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Updated Name", result.UserName);
        }


        [Fact]
        public async Task DeleteUserAsync_ValidId_ReturnsTrue()
        {
            // Arrange
            _mockRepo.Setup(r => r.DeleteAsync(1)).ReturnsAsync(true);

            // Act
            var result = await _service.DeleteUserAsync(1);

            // Assert
            Assert.True(result);
        }
    }
}
