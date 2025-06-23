using Microsoft.EntityFrameworkCore;
using MBStream.Data;
using MBStream.Models;
using MBStream.Repositories;
using Xunit;
using System.Threading.Tasks;

namespace MBStream.Tests.Repositories
{
    public class UserRepositoryTests : IDisposable
    {
        private readonly AppDbContext _context;
        private readonly UserRepository _repo;

        public UserRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "UserTestDb")
                .Options;
            _context = new AppDbContext(options);
            _repo = new UserRepository(_context);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
        [Fact]
        public async Task GetByEmailAsync_ReturnsUserByEmail()
        {
            // Arrange
            await _repo.AddAsync(new User
            {
                UserName = "Test User",
                UserEmail = "test@example.com",
                PasswordHash = "hashed_password",
                Role = "User" // Add required role
            });

            // Act
            var user = await _repo.GetByEmailAsync("test@example.com");

            // Assert
            Assert.NotNull(user);
            Assert.Equal("test@example.com", user.UserEmail);
        }

        [Fact]
        public async Task AddAsync_AddsUserToDatabase()
        {
            // Act
            var user = await _repo.AddAsync(new User
            {
                UserName = "New User",
                UserEmail = "new@example.com",
                PasswordHash = "hashed_password",
                Role = "User" // Add required role
            });

            // Assert
            Assert.Equal(1, await _context.Users.CountAsync());
            Assert.Equal("New User", user.UserName);
        }


        [Fact]
        public async Task DeleteAsync_RemovesUserFromDatabase()
        {
            // Arrange
            var user = await _repo.AddAsync(new User
            {
                UserName = "To Delete",
                UserEmail = "delete@example.com", // Required
                PasswordHash = "hashed_password"  // Required
            });

            // Act
            var result = await _repo.DeleteAsync(user.UserId);

            // Assert
            Assert.True(result);
            Assert.Empty(await _repo.GetAllAsync());
        }
    }
}
