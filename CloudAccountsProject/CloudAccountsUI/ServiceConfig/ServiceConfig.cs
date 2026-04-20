using CloudAccountsShared.Configuration.Services;
using CloudAccountsShared.Configuration.Services.Contracts;
using CloudAccountsUI.Services;
using CloudAccountsUI.Services.Contracts;

namespace CloudAccountsUI.ServiceConfig;

public static class ServicesConfig
{
    public static void AddDIServices(this IServiceCollection Services)
    {
        Services.AddScoped<ICloudHistoryService, CloudHistoryService>();

        Services.AddScoped<IBusinessFunctionService, BusinessFunctionService>();

        Services.AddScoped<ICloudRecordsService, CloudRecordsService>();

        Services.AddScoped<ICrowdGroupMasterService, CrowdGroupMasterService>();

        Services.AddScoped<ITableConfigService, TableConfigService>();
    }
}
