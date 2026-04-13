namespace CloudAccountsUI.Models;

public class CloudAccountManualDetail
{
    public int Id { get; set; }

    public int CloudAccountId { get; set; }

    public string? BusinessFunctionId { get; set; }

    public string? BusinessFunctionOwner { get; set; }

    public string? BusinessFunctionLtMember { get; set; }

    public string? BusinessFunctionSpoc { get; set; }

    public string? AccountType { get; set; }

    public string? OverallStatus { get; set; }

    public string? Remarks { get; set; }

    public string? AttachmentPath { get; set; }

    public string? CloudTagEmail { get; set; }

    public string? FirstUpdatedBy { get; set; }

    public DateTime? FirstUpdatedDate { get; set; }

    public string? LastUpdatedBy { get; set; }

    public DateTime? LastUpdatedDate { get; set; }
}