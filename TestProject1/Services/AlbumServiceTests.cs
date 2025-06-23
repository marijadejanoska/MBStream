using Xunit;
using MBStream.Services;
using MBStream.Repositories;
using MBStream.DTOs;
using MBStream.Models;
using MBStream.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MBStream.Tests.Services
{
    public class AlbumServiceTests : IDisposable
    {
        private readonly AppDbContext _context;
        private readonly AlbumRepository _repo;
        private readonly AlbumService _service;

        public AlbumServiceTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "AlbumTestDb")
                .Options;
            _context = new AppDbContext(options);
            _repo = new AlbumRepository(_context);
            _service = new AlbumService(_repo, _context);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact]
        public async Task GetAlbumByIdAsync_ExistingId_ReturnsAlbum()
        {
            // Arrange
            var album = new Album
            {
                AlbumTitle = "Test Album",
                AlbumArtists = new List<AlbumArtist>(),
                Songs = new List<Song>()
            };
            _context.Albums.Add(album);
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.GetAlbumByIdAsync(album.AlbumId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(album.AlbumId, result.AlbumId);
        }

        [Fact]
        public async Task CreateAlbumAsync_ValidInput_CreatesAlbum()
        {
            // Arrange
            var albumDto = new AlbumDTO
            {
                AlbumTitle = "New Album",
                ArtistIds = new List<int> { 1 }
            };

            // Act
            var result = await _service.CreateAlbumAsync(albumDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, await _context.Albums.CountAsync());
        }

        [Fact]
        public async Task UpdateAlbumAsync_ExistingId_UpdatesAlbum()
        {
            // Arrange
            var album = new Album
            {
                AlbumTitle = "Original Title",
                AlbumArtists = new List<AlbumArtist>(),
                Songs = new List<Song>()
            };
            _context.Albums.Add(album);
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.UpdateAlbumAsync(album.AlbumId, new AlbumDTO
            {
                AlbumTitle = "Updated Title",
                ArtistIds = new List<int> { 2 }
            });

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Updated Title", result.AlbumTitle);
        }

        [Fact]
        public async Task DeleteAlbumAsync_ValidId_ReturnsTrue()
        {
            // Arrange
            var album = new Album
            {
                AlbumTitle = "To Delete",
                AlbumArtists = new List<AlbumArtist>(),
                Songs = new List<Song>()
            };
            _context.Albums.Add(album);
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.DeleteAlbumAsync(album.AlbumId);

            // Assert
            Assert.True(result);
            Assert.Empty(await _repo.GetAllAsync());
        }
    }
}
