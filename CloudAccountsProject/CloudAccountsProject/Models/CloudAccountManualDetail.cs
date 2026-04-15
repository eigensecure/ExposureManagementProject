using System.Text.Json.Serialization;
namespace CloudAccountsProject.Models;


public partial class CloudAccountManualDetail
{
    public int Id { get; set; }

    public int CloudAccountId { get; set; }

    public int? BusinessFunctionId { get; set; }

    public string? AccountType { get; set; }

    public string? OverallStatus { get; set; }

    public string? Remarks { get; set; }

    public string? AttachmentPath { get; set; }

    public string? CloudTagEmail { get; set; }

    public DateTime? FirstUpdatedDate { get; set; }

    public DateTime? LastUpdatedDate { get; set; }

    public DateTime DateCreated { get; set; }

    public DateTime DateModified { get; set; }

    public virtual BusinessFunction? BusinessFunction { get; set; }

    [JsonIgnore]
    public virtual CloudAccount? CloudAccount { get; set; }
}

