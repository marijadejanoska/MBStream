using Microsoft.EntityFrameworkCore;
using MBStream.Data;
using MBStream.Models;
using MBStream.Repositories;
using Xunit;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace MBStream.Tests.Repositories
{
    public class PlaylistRepositoryTests
    {
        // Helper method to create a new context with a unique DB name
        private AppDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // unique DB per test
                .Options;

            return new AppDbContext(options);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsPlaylistWithSongs()
        {
            using var context = CreateDbContext();
            var repo = new PlaylistRepository(context);

            // Arrange
            var playlist = new Playlist { PlaylistTitle = "Test", PlaylistSongs = new List<PlaylistSong>() };
            context.Playlists.Add(playlist);
            await context.SaveChangesAsync();

            // Act
            var result = await repo.GetByIdAsync(playlist.PlaylistId);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.PlaylistSongs);
        }

        [Fact]
        public async Task AddAsync_AddsPlaylistToDatabase()
        {
            using var context = CreateDbContext();
            var repo = new PlaylistRepository(context);

            // Arrange
            var playlist = new Playlist
            {
                PlaylistTitle = "New Playlist",
                UserId = 1 // Provide valid UserId as required
            };

            // Act
            var added = await repo.AddAsync(playlist);

            // Assert
            Assert.Equal(1, await context.Playlists.CountAsync());
            Assert.Equal("New Playlist", added.PlaylistTitle);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesPlaylistInDatabase()
        {
            using var context = CreateDbContext();
            var repo = new PlaylistRepository(context);

            // Arrange
            var playlist = await repo.AddAsync(new Playlist { PlaylistTitle = "Old Title" });
            playlist.PlaylistTitle = "Updated Title";

            // Act
            var updated = await repo.UpdateAsync(playlist);

            // Assert
            Assert.Equal("Updated Title", updated.PlaylistTitle);
        }

        [Fact]
        public async Task DeleteAsync_RemovesPlaylistFromDatabase()
        {
            using var context = CreateDbContext();
            var repo = new PlaylistRepository(context);

            // Arrange
            var playlist = await repo.AddAsync(new Playlist
            {
                PlaylistTitle = "Test Playlist",
                UserId = 1
            });

            // Act
            var result = await repo.DeleteAsync(playlist.PlaylistId);

            // Assert
            Assert.True(result);
            var all = await repo.GetAllAsync();
            Assert.Empty(all);
        }
    }
}
