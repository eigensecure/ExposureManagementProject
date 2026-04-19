using System;
using System.Collections.Generic;

namespace CloudAccountsShared.Models;

public partial class CrowdGroupMaster
{
    public int Id { get; set; }

    public string CrwdgroupName { get; set; } = null!;

    public string? GroupType { get; set; }

    public string GroupId { get; set; } = null!;

    public string? FilterBy { get; set; }

    public int? BusinessFunctionId { get; set; }

    public string? Provider { get; set; }

    public string? Remarks { get; set; }

    public string? AllAccountIds { get; set; }

    public DateTime? LastSuccessfulDateOfApi { get; set; }

    public DateTime DateCreated { get; set; }

    public DateTime DateModified { get; set; }

    public virtual BusinessFunctionMaster? BusinessFunction { get; set; }
}