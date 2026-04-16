using CloudAccountsProject.Repositories.Contracts;
using CloudAccountsProjects.Data;
using CloudAccountsShared.Models;
using CloudAccountsShared.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace CloudAccountsProject.Repositories;

public class CloudHistoryRepository : ICloudHistoryRepository
{
    private readonly CloudAccountsDbContext _context;

    public CloudHistoryRepository(CloudAccountsDbContext context)
    {
        _context = context;
    }

    public async Task<List<AuditHistoryDTO>> GetAuditByAccId(string accId)
    {
        return await _context.AuditTableMasters.Where(x => x.CloudAccountId == accId)
            .OrderByDescending(x => x.DateTime)
            .Select(x => new AuditHistoryDTO
            {
                Id = x.Id,
                AuditReference = x.CloudAccountId,
                TableName = x.TableName,
                ModifiedByUser = x.ModifiedByUser,
                Type = x.Type,
                DateTime = x.DateTime,
                OldValues = x.OldValues,
                NewValues = x.NewValues,
                AffectedColumns = x.AffectedColumns
            })
            .ToListAsync();
    }
}
