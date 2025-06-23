using Microsoft.EntityFrameworkCore;
using MBStream.Data;
using MBStream.Models;
using MBStream.Repositories;
using Xunit;

namespace MBStream.Tests.Repositories
{
    public class AlbumRepositoryTests : IDisposable
    {
        private readonly AppDbContext _context;
        private readonly AlbumRepository _repo;

        public AlbumRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);
            _repo = new AlbumRepository(_context);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact]
        public async Task GetByIdAsync_ExistingId_ReturnsAlbumWithArtistsAndSongs()
        {
            // Arrange
            var artist = new Artist { ArtistName = "Test Artist" };
            var album = new Album
            {
                AlbumTitle = "Test Album",
                AlbumArtists = new List<AlbumArtist> { new AlbumArtist { Artist = artist } },
                Songs = new List<Song> { new Song { SongTitle = "Test Song" } }
            };
            _context.Albums.Add(album);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repo.GetByIdAsync(album.AlbumId);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result.AlbumArtists);
            Assert.Single(result.Songs);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllAlbumsWithIncludes()
        {
            // Arrange
            _context.Albums.AddRange(
                new Album { AlbumTitle = "Album 1" },
                new Album { AlbumTitle = "Album 2" }
            );
            await _context.SaveChangesAsync();

            // Act
            var result = await _repo.GetAllAsync();

            // Assert
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task AddAsync_SavesAlbumToDatabase()
        {
            // Arrange
            var album = new Album { AlbumTitle = "New Album" };

            // Act
            var result = await _repo.AddAsync(album);

            // Assert
            Assert.Equal(1, _context.Albums.Count());
            Assert.Equal("New Album", result.AlbumTitle);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesExistingAlbum()
        {
            // Arrange
            var album = new Album { AlbumTitle = "Original Title" };
            _context.Albums.Add(album);
            await _context.SaveChangesAsync();

            album.AlbumTitle = "Updated Title";

            // Act
            var result = await _repo.UpdateAsync(album);

            // Assert
            Assert.Equal("Updated Title", _context.Albums.Find(album.AlbumId).AlbumTitle);
        }

        [Fact]
        public async Task DeleteAsync_RemovesAlbum()
        {
            // Arrange
            var album = new Album { AlbumTitle = "To Delete" };
            _context.Albums.Add(album);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repo.DeleteAsync(album.AlbumId);

            // Assert
            Assert.True(result);
            Assert.Empty(_context.Albums);
        }
    }
}
