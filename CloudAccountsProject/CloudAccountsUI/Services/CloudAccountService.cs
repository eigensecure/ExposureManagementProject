using CloudAccountsShared.Models;
using CloudAccountsShared.Models.DTOs;
using CloudAccountsUI.Services.Contracts;
using System.Net.Http.Json;
using System.Text.Json;

namespace CloudAccountsUI.Services
{
    public class CloudAccountService : ICloudAccountService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _options =
            new() { PropertyNameCaseInsensitive = true };

        public CloudAccountService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<(List<CloudAccount> Accounts, List<CloudAccountColumnMetadata> Metadata)> GetCloudAccountsDataAsync()
        {
            var accountsTask = _httpClient.GetAsync("api/CloudAccounts");
            var metadataTask = _httpClient.GetAsync("api/CloudAccounts/column-metadata");

            await Task.WhenAll(accountsTask, metadataTask);

            var accountsResponse = await accountsTask;
            var metadataResponse = await metadataTask;

            if (!accountsResponse.IsSuccessStatusCode)
                throw new Exception(await accountsResponse.Content.ReadAsStringAsync());

            if (!metadataResponse.IsSuccessStatusCode)
                throw new Exception(await metadataResponse.Content.ReadAsStringAsync());

            var accounts = await accountsResponse.Content.ReadFromJsonAsync<List<CloudAccount>>(_options)
                           ?? new();

            var metadata = await metadataResponse.Content.ReadFromJsonAsync<List<CloudAccountColumnMetadata>>(_options)
                           ?? new();

            return (accounts, metadata);
        }

        public async Task<List<BusinessFunction>> GetBusinessFunctionsAsync()
        {
            var response = await _httpClient.GetAsync("api/BusinessFunction");

            if (!response.IsSuccessStatusCode)
                throw new Exception(await response.Content.ReadAsStringAsync());

            return await response.Content.ReadFromJsonAsync<List<BusinessFunction>>(_options)
                   ?? new();
        }

        public async Task UpdateAsync(int id, CloudAccount account)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/CloudAccounts/{id}", account);

            if (!response.IsSuccessStatusCode)
                throw new Exception(await response.Content.ReadAsStringAsync());
        }

        //public async Task<List<CloudAccountColumnMetadata>> GetColumnMetadataAsync()
        //{
        //    var response = await _httpClient.GetAsync("api/CloudAccounts/column-metadata");

        //    if (!response.IsSuccessStatusCode)
        //        throw new Exception(await response.Content.ReadAsStringAsync());

        //    return await response.Content.ReadFromJsonAsync<List<CloudAccountColumnMetadata>>(_options)
        //           ?? new();
        //}
    }
}

