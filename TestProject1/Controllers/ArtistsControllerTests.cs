using MBStream.Controllers;
using MBStream.DTOs;
using MBStream.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace MBStream.Tests.Controllers
{
    public class ArtistsControllerTests
    {
        private readonly Mock<IArtistService> _mockService;
        private readonly ArtistsController _controller;

        public ArtistsControllerTests()
        {
            _mockService = new Mock<IArtistService>();
            _controller = new ArtistsController(_mockService.Object);
        }

        // GET: api/artists
        [Fact]
        public async Task GetAllArtists_ReturnsOkResultWithArtists()
        {
            // Arrange
            var artists = new List<ArtistResponseDTO> { new ArtistResponseDTO { ArtistId = 1, ArtistName = "Test" } };
            _mockService.Setup(s => s.GetAllArtistsAsync()).ReturnsAsync(artists);

            // Act
            var result = await _controller.GetAllArtists();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedArtists = Assert.IsType<List<ArtistResponseDTO>>(okResult.Value);
            Assert.Single(returnedArtists);
        }

        // GET: api/artists/5
        [Fact]
        public async Task GetArtist_ExistingId_ReturnsArtist()
        {
            // Arrange
            var artist = new ArtistResponseDTO { ArtistId = 1, ArtistName = "Test" };
            _mockService.Setup(s => s.GetArtistByIdAsync(1)).ReturnsAsync(artist);

            // Act
            var result = await _controller.GetArtist(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(artist, okResult.Value);
        }

        [Fact]
        public async Task GetArtist_InvalidId_ReturnsNotFound()
        {
            // Arrange
            _mockService.Setup(s => s.GetArtistByIdAsync(1)).ReturnsAsync((ArtistResponseDTO?)null);

            // Act
            var result = await _controller.GetArtist(1);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        // POST: api/artists
        [Fact]
        public async Task CreateArtist_ValidInput_ReturnsCreatedAtAction()
        {
            // Arrange
            var artistDto = new ArtistDTO { ArtistName = "New Artist" };
            var newArtist = new ArtistResponseDTO { ArtistId = 1, ArtistName = "New Artist" };
            _mockService.Setup(s => s.CreateArtistAsync(artistDto)).ReturnsAsync(newArtist);

            // Act
            var result = await _controller.CreateArtist(artistDto);

            // Assert
            var createdAtResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal("GetArtist", createdAtResult.ActionName);
            Assert.Equal(1, createdAtResult.RouteValues?["id"]);
        }

        // PUT: api/artists/5
        [Fact]
        public async Task UpdateArtist_ValidId_ReturnsNoContent()
        {
            // Arrange
            var artistDto = new ArtistDTO { ArtistName = "Updated" };
            _mockService.Setup(s => s.UpdateArtistAsync(1, artistDto)).ReturnsAsync(new ArtistResponseDTO());

            // Act
            var result = await _controller.UpdateArtist(1, artistDto);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateArtist_InvalidId_ReturnsNotFound()
        {
            // Arrange
            _mockService.Setup(s => s.UpdateArtistAsync(1, It.IsAny<ArtistDTO>())).ReturnsAsync((ArtistResponseDTO?)null);

            // Act
            var result = await _controller.UpdateArtist(1, new ArtistDTO());

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        // DELETE: api/artists/5
        [Fact]
        public async Task DeleteArtist_ValidId_ReturnsNoContent()
        {
            // Arrange
            _mockService.Setup(s => s.DeleteArtistAsync(1)).ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteArtist(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteArtist_InvalidId_ReturnsNotFound()
        {
            // Arrange
            _mockService.Setup(s => s.DeleteArtistAsync(1)).ReturnsAsync(false);

            // Act
            var result = await _controller.DeleteArtist(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
