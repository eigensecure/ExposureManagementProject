using System;
using System.Collections.Generic;

namespace CloudAccountsShared.Models;

public partial class AuditTableTransaction
{
    public int Id { get; set; }

    public string? PrimaryKey { get; set; }

    public string? TableName { get; set; }

    public string? Reference { get; set; }

    public string? ModifiedByUser { get; set; }

    public string? Type { get; set; }

    public DateTime? DateTime { get; set; }

    public string? OldValues { get; set; }

    public string? NewValues { get; set; }

    public string? AffectedColumns { get; set; }
}
