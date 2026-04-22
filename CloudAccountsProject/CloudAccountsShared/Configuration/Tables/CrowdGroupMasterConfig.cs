using CloudAccountsShared.Configuration.Schemas;
using CloudAccountsShared.Models;

namespace CloudAccountsShared.Configuration.Tables;

public static class CrowdGroupMasterConfig
{
    public static TableConfig Get() => new()
    {
        TableName = nameof(CrowdGroupMaster),

        Permissions =
        [
            TablePermission.View,
            TablePermission.Edit
        ],

        Columns =
        [
            new() {
                Name = nameof(CrowdGroupMaster.Id),
                Permissions = [],
                AuditVisibility = AuditVisibility.Hidden,
                IsPrimaryKey = true
            },
            new() {
                Name = nameof(CrowdGroupMaster.CrwdgroupName),
                Permissions = [ColumnPermission.View, ColumnPermission.Edit]
            },
            new() {
                Name = nameof(CrowdGroupMaster.GroupType),
                Permissions = [ColumnPermission.View, ColumnPermission.Edit]
            },
            new() {
                Name = nameof(CrowdGroupMaster.GroupId),
                Permissions = [ColumnPermission.View, ColumnPermission.Edit]
            },
            new() {
                Name = nameof(CrowdGroupMaster.FilterBy),
                Permissions = [ColumnPermission.View, ColumnPermission.Edit]
            },
            new() {
                Name = nameof(CrowdGroupMaster.BusinessFunctionId),
                Permissions = [],
                AuditVisibility = AuditVisibility.Hidden
            },
            new() {
                Name = nameof(CrowdGroupMaster.BusinessFunction.BusinessFunctionName),
                Permissions = [ColumnPermission.View, ColumnPermission.Edit]
            },
            new() {
                Name = nameof(CrowdGroupMaster.Provider),
                Permissions = [ColumnPermission.View, ColumnPermission.Edit]
            },
            new() {
                Name = nameof(CrowdGroupMaster.Remarks),
                Permissions = [ColumnPermission.View, ColumnPermission.Edit]
            },
            new() {
                Name = nameof(CrowdGroupMaster.AllAccountIds),
                Permissions = [ColumnPermission.View]
            },
            new() {
                Name = nameof(CrowdGroupMaster.LastSuccessfulDateOfApi),
                Permissions = [ColumnPermission.View, ColumnPermission.Edit]
            },
            new() {
                Name = nameof(CrowdGroupMaster.DateCreated),
                Permissions = [ColumnPermission.View],
                AuditVisibility = AuditVisibility.Hidden
            },
            new() {
                Name = nameof(CrowdGroupMaster.DateModified),
                Permissions = [ColumnPermission.View],
                AuditVisibility = AuditVisibility.Hidden
            }
        ]
    };
}