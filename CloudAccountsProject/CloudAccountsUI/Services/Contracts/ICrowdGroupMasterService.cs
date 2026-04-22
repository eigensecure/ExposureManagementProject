using CloudAccountsShared.Models;

namespace CloudAccountsUI.Services.Contracts;
public interface ICrowdGroupMasterService
{
    Task<List<CrowdGroupMaster>> GetAllAsync();
    Task<CrowdGroupMaster?> GetByIdAsync(int id);
    Task<CrowdGroupMaster> CreateAsync(CrowdGroupMaster group);
    Task<CrowdGroupMaster> UpdateAsync(CrowdGroupMaster group);
    Task<string> GetAllAccountIdsAsync(int businessFunctionId, string provider);
    Task PatchGroupAsync(int id);
    Task DeleteAsync(int id);
}
