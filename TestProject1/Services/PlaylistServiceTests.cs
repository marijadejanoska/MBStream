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
    public class PlaylistServiceTests : IDisposable
    {
        private readonly AppDbContext _context;
        private readonly PlaylistRepository _repo;
        private readonly PlaylistService _service;

        public PlaylistServiceTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "PlaylistTestDb")
                .Options;

            _context = new AppDbContext(options);

            // Seed required User
            _context.Users.Add(new User
            {
                UserId = 1,
                UserName = "TestUser",
                UserEmail = "test@example.com",
                PasswordHash = "hashed-password"
            });
            _context.SaveChanges();

            _repo = new PlaylistRepository(_context);
            _service = new PlaylistService(_repo, _context);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        public async Task<Playlist> CreatePlaylistAsync(PlaylistDTO dto)
        {
            var playlist = new Playlist
            {
                PlaylistTitle = dto.PlaylistTitle,
                UserId = dto.UserId,
                PlaylistSongs = dto.SongIds?.Select(id => new PlaylistSong
                {
                    SongId = id
                }).ToList()
            };

            _context.Playlists.Add(playlist);
            await _context.SaveChangesAsync();

            // Optionally include related data in return
            return await _context.Playlists
                .Include(p => p.PlaylistSongs)
                .ThenInclude(ps => ps.Song)
                .FirstOrDefaultAsync(p => p.PlaylistId == playlist.PlaylistId);
        }


        [Fact]
        public async Task UpdatePlaylistAsync_ValidId_UpdatesSongs()
        {
            // Arrange
            var playlist = new Playlist
            {
                PlaylistTitle = "Original",
                PlaylistSongs = new List<PlaylistSong>
                {
                    new PlaylistSong { SongId = 1 }
                }
            };
            _context.Playlists.Add(playlist);

            _context.Songs.AddRange(
                new Song { SongId = 2, SongTitle = "Song 2" },
                new Song { SongId = 3, SongTitle = "Song 3" }
            );
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.UpdatePlaylistAsync(playlist.PlaylistId, new PlaylistDTO
            {
                PlaylistTitle = "Updated",
                SongIds = new List<int> { 2, 3 }
            });

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Updated", result.PlaylistTitle);
            Assert.Equal(2, result.Songs.Count);
        }
        [Fact]
        public async Task GetPlaylistByIdAsync_IncludesSongDetails()
        {
            // Arrange
            var playlist = new Playlist
            {
                PlaylistId = 1,
                PlaylistTitle = "Test Playlist",
                UserId = 1,
                PlaylistSongs = new List<PlaylistSong>
                {
                    new PlaylistSong
                    {
                        Song = new Song
                        {
                            SongId = 1,
                            SongTitle = "Test Song",
                            DurationSeconds = 180
                        }
                    }
                }
            };

            _context.Playlists.Add(playlist);
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.GetPlaylistByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Songs);
            Assert.Single(result.Songs);
            Assert.Equal("Test Song", result.Songs[0].SongTitle);
        }
    

}
}
