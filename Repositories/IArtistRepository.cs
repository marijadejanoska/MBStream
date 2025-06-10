using MBStream.Models;

namespace MBStream.Repositories
{
    public interface IArtistRepository
    {
        Task<Artist> GetByIdAsync(int id);
        Task<List<Artist>> GetAllAsync();
        Task<Artist> AddAsync(Artist artist);
        Task<Artist> UpdateAsync(Artist artist);
        Task<bool> DeleteAsync(int id);
    }
}
