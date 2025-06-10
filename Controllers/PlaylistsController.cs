// Controllers/PlaylistsController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MBStream.DTOs;
using MBStream.Services;

namespace MBStream.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlaylistsController : ControllerBase
    {
        private readonly IPlaylistService _playlistService;

        public PlaylistsController(IPlaylistService playlistService)
        {
            _playlistService = playlistService;
        }

        // GET: api/playlists
        [HttpGet]
        [AllowAnonymous]

        public async Task<ActionResult<List<PlaylistResponseDTO>>> GetAllPlaylists()
        {
            return Ok(await _playlistService.GetAllPlaylistsAsync());
        }

        // GET: api/playlists/5
        [HttpGet("{id}")]
        [AllowAnonymous]

        public async Task<ActionResult<PlaylistResponseDTO>> GetPlaylist(int id)
        {
            var playlist = await _playlistService.GetPlaylistByIdAsync(id);
            return playlist == null ? NotFound() : Ok(playlist);
        }

        // POST: api/playlists
        [HttpPost]
        [Authorize(Roles = "Admin")]

        public async Task<ActionResult<PlaylistResponseDTO>> CreatePlaylist(PlaylistDTO playlistDto)
        {
            var newPlaylist = await _playlistService.CreatePlaylistAsync(playlistDto);
            return CreatedAtAction(nameof(GetPlaylist), new { id = newPlaylist.PlaylistId }, newPlaylist);
        }

        // PUT: api/playlists/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> UpdatePlaylist(int id, PlaylistDTO playlistDto)
        {
            var updatedPlaylist = await _playlistService.UpdatePlaylistAsync(id, playlistDto);
            return updatedPlaylist == null ? NotFound() : NoContent();
        }

        // DELETE: api/playlists/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> DeletePlaylist(int id)
        {
            var result = await _playlistService.DeletePlaylistAsync(id);
            return result ? NoContent() : NotFound();
        }
    }
}
