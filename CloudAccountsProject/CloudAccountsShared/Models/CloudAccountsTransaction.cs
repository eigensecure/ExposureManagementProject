using System;
using System.Collections.Generic;

namespace CloudAccountsShared.Models;

public partial class CloudAccountsTransaction
{
    public int Id { get; set; }

    public int CloudAccRef { get; set; }

    public int? BusFuncRef { get; set; }

    public string? AccountType { get; set; }

    public string? OverallStatus { get; set; }

    public string? Remarks { get; set; }

    public string? AttachmentPath { get; set; }

    public DateTime DateCreated { get; set; }

    public DateTime DateModified { get; set; }

    public virtual BusinessFunctionMaster? BusFuncRefNavigation { get; set; }

    public virtual CloudAccountsMaster CloudAccRefNavigation { get; set; } = null!;
}
