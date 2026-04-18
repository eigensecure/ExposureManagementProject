using CloudAccountsShared.Models;
using CloudAccountsShared.Models.DTOs;

namespace CloudAccountsProject.Repositories.Contracts;

public interface ICloudAccountRepository
{
    Task ImportAsync(string provider, string json);
}