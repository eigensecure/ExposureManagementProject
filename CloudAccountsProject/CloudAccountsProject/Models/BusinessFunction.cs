namespace CloudAccountsProject.Models;

public partial class BusinessFunction
{
    public int Id { get; set; }

    public string? BusinessFunctionName { get; set; }

    public string? BusinessFunctionLtMember { get; set; }

    public string? BusinessFunctionOwner { get; set; }

    public string? BusinessFunctionSpoc { get; set; }

    public string? BusinessFunctionGroupDl { get; set; }

    public string? Remarks { get; set; }

    public string? BusinessTagValue { get; set; }

    public DateTime DateCreated { get; set; }

    public DateTime DateModified { get; set; }

    public virtual ICollection<CloudAccountManualDetail> CloudAccountManualDetails { get; set; } = new List<CloudAccountManualDetail>();
}
