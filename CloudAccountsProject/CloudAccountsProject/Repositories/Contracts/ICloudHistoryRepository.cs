using CloudAccountsShared.Models.DTOs;

namespace CloudAccountsProject.Repositories.Contracts;

public interface ICloudHistoryRepository
{
    Task<List<AuditHistoryDTO>> GetAuditByAccId(string accId);
}
