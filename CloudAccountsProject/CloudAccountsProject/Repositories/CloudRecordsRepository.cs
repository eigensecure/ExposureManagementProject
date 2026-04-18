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

        BusinessFunction? businessFunction = null;

        // ----------------------------
        // BusinessFunction Add / Update
        // ----------------------------
        if (item.BusinessFunctionId.HasValue && item.BusinessFunctionId.Value > 0)
        {
            businessFunction = await _Dbcontext.BusinessFunctions
                .FirstOrDefaultAsync(x => x.Id == item.BusinessFunctionId.Value);

            if (businessFunction != null)
            {
                businessFunction.BusinessFunctionName = item.BusinessFunctionName;
                businessFunction.BusinessFunctionLtMember = item.BusinessFunctionLtMember;
                businessFunction.BusinessFunctionOwner = item.BusinessFunctionOwner;
                businessFunction.BusinessFunctionSpoc = item.BusinessFunctionSpoc;
                businessFunction.BusinessFunctionGroupDl = item.BusinessFunctionGroupDL;
                businessFunction.Remarks = item.BusinessFunctionRemarks;
                businessFunction.BusinessTagValue = item.BusinessTagValue;
                businessFunction.DateModified = now;
            }
        }

        // Create new BusinessFunction if not found / no id passed
        if (businessFunction == null &&
            (!string.IsNullOrWhiteSpace(item.BusinessFunctionName) ||
             !string.IsNullOrWhiteSpace(item.BusinessFunctionOwner) ||
             !string.IsNullOrWhiteSpace(item.BusinessTagValue)))
        {
            businessFunction = new BusinessFunction
            {
                BusinessFunctionName = item.BusinessFunctionName,
                BusinessFunctionLtMember = item.BusinessFunctionLtMember,
                BusinessFunctionOwner = item.BusinessFunctionOwner,
                BusinessFunctionSpoc = item.BusinessFunctionSpoc,
                BusinessFunctionGroupDl = item.BusinessFunctionGroupDL,
                Remarks = item.BusinessFunctionRemarks,
                BusinessTagValue = item.BusinessTagValue,
                DateCreated = now,
                DateModified = now
            };

            _Dbcontext.BusinessFunctions.Add(businessFunction);
            await _Dbcontext.SaveChangesAsync();
        }

        // ---------------------------------------
        // CloudAccountManualDetail Add / Update
        // ---------------------------------------
        CloudAccountManualDetail? manualDetail = null;

        if (item.ManualDetailsId.HasValue && item.ManualDetailsId.Value > 0)
        {
            manualDetail = await _Dbcontext.CloudAccountManualDetails
                .FirstOrDefaultAsync(x => x.Id == item.ManualDetailsId.Value);

            if (manualDetail != null)
            {
                manualDetail.BusFuncRef = businessFunction?.Id;
                manualDetail.AccountType = item.AccountType;
                manualDetail.OverallStatus = item.OverallStatus;
                manualDetail.Remarks = item.ManualRemarks;
                manualDetail.AttachmentPath = item.AttachmentPath;
                manualDetail.LastUpdatedBy = item.LastUpdatedBy;
                manualDetail.DateModified = now;
            }
        }

        // Create new CloudAccountManualDetail if not found
        if (manualDetail == null)
        {
            manualDetail = new CloudAccountManualDetail
            {
                CloudAccRef = item.Id,
                BusFuncRef = businessFunction?.Id,
                AccountType = item.AccountType,
                OverallStatus = item.OverallStatus,
                Remarks = item.ManualRemarks,
                AttachmentPath = item.AttachmentPath,
                FirstUpdatedBy = item.FirstUpdatedBy,
                DateCreated = now,
                DateModified = now
            };

            _Dbcontext.CloudAccountManualDetails.Add(manualDetail);
        }

        await _Dbcontext.SaveChangesAsync();
    }
}