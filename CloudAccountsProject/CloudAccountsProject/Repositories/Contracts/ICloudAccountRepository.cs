using CloudAccountsShared.Models;
using CloudAccountsShared.Models.DTOs;

namespace CloudAccountsProject.Repositories.Contracts;

public interface ICloudAccountRepository
{
    Task<List<CloudAccount>> GetAllAsync();
    Task<List<CloudAccountColumnMetadata>> GetColumnMetadataAsync();
    Task ImportAsync(string provider, string json);
    //Task UpdateAsync(CloudAccount account);
    Task DeleteAsync(int id);
}