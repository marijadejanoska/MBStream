using Moq;
using Xunit;
using MBStream.Services;
using MBStream.Repositories;
using MBStream.DTOs;
using MBStream.Models;
using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MBStream.Tests.Services
{
    public class ArtistServiceTests
    {
        private readonly Mock<IArtistRepository> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly ArtistService _service;

        public ArtistServiceTests()
        {
            _mockRepo = new Mock<IArtistRepository>();
            _mockMapper = new Mock<IMapper>();
            _service = new ArtistService(_mockRepo.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task GetAllArtistsAsync_ReturnsMappedList()
        {
            // Arrange
            var artists = new List<Artist> { new Artist() };
            var dtos = new List<ArtistResponseDTO> { new ArtistResponseDTO() };
            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(artists);
            _mockMapper.Setup(m => m.Map<List<ArtistResponseDTO>>(artists)).Returns(dtos);

            // Act
            var result = await _service.GetAllArtistsAsync();

            // Assert
            Assert.Single(result);
        }

        [Fact]
        public async Task CreateArtistAsync_ValidInput_CreatesArtist()
        {
            // Arrange
            var dto = new ArtistDTO { ArtistName = "Test" };
            var artist = new Artist { ArtistId = 1 };
            _mockMapper.Setup(m => m.Map<Artist>(dto)).Returns(artist);
            _mockRepo.Setup(r => r.AddAsync(artist)).ReturnsAsync(artist);
            _mockMapper.Setup(m => m.Map<ArtistResponseDTO>(artist)).Returns(new ArtistResponseDTO());

            // Act
            var result = await _service.CreateArtistAsync(dto);

            // Assert
            Assert.NotNull(result);
            _mockRepo.Verify(r => r.AddAsync(artist), Times.Once);
        }

        [Fact]
        public async Task UpdateArtistAsync_ExistingId_UpdatesArtist()
        {
            // Arrange
            var artist = new Artist { ArtistId = 1 };
            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(artist);
            _mockMapper.Setup(m => m.Map(It.IsAny<ArtistDTO>(), artist));
            _mockRepo.Setup(r => r.UpdateAsync(artist)).ReturnsAsync(artist);

            // Add this line
            _mockMapper.Setup(m => m.Map<ArtistResponseDTO>(It.IsAny<Artist>()))
                .Returns(new ArtistResponseDTO());

            // Act
            var result = await _service.UpdateArtistAsync(1, new ArtistDTO());

            // Assert
            Assert.NotNull(result);
        }


        [Fact]
        public async Task DeleteArtistAsync_ValidId_ReturnsTrue()
        {
            // Arrange
            _mockRepo.Setup(r => r.DeleteAsync(1)).ReturnsAsync(true);

            // Act
            var result = await _service.DeleteArtistAsync(1);

            // Assert
            Assert.True(result);
        }
    }
}
