using CloudAccountsProject.Repositories.Contracts;
using CloudAccountsProjects.Data;
using CloudAccountsShared.Models;
using CloudAccountsShared.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Text;

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
                PrimaryKey = x.PrimaryKey,
                ModifiedByUser = x.ModifiedByUser,
                Type = x.Type,
                DateTime = x.DateTime,
                OldValues = x.OldValues,
                NewValues = x.NewValues,
                AffectedColumns = x.AffectedColumns
            })
            .ToListAsync();
    }

    public async Task<List<AuditHistoryDTO>> GetManAuditByRef(int Id)
    {
        var manId = await _context.CloudAccountManualDetails
        .Where(x => x.CloudAccRef == Id)
        .Select(x => x.Id)
        .FirstOrDefaultAsync();

        var auditEntities = await _context.AuditTableTransactions
        .FromSqlRaw(@"
            SELECT * 
            FROM AuditTableTransaction
            WHERE JSON_VALUE(PrimaryKey, '$.Id') = {0} 
              AND TableName = {1}
            ORDER BY DateTime DESC", manId, "CloudAccountManualDetails")
        .ToListAsync();

        return [.. auditEntities.Select(x => new AuditHistoryDTO
        {
            Id = x.Id,
            TableName = x.TableName,
            PrimaryKey = x.PrimaryKey,
            ModifiedByUser = x.ModifiedByUser,
            Type = x.Type,
            DateTime = x.DateTime,
            OldValues = x.OldValues,
            NewValues = x.NewValues,
            AffectedColumns = x.AffectedColumns
        })];
    }

    public async Task<List<AuditHistoryDTO>> GetBusAuditByRef(int Id)
    {
        var auditEntities = await _context.AuditTableTransactions
        .FromSqlRaw(@"
            SELECT * 
            FROM AuditTableTransaction
            WHERE JSON_VALUE(PrimaryKey, '$.Id') = {0} 
              AND TableName = {1}
            ORDER BY DateTime DESC", Id, "BusinessFunction")
        .ToListAsync();

        return [.. auditEntities.Select(x => new AuditHistoryDTO
        {
            Id = x.Id,
            TableName = x.TableName,
            PrimaryKey = x.PrimaryKey,
            ModifiedByUser = x.ModifiedByUser,
            Type = x.Type,
            DateTime = x.DateTime,
            OldValues = x.OldValues,
            NewValues = x.NewValues,
            AffectedColumns = x.AffectedColumns
        })];
    }
}
