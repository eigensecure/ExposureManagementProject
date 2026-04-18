using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudAccountsShared.Models.DTOs;
public partial class CloudAccountDetailsDTO
{
    // CloudAccounts
    public int Id { get; set; }
    public string? Provider { get; set; }
    public string? CloudAccountId { get; set; }
    public string? CloudName { get; set; }
    public string? CloudOrgId { get; set; }
    public string? CloudRootAccountID { get; set; }
    public string? RegistrationType { get; set; }
    public string? DeploymentMethod { get; set; }
    public DateTime? RegisteredAtCRWD { get; set; }
    public DateTime? LastUpdatedAtCRWD { get; set; }
    public string? IOMStatus { get; set; }
    public string? RealTimeVisibilityAndDetectionStatus { get; set; }
    public string? OneClickSensorStatus { get; set; }
    public string? IdentityProtectionStatus { get; set; }
    public string? DSPMStatus { get; set; }
    public string? VulnerabilityScanningStatus { get; set; }

    // CloudAccountManualDetails
    public int? ManualDetailsId { get; set; }
    public string? AccountType { get; set; }
    public string? OverallStatus { get; set; }
    public string? ManualRemarks { get; set; }
    public string? AttachmentPath { get; set; }
    public string? FirstUpdatedBy { get; set; }
    public string? LastUpdatedBy { get; set; }

    // BusinessFunction
    public int? BusinessFunctionId { get; set; }
    public string? BusinessFunctionName { get; set; }
    public string? BusinessFunctionLtMember { get; set; }
    public string? BusinessFunctionOwner { get; set; }
    public string? BusinessFunctionSpoc { get; set; }
    public string? BusinessFunctionGroupDL { get; set; }
    public string? BusinessFunctionRemarks { get; set; }
    public string? BusinessTagValue { get; set; }
}