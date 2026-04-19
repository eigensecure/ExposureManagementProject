using CloudAccountsShared.Models;

namespace CloudAccountsUI.Services.Contracts
{
    public interface IBusinessFunctionService
    {
        Task<List<BusinessFunctionMaster>> GetAllAsync();

        Task CreateAsync(BusinessFunctionMaster item);

        Task UpdateAsync(BusinessFunctionMaster item);

        Task DeleteAsync(int id);
    }
}
