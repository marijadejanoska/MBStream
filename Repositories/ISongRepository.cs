using MBStream.Models;

namespace MBStream.Repositories
{
    public interface ISongRepository
    {
        Task<Song> GetByIdAsync(int id);
        Task<List<Song>> GetAllAsync();
        Task<Song> AddAsync(Song song);
        Task<Song> UpdateAsync(Song song);
        Task<bool> DeleteAsync(int id);
    }
}
