using CloudAccountsShared.Configuration.DTOs;
using CloudAccountsShared.Configuration.Schemas;
using CloudAccountsShared.Configuration.Tables;
using CloudAccountsShared.Models;
using CloudAccountsShared.Models.DTOs;

namespace CloudAccountsShared.Configuration.Registry;

public static class TableConfigurationRegistry
{
    public static readonly Dictionary<string, TableConfig> Tables = new()
    {
        // Tables
        [nameof(CloudAccountsMaster)] = CloudAccountsMasterConfig.Get(),
        [nameof(CloudAccountsTransaction)] = CloudAccountsTransactionConfig.Get(),
        [nameof(BusinessFunctionMaster)] = BusinessFunctionMasterConfig.Get(),
        [nameof(CrowdGroupMaster)] = CrowdGroupMasterConfig.Get(),

        // DTOs
        [nameof(CloudAccountDetailsDTO)] = CloudAccountDetailsDTOConfig.Get(),
    };
}
