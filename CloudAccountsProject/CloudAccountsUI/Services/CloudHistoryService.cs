using CloudAccountsShared.Models.DTOs;
using CloudAccountsUI.Services.Contracts;
using System.Net.Http.Json;
using System.Text.Json;

namespace CloudAccountsUI.Services;

public class CloudHistoryService : ICloudHistoryService
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _options;

    public CloudHistoryService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        this._options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

    }

    public async Task<List<AuditHistoryDTO>> GetAuditByAccId(string accId)
    {
        try
        {
            var url = $"api/CloudHistory/auditMaster/{accId}";

            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                var message = await response.Content.ReadAsStringAsync();
                throw new Exception($"HTTP {response.StatusCode}: {message}");
            }

            return await response.Content.ReadFromJsonAsync<List<AuditHistoryDTO>>(_options);
        }
        catch (Exception)
        {
            throw;
        }
    }
}
