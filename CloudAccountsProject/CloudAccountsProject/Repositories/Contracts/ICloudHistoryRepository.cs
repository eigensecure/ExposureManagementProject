using CloudAccountsShared.Models.DTOs;

namespace CloudAccountsProject.Repositories.Contracts;

public interface ICloudHistoryRepository
{
    Task<List<AuditHistoryDTO>> GetAuditByAccId(string accId);

    Task<List<AuditHistoryDTO>> GetManAuditByRef(int Id);

    Task<List<AuditHistoryDTO>> GetBusAuditByRef(int Id);
}
