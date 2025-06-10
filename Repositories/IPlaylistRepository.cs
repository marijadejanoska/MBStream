using MBStream.Models;

namespace MBStream.Repositories
{
    public interface IPlaylistRepository
    {
        Task<Playlist> GetByIdAsync(int id);
        Task<List<Playlist>> GetAllAsync();
        Task<Playlist> AddAsync(Playlist playlist);
        Task<Playlist> UpdateAsync(Playlist playlist);
        Task<bool> DeleteAsync(int id);
    }
}
