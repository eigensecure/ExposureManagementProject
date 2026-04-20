using CloudAccountsShared.Models.DTOs;

namespace CloudAccountsUI.Services.Contracts;

public interface ICloudHistoryService
{
    Task<List<AuditHistoryDTO>> GetAuditByAccId(string accId);

    Task<List<AuditHistoryDTO>> GetManAuditByRef(int Id);

    Task<List<AuditHistoryDTO>> GetBusAuditByRef(int Id);

    Task<List<AuditHistoryDTO>> GetCrowdGroupAudit(int Id);
}

