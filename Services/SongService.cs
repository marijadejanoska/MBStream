using MBStream.DTOs;
using MBStream.Models;
using MBStream.Repositories;
using Microsoft.EntityFrameworkCore;
using MBStream.Data;
using MBStream.Models;

namespace MBStream.Services
{
    public class SongService : ISongService
    {
        private readonly ISongRepository _songRepo;
        private readonly AppDbContext _context;

        public SongService(ISongRepository songRepo, AppDbContext context)
        {
            _songRepo = songRepo;
            _context = context;
        }

        public async Task<List<SongResponseDTO>> GetAllSongsAsync()
        {
            var songs = await _songRepo.GetAllAsync();
            var result = new List<SongResponseDTO>();

            foreach (var song in songs)
            {
                result.Add(await MapToResponseDTO(song));
            }
            return result;
        }

        public async Task<SongResponseDTO?> GetSongByIdAsync(int id)
        {
            var song = await _songRepo.GetByIdAsync(id);
            if (song == null) return null;
            return await MapToResponseDTO(song);
        }

        public async Task<SongResponseDTO> CreateSongAsync(SongDTO songDto)
        {
            var song = new Song
            {
                SongTitle = songDto.SongTitle,
                ReleaseDate = songDto.ReleaseDate.HasValue
                    ? DateTime.SpecifyKind(songDto.ReleaseDate.Value, DateTimeKind.Utc) // Set as UTC
                    : null,
                DurationSeconds = songDto.DurationSeconds,
                AlbumId = songDto.AlbumId,
                SongArtists = songDto.ArtistIds.Select(artistId => new SongArtist
                {
                    ArtistId = artistId
                }).ToList()
            };

            var created = await _songRepo.AddAsync(song);
            return await MapToResponseDTO(created);
        }


        public async Task<SongResponseDTO?> UpdateSongAsync(int id, SongDTO songDto)
        {
            var song = await _songRepo.GetByIdAsync(id);
            if (song == null) return null;

            // Update basic fields
            song.SongTitle = songDto.SongTitle;
            song.ReleaseDate = songDto.ReleaseDate;
            song.DurationSeconds = songDto.DurationSeconds;
            song.AlbumId = songDto.AlbumId;

            // Update artists (clear existing and add new)
            song.SongArtists.Clear();
            foreach (var artistId in songDto.ArtistIds)
            {
                song.SongArtists.Add(new SongArtist { ArtistId = artistId });
            }

            var updated = await _songRepo.UpdateAsync(song);
            return await MapToResponseDTO(updated);
        }

        public async Task<bool> DeleteSongAsync(int id)
        {
            return await _songRepo.DeleteAsync(id);
        }

        private async Task<SongResponseDTO> MapToResponseDTO(Song song)
        {
            // Get artist IDs from SongArtists (already loaded)
            var artistIds = song.SongArtists.Select(sa => sa.ArtistId).ToList();

            // Load artists using the IDs
            var artists = await _context.Artists
                .Where(a => artistIds.Contains(a.ArtistId)) // ✅ EF Core can translate this
                .Select(a => new ArtistResponseDTO
                {
                    ArtistId = a.ArtistId,
                    ArtistName = a.ArtistName
                })
                .ToListAsync();

            return new SongResponseDTO
            {
                SongId = song.SongId,
                SongTitle = song.SongTitle,
                ReleaseDate = song.ReleaseDate ?? DateTime.MinValue, // or any sensible default
                DurationSeconds = song.DurationSeconds ?? 0,
                AlbumId = song.AlbumId ?? 0, // or any sensible default
                ArtistIds = artistIds,
                Artists = artists
            };

        }
    }
}
