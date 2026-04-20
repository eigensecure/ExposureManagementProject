using System;
using System.Collections.Generic;

namespace CloudAccountsShared.Models;

public partial class BusinessFunctionMaster
{
    public int Id { get; set; }

    public string BusinessFunctionName { get; set; } = null!;

    public string? BusinessFunctionLtMember { get; set; }

    public string? BusinessFunctionOwner { get; set; }

    public string? BusinessFunctionSpoc { get; set; }

    public string? BusinessFunctionGroupDl { get; set; }

    public string? Remarks { get; set; }

    public string? BusinessTagValue { get; set; }

    public DateTime DateCreated { get; set; }

    public DateTime DateModified { get; set; }

    public virtual ICollection<BusinessTag> BusinessTags { get; set; } = new List<BusinessTag>();

    public virtual ICollection<CloudAccountsTransaction> CloudAccountsTransactions { get; set; } = new List<CloudAccountsTransaction>();

    public virtual ICollection<CrowdGroupMaster> CrowdGroupMasters { get; set; } = new List<CrowdGroupMaster>();
}