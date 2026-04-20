using CloudAccountsShared.Configuration.Schemas;
using CloudAccountsShared.Models;

namespace CloudAccountsShared.Configuration.Tables;

public class BusinessFunctionMasterConfig
{
    public static TableConfig Get() => new()
    {
        TableName = nameof(BusinessFunctionMaster),

        Permissions =
        [
            TablePermission.View,
            TablePermission.Edit
        ],

        Columns =
        [
            new() {
                Name = nameof(BusinessFunctionMaster.Id),
                Permissions = [],
                AuditVisibility = AuditVisibility.Hidden,
                IsPrimaryKey = true,
            },
            new() {
                Name = nameof(BusinessFunctionMaster.BusinessFunctionName),
                Permissions = [ColumnPermission.View, ColumnPermission.Edit]
            },
            new() {
                Name = nameof(BusinessFunctionMaster.BusinessFunctionLtMember),
                Permissions = [ColumnPermission.View, ColumnPermission.Edit]
            },
            new() {
                Name = nameof(BusinessFunctionMaster.BusinessFunctionOwner),
                Permissions = [ColumnPermission.View, ColumnPermission.Edit]
            },
            new() {
                Name = nameof(BusinessFunctionMaster.BusinessFunctionSpoc),
                Permissions = [ColumnPermission.View, ColumnPermission.Edit]
            },
            new() {
                Name = nameof(BusinessFunctionMaster.BusinessFunctionGroupDl),
                Permissions = [ColumnPermission.View, ColumnPermission.Edit]
            },
            new() {
                Name = nameof(BusinessFunctionMaster.Remarks),
                Permissions = [ColumnPermission.View, ColumnPermission.Edit]
            },
            new() {
                Name = nameof(BusinessFunctionMaster.BusinessTagValue),
                Permissions = [ColumnPermission.View, ColumnPermission.Edit]
            },
            new() {
                Name = nameof(BusinessFunctionMaster.DateCreated),
                Permissions = [],
                AuditVisibility = AuditVisibility.Hidden
            },
            new() {
                Name = nameof(BusinessFunctionMaster.DateModified),
                Permissions = [],
                AuditVisibility = AuditVisibility.Hidden
            }
        ]
    };
}
