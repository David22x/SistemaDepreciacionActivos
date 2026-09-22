using System.Net.Http.Headers;
using System.Net.Http.Json;
using DepreciationService.Application.Common.Interfaces;
using DepreciationService.Domain.Models;

namespace DepreciationService.Infrastructure.Clients;

public class AssetServiceClient : IAssetServiceClient
{
    private readonly HttpClient _http;
    public AssetServiceClient(HttpClient http) => _http = http;

    public async Task<ActivoDto?> ObtenerActivoAsync(int activoId, string bearerToken)
    {
        _http.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", bearerToken);

        var response = await _http.GetAsync($"api/activos/{activoId}");
        if (!response.IsSuccessStatusCode) return null;

        return await response.Content.ReadFromJsonAsync<ActivoDto>();
    }
}