using CloudAccountsUI.Services;
using CloudAccountsUI.Services.Contracts;

namespace CloudAccountsUI.ServiceConfig;

public static class ServicesConfig
{
    public static void AddDIServices(this IServiceCollection Services)
    {
        Services.AddScoped<ICloudHistoryService, CloudHistoryService>();
    }
}
