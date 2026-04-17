using CloudAccountsShared.Models;

namespace CloudAccountsProject.Repositories.Contracts;
public interface ICrowdGroupMasterRepository
{
    Task<List<CrowdGroupMaster>> GetAllAsync();
    Task<CrowdGroupMaster?> GetByIdAsync(int id);
    Task<CrowdGroupMaster> CreateAsync(CrowdGroupMaster group);
    Task<CrowdGroupMaster> UpdateAsync(CrowdGroupMaster group);
    Task DeleteAsync(int id);
}