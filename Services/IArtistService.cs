using MBStream.DTOs;

namespace MBStream.Services
{
    public interface IArtistService
    {
        Task<List<ArtistResponseDTO>> GetAllArtistsAsync();
        Task<ArtistResponseDTO> GetArtistByIdAsync(int id);
        Task<ArtistResponseDTO> CreateArtistAsync(ArtistDTO artistDto);
        Task<ArtistResponseDTO> UpdateArtistAsync(int id, ArtistDTO artistDto);
        Task<bool> DeleteArtistAsync(int id);
    }
}
