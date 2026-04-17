using CloudAccountsShared.Models;

namespace CloudAccountsUI.Services.Contracts
{
    public interface IBusinessFunctionService
    {
        Task<List<BusinessFunction>> GetAllAsync();

        Task CreateAsync(BusinessFunction item);

        Task UpdateAsync(BusinessFunction item);

        Task DeleteAsync(int id);
    }
}
