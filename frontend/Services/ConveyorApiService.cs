using System.Net.Http.Json;
using frontend.Models;

namespace frontend.Services;

public class ConveyorApiService
{
    private readonly HttpClient _http;

    public ConveyorApiService(HttpClient http)
    {
        _http = http;
    }

    // Health check
    public async Task<bool> HealthCheckAsync()
    {
        try
        {
            var response = await _http.GetAsync("/api/health");
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    // Client endpoints
    public async Task<List<Client>> GetClientsAsync()
    {
        return await _http.GetFromJsonAsync<List<Client>>("/api/clients") ?? [];
    }

    public async Task<Client?> GetClientAsync(string code)
    {
        try
        {
            return await _http.GetFromJsonAsync<Client>($"/api/clients/{code}");
        }
        catch
        {
            return null;
        }
    }

    // Categories
    public async Task<List<Category>> GetCategoriesAsync(string clientCode)
    {
        return await _http.GetFromJsonAsync<List<Category>>($"/api/clients/{clientCode}/categories") ?? [];
    }

    // Profile Series
    public async Task<List<ProfileSeries>> GetProfileSeriesAsync(string clientCode)
    {
        return await _http.GetFromJsonAsync<List<ProfileSeries>>($"/api/clients/{clientCode}/series") ?? [];
    }

    // Trolleys
    public async Task<List<Trolley>> GetTrolleysAsync(string clientCode, string? series = null)
    {
        var url = $"/api/clients/{clientCode}/trolleys";
        if (!string.IsNullOrEmpty(series))
            url += $"?series={series}";
        return await _http.GetFromJsonAsync<List<Trolley>>(url) ?? [];
    }

    // Track Bends
    public async Task<List<TrackBend>> GetTrackBendsAsync(string clientCode, string? series = null, int? angle = null)
    {
        var url = $"/api/clients/{clientCode}/bends";
        var queryParams = new List<string>();
        if (!string.IsNullOrEmpty(series)) queryParams.Add($"series={series}");
        if (angle.HasValue) queryParams.Add($"angle={angle}");
        if (queryParams.Count > 0) url += "?" + string.Join("&", queryParams);
        return await _http.GetFromJsonAsync<List<TrackBend>>(url) ?? [];
    }

    // Brackets
    public async Task<List<Bracket>> GetBracketsAsync(string clientCode, string? series = null)
    {
        var url = $"/api/clients/{clientCode}/brackets";
        if (!string.IsNullOrEmpty(series))
            url += $"?series={series}";
        return await _http.GetFromJsonAsync<List<Bracket>>(url) ?? [];
    }

    // Switches
    public async Task<List<Switch>> GetSwitchesAsync(string clientCode, string? series = null)
    {
        var url = $"/api/clients/{clientCode}/switches";
        if (!string.IsNullOrEmpty(series))
            url += $"?series={series}";
        return await _http.GetFromJsonAsync<List<Switch>>(url) ?? [];
    }

    // Flight Bars
    public async Task<List<FlightBar>> GetFlightBarsAsync(string clientCode, string? series = null)
    {
        var url = $"/api/clients/{clientCode}/flightbars";
        if (!string.IsNullOrEmpty(series))
            url += $"?series={series}";
        return await _http.GetFromJsonAsync<List<FlightBar>>(url) ?? [];
    }

    // Export STEP file (Overhead)
    public async Task<byte[]?> ExportOverheadStepAsync(OverheadConveyorConfig config)
    {
        var response = await _http.PostAsJsonAsync("/api/export/overhead-step", config);
        if (response.IsSuccessStatusCode)
            return await response.Content.ReadAsByteArrayAsync();
        return null;
    }

    // Submit quote
    public async Task<QuoteResponse?> SubmitQuoteAsync(QuoteRequest quote)
    {
        var response = await _http.PostAsJsonAsync("/api/quotes", quote);
        if (response.IsSuccessStatusCode)
            return await response.Content.ReadFromJsonAsync<QuoteResponse>();
        return null;
    }

    // Import CAD file (DWG/DXF)
    public async Task<CadImportResult?> ImportCadFileAsync(Stream fileStream, string fileName)
    {
        using var content = new MultipartFormDataContent();
        using var streamContent = new StreamContent(fileStream);
        content.Add(streamContent, "file", fileName);

        var response = await _http.PostAsync("/api/import/cad", content);
        if (response.IsSuccessStatusCode)
            return await response.Content.ReadFromJsonAsync<CadImportResult>();

        return new CadImportResult
        {
            Success = false,
            Error = $"Server returned {response.StatusCode}"
        };
    }
}
