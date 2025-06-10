using MBStream.Models;

namespace MBStream.Repositories
{
    public interface IAlbumRepository
    {
        Task<Album> GetByIdAsync(int id);
        Task<List<Album>> GetAllAsync();
        Task<Album> AddAsync(Album album);
        Task<Album> UpdateAsync(Album album);
        Task<bool> DeleteAsync(int id);
    }
}
