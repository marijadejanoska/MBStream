using MBStream.DTOs;

namespace MBStream.Services
{
    public interface IPlaylistService
    {
        Task<List<PlaylistResponseDTO>> GetAllPlaylistsAsync();
        Task<PlaylistResponseDTO?> GetPlaylistByIdAsync(int id);
        Task<PlaylistResponseDTO> CreatePlaylistAsync(PlaylistDTO playlistDto);
        Task<PlaylistResponseDTO?> UpdatePlaylistAsync(int id, PlaylistDTO playlistDto);
        Task<bool> DeletePlaylistAsync(int id);
    }
}
