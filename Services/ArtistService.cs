using AutoMapper;
using MBStream.Models;
using MBStream.DTOs;
using MBStream.Models;
using MBStream.Repositories;

namespace MBStream.Services
{
    public class ArtistService : IArtistService
    {
        private readonly IArtistRepository _artistRepository;
        private readonly IMapper _mapper;

        public ArtistService(IArtistRepository artistRepository, IMapper mapper)
        {
            _artistRepository = artistRepository;
            _mapper = mapper;
        }

        public async Task<List<ArtistResponseDTO>> GetAllArtistsAsync()
        {
            var artists = await _artistRepository.GetAllAsync();
            return _mapper.Map<List<ArtistResponseDTO>>(artists);
        }

        public async Task<ArtistResponseDTO> GetArtistByIdAsync(int id)
        {
            var artist = await _artistRepository.GetByIdAsync(id);
            return _mapper.Map<ArtistResponseDTO>(artist);
        }

        public async Task<ArtistResponseDTO> CreateArtistAsync(ArtistDTO artistDto)
        {
            var artist = _mapper.Map<Artist>(artistDto);
            var created = await _artistRepository.AddAsync(artist);
            return _mapper.Map<ArtistResponseDTO>(created); // ✅ Map the saved entity
        }


        public async Task<ArtistResponseDTO> UpdateArtistAsync(int id, ArtistDTO artistDto)
        {
            var artist = await _artistRepository.GetByIdAsync(id);
            if (artist == null) return null;

            _mapper.Map(artistDto, artist); // Only updates ArtistName
            var updatedArtist = await _artistRepository.UpdateAsync(artist);
            return _mapper.Map<ArtistResponseDTO>(updatedArtist);
        }

        public async Task<bool> DeleteArtistAsync(int id)
        {
            return await _artistRepository.DeleteAsync(id);
        }
    }
}
