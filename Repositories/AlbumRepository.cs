using MBStream.Data;
using MBStream.Models;
using Microsoft.EntityFrameworkCore;

namespace MBStream.Repositories
{
    public class AlbumRepository : IAlbumRepository
    {
        private readonly AppDbContext _context;
        public AlbumRepository(AppDbContext context) { _context = context; }

        // In your AlbumRepository
        public async Task<Album> GetByIdAsync(int id) =>
    await _context.Albums
        .Include(a => a.AlbumArtists)
            .ThenInclude(aa => aa.Artist)
        .FirstOrDefaultAsync(a => a.AlbumId == id);


        public async Task<List<Album>> GetAllAsync() =>
            await _context.Albums
                .Include(a => a.AlbumArtists)
                    .ThenInclude(aa => aa.Artist)
                .Include(a => a.Songs) // <-- Add this to load songs
                .ToListAsync();
        public async Task<Album> AddAsync(Album album) { _context.Albums.Add(album); await _context.SaveChangesAsync(); return album; }
        public async Task<Album> UpdateAsync(Album album) { _context.Albums.Update(album); await _context.SaveChangesAsync(); return album; }
        public async Task<bool> DeleteAsync(int id) { var album = await _context.Albums.FindAsync(id); if (album == null) return false; _context.Albums.Remove(album); await _context.SaveChangesAsync(); return true; }
    }
}
