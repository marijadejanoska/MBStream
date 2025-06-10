using MBStream.DTOs;

namespace MBStream.Services
{
    public interface IAlbumService
    {
        Task<List<AlbumResponseDTO>> GetAllAlbumsAsync();
        Task<AlbumResponseDTO?> GetAlbumByIdAsync(int id);
        Task<AlbumResponseDTO> CreateAlbumAsync(AlbumDTO albumDto);
        Task<AlbumResponseDTO?> UpdateAlbumAsync(int id, AlbumDTO albumDto);
        Task<bool> DeleteAlbumAsync(int id);
    }
}
