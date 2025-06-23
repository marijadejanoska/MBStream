using Microsoft.AspNetCore.Mvc;
using Moq;
using MBStream.Controllers;
using MBStream.DTOs;
using MBStream.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace MBStream.Tests.Controllers
{
    public class PlaylistsControllerTests
    {
        private readonly Mock<IPlaylistService> _mockService;
        private readonly PlaylistsController _controller;

        public PlaylistsControllerTests()
        {
            _mockService = new Mock<IPlaylistService>();
            _controller = new PlaylistsController(_mockService.Object);
        }

        [Fact]
        public async Task GetAllPlaylists_ReturnsOkResult()
        {
            // Arrange
            var playlists = new List<PlaylistResponseDTO> { new PlaylistResponseDTO() };
            _mockService.Setup(s => s.GetAllPlaylistsAsync()).ReturnsAsync(playlists);

            // Act
            var result = await _controller.GetAllPlaylists();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(playlists, okResult.Value);
        }

        [Fact]
        public async Task GetPlaylist_ExistingId_ReturnsPlaylist()
        {
            // Arrange
            var playlist = new PlaylistResponseDTO { PlaylistId = 1 };
            _mockService.Setup(s => s.GetPlaylistByIdAsync(1)).ReturnsAsync(playlist);

            // Act
            var result = await _controller.GetPlaylist(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(playlist, okResult.Value);
        }

        [Fact]
        public async Task GetPlaylist_InvalidId_ReturnsNotFound()
        {
            // Arrange
            _mockService.Setup(s => s.GetPlaylistByIdAsync(1)).ReturnsAsync((PlaylistResponseDTO)null);

            // Act
            var result = await _controller.GetPlaylist(1);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task CreatePlaylist_ValidInput_ReturnsCreated()
        {
            // Arrange
            var dto = new PlaylistDTO { PlaylistTitle = "Test" };
            var createdPlaylist = new PlaylistResponseDTO { PlaylistId = 1 };
            _mockService.Setup(s => s.CreatePlaylistAsync(dto)).ReturnsAsync(createdPlaylist);

            // Act
            var result = await _controller.CreatePlaylist(dto);

            // Assert
            var createdAtResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal("GetPlaylist", createdAtResult.ActionName);
            Assert.Equal(1, createdAtResult.RouteValues["id"]);
        }

        [Fact]
        public async Task UpdatePlaylist_ValidId_ReturnsNoContent()
        {
            // Arrange
            var dto = new PlaylistDTO { PlaylistTitle = "Updated" };
            _mockService.Setup(s => s.UpdatePlaylistAsync(1, dto)).ReturnsAsync(new PlaylistResponseDTO());

            // Act
            var result = await _controller.UpdatePlaylist(1, dto);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdatePlaylist_InvalidId_ReturnsNotFound()
        {
            // Arrange
            var dto = new PlaylistDTO { PlaylistTitle = "Updated" };
            _mockService.Setup(s => s.UpdatePlaylistAsync(1, dto)).ReturnsAsync((PlaylistResponseDTO)null);

            // Act
            var result = await _controller.UpdatePlaylist(1, dto);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeletePlaylist_ValidId_ReturnsNoContent()
        {
            // Arrange
            _mockService.Setup(s => s.DeletePlaylistAsync(1)).ReturnsAsync(true);

            // Act
            var result = await _controller.DeletePlaylist(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeletePlaylist_InvalidId_ReturnsNotFound()
        {
            // Arrange
            _mockService.Setup(s => s.DeletePlaylistAsync(1)).ReturnsAsync(false);

            // Act
            var result = await _controller.DeletePlaylist(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
