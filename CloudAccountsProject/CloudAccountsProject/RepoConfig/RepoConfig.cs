using CloudAccountsProject.Repositories;
using CloudAccountsProject.Repositories.Contracts;

namespace CloudAccountsProject.RepoConfig;

public static class RepoConfig
{
    public static void AddDIServices(this IServiceCollection Services)
    {
        Services.AddScoped<ICloudAccountRepository, CloudAccountRepository>();

        Services.AddScoped<ICloudHistoryRepository, CloudHistoryRepository>();

        Services.AddScoped<IBusinessFunctionRepository, BusinessFunctionRepository>();

        Services.AddScoped<ICloudRecordsRepository, CloudRecordsRepository>();

        Services.AddScoped<ICrowdGroupMasterRepository, CrowdGroupMasterRepository>();

        Services.AddScoped<ICurrentUserService, CurrentUserService>();
    }
}
