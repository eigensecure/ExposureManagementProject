using System;
using System.Collections.Generic;

namespace CloudAccountsProject.Models;

public partial class AuditTable
{
    public int Id { get; set; }

    public string? PrimaryKey { get; set; }

    public string? TableName { get; set; }

    public string? ModifiedByUser { get; set; }

    public string? CloudAccountId { get; set; }

    public string? Type { get; set; }

    public DateTime? DateTime { get; set; }

    public string? OldValues { get; set; }

    public string? NewValues { get; set; }

    public string? AffectedColumns { get; set; }
}

