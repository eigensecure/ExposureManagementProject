namespace CloudAccountsProject.Models
{
    public partial class BusinessFunctionMaster
    {
        public int Id { get; set; }

        public string BusinessFunctionName { get; set; } = null!;

        public string BusinessFunctionLtMember { get; set; } = null!;

        public string BusinessFunctionOwner { get; set; } = null!;

        public string BusinessFunctionSpoc { get; set; } = null!;

        public string? BusinessFunctionGroupDL { get; set; }

        public string? Remarks { get; set; }

        public string BusinessTagValue { get; set; } = null!;

        public DateTime DateCreated { get; set; }

        public DateTime DateModified { get; set; }
    }
}
