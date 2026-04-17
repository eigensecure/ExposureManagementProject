namespace CloudAccountsShared.Models.DTOs;

public partial class AuditHistoryDTO
{
    public int? Id { get; set; }

    public string? TableName { get; set; }

    public string? PrimaryKey { get; set; }

    public string? AuditReference { get; set; }

    public string? ModifiedByUser { get; set; }

    public string? Type { get; set; }

    public DateTime? DateTime { get; set; }

    public string? AuditTable { get; set; }

    public string? OldValues { get; set; }

    public string? NewValues { get; set; }

    public string? AffectedColumns { get; set; }
}

public enum UnwantedField
{
    Id,
    DateCreated,
    DateModified,
    CloudAccRef,
    BusFuncRef,
    LastUpdatedDate,
    FirstUpdatedDate
}
