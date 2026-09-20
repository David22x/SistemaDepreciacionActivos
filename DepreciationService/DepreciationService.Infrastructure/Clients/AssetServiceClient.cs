using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using DepreciationService.Domain.Models;

namespace DepreciationService.Infrastructure.Clients;

public class AssetServiceClient
{
    private readonly HttpClient _httpClient;

    public AssetServiceClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ActivoDto?> ObtenerActivoAsync(int activoId, string? bearerToken)
    {
        if (!string.IsNullOrWhiteSpace(bearerToken))
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", bearerToken);
        }

        var response = await _httpClient.GetAsync($"api/Activos/{activoId}");

        if (response.StatusCode == HttpStatusCode.NotFound)
            return null;

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<ActivoDto>();
    }
}