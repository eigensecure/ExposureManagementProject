using CloudAccountsShared.Models;

namespace CloudAccountsShared.DbSyncModels.CloudMasterSync;

public partial class ActionFormat
{
    public string Action { get; set; } = null!;

    public string CloudAccountId { get; set; } = null!;

    public CloudAccountsMaster? NewValue { get; set; } = null!;
}
