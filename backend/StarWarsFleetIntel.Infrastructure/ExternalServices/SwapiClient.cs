using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using StarWarsFleetIntel.Application.Common.Wrappers;
using StarWarsFleetIntel.Application.Interfaces;
using StarWarsFleetIntel.Domain.Entities;

namespace StarWarsFleetIntel.Infrastructure.ExternalServices
{
    public class SwapiClient : ISwapiClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<SwapiClient> _logger;
        private readonly IMemoryCache _cache;
        private const int CacheDurationMinutes = 60;

        public SwapiClient(
            HttpClient httpClient,
            ILogger<SwapiClient> logger,
            IMemoryCache cache)
        {
            _httpClient = httpClient;
            _logger = logger;
            _cache = cache;
        }

        public async Task<Result<Starship>> GetStarshipAsync(int id, CancellationToken cancellationToken = default)
        {
            var cacheKey = $"starship_{id}";

            if (_cache.TryGetValue<Starship>(cacheKey, out var cachedStarship))
            {
                _logger.LogInformation($"Returning cached starship {id}");
                return Result<Starship>.Success(cachedStarship!);
            }

            try
            {
                var response = await _httpClient.GetAsync($"starships/{id}/", cancellationToken);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning($"SWAPI returned {response.StatusCode} for starship {id}");
                    return Result<Starship>.Failure($"Starship not found: {id}");
                }

                var starship = await response.Content.ReadFromJsonAsync<Starship>(cancellationToken);

                if (starship == null)
                {
                    return Result<Starship>.Failure("Failed to deserialize starship data");
                }

                _cache.Set(cacheKey, starship, TimeSpan.FromMinutes(CacheDurationMinutes));

                return Result<Starship>.Success(starship);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, $"HTTP error fetching starship {id}");
                return Result<Starship>.Failure("External API error", new List<string> { ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unexpected error fetching starship {id}");
                return Result<Starship>.Failure("Unexpected error occurred", new List<string> { ex.Message });
            }
        }

        public async Task<Result<PaginatedResult<Starship>>> GetStarshipsAsync(int page = 1, CancellationToken cancellationToken = default)
        {
            var cacheKey = $"starships_page_{page}";

            if (_cache.TryGetValue<PaginatedResult<Starship>>(cacheKey, out var cachedResult))
            {
                _logger.LogInformation("Returning cached starships page {Page}", page);
                return Result<PaginatedResult<Starship>>.Success(cachedResult!);
            }

            try
            {
                var response = await _httpClient.GetAsync($"starships/?page={page}", cancellationToken);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("SWAPI returned {StatusCode} for page {Page}", response.StatusCode, page);
                    return Result<PaginatedResult<Starship>>.Failure($"Failed to fetch starships page {page}");
                }

                var swapiResponse = await response.Content.ReadFromJsonAsync<SwapiPaginatedResponse<Starship>>(cancellationToken);

                if (swapiResponse == null)
                {
                    return Result<PaginatedResult<Starship>>.Failure("Failed to deserialize starships data");
                }

                var paginatedResult = new PaginatedResult<Starship>
                {
                    Items = swapiResponse.Results,
                    PageNumber = page,
                    PageSize = swapiResponse.Results.Count,
                    TotalCount = swapiResponse.Count
                };

                _cache.Set(cacheKey, paginatedResult, TimeSpan.FromMinutes(CacheDurationMinutes));

                return Result<PaginatedResult<Starship>>.Success(paginatedResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching starships page {Page}", page);
                return Result<PaginatedResult<Starship>>.Failure("Unexpected error occurred", new List<string> { ex.Message });
            }
        }
        private class SwapiPaginatedResponse<T>
        {
            public int Count { get; set; }
            public string? Next { get; set; }
            public string? Previous { get; set; }
            public List<T> Results { get; set; } = new();
        }
    }
}
