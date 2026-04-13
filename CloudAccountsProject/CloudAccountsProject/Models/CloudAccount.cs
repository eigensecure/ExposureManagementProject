using System;
using System.Collections.Generic;

namespace CloudAccountsProject.Models;

public partial class CloudAccount
{
    public int Id { get; set; }

    public string Provider { get; set; } = null!;

    public string? CloudAccountId { get; set; }

    public string? CloudName { get; set; }

    public string? CloudOrgId { get; set; }

    public string? CloudRootAccountId { get; set; }

    public string? RegistrationType { get; set; }

    public string? DeploymentMethod { get; set; }

    public DateTime? RegisteredAtCrwd { get; set; }

    public DateTime? LastUpdatedAtCrwd { get; set; }

    public string? Iomstatus { get; set; }

    public string? RealTimeVisibilityAndDetectionStatus { get; set; }

    public string? OneClickSensorStatus { get; set; }

    public string? IdentityProtectionStatus { get; set; }

    public string? Dspmstatus { get; set; }

    public string? VulnerabilityScanningStatus { get; set; }

    public string? RawJson { get; set; }

    public DateTime DateCreated { get; set; }

    public DateTime DateModified { get; set; }

    public virtual CloudAccountManualDetail? CloudAccountManualDetail { get; set; }
}
