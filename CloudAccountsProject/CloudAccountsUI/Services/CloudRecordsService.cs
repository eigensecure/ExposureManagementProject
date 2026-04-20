using CloudAccountsShared.Models.DTOs;
using CloudAccountsUI.Services.Contracts;
using Microsoft.AspNetCore.Components.Forms;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace CloudAccountsUI.Services;
public class CloudRecordsService(IHttpClientFactory httpClient) : ICloudRecordsService
{
    private readonly HttpClient _httpClient = httpClient.CreateClient("ApiClient");

    public async Task<(List<CloudAccountDetailsDTO> CloudAccounts, List<CloudAccountColumnMetadata> ColumnMetadata)> GetCloudAccountDetailsAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("api/CloudRecords/details");

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<CloudRecordsResponse>();

                if (result != null)
                {
                    return (result.CloudAccounts ?? new List<CloudAccountDetailsDTO>(),
                            result.ColumnMetadata ?? new List<CloudAccountColumnMetadata>());
                }
            }

            return (new List<CloudAccountDetailsDTO>(), new List<CloudAccountColumnMetadata>());
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading data: {ex.Message}");
            return (new List<CloudAccountDetailsDTO>(), new List<CloudAccountColumnMetadata>());
        }
    }

    public class CloudRecordsResponse
    {
        public List<CloudAccountDetailsDTO> CloudAccounts { get; set; } = new();
        public List<CloudAccountColumnMetadata> ColumnMetadata { get; set; } = new();
    }

    public async Task SaveBusManDetailsAsync(CloudAccountDetailsDTO item)
    {
        var response = await _httpClient.PostAsJsonAsync(
            "api/CloudRecords/savedetails",
            item);

        response.EnsureSuccessStatusCode();
    }

    public async Task<string> UploadAttachmentAsync(IBrowserFile file, string cloudAccountId)
    {
        using var content = new MultipartFormDataContent();

        var streamContent = new StreamContent(file.OpenReadStream(long.MaxValue));

        streamContent.Headers.ContentType =
            new MediaTypeHeaderValue(file.ContentType);

        content.Add(streamContent, "file", file.Name);
        content.Add(new StringContent(cloudAccountId), "cloudAccountId");

        var response = await _httpClient.PostAsync(
            "api/CloudRecords/uploadattachment",
            content);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception(error);
        }

        return (await response.Content.ReadAsStringAsync()).Trim('"');
    }
}

