using Microsoft.EntityFrameworkCore;
using MBStream.Data;
using MBStream.Models;
using MBStream.Repositories;
using Xunit;
using System.Threading.Tasks;

namespace MBStream.Tests.Repositories
{
    public class SongRepositoryTests : IDisposable
    {
        private readonly AppDbContext _context;
        private readonly SongRepository _repo;

        public SongRepositoryTests()
        {
            // Use a UNIQUE database name for each test class instance
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique name
                .Options;
            _context = new AppDbContext(options);
            _repo = new SongRepository(_context);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsSongWithArtists()
        {
            // Arrange
            var song = new Song { SongTitle = "Test", SongArtists = new List<SongArtist>() };
            _context.Songs.Add(song);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repo.GetByIdAsync(song.SongId);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.SongArtists);
        }

        [Fact]
        public async Task AddAsync_AddsSongToDatabase()
        {
            // Act
            var song = new Song { SongTitle = "New Song" };
            var addedSong = await _repo.AddAsync(song);

            // Assert
            Assert.Equal(1, await _context.Songs.CountAsync());
            Assert.Equal("New Song", addedSong.SongTitle);
            Assert.True(addedSong.SongId > 0); // Ensure ID is generated
        }

        [Fact]
        public async Task UpdateAsync_UpdatesSongInDatabase()
        {
            // Arrange
            var song = new Song { SongTitle = "Old Title" };
            _context.Songs.Add(song);
            await _context.SaveChangesAsync(); // Explicitly save

            song.SongTitle = "Updated Title";

            // Act
            var updated = await _repo.UpdateAsync(song);

            // Assert
            Assert.Equal("Updated Title", updated.SongTitle);
            var dbSong = await _context.Songs.FindAsync(song.SongId);
            Assert.Equal("Updated Title", dbSong.SongTitle);
        }


    }
}
