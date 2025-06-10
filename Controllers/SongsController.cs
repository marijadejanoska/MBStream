// Controllers/SongsController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MBStream.DTOs;
using MBStream.Services;

namespace MBStream.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SongsController : ControllerBase
    {
        private readonly ISongService _songService;

        public SongsController(ISongService songService)
        {
            _songService = songService;
        }

        // GET: api/songs
        [HttpGet]
        [AllowAnonymous]

        public async Task<ActionResult<List<SongResponseDTO>>> GetAllSongs()
        {
            return Ok(await _songService.GetAllSongsAsync());
        }

        // GET: api/songs/5
        [HttpGet("{id}")]
        [AllowAnonymous]

        public async Task<ActionResult<SongResponseDTO>> GetSong(int id)
        {
            var song = await _songService.GetSongByIdAsync(id);
            return song == null ? NotFound() : Ok(song);
        }

        // POST: api/songs
        [HttpPost]
        [Authorize(Roles = "Admin")]

        public async Task<ActionResult<SongResponseDTO>> CreateSong(SongDTO songDto)
        {
            var newSong = await _songService.CreateSongAsync(songDto);
            return CreatedAtAction(nameof(GetSong), new { id = newSong.SongId }, newSong);
        }

        // PUT: api/songs/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> UpdateSong(int id, SongDTO songDto)
        {
            var updatedSong = await _songService.UpdateSongAsync(id, songDto);
            return updatedSong == null ? NotFound() : NoContent();
        }

        // DELETE: api/songs/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> DeleteSong(int id)
        {
            var result = await _songService.DeleteSongAsync(id);
            return result ? NoContent() : NotFound();
        }
    }
}
