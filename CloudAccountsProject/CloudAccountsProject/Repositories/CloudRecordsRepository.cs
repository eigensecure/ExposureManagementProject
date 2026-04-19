using CloudAccountsProject.Data;
using CloudAccountsProject.Repositories.Contracts;
using CloudAccountsProjects.Data;
using CloudAccountsShared.Models;
using CloudAccountsShared.Models.DTOs;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace CloudAccountsProject.Repositories;

public class CloudRecordsRepository(CloudAccountsDbContext Dbcontext,
    CloudAccountsDbSPContext DbSPcontext,
    IWebHostEnvironment environment) : ICloudRecordsRepository
{
    private readonly CloudAccountsDbContext _Dbcontext = Dbcontext;
    private readonly CloudAccountsDbSPContext _DbSPcontext = DbSPcontext;
    private readonly IWebHostEnvironment _environment = environment;

    public async Task<(List<CloudAccountDetailsDTO> CloudAccounts, List<CloudAccountColumnMetadata> ColumnMetadata)> GetCloudAccountDetailsAsync()
    {
        var cloudAccounts = await _DbSPcontext.CloudAccountDetailsDTOs
            .FromSqlRaw("EXEC dbo.GetCloudAccountDetails")
            .ToListAsync();
        cloudAccounts = [.. cloudAccounts.Where(x => x.IsActive == true)];

        var filePath = Path.Combine(
            _environment.ContentRootPath,
            "Metadata",
            "CloudAccountColumnMetadata.json");

        List<CloudAccountColumnMetadata> columnMetadata;

        if (!File.Exists(filePath))
        {
            columnMetadata = new List<CloudAccountColumnMetadata>();
        }
        else
        {
            var json = await File.ReadAllTextAsync(filePath);

            columnMetadata = JsonSerializer.Deserialize<List<CloudAccountColumnMetadata>>(json)
                             ?? new List<CloudAccountColumnMetadata>();
        }

        return (cloudAccounts, columnMetadata);
    }

    public async Task SaveBusManDetailsAsync(CloudAccountDetailsDTO item)
    {
        var now = DateTime.UtcNow;

        // Only use the selected BusinessFunctionId from UI autocomplete.
        // No add/update should happen in BusinessFunction table.

        CloudAccountsTransaction? manualDetail = null;

        if (item.ManualDetailsId.HasValue && item.ManualDetailsId.Value > 0)
        {
            manualDetail = await _Dbcontext.CloudAccountsTransactions
                .FirstOrDefaultAsync(x => x.Id == item.ManualDetailsId.Value);

            if (manualDetail != null)
            {
                manualDetail.BusFuncRef = item.BusinessFunctionId;
                manualDetail.AccountType = item.AccountType;
                manualDetail.OverallStatus = item.OverallStatus;
                manualDetail.Remarks = item.ManualRemarks;
                manualDetail.AttachmentPath = item.AttachmentPath;
                manualDetail.DateModified = now;
            }
        }

        // If no manual detail exists yet for this Cloud Account, create one
        if (manualDetail == null)
        {
            manualDetail = await _Dbcontext.CloudAccountsTransactions
                .FirstOrDefaultAsync(x => x.CloudAccRef == item.Id);

            if (manualDetail != null)
            {
                manualDetail.BusFuncRef = item.BusinessFunctionId;
                manualDetail.AccountType = item.AccountType;
                manualDetail.OverallStatus = item.OverallStatus;
                manualDetail.Remarks = item.ManualRemarks;
                manualDetail.AttachmentPath = item.AttachmentPath;
                manualDetail.DateModified = now;
            }
            else
            {
                manualDetail = new CloudAccountsTransaction
                {
                    CloudAccRef = item.Id,
                    BusFuncRef = item.BusinessFunctionId,
                    AccountType = item.AccountType,
                    OverallStatus = item.OverallStatus,
                    Remarks = item.ManualRemarks,
                    AttachmentPath = item.AttachmentPath,
                    DateCreated = now,
                    DateModified = now
                };

                _Dbcontext.CloudAccountsTransactions.Add(manualDetail);
            }
        }

        await _Dbcontext.SaveChangesAsync();
    }
}