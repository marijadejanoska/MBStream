using Microsoft.AspNetCore.Mvc;
using Moq;
using MBStream.Controllers;
using MBStream.DTOs;
using MBStream.Services;
using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MBStream.Tests.Controllers
{
    public class SongsControllerTests
    {
        private readonly Mock<ISongService> _mockService;
        private readonly SongsController _controller;

        public SongsControllerTests()
        {
            _mockService = new Mock<ISongService>();
            _controller = new SongsController(_mockService.Object);
        }

        [Fact]
        public async Task GetAllSongs_ReturnsOkWithSongs()
        {
            // Arrange
            var songs = new List<SongResponseDTO> { new SongResponseDTO() };
            _mockService.Setup(s => s.GetAllSongsAsync()).ReturnsAsync(songs);

            // Act
            var result = await _controller.GetAllSongs();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(songs, okResult.Value);
        }

        [Fact]
        public async Task GetSong_ValidId_ReturnsSong()
        {
            // Arrange
            var song = new SongResponseDTO { SongId = 1 };
            _mockService.Setup(s => s.GetSongByIdAsync(1)).ReturnsAsync(song);

            // Act
            var result = await _controller.GetSong(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(song, okResult.Value);
        }

        [Fact]
        public async Task GetSong_InvalidId_ReturnsNotFound()
        {
            // Arrange
            _mockService.Setup(s => s.GetSongByIdAsync(1)).ReturnsAsync((SongResponseDTO)null);

            // Act
            var result = await _controller.GetSong(1);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task CreateSong_ValidInput_ReturnsCreated()
        {
            // Arrange
            var songDto = new SongDTO();
            var newSong = new SongResponseDTO { SongId = 1 };
            _mockService.Setup(s => s.CreateSongAsync(songDto)).ReturnsAsync(newSong);

            // Act
            var result = await _controller.CreateSong(songDto);

            // Assert
            var createdAtResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal("GetSong", createdAtResult.ActionName);
        }

        [Fact]
        public async Task UpdateSong_ValidId_ReturnsNoContent()
        {
            // Arrange
            var songDto = new SongDTO();
            _mockService.Setup(s => s.UpdateSongAsync(1, songDto)).ReturnsAsync(new SongResponseDTO());

            // Act
            var result = await _controller.UpdateSong(1, songDto);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateSong_InvalidId_ReturnsNotFound()
        {
            // Arrange
            _mockService.Setup(s => s.UpdateSongAsync(1, It.IsAny<SongDTO>())).ReturnsAsync((SongResponseDTO)null);

            // Act
            var result = await _controller.UpdateSong(1, new SongDTO());

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteSong_ValidId_ReturnsNoContent()
        {
            // Arrange
            _mockService.Setup(s => s.DeleteSongAsync(1)).ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteSong(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteSong_InvalidId_ReturnsNotFound()
        {
            // Arrange
            _mockService.Setup(s => s.DeleteSongAsync(1)).ReturnsAsync(false);

            // Act
            var result = await _controller.DeleteSong(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
