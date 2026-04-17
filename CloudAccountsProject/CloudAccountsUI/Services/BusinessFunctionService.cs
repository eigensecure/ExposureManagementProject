using CloudAccountsShared.Models;
using CloudAccountsUI.Services.Contracts;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

namespace CloudAccountsUI.Services
{
    public class BusinessFunctionService : IBusinessFunctionService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _options =
            new() { PropertyNameCaseInsensitive = true };

        public BusinessFunctionService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<BusinessFunction>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync("api/BusinessFunction");

            if (!response.IsSuccessStatusCode)
                throw new Exception(await response.Content.ReadAsStringAsync());

            return await response.Content.ReadFromJsonAsync<List<BusinessFunction>>(_options)
                   ?? new();
        }

        public async Task CreateAsync(BusinessFunction item)
        {
            var response = await _httpClient.PostAsJsonAsync("api/BusinessFunction", item);

            if (!response.IsSuccessStatusCode)
                throw new Exception(await response.Content.ReadAsStringAsync());
        }

        public async Task UpdateAsync(BusinessFunction item)
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
