using CloudAccountsShared.Configuration.Schemas;

namespace CloudAccountsShared.Configuration.Services.Contracts;

public interface ITableConfigService
{
    TableConfig GetTable(string tableName);

    IEnumerable<string> GetViewableColumns(string tableName);

    IEnumerable<string> GetEditableColumns(string tableName);

    bool CanEditTable(string tableName);

    bool CanDeleteTable(string tableName);

    IEnumerable<string> GetAuditColumns(string tableName);
}