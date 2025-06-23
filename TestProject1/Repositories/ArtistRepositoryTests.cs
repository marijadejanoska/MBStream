using Microsoft.EntityFrameworkCore;
using MBStream.Data;
using MBStream.Models;
using MBStream.Repositories;
using Xunit;

namespace MBStream.Tests.Repositories
{
    public class ArtistRepositoryTests : IDisposable
    {
        private readonly AppDbContext _context;
        private readonly ArtistRepository _repo;

        public ArtistRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);
            _repo = new ArtistRepository(_context);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact]
        public async Task GetByIdAsync_ExistingId_ReturnsArtist()
        {
            // Arrange
            var artist = new Artist { ArtistName = "Test Artist" };
            _context.Artists.Add(artist);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repo.GetByIdAsync(artist.ArtistId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test Artist", result.ArtistName);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllArtists()
        {
            // Arrange
            _context.Artists.AddRange(
                new Artist { ArtistName = "Artist 1" },
                new Artist { ArtistName = "Artist 2" }
            );
            await _context.SaveChangesAsync();

            // Act
            var result = await _repo.GetAllAsync();

            // Assert
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task AddAsync_SavesArtistToDatabase()
        {
            // Arrange
            var artist = new Artist { ArtistName = "New Artist" };

            // Act
            var result = await _repo.AddAsync(artist);

            // Assert
            Assert.Equal(1, _context.Artists.Count());
            Assert.Equal("New Artist", result.ArtistName);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesExistingArtist()
        {
            // Arrange
            var artist = new Artist { ArtistName = "Original Name" };
            _context.Artists.Add(artist);
            await _context.SaveChangesAsync();

            artist.ArtistName = "Updated Name";

            // Act
            var result = await _repo.UpdateAsync(artist);

            // Assert
            Assert.Equal("Updated Name", _context.Artists.Find(artist.ArtistId).ArtistName);
        }

        [Fact]
        public async Task DeleteAsync_RemovesArtist()
        {
            // Arrange
            var artist = new Artist { ArtistName = "To Delete" };
            _context.Artists.Add(artist);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repo.DeleteAsync(artist.ArtistId);

            // Assert
            Assert.True(result);
            Assert.Empty(_context.Artists);
        }
    }
}
