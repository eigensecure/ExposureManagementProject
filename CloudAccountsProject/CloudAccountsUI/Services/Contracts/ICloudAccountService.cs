using CloudAccountsShared.Models;
using CloudAccountsShared.Models.DTOs;

namespace CloudAccountsUI.Services.Contracts
{
    public interface ICloudAccountService
    {
        Task<(List<CloudAccount> Accounts, List<CloudAccountColumnMetadata> Metadata)> GetCloudAccountsDataAsync();
        //Task<List<CloudAccount>> GetAllAsync();

        //Task<List<CloudAccountColumnMetadata>> GetColumnMetadataAsync();

        Task<List<BusinessFunction>> GetBusinessFunctionsAsync();

        Task UpdateAsync(int id, CloudAccount account);
    }
}


