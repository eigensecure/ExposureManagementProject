using CloudAccountsShared.Configuration.Schemas;
using CloudAccountsShared.Models;

namespace CloudAccountsShared.Configuration.Tables;

public class CloudAccountsMasterConfig
{
    public static TableConfig Get() => new()
    {
        TableName = nameof(CloudAccountsMaster),

        Permissions =
        [
            TablePermission.View,
            TablePermission.Edit
        ],

        Columns =
        [
            new() {
                Name = nameof(CloudAccountsMaster.Id),
                Permissions = [],
                AuditVisibility = AuditVisibility.Hidden,
                IsPrimaryKey = true
            },
            new() {
                Name = nameof(CloudAccountsMaster.Provider),
                Permissions = [ColumnPermission.View]
            },
            new() {
                Name = nameof(CloudAccountsMaster.CloudAccountId),
                Permissions = [ColumnPermission.View]
            },
            new() {
                Name = nameof(CloudAccountsMaster.CloudName),
                Permissions = [ColumnPermission.View]
            },
            new() {
                Name = nameof(CloudAccountsMaster.CloudOrgId),
                Permissions = [ColumnPermission.View]
            },
            new() {
                Name = nameof(CloudAccountsMaster.CloudRootAccountId),
                Permissions = [ColumnPermission.View]
            },
            new() {
                Name = nameof(CloudAccountsMaster.RegistrationType),
                Permissions = [ColumnPermission.View]
            },
            new() {
                Name = nameof(CloudAccountsMaster.DeploymentMethod),
                Permissions = [ColumnPermission.View]
            },
            new() {
                Name = nameof(CloudAccountsMaster.RegisteredAtCrwd),
                Permissions = [ColumnPermission.View]
            },
            new() {
                Name = nameof(CloudAccountsMaster.LastUpdatedAtCrwd),
                Permissions = [ColumnPermission.View]
            },
            new() {
                Name = nameof(CloudAccountsMaster.Iomstatus),
                Permissions = [ColumnPermission.View]
            },
            new() {
                Name = nameof(CloudAccountsMaster.RealTimeVisibilityAndDetectionStatus),
                Permissions = [ColumnPermission.View]
            },
            new() {
                Name = nameof(CloudAccountsMaster.OneClickSensorStatus),
                Permissions = [ColumnPermission.View]
            },
            new() {
                Name = nameof(CloudAccountsMaster.IdentityProtectionStatus),
                Permissions = [ColumnPermission.View]
            },
            new() {
                Name = nameof(CloudAccountsMaster.Dspmstatus),
                Permissions = [ColumnPermission.View]
            },
            new() {
                Name = nameof(CloudAccountsMaster.VulnerabilityScanningStatus),
                Permissions = [ColumnPermission.View]
            },
            new() {
                Name = nameof(CloudAccountsMaster.RawJson),
                Permissions = [],
                AuditVisibility = AuditVisibility.Hidden
            },
            new() {
                Name = nameof(CloudAccountsMaster.IsActive),
                Permissions = [],
                AuditVisibility = AuditVisibility.Hidden
            },
            new() {
                Name = nameof(CloudAccountsMaster.DateCreated),
                Permissions = [],
                AuditVisibility = AuditVisibility.Hidden
            },
            new() {
                Name = nameof(CloudAccountsMaster.DateModified),
                Permissions = [],
                AuditVisibility = AuditVisibility.Hidden
            }
        ]
    };
}
