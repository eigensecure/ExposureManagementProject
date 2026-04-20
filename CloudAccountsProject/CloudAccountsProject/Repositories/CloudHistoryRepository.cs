using CloudAccountsProject.Repositories.Contracts;
using CloudAccountsProjects.Data;
using CloudAccountsShared.Models;
using CloudAccountsShared.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace CloudAccountsProject.Repositories;

public class CloudHistoryRepository(CloudAccountsDbContext context) : ICloudHistoryRepository
{
    private readonly CloudAccountsDbContext _context = context;

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
        var manId = await _context.CloudAccountsTransactions
        .Where(x => x.CloudAccRef == Id)
        .Select(x => x.Id)
        .FirstOrDefaultAsync();

        var auditEntities = await _context.AuditTableTransactions
        .FromSqlRaw(@"
            SELECT * 
            FROM AuditTableTransaction
            WHERE JSON_VALUE(PrimaryKey, '$.Id') = {0} 
              AND TableName = {1}
            ORDER BY DateTime DESC",
            manId,
            nameof(CloudAccountsTransaction))
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
            ORDER BY DateTime DESC",
            Id,
            nameof(BusinessFunctionMaster))
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

    public async Task<List<AuditHistoryDTO>> GetCrowdGroupAudit(int Id)
    {
        var auditEntities = await _context.AuditTableTransactions
        .FromSqlRaw(@"
            SELECT * 
            FROM AuditTableTransaction
            WHERE JSON_VALUE(PrimaryKey, '$.Id') = {0} 
              AND TableName = {1}
            ORDER BY DateTime DESC",
            Id,
            nameof(CrowdGroupMaster))
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
