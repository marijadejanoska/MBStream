// Services/PlaylistService.cs
using MBStream.DTOs;
using MBStream.Models;
using MBStream.Repositories;
using Microsoft.EntityFrameworkCore;
using MBStream.Data;
using MBStream.Models;

namespace MBStream.Services
{
    public class PlaylistService : IPlaylistService
    {
        private readonly IPlaylistRepository _playlistRepo;
        private readonly AppDbContext _context;

        public PlaylistService(IPlaylistRepository playlistRepo, AppDbContext context)
        {
            _playlistRepo = playlistRepo;
            _context = context;
        }

        public async Task<List<PlaylistResponseDTO>> GetAllPlaylistsAsync()
        {
            var playlists = await _playlistRepo.GetAllAsync();
            var result = new List<PlaylistResponseDTO>();

            foreach (var playlist in playlists)
            {
                result.Add(await MapToResponseDTO(playlist));
            }
            return result;
        }

        public async Task<PlaylistResponseDTO?> GetPlaylistByIdAsync(int id)
        {
            var playlist = await _playlistRepo.GetByIdAsync(id);
            if (playlist == null) return null;
            return await MapToResponseDTO(playlist);
        }

        public async Task<PlaylistResponseDTO> CreatePlaylistAsync(PlaylistDTO playlistDto)
        {
            var playlist = new Playlist
            {
                PlaylistTitle = playlistDto.PlaylistTitle,
                UserId = playlistDto.UserId,
                PlaylistSongs = playlistDto.SongIds.Select(songId => new PlaylistSong
                {
                    SongId = songId
                }).ToList()
            };

            var created = await _playlistRepo.AddAsync(playlist);
            return await MapToResponseDTO(created);
        }

        public async Task<PlaylistResponseDTO?> UpdatePlaylistAsync(int id, PlaylistDTO playlistDto)
        {
            // Fetch playlist with included PlaylistSongs
            var playlist = await _playlistRepo.GetByIdAsync(id);
            if (playlist == null) return null;

            // Update basic fields
            playlist.PlaylistTitle = playlistDto.PlaylistTitle;
            playlist.UserId = playlistDto.UserId;

            // Clear existing songs and add new ones (distinct to avoid duplicates)
            playlist.PlaylistSongs.Clear();
            foreach (var songId in playlistDto.SongIds.Distinct())
            {
                playlist.PlaylistSongs.Add(new PlaylistSong { SongId = songId });
            }

            var updated = await _playlistRepo.UpdateAsync(playlist);
            return await MapToResponseDTO(updated);
        }



        public async Task<bool> DeletePlaylistAsync(int id)
        {
            return await _playlistRepo.DeleteAsync(id);
        }

        private async Task<PlaylistResponseDTO> MapToResponseDTO(Playlist playlist)
        {
            // Load song details for each song in the playlist
            var songs = await _context.Songs
                .Where(s => playlist.PlaylistSongs.Select(ps => ps.SongId).Contains(s.SongId))
                .Select(s => new SongResponseDTO
                {
                    SongId = s.SongId,
                    SongTitle = s.SongTitle,
                    ReleaseDate = s.ReleaseDate ?? DateTime.MinValue, // ✅ Handle nullable DateTime
                    DurationSeconds = s.DurationSeconds ?? 0,
                    AlbumId = s.AlbumId,
                    ArtistIds = s.SongArtists.Select(sa => sa.ArtistId).ToList(),
                    Artists = new List<ArtistResponseDTO>() // Fill as needed
                })
                .ToListAsync();

            return new PlaylistResponseDTO
            {
                PlaylistId = playlist.PlaylistId,
                PlaylistTitle = playlist.PlaylistTitle,
                UserId = playlist.UserId,
                Songs = songs
            };
        }

    }
}
