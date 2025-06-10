using MBStream.DTOs;
using MBStream.Models;
using MBStream.Repositories;
using Microsoft.EntityFrameworkCore;
using MBStream.Data;

namespace MBStream.Services
{
    public class AlbumService : IAlbumService
    {
        private readonly IAlbumRepository _albumRepo;
        private readonly AppDbContext _context;

        public AlbumService(IAlbumRepository albumRepo, AppDbContext context)
        {
            _albumRepo = albumRepo;
            _context = context;
        }

        public async Task<List<AlbumResponseDTO>> GetAllAlbumsAsync()
        {
            var albums = await _albumRepo.GetAllAsync();
            var result = new List<AlbumResponseDTO>();

            foreach (var album in albums)
            {
                result.Add(await MapToResponseDTO(album));
            }
            return result;
        }

        public async Task<AlbumResponseDTO?> GetAlbumByIdAsync(int id)
        {
            var album = await _albumRepo.GetByIdAsync(id);
            if (album == null) return null;
            return await MapToResponseDTO(album);
        }

        public async Task<AlbumResponseDTO> CreateAlbumAsync(AlbumDTO albumDto)
        {
            var album = new Album
            {
                AlbumTitle = albumDto.AlbumTitle,
                ReleaseDate = albumDto.ReleaseDate.HasValue
                    ? DateTime.SpecifyKind(albumDto.ReleaseDate.Value, DateTimeKind.Unspecified) // <-- Unspecified
                    : null,
                AlbumArtists = albumDto.ArtistIds.Select(artistId => new AlbumArtist
                {
                    ArtistId = artistId
                }).ToList()
            };
            var created = await _albumRepo.AddAsync(album);
            var albumWithArtists = await _albumRepo.GetByIdAsync(created.AlbumId);
            return await MapToResponseDTO(albumWithArtists);
        }




        public async Task<AlbumResponseDTO?> UpdateAlbumAsync(int id, AlbumDTO albumDto)
        {
            var album = await _albumRepo.GetByIdAsync(id);
            if (album == null) return null;

            // Update basic fields
            album.AlbumTitle = albumDto.AlbumTitle;
            album.ReleaseDate = albumDto.ReleaseDate;

            // Update artists (clear existing and add new)
            album.AlbumArtists.Clear();
            foreach (var artistId in albumDto.ArtistIds)
            {
                album.AlbumArtists.Add(new AlbumArtist { ArtistId = artistId });
            }

            var updated = await _albumRepo.UpdateAsync(album);
            return await MapToResponseDTO(updated);
        }

        public async Task<bool> DeleteAlbumAsync(int id)
        {
            return await _albumRepo.DeleteAsync(id);
        }

        private async Task<AlbumResponseDTO> MapToResponseDTO(Album album)
        {
            // Handle null AlbumArtists
            var artists = (album.AlbumArtists ?? new List<AlbumArtist>())
                .Where(aa => aa.Artist != null)
                .Select(aa => new ArtistResponseDTO
                {
                    ArtistId = aa.Artist.ArtistId,
                    ArtistName = aa.Artist.ArtistName
                }).ToList();

            // Handle null Songs
            var songs = await _context.Songs
                .Where(s => s.AlbumId == album.AlbumId)
                .Select(s => new SongDTO { /* ... */ })
                .ToListAsync();

            return new AlbumResponseDTO
            {
                AlbumId = album.AlbumId,
                AlbumTitle = album.AlbumTitle,
                ArtistIds = (album.AlbumArtists ?? new List<AlbumArtist>()).Select(aa => aa.ArtistId).ToList(),
                Artists = artists,
                Songs = songs
            };
        }


    }
}