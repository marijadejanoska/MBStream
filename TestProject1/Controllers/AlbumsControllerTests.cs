using MBStream.Controllers;
using MBStream.DTOs;
using MBStream.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace MBStream.Tests.Controllers
{
    public class AlbumsControllerTests
    {
        private readonly Mock<IAlbumService> _mockService;
        private readonly AlbumsController _controller;

        public AlbumsControllerTests()
        {
            _mockService = new Mock<IAlbumService>();
            _controller = new AlbumsController(_mockService.Object);
        }

        // GET: api/albums
        [Fact]
        public async Task GetAllAlbums_ReturnsOkResultWithAlbums()
        {
            // Arrange
            var albums = new List<AlbumResponseDTO> { new AlbumResponseDTO { AlbumId = 1, AlbumTitle = "Test" } };
            _mockService.Setup(s => s.GetAllAlbumsAsync()).ReturnsAsync(albums);

            // Act
            var result = await _controller.GetAllAlbums();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedAlbums = Assert.IsType<List<AlbumResponseDTO>>(okResult.Value);
            Assert.Single(returnedAlbums);
        }

        // GET: api/albums/5
        [Fact]
        public async Task GetAlbum_ExistingId_ReturnsAlbum()
        {
            // Arrange
            var album = new AlbumResponseDTO { AlbumId = 1, AlbumTitle = "Test" };
            _mockService.Setup(s => s.GetAlbumByIdAsync(1)).ReturnsAsync(album);

            // Act
            var result = await _controller.GetAlbum(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(album, okResult.Value);
        }

        [Fact]
        public async Task GetAlbum_InvalidId_ReturnsNotFound()
        {
            // Arrange
            _mockService.Setup(s => s.GetAlbumByIdAsync(1)).ReturnsAsync((AlbumResponseDTO?)null);

            // Act
            var result = await _controller.GetAlbum(1);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        // POST: api/albums
        [Fact]
        public async Task CreateAlbum_ValidInput_ReturnsCreatedAtAction()
        {
            // Arrange
            var albumDto = new AlbumDTO { AlbumTitle = "New Album" };
            var newAlbum = new AlbumResponseDTO { AlbumId = 1, AlbumTitle = "New Album" };
            _mockService.Setup(s => s.CreateAlbumAsync(albumDto)).ReturnsAsync(newAlbum);

            // Act
            var result = await _controller.CreateAlbum(albumDto);

            // Assert
            var createdAtResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal("GetAlbum", createdAtResult.ActionName);
            Assert.Equal(1, createdAtResult.RouteValues?["id"]);
        }

        // PUT: api/albums/5
        [Fact]
        public async Task UpdateAlbum_ValidId_ReturnsNoContent()
        {
            // Arrange
            var albumDto = new AlbumDTO { AlbumTitle = "Updated" };
            _mockService.Setup(s => s.UpdateAlbumAsync(1, albumDto)).ReturnsAsync(new AlbumResponseDTO());

            // Act
            var result = await _controller.UpdateAlbum(1, albumDto);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateAlbum_InvalidId_ReturnsNotFound()
        {
            // Arrange
            _mockService.Setup(s => s.UpdateAlbumAsync(1, It.IsAny<AlbumDTO>())).ReturnsAsync((AlbumResponseDTO?)null);

            // Act
            var result = await _controller.UpdateAlbum(1, new AlbumDTO());

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        // DELETE: api/albums/5
        [Fact]
        public async Task DeleteAlbum_ValidId_ReturnsNoContent()
        {
            // Arrange
            _mockService.Setup(s => s.DeleteAlbumAsync(1)).ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteAlbum(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteAlbum_InvalidId_ReturnsNotFound()
        {
            // Arrange
            _mockService.Setup(s => s.DeleteAlbumAsync(1)).ReturnsAsync(false);

            // Act
            var result = await _controller.DeleteAlbum(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
