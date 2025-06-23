using Microsoft.EntityFrameworkCore;
using MBStream.Data;
using MBStream.Models;
using MBStream.Repositories;
using MBStream.Services;
using MBStream.DTOs;
using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MBStream.Tests.Services
{
    public class SongServiceTests : IDisposable
    {
        private readonly AppDbContext _context;
        private readonly SongRepository _repo;
        private readonly SongService _service;

        public SongServiceTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "SongTestDb")
                .Options;
            _context = new AppDbContext(options);
            _repo = new SongRepository(_context);
            _service = new SongService(_repo, _context);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact]
        public async Task GetAllSongsAsync_ReturnsMappedSongs()
        {
            // Arrange
            _context.Songs.Add(new Song
            {
                SongId = 1,
                SongTitle = "Test Song",
                SongArtists = new List<SongArtist>()
            });
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.GetAllSongsAsync();

            // Assert
            Assert.Single(result);
            Assert.Equal("Test Song", result[0].SongTitle);
        }

        [Fact]
        public async Task GetSongByIdAsync_ExistingId_ReturnsSong()
        {
            // Arrange
            var song = new Song
            {
                SongId = 1,
                SongTitle = "Test Song",
                SongArtists = new List<SongArtist>()
            };
            _context.Songs.Add(song);
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.GetSongByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.SongId);
        }

        [Fact]
        public async Task CreateSongAsync_ValidInput_CreatesSong()
        {
            // Arrange
            var songDto = new SongDTO
            {
                SongTitle = "New Song",
                ArtistIds = new List<int> { 1 }
            };

            // Act
            var result = await _service.CreateSongAsync(songDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, await _context.Songs.CountAsync());
        }

        [Fact]
        public async Task UpdateSongAsync_UpdatesArtists()
        {
            // Arrange
            var song = new Song
            {
                SongTitle = "Old Song",
                SongArtists = new List<SongArtist>()
            };
            _context.Songs.Add(song);
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.UpdateSongAsync(song.SongId, new SongDTO
            {
                ArtistIds = new List<int> { 2 }
            });

            // Assert
            Assert.Contains(2, result.ArtistIds);
        }

        [Fact]
        public async Task DeleteSongAsync_ValidId_ReturnsTrue()
        {
            // Arrange
            var song = new Song
            {
                SongTitle = "To Delete",
                SongArtists = new List<SongArtist>()
            };
            _context.Songs.Add(song);
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.DeleteSongAsync(song.SongId);

            // Assert
            Assert.True(result);
            Assert.Empty(await _repo.GetAllAsync());
        }
    }
}
