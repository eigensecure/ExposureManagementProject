using CloudAccountsShared.Models;
namespace CloudAccountsProject.Repositories.Contracts;

public interface IBusinessFunctionRepository
{
    Task<List<BusinessFunction>> GetAllAsync();

    Task<BusinessFunction?> GetByIdAsync(int id);

    Task CreateAsync(BusinessFunction item);

    Task UpdateAsync(BusinessFunction item);
}