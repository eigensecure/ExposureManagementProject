using System;
using System.Collections.Generic;

namespace CloudAccountsShared.Models;

public partial class CrowdGroupMaster
{
    public int Id { get; set; }

    public string CrwdgroupName { get; set; } = null!;

    public string GroupType { get; set; } = null!;

    public string? GroupId { get; set; }

    public string? FilterBy { get; set; }

    public int BusinessFunctionId { get; set; }

    public string Provider { get; set; } = null!;

    public string? Remarks { get; set; }

    public string AllAccountIds { get; set; } = null!;

    public DateTime? LastsuccessfulDateofapi { get; set; }

    public string? CommentsLogs { get; set; }

    public DateTime DateCreated { get; set; }

    public DateTime DateModified { get; set; }

    public virtual BusinessFunction BusinessFunction { get; set; } = null!;
}
