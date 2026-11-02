using System.Net.Http.Json;

namespace dotnet2026.Infrastructure.External
{
    public sealed class UniversityService
    {
        private readonly HttpClient _httpClient;

        public UniversityService(HttpClient client)
        {
            _httpClient = client;
        }

        public async Task<object?> GetUniversitiesByCountry(
            string country,
            CancellationToken cancellationToken = default)
        {
            var result = await _httpClient.GetFromJsonAsync<object>(
                $"search?country={Uri.EscapeDataString(country)}",
                cancellationToken);

            return result;
        }
    }
}