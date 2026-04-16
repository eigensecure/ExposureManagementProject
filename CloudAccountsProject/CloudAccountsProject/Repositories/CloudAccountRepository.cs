using CloudAccountsProject.Repositories.Contracts;
using CloudAccountsProjects.Data;
using CloudAccountsShared.Models;
using CloudAccountsShared.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace CloudAccountsProject.Repositories;

public class CloudAccountRepository : ICloudAccountRepository
{
    private readonly CloudAccountsDbContext _context;

    public CloudAccountRepository(CloudAccountsDbContext context)
    {
        _context = context;
    }

    private readonly IWebHostEnvironment _environment;

    public CloudAccountRepository(
        CloudAccountsDbContext context,
        IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
    }

    public async Task<List<CloudAccount>> GetAllAsync()
    {
        return await _context.CloudAccounts
            .Include(x => x.CloudAccountManualDetails)
            .OrderByDescending(x => x.DateCreated)
            .ToListAsync();
    }

    public async Task ImportAsync(string provider, string json)
    {
        using var document = JsonDocument.Parse(json);

        JsonElement items;

        if (provider.Equals("gcp", StringComparison.OrdinalIgnoreCase))
        {
            if (document.RootElement.ValueKind != JsonValueKind.Array)
                throw new Exception("Expected flattened GCP JSON array.");

            items = document.RootElement;
        }
        else
        {
            items = document.RootElement;
        }

        foreach (var item in items.EnumerateArray())
        {
            var account = new CloudAccount
            {
                Provider = provider.ToLower(),
                RawJson = item.GetRawText(),
                DateCreated = DateTime.UtcNow,
                DateModified = DateTime.UtcNow
            };

            switch (provider.ToLower())
            {
                case "azure":
                    account.CloudAccountId = item.TryGetProperty("subscription_id", out var azId) ? azId.GetString() : null;
                    account.CloudName = item.TryGetProperty("subscription_name", out var azName) ? azName.GetString() : null;
                    account.CloudOrgId = item.TryGetProperty("tenant_id", out var azOrg) ? azOrg.GetString() : null;
                    account.RegistrationType = item.TryGetProperty("account_type", out var azType) ? azType.GetString() : null;
                    account.RegisteredAtCrwd = item.TryGetProperty("CreatedAt", out var azCreated) ? azCreated.GetDateTime() : null;
                    account.LastUpdatedAtCrwd = item.TryGetProperty("UpdatedAt", out var azUpdated) ? azUpdated.GetDateTime() : null;
                    account.Iomstatus = item.TryGetProperty("iom_status", out var azIom) ? azIom.GetString() : null;
                    account.RealTimeVisibilityAndDetectionStatus = item.TryGetProperty("ioa_status", out var azIoa) ? azIoa.GetString() : null;
                    break;

                case "aws":

                    account.CloudAccountId = item.TryGetProperty("account_id", out var awsId) ? awsId.GetString() : null;
                    account.CloudName = item.TryGetProperty("account_name", out var awsName) ? awsName.GetString() : null;
                    account.CloudOrgId = item.TryGetProperty("organization_id", out var awsOrg) ? awsOrg.GetString() : null;
                    account.CloudRootAccountId = item.TryGetProperty("root_account_id", out var awsRoot) ? awsRoot.GetString() : null;
                    account.RegistrationType = item.TryGetProperty("account_type", out var awsType) ? awsType.GetString() : null;
                    account.RegisteredAtCrwd = item.TryGetProperty("CreatedAt", out var awsCreated) ? awsCreated.GetDateTime() : null;
                    account.LastUpdatedAtCrwd = item.TryGetProperty("UpdatedAt", out var awsUpdated) ? awsUpdated.GetDateTime() : null;

                    if (item.TryGetProperty("settings", out var settings))
                    {
                        if (settings.TryGetProperty("deployment.method", out var deployment))
                            account.DeploymentMethod = deployment.GetString();

                        if (settings.TryGetProperty("dspm.role", out var dspm))
                            account.Dspmstatus = dspm.GetString();

                        if (settings.TryGetProperty("vulnerability_scanning.role", out var vuln))
                            account.VulnerabilityScanningStatus = vuln.GetString();
                    }

                    if (item.TryGetProperty("status", out var statusArray) &&
                        statusArray.ValueKind == JsonValueKind.Array)
                    {
                        foreach (var product in statusArray.EnumerateArray())
                        {
                            var productName = product.TryGetProperty("product", out var p)
                                ? p.GetString()
                                : null;

                            if (!product.TryGetProperty("features", out var features) ||
                                features.ValueKind != JsonValueKind.Array)
                                continue;

                            foreach (var feature in features.EnumerateArray())
                            {
                                var featureName = feature.TryGetProperty("feature", out var f)
                                    ? f.GetString()
                                    : null;

                                var featureStatus = feature.TryGetProperty("status", out var s)
                                    ? s.GetString()
                                    : null;

                                if (productName == "cspm" && featureName == "iom")
                                    account.Iomstatus = featureStatus;

                                if (productName == "cspm" && featureName == "ioa")
                                    account.RealTimeVisibilityAndDetectionStatus = featureStatus;

                                if (productName == "cspm" && featureName == "sensormgmt")
                                    account.OneClickSensorStatus = featureStatus;

                                if (productName == "idp" && featureName == "default")
                                    account.IdentityProtectionStatus = featureStatus;
                            }
                        }
                    }

                    break;

                case "gcp":

                    account.CloudAccountId = item.TryGetProperty("Cloud_Account_ID", out var gcpId)
                        ? gcpId.GetString()
                        : null;

                    account.CloudName = item.TryGetProperty("Cloud_Name", out var gcpName)
                        ? gcpName.GetString()
                        : null;

                    account.CloudOrgId = item.TryGetProperty("Cloud_ORG_ID", out var gcpOrg)
                        ? gcpOrg.GetString()
                        : null;

                    //account.CloudRootAccountId = null;

                    account.RegistrationType = item.TryGetProperty("Registration type", out var gcpType)
                        ? gcpType.GetString()
                        : null;

                    account.DeploymentMethod = item.TryGetProperty("Deployment method", out var gcpDeploy)
                        ? gcpDeploy.GetString()
                        : null;

                    account.RegisteredAtCrwd = item.TryGetProperty("Registered_At_CRWD", out var gcpCreated)
                        ? gcpCreated.GetDateTime()
                        : null;

                    account.LastUpdatedAtCrwd = item.TryGetProperty("Last_Updated_At_CRWD", out var gcpUpdated)
                        ? gcpUpdated.GetDateTime()
                        : null;

                    account.Iomstatus = item.TryGetProperty("IOMStatus", out var gcpIom)
                        ? gcpIom.GetString()
                        : null;

                    account.RealTimeVisibilityAndDetectionStatus =
                        item.TryGetProperty("Real-time visibility and detection Status", out var gcpIoa)
                            ? gcpIoa.GetString()
                            : null;

                    account.OneClickSensorStatus =
                        item.TryGetProperty("1-click sensor Status", out var gcpSensor)
                            ? gcpSensor.GetString()
                            : null;

                    account.IdentityProtectionStatus =
                        item.TryGetProperty("Identity protection Status", out var gcpIdp)
                            ? gcpIdp.GetString()
                            : null;

                    account.Dspmstatus =
                        item.TryGetProperty("DSPM Status", out var gcpDspm)
                            ? gcpDspm.GetString()
                            : null;

                    account.VulnerabilityScanningStatus =
                        item.TryGetProperty("Vulnerability scanning Status", out var gcpVuln)
                            ? gcpVuln.GetString()
                            : null;

                    break;
            }

            var exists = await _context.CloudAccounts.AnyAsync(x =>
                x.Provider == account.Provider &&
                x.CloudAccountId == account.CloudAccountId);

            if (!exists)
            {
                _context.CloudAccounts.Add(account);
            }
        }

        await _context.SaveChangesAsync();
    }

    //public async Task UpdateAsync(CloudAccount account)
    //{
    //    var existing = await _context.CloudAccounts
    //        .Include(x => x.CloudAccountManualDetail)
    //        .FirstOrDefaultAsync(x => x.Id == account.Id);

    //    if (existing == null)
    //        return;

    //    existing.CloudName = account.CloudName;
    //    existing.CloudOrgId = account.CloudOrgId;
    //    existing.CloudRootAccountId = account.CloudRootAccountId;
    //    existing.RegistrationType = account.RegistrationType;
    //    existing.DeploymentMethod = account.DeploymentMethod;

    //    existing.Iomstatus = account.Iomstatus;
    //    existing.RealTimeVisibilityAndDetectionStatus = account.RealTimeVisibilityAndDetectionStatus;
    //    existing.OneClickSensorStatus = account.OneClickSensorStatus;
    //    existing.IdentityProtectionStatus = account.IdentityProtectionStatus;
    //    existing.Dspmstatus = account.Dspmstatus;
    //    existing.VulnerabilityScanningStatus = account.VulnerabilityScanningStatus;

    //    existing.DateModified = DateTime.UtcNow;

    //    if (account.CloudAccountManualDetail != null)
    //    {
    //        if (existing.CloudAccountManualDetail == null)
    //        {
    //            existing.CloudAccountManualDetail = new CloudAccountManualDetail
    //            {
    //                CloudAccountId = existing.Id,
    //                BusinessFunctionId = account.CloudAccountManualDetail.BusinessFunctionId,
    //                AccountType = account.CloudAccountManualDetail.AccountType,
    //                OverallStatus = account.CloudAccountManualDetail.OverallStatus,
    //                Remarks = account.CloudAccountManualDetail.Remarks,
    //                AttachmentPath = account.CloudAccountManualDetail.AttachmentPath,
    //                CloudTagEmail = account.CloudAccountManualDetail.CloudTagEmail,
    //                FirstUpdatedDate = DateTime.UtcNow,
    //                LastUpdatedDate = DateTime.UtcNow,
    //                DateCreated = DateTime.UtcNow,
    //                DateModified = DateTime.UtcNow
    //            };
    //            _context.CloudAccountManualDetails.Add(existing.CloudAccountManualDetail);
    //        }
    //        else
    //        {
    //            existing.CloudAccountManualDetail.BusinessFunctionId =
    //                account.CloudAccountManualDetail.BusinessFunctionId;

