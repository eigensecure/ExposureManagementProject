using CloudAccountsShared.Models.DTOs;
using CloudAccountsUI.Services.Contracts;
using System.Net.Http.Json;
using System.Text.Json;

namespace CloudAccountsUI.Services;

public class CloudHistoryService(IHttpClientFactory httpClient) : ICloudHistoryService
{
    private readonly HttpClient _httpClient = httpClient.CreateClient("ApiClient");
    private readonly JsonSerializerOptions _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

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

    public async Task<List<AuditHistoryDTO>> GetManAuditByRef(int Id)
    {
        try
        {
            var url = $"api/CloudHistory/auditTransaction/{Id}";

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

    public async Task<List<AuditHistoryDTO>> GetBusAuditByRef(int Id)
    {
        try
        {
            var url = $"api/CloudHistory/auditBusiness/{Id}";

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

    public async Task<List<AuditHistoryDTO>> GetCrowdGroupAudit(int Id)
    {
        try
        {
            var url = $"api/CloudHistory/auditCrowdGroup/{Id}";

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
