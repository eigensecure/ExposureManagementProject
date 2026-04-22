namespace CloudAccountsShared.DbSyncModels;

public partial class BlobRequest
{
    public string Container { get; set; }
    public string BlobName { get; set; }
}