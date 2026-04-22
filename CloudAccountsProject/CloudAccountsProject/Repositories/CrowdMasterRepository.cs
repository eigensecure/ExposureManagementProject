using CloudAccountsProject.Repositories.Contracts;
using CloudAccountsProjects.Data;
using CloudAccountsShared.Models;
using Microsoft.EntityFrameworkCore;

namespace CloudAccountsProject.Repositories
{
    public class CrowdGroupMasterRepository(CloudAccountsDbContext context, IHttpClientFactory httpClientFactory, IConfiguration configuration) : ICrowdGroupMasterRepository
    {
        private readonly CloudAccountsDbContext _context = context;
        private readonly HttpClient _httpClient = httpClientFactory.CreateClient();
        private readonly IConfiguration _config = configuration;

        public async Task<List<CrowdGroupMaster>> GetAllAsync()
        {
            return await _context.CrowdGroupMasters
                .OrderByDescending(x => x.DateCreated)
                .ToListAsync();
        }

        public async Task<CrowdGroupMaster?> GetByIdAsync(int id)
        {
            return await _context.CrowdGroupMasters
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<CrowdGroupMaster> CreateAsync(CrowdGroupMaster group)
        {
            group.DateCreated = DateTime.UtcNow;
            group.DateModified = DateTime.UtcNow;

            _context.CrowdGroupMasters.Add(group);
            await _context.SaveChangesAsync();

            return group;
        }

        public async Task<CrowdGroupMaster> UpdateAsync(CrowdGroupMaster group)
        {
            var existing = await _context.CrowdGroupMasters
                .FirstOrDefaultAsync(x => x.Id == group.Id);

            if (existing == null)
                throw new Exception("Crowd Group not found");

            existing.CrwdgroupName = group.CrwdgroupName;
            existing.GroupType = group.GroupType;
            existing.GroupId = group.GroupId;
            existing.FilterBy = group.FilterBy;
            existing.BusinessFunctionId = group.BusinessFunctionId;
            existing.Provider = group.Provider;
            existing.Remarks = group.Remarks;
            existing.AllAccountIds = group.AllAccountIds;
            existing.DateModified = DateTime.UtcNow;
            existing.LastSuccessfulDateOfApi = group.LastSuccessfulDateOfApi;

            await _context.SaveChangesAsync();

            return existing;
        }

        public async Task DeleteAsync(int id)
        {
            var existing = await _context.CrowdGroupMasters
                .FirstOrDefaultAsync(x => x.Id == id);

            if (existing == null)
                return;

            _context.CrowdGroupMasters.Remove(existing);
            await _context.SaveChangesAsync();
        }

        public async Task<string> GetAllAccountIdsAsync(int businessFunctionId, string provider)
        {
            var accountIds = await (
                from transaction in _context.CloudAccountsTransactions
                join account in _context.CloudAccountsMasters
                    on transaction.CloudAccRef equals account.Id
                where transaction.BusFuncRef == businessFunctionId
                      && transaction.OverallStatus == "Active"
                      && account.Provider == provider
                select account.CloudAccountId
            )
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Distinct()
            .ToListAsync();

            if (!accountIds.Any())
                return string.Empty;

            return $"['{string.Join("','", accountIds)}']";
        }

        public async Task PatchGroupAsync(int id)
        {
            var group = await _context.CrowdGroupMasters
                .FirstOrDefaultAsync(x => x.Id == id);

            if (group == null)
                throw new Exception("Crowd Group not found");

            string apiUrl;
            string payload;

            if (group.GroupType == "Host_Group")
            {
                apiUrl = "https://api.eu-1.crowdstrike.com/devices/entities/host-groups/v1";

                payload = $@"
        {{
            ""resources"": [
                {{
                    ""id"": ""{group.GroupId}"",
                    ""group_type"": ""dynamic"",
                    ""assignment_rule"": ""service_provider_account_id:{group.AllAccountIds}""
                }}
            ]
        }}";
            }
            else
            {
                apiUrl = "https://api.eu-1.crowdstrike.com/cloud-security/entities/cloud-groups/v1";

                var provider = group.Provider?.ToLower();

                payload = $@"
        {{
            ""id"": ""{group.GroupId}"",
            ""selectors"": {{
                ""cloud_resources"": [
                    {{
                        ""cloud_provider"": ""{provider}"",
                        ""account_ids"": {group.AllAccountIds?.Replace('\'', '\"')},
                        ""filters"": {{}}
                    }}
                ]
            }}
        }}";
            }

            var token = _config["CrowdStrike:AccessToken"];

            using var request = new HttpRequestMessage(HttpMethod.Patch, apiUrl);
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            request.Content = new StringContent(
                payload,
                System.Text.Encoding.UTF8,
                "application/json");

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception(error);
            }

            group.LastSuccessfulDateOfApi = DateTime.UtcNow;
            group.DateModified = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }
    }
}