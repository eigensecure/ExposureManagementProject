using CloudAccountsShared.Configuration.Schemas;
using CloudAccountsShared.Models.DTOs;

namespace CloudAccountsShared.Configuration.DTOs;

public class CloudAccountDetailsDTOConfig
{
    public static TableConfig Get() => new()
    {
        TableName = nameof(CloudAccountDetailsDTO),

        Permissions =
        [
            TablePermission.View,
            TablePermission.Edit
        ],

        Columns =
        [
            new() {
                Name = nameof(CloudAccountDetailsDTO.Id),
                Permissions = [],
                AuditVisibility = AuditVisibility.Hidden,
                IsPrimaryKey = true
            },
            new() {
                Name = nameof(CloudAccountDetailsDTO.Provider),
                Permissions = [ColumnPermission.View]
            },
            new() {
                Name = nameof(CloudAccountDetailsDTO.CloudAccountId),
                Permissions = [ColumnPermission.View]
            },
            new() {
                Name = nameof(CloudAccountDetailsDTO.CloudName),
                Permissions = [ColumnPermission.View]
            },
            new() {
                Name = nameof(CloudAccountDetailsDTO.CloudOrgId),
                Permissions = [ColumnPermission.View]
            },
            new() {
                Name = nameof(CloudAccountDetailsDTO.CloudRootAccountID),
                Permissions = [ColumnPermission.View]
            },
            new() {
                Name = nameof(CloudAccountDetailsDTO.RegistrationType),
                Permissions = [ColumnPermission.View]
            },
            new() {
                Name = nameof(CloudAccountDetailsDTO.DeploymentMethod),
                Permissions = [ColumnPermission.View]
            },
            new() {
                Name = nameof(CloudAccountDetailsDTO.RegisteredAtCRWD),
                Permissions = [ColumnPermission.View]
            },
            new() {
                Name = nameof(CloudAccountDetailsDTO.LastUpdatedAtCRWD),
                Permissions = [ColumnPermission.View]
            },
            new() {
                Name = nameof(CloudAccountDetailsDTO.IOMStatus),
                Permissions = [ColumnPermission.View]
            },
            new() {
                Name = nameof(CloudAccountDetailsDTO.RealTimeVisibilityAndDetectionStatus),
                Permissions = [ColumnPermission.View]
            },
            new() {
                Name = nameof(CloudAccountDetailsDTO.OneClickSensorStatus),
                Permissions = [ColumnPermission.View]
            },
            new() {
                Name = nameof(CloudAccountDetailsDTO.IdentityProtectionStatus),
                Permissions = [ColumnPermission.View]
            },
            new() {
                Name = nameof(CloudAccountDetailsDTO.DSPMStatus),
                Permissions = [ColumnPermission.View]
            },
            new() {
                Name = nameof(CloudAccountDetailsDTO.VulnerabilityScanningStatus),
                Permissions = [ColumnPermission.View]
            },
            new() {
                Name = nameof(CloudAccountDetailsDTO.IsActive),
                Permissions = []
            },

            // CloudAccountsTransaction
            new() {
                Name = nameof(CloudAccountDetailsDTO.ManualDetailsId),
                Permissions = []
            },
            new() {
                Name = nameof(CloudAccountDetailsDTO.AccountType),
                Permissions = [ColumnPermission.Edit]
            },
            new() {
                Name = nameof(CloudAccountDetailsDTO.OverallStatus),
                Permissions = [ColumnPermission.Edit]
            },
            new() {
                Name = nameof(CloudAccountDetailsDTO.ManualRemarks),
                Permissions = [ColumnPermission.Edit]
            },
            new() {
                Name = nameof(CloudAccountDetailsDTO.AttachmentPath),
                Permissions = [ColumnPermission.Edit]
            },
        
            // Business Function
            new() {
                Name = nameof(CloudAccountDetailsDTO.BusinessFunctionId),
                Permissions = []
            },
            new() {
                Name = nameof(CloudAccountDetailsDTO.BusinessFunctionName),
                Permissions = [ColumnPermission.Edit]
            },
            new() {
                Name = nameof(CloudAccountDetailsDTO.BusinessFunctionLtMember),
                Permissions = [ColumnPermission.Edit]
            },
            new() {
                Name = nameof(CloudAccountDetailsDTO.BusinessFunctionOwner),
                Permissions = [ColumnPermission.Edit]
            },
            new() {
                Name = nameof(CloudAccountDetailsDTO.BusinessFunctionSpoc),
                Permissions = [ColumnPermission.Edit]
            },
            new() {
                Name = nameof(CloudAccountDetailsDTO.BusinessFunctionGroupDL),
                Permissions = [ColumnPermission.Edit]
            },
            new() {
                Name = nameof(CloudAccountDetailsDTO.BusinessFunctionRemarks),
                Permissions = [ColumnPermission.Edit]
            },
            new() {
                Name = nameof(CloudAccountDetailsDTO.BusinessTagValue),
                Permissions = [ColumnPermission.Edit]
            }
        ]
    };
}