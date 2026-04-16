using CloudAccountsShared.Models.DTOs;

namespace CloudAccountsUI.Services.Contracts;

public interface ICloudHistoryService
{
    Task<List<AuditHistoryDTO>> GetAuditByAccId(string accId);
}

