namespace CloudAccountsShared.Models.DTOs;

public class CloudAccountEditModel
{
    public int Id { get; set; }

    public int? CloudAccountManualDetailId { get; set; }
    public string? CloudName { get; set; }
    public string? CloudOrgId { get; set; }
    public string? Iomstatus { get; set; }
    public string? RealTimeVisibilityAndDetectionStatus { get; set; }

    public int? BusinessFunctionId { get; set; }
    public string? BusinessFunctionName { get; set; }

    public string? BusinessFunctionOwner { get; set; }
    public string? BusinessFunctionLtMember { get; set; }
    public string? BusinessFunctionSpoc { get; set; }

    public string? AccountType { get; set; }
    public string? OverallStatus { get; set; }
    public string? Remarks { get; set; }
    public string? CloudTagEmail { get; set; }
}