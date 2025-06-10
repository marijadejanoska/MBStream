using MBStream.Data;
using MBStream.Models;
using Microsoft.EntityFrameworkCore;

namespace MBStream.Repositories
{
    public class PlaylistRepository : IPlaylistRepository
    {
        private readonly AppDbContext _context;
        public PlaylistRepository(AppDbContext context) { _context = context; }

        // Example for EF Core
        public async Task<Playlist> GetByIdAsync(int id)
        {
            return await _context.Playlists
                .Include(p => p.PlaylistSongs)
                .FirstOrDefaultAsync(p => p.PlaylistId == id);
        }

        public async Task<List<Playlist>> GetAllAsync() => await _context.Playlists.ToListAsync();
        public async Task<Playlist> AddAsync(Playlist playlist) { _context.Playlists.Add(playlist); await _context.SaveChangesAsync(); return playlist; }
        public async Task<Playlist> UpdateAsync(Playlist playlist) { _context.Playlists.Update(playlist); await _context.SaveChangesAsync(); return playlist; }
        public async Task<bool> DeleteAsync(int id) { var playlist = await _context.Playlists.FindAsync(id); if (playlist == null) return false; _context.Playlists.Remove(playlist); await _context.SaveChangesAsync(); return true; }
    }
}
