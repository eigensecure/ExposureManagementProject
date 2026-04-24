using CloudAccountsShared.Configuration.Schemas;
using CloudAccountsShared.Models;
using CloudAccountsShared.Models.DTOs;

namespace CloudAccountsShared.Configuration.Tables;

public class CloudAccountsTransactionConfig
{
    public static TableConfig Get() => new()
    {
        TableName = nameof(CloudAccountsTransaction),

        Permissions =
        [
            TablePermission.View,
            TablePermission.Edit
        ],

        Columns =
        [
            new() {
                Name = nameof(CloudAccountsTransaction.Id),
                Permissions = [],
                AuditVisibility = AuditVisibility.Hidden,
                IsPrimaryKey = true
            },
            new() {
                Name = nameof(CloudAccountsTransaction.CloudAccRef),
                Permissions = [],
                AuditVisibility = AuditVisibility.Hidden
            },
            new() {
                Name = nameof(CloudAccountsTransaction.BusFuncRef),
                Permissions = [],
                AuditVisibility = AuditVisibility.Hidden
            },
            new() {
                Name = nameof(CloudAccountsTransaction.BusFuncRefNavigation.BusinessFunctionName), //for audit only
                Permissions= []
            },
            new() {
                Name = nameof(CloudAccountsTransaction.AccountType),
                Permissions = [ColumnPermission.View, ColumnPermission.Edit]
            },
            new() {
                Name = nameof(CloudAccountsTransaction.OverallStatus),
                Permissions = [ColumnPermission.View, ColumnPermission.Edit]
            },
            new() {
                Name = nameof(CloudAccountsTransaction.Remarks),
                Permissions = [ColumnPermission.View, ColumnPermission.Edit]
            },
            new() {
                Name = nameof(CloudAccountsTransaction.AttachmentPath),
                Permissions = [ColumnPermission.View, ColumnPermission.Edit],
                AuditVisibility = AuditVisibility.Hidden
            },
            new() {
                Name = nameof(CloudAccountsTransaction.DateCreated),
                Permissions = [],
                AuditVisibility = AuditVisibility.Hidden
            },
            new() {
                Name = nameof(CloudAccountsTransaction.DateModified),
                Permissions = [],
                AuditVisibility = AuditVisibility.Hidden
            }
        ]
    };
}