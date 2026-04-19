using CloudAccountsShared.Models;
using CloudAccountsUI.Services.Contracts;
using System.Net.Http.Json;

namespace CloudAccountsUI.Services
{
    public class CrowdGroupMasterService(IHttpClientFactory httpClient) : ICrowdGroupMasterService
    {
        private readonly HttpClient _httpClient = httpClient.CreateClient("ApiClient");

        public async Task<List<CrowdGroupMaster>> GetAllAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<CrowdGroupMaster>>(
                "api/CrowdGroupMaster")
                ?? new List<CrowdGroupMaster>();
        }

        public async Task<CrowdGroupMaster?> GetByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<CrowdGroupMaster>(
                $"api/CrowdGroupMaster/{id}");
        }

        public async Task<CrowdGroupMaster> CreateAsync(CrowdGroupMaster group)
        {
            var response = await _httpClient.PostAsJsonAsync(
                "api/CrowdGroupMaster/create", group);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception(error);
            }

            return await response.Content.ReadFromJsonAsync<CrowdGroupMaster>()
                   ?? throw new Exception("Failed to create crowd group");
        }

        public async Task<CrowdGroupMaster> UpdateAsync(CrowdGroupMaster group)
        {
            var response = await _httpClient.PutAsJsonAsync(
                $"api/CrowdGroupMaster/{group.Id}", group);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<CrowdGroupMaster>()
                   ?? throw new Exception("Failed to update crowd group");
        }

        public async Task DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync(
                $"api/CrowdGroupMaster/{id}");

            response.EnsureSuccessStatusCode();
        }
    }
}