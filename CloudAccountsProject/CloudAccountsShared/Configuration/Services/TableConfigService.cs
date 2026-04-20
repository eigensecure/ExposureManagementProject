using CloudAccountsShared.Configuration.Registry;
using CloudAccountsShared.Configuration.Schemas;
using CloudAccountsShared.Configuration.Services.Contracts;

namespace CloudAccountsShared.Configuration.Services;

public class TableConfigService : ITableConfigService
{
    public TableConfig GetTable(string tableName)
    {
        return TableConfigurationRegistry.Tables[tableName];
    }

    public IEnumerable<string> GetViewableColumns(string tableName)
    {
        return GetTable(tableName)
            .Columns
            .Where(c => c.Permissions.Contains(ColumnPermission.View))
            .Select(c => c.Name);
    }

    public IEnumerable<string> GetEditableColumns(string tableName)
    {
        return GetTable(tableName)
            .Columns
            .Where(c => c.Permissions.Contains(ColumnPermission.Edit))
            .Select(c => c.Name);
    }

    public bool CanEditTable(string tableName)
    {
        return GetTable(tableName)
            .Permissions.Contains(TablePermission.Edit);
    }

    public bool CanDeleteTable(string tableName)
    {
        return GetTable(tableName)
            .Permissions.Contains(TablePermission.Delete);
    }

    public IEnumerable<string> GetAuditColumns(string tableName)
    {
        return GetTable(tableName).AuditVisibleColumns;
    }
}