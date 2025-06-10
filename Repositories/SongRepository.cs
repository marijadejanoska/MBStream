using MBStream.Data;
using MBStream.Models;
using Microsoft.EntityFrameworkCore;

namespace MBStream.Repositories
{
    public class SongRepository : ISongRepository
    {
        private readonly AppDbContext _context;
        public SongRepository(AppDbContext context) { _context = context; }

        public async Task<Song> GetByIdAsync(int id) =>
            await _context.Songs
                .Include(s => s.SongArtists)
                .FirstOrDefaultAsync(s => s.SongId == id);

        public async Task<List<Song>> GetAllAsync() =>
            await _context.Songs
                .Include(s => s.SongArtists)
                .ToListAsync();

        public async Task<Song> AddAsync(Song song)
        {
            _context.Songs.Add(song);
            await _context.SaveChangesAsync();
            return song;
        }

        public async Task<Song> UpdateAsync(Song song)
        {
            _context.Songs.Update(song);
            await _context.SaveChangesAsync();
            return song;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var song = await _context.Songs.FindAsync(id);
            if (song == null) return false;
            _context.Songs.Remove(song);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
