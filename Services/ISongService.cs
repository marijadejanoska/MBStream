// Services/ISongService.cs
using MBStream.DTOs;

namespace MBStream.Services
{
    public interface ISongService
    {
        Task<List<SongResponseDTO>> GetAllSongsAsync();
        Task<SongResponseDTO?> GetSongByIdAsync(int id);
        Task<SongResponseDTO> CreateSongAsync(SongDTO songDto);
        Task<SongResponseDTO?> UpdateSongAsync(int id, SongDTO songDto);
        Task<bool> DeleteSongAsync(int id);
    }
}
