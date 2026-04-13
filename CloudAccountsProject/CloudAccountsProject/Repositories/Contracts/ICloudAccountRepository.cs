using CloudAccountsProject.Models;

namespace CloudAccountsProject.Repositories.Contracts;

public interface ICloudAccountRepository
{
    Task<List<CloudAccount>> GetAllAsync();
    Task ImportAsync(string provider, string json);
    Task UpdateAsync(CloudAccount account);
    Task DeleteAsync(int id);
}