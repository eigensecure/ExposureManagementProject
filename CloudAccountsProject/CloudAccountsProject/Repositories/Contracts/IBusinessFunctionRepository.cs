using CloudAccountsShared.Models;
namespace CloudAccountsProject.Repositories.Contracts;

public interface IBusinessFunctionRepository
{
    Task<List<BusinessFunctionMaster>> GetAllAsync();

    Task<BusinessFunctionMaster?> GetByIdAsync(int id);

    Task CreateAsync(BusinessFunctionMaster item);

    Task UpdateAsync(BusinessFunctionMaster item);
}