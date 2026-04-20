namespace CloudAccountsShared.Configuration.Schemas;

public class ColumnConfig
{
    public string Name { get; set; } = default!;

    public HashSet<ColumnPermission> Permissions { get; set; } = [];

    public AuditVisibility AuditVisibility { get; set; } = AuditVisibility.Visible;

    public bool IsPrimaryKey { get; set; } = false;

    public bool IsSensitive { get; set; } = false;
}