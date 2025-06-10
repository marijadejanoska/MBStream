using MBStream.Data;
using MBStream.Models;
using Microsoft.EntityFrameworkCore;

namespace MBStream.Repositories
{
    public class ArtistRepository : IArtistRepository
    {
        private readonly AppDbContext _context;
        public ArtistRepository(AppDbContext context) { _context = context; }

        public async Task<Artist> GetByIdAsync(int id) => await _context.Artists.FindAsync(id);
        public async Task<List<Artist>> GetAllAsync() => await _context.Artists.ToListAsync();
        public async Task<Artist> AddAsync(Artist artist) { _context.Artists.Add(artist); await _context.SaveChangesAsync(); return artist; }
        public async Task<Artist> UpdateAsync(Artist artist) { _context.Artists.Update(artist); await _context.SaveChangesAsync(); return artist; }
        public async Task<bool> DeleteAsync(int id) { var artist = await _context.Artists.FindAsync(id); if (artist == null) return false; _context.Artists.Remove(artist); await _context.SaveChangesAsync(); return true; }
    }
}
