// Controllers/ArtistsController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MBStream.DTOs;
using MBStream.Services;

namespace MBStream.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtistsController : ControllerBase
    {
        private readonly IArtistService _artistService;

        public ArtistsController(IArtistService artistService)
        {
            _artistService = artistService;
        }

        // GET: api/artists
        [HttpGet]
        [AllowAnonymous]

        public async Task<ActionResult<List<ArtistResponseDTO>>> GetAllArtists()
        {
            return Ok(await _artistService.GetAllArtistsAsync());
        }

        // GET: api/artists/5
        [HttpGet("{id}")]
        [AllowAnonymous]

        public async Task<ActionResult<ArtistResponseDTO>> GetArtist(int id)
        {
            var artist = await _artistService.GetArtistByIdAsync(id);
            return artist == null ? NotFound() : Ok(artist);
        }

        // POST: api/artists
        [HttpPost]
        [Authorize(Roles = "Admin")]

        public async Task<ActionResult<ArtistResponseDTO>> CreateArtist(ArtistDTO artistDto)
        {
            var newArtist = await _artistService.CreateArtistAsync(artistDto);
            return CreatedAtAction(nameof(GetArtist), new { id = newArtist.ArtistId }, newArtist);
        }

        // PUT: api/artists/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> UpdateArtist(int id, ArtistDTO artistDto)
        {
            var updatedArtist = await _artistService.UpdateArtistAsync(id, artistDto);
            return updatedArtist == null ? NotFound() : NoContent();
        }

        // DELETE: api/artists/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> DeleteArtist(int id)
        {
            var result = await _artistService.DeleteArtistAsync(id);
            return result ? NoContent() : NotFound();
        }
    }
}
