using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using StarWarsFleetIntel.Infrastructure.ExternalServices;

namespace StarWarsFleetIntel.Tests.Infrastructure
{
    public class SwapiClientTests : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly Mock<ILogger<SwapiClient>> _loggerMock;
        private readonly IMemoryCache _cache;
        private readonly SwapiClient _swapiClient;

        public SwapiClientTests()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://swapi.dev/api/")
            };
            _loggerMock = new Mock<ILogger<SwapiClient>>();
            _cache = new MemoryCache(new MemoryCacheOptions());
            _swapiClient = new SwapiClient(_httpClient, _loggerMock.Object, _cache);
        }

        [Fact]
        public async Task GetStarshipAsync_ValidId_ReturnsStarship()
        {
            // Arrange
            const int starshipId = 9;

            // Act
            var result = await _swapiClient.GetStarshipAsync(starshipId);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data!.Name.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task GetStarshipAsync_InvalidId_ReturnsFailure()
        {
            // Arrange
            const int invalidStarshipId = 99999;

            // Act
            var result = await _swapiClient.GetStarshipAsync(invalidStarshipId);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeFalse();
            result.Message.Should().Contain("not found");
        }

        [Fact]
        public async Task GetStarshipAsync_CachesResult()
        {
            // Arrange
            const int starshipId = 10;

            // Act
            var firstCall = await _swapiClient.GetStarshipAsync(starshipId);
            var secondCall = await _swapiClient.GetStarshipAsync(starshipId);

            // Assert
            firstCall.Succeeded.Should().BeTrue();
            secondCall.Succeeded.Should().BeTrue();

            // Verify cache hit was logged
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("cached")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
            _cache?.Dispose();
        }
    }
}