    //            existing.CloudAccountManualDetail.AccountType =
    //                account.CloudAccountManualDetail.AccountType;

    //            existing.CloudAccountManualDetail.OverallStatus =
    //                account.CloudAccountManualDetail.OverallStatus;

    //            existing.CloudAccountManualDetail.Remarks =
    //                account.CloudAccountManualDetail.Remarks;

    //            existing.CloudAccountManualDetail.AttachmentPath =
    //                account.CloudAccountManualDetail.AttachmentPath;

    //            existing.CloudAccountManualDetail.CloudTagEmail =
    //                account.CloudAccountManualDetail.CloudTagEmail;

    //            existing.CloudAccountManualDetail.DateModified = DateTime.UtcNow;
    //            existing.CloudAccountManualDetail.LastUpdatedDate = DateTime.UtcNow;
    //        }
    //    }

    //    await _context.SaveChangesAsync();
    //}
    public async Task UpdateAsync(CloudAccount account)
    {
        var existing = await _context.CloudAccounts
            .Include(x => x.CloudAccountManualDetails) 
            .FirstOrDefaultAsync(x => x.Id == account.Id);

        if (existing == null)
            return;

        existing.CloudName = account.CloudName;
        existing.CloudOrgId = account.CloudOrgId;
        existing.CloudRootAccountId = account.CloudRootAccountId;
        existing.RegistrationType = account.RegistrationType;
        existing.DeploymentMethod = account.DeploymentMethod;

        existing.Iomstatus = account.Iomstatus;
        existing.RealTimeVisibilityAndDetectionStatus = account.RealTimeVisibilityAndDetectionStatus;
        existing.OneClickSensorStatus = account.OneClickSensorStatus;
        existing.IdentityProtectionStatus = account.IdentityProtectionStatus;
        existing.Dspmstatus = account.Dspmstatus;
        existing.VulnerabilityScanningStatus = account.VulnerabilityScanningStatus;

        existing.DateModified = DateTime.UtcNow;

        if (account.CloudAccountManualDetails != null && account.CloudAccountManualDetails.Any())
        {
            foreach (var incoming in account.CloudAccountManualDetails)
            {
                var existingDetail = existing.CloudAccountManualDetails
    .FirstOrDefault(x => x.Id == incoming.Id);

                if (existingDetail == null)
                {
                    var newDetail = new CloudAccountManualDetail
                    {
                        CloudAccountId = existing.Id,
                        BusinessFunctionId = incoming.BusinessFunctionId,
                        AccountType = incoming.AccountType,
                        OverallStatus = incoming.OverallStatus,
                        Remarks = incoming.Remarks,
                        AttachmentPath = incoming.AttachmentPath,
                        CloudTagEmail = incoming.CloudTagEmail,
                        FirstUpdatedDate = DateTime.UtcNow,
                        LastUpdatedDate = DateTime.UtcNow,
                        DateCreated = DateTime.UtcNow,
                        DateModified = DateTime.UtcNow
                    };

                    existing.CloudAccountManualDetails.Add(newDetail);
                }
                else
                {
                    existingDetail.BusinessFunctionId = incoming.BusinessFunctionId;
                    existingDetail.AccountType = incoming.AccountType;
                    existingDetail.OverallStatus = incoming.OverallStatus;
                    existingDetail.Remarks = incoming.Remarks;
                    existingDetail.AttachmentPath = incoming.AttachmentPath;
                    existingDetail.CloudTagEmail = incoming.CloudTagEmail;
                    existingDetail.DateModified = DateTime.UtcNow;
                    existingDetail.LastUpdatedDate = DateTime.UtcNow;
                }
            }
        }

        await _context.SaveChangesAsync();
    }

    public async Task<List<CloudAccountColumnMetadata>> GetColumnMetadataAsync()
    {
        var filePath = Path.Combine(
            _environment.ContentRootPath,
            "Metadata",
            "CloudAccountColumnMetadata.json");

        if (!File.Exists(filePath))
        {
            return new List<CloudAccountColumnMetadata>();
        }

        var json = await File.ReadAllTextAsync(filePath);

        return JsonSerializer.Deserialize<List<CloudAccountColumnMetadata>>(json)
               ?? new List<CloudAccountColumnMetadata>();
    }
    public async Task DeleteAsync(int id)
    {
        var existing = await _context.CloudAccounts.FindAsync(id);

        if (existing == null)
            return;

        _context.CloudAccounts.Remove(existing);

        await _context.SaveChangesAsync();
    }
}