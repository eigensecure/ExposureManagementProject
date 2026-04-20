namespace CloudAccountsShared.Configuration.Schemas;

public class TableConfig
{
    public string TableName { get; set; } = default!;

    public HashSet<TablePermission> Permissions { get; set; } = [];

    public List<ColumnConfig> Columns { get; set; } = [];

    public List<string> AuditVisibleColumns =>
        [.. Columns
            .Where(c => c.AuditVisibility == AuditVisibility.Visible)
            .Select(c => c.Name)];
}