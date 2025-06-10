using Microsoft.AspNetCore.Mvc;
using MBStream.DTOs;
using MBStream.Services;
using Microsoft.AspNetCore.Authorization;

namespace MBStream.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlbumsController : ControllerBase
    {
        private readonly IAlbumService _albumService;

        public AlbumsController(IAlbumService albumService)
        {
            _albumService = albumService;
        }

        // GET: api/albums
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<AlbumResponseDTO>>> GetAllAlbums()
        {
            return Ok(await _albumService.GetAllAlbumsAsync());
        }

        // GET: api/albums/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<AlbumResponseDTO>> GetAlbum(int id)
        {
            var album = await _albumService.GetAlbumByIdAsync(id);
            return album == null ? NotFound() : Ok(album);
        }

        // POST: api/albums
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<AlbumResponseDTO>> CreateAlbum(AlbumDTO albumDto)
        {
            var newAlbum = await _albumService.CreateAlbumAsync(albumDto);
            return CreatedAtAction(nameof(GetAlbum), new { id = newAlbum.AlbumId }, newAlbum);
        }

        // PUT: api/albums/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")] // Only users with "Admin" role can access
        public async Task<IActionResult> UpdateAlbum(int id, AlbumDTO albumDto)
        {
            var updatedAlbum = await _albumService.UpdateAlbumAsync(id, albumDto);
            return updatedAlbum == null ? NotFound() : NoContent();
        }

        // DELETE: api/albums/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> DeleteAlbum(int id)
        {
            var result = await _albumService.DeleteAlbumAsync(id);
            return result ? NoContent() : NotFound();
        }
    }
}
