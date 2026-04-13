using CloudAccountsProject.Data;
using CloudAccountsProject.Models;
using CloudAccountsProject.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace CloudAccountsProject.Repositories;

public class BusinessFunctionRepository : IBusinessFunctionRepository
{
    private readonly CloudAccountsDbContext _context;

    public BusinessFunctionRepository(CloudAccountsDbContext context)
    {
        _context = context;
    }

    public async Task<List<BusinessFunction>> GetAllAsync()
    {
        return await _context.BusinessFunctions
            .OrderBy(x => x.BusinessFunctionName)
            .ToListAsync();
    }

    public async Task<BusinessFunction?> GetByIdAsync(int id)
    {
        return await _context.BusinessFunctions
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task CreateAsync(BusinessFunction item)
    {
        var normalizedName = item.BusinessFunctionName
            .Trim()
            .Replace(" ", "")
            .ToLowerInvariant();

        var exists = await _context.BusinessFunctions.AnyAsync(x =>
            x.BusinessFunctionName
                .ToLower()
                .Replace(" ", "")
                .Trim() == normalizedName);

        if (exists)
            throw new Exception("Business Function Name already exists.");

        item.DateCreated = DateTime.UtcNow;
        item.DateModified = DateTime.UtcNow;

        _context.BusinessFunctions.Add(item);

        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(BusinessFunction item)
    {
        var existing = await _context.BusinessFunctions
            .FirstOrDefaultAsync(x => x.Id == item.Id);

        if (existing == null)
            throw new Exception("Business Function not found.");

        var normalizedName = item.BusinessFunctionName
            .Trim()
            .Replace(" ", "")
            .ToLowerInvariant();

        var duplicate = await _context.BusinessFunctions.AnyAsync(x =>
            x.Id != item.Id &&
            x.BusinessFunctionName
                .ToLower()
                .Replace(" ", "")
                .Trim() == normalizedName);

        if (duplicate)
            throw new Exception("Business Function Name already exists.");

        existing.BusinessFunctionName = item.BusinessFunctionName;
        existing.BusinessFunctionLtMember = item.BusinessFunctionLtMember;
        existing.BusinessFunctionOwner = item.BusinessFunctionOwner;
        existing.BusinessFunctionSpoc = item.BusinessFunctionSpoc;
        existing.BusinessFunctionGroupDl = item.BusinessFunctionGroupDl;
        existing.Remarks = item.Remarks;
        existing.BusinessTagValue = item.BusinessTagValue;
        existing.DateModified = DateTime.UtcNow;

        await _context.SaveChangesAsync();
    }
}