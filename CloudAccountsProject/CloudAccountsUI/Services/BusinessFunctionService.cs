using CloudAccountsShared.Models;
using CloudAccountsUI.Services.Contracts;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

namespace CloudAccountsUI.Services
{
    public class BusinessFunctionService(IHttpClientFactory httpClient) : IBusinessFunctionService
    {
        private readonly HttpClient _httpClient = httpClient.CreateClient("ApiClient");
        private readonly JsonSerializerOptions _options =
            new() { PropertyNameCaseInsensitive = true };

        public async Task<List<BusinessFunctionMaster>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync("api/BusinessFunction");

            if (!response.IsSuccessStatusCode)
                throw new Exception(await response.Content.ReadAsStringAsync());

            return await response.Content.ReadFromJsonAsync<List<BusinessFunctionMaster>>(_options)
                   ?? new();
        }

        public async Task CreateAsync(BusinessFunctionMaster item)
        {
            var response = await _httpClient.PostAsJsonAsync("api/BusinessFunction", item);

            if (!response.IsSuccessStatusCode)
                throw new Exception(await response.Content.ReadAsStringAsync());
        }

        public async Task UpdateAsync(BusinessFunctionMaster item)
        {
            var response = await _httpClient.PutAsJsonAsync(
                $"api/BusinessFunction/{item.Id}",
                item);

            if (!response.IsSuccessStatusCode)
                throw new Exception(await response.Content.ReadAsStringAsync());
        }

        public async Task DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/BusinessFunction/{id}");

            if (!response.IsSuccessStatusCode)
                throw new Exception(await response.Content.ReadAsStringAsync());
        }
    }
}
