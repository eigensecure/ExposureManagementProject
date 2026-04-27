using CloudAccountsProject.Repositories.Contracts;
using CloudAccountsProjects.Data;
using CloudAccountsShared.Models;
using Microsoft.EntityFrameworkCore;

namespace CloudAccountsProject.Repositories;

public class BusinessFunctionRepository(CloudAccountsDbContext context) : IBusinessFunctionRepository
{
    private readonly CloudAccountsDbContext _context = context;

    public async Task<List<BusinessFunctionMaster>> GetAllAsync()
    {
        return await _context.BusinessFunctionMasters
            //.Include(x => x.BusinessTags)
            .OrderBy(x => x.BusinessFunctionName)
            .ToListAsync();
    }

    public async Task<BusinessFunctionMaster?> GetByIdAsync(int id)
    {
        return await _context.BusinessFunctionMasters
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task CreateAsync(BusinessFunctionMaster item)
    {
        var normalizedName = item.BusinessFunctionName
            .Trim()
            .Replace(" ", "")
            .ToLowerInvariant();

        var exists = await _context.BusinessFunctionMasters.AnyAsync(x =>
            x.BusinessFunctionName
                .ToLower()
                .Replace(" ", "")
                .Trim() == normalizedName);

        if (exists)
            throw new Exception("Business Function Name already exists.");

        item.DateCreated = DateTime.UtcNow;
        item.DateModified = DateTime.UtcNow;

        //foreach (var tag in item.BusinessTags)
        //{
        //    tag.DateCreated = DateTime.UtcNow;
        //    tag.DateModified = DateTime.UtcNow;
        //}

        _context.BusinessFunctionMasters.Add(item);

        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(BusinessFunctionMaster item)
    {
        var existing = await _context.BusinessFunctionMasters
            //.Include(x => x.BusinessTags)
            .FirstOrDefaultAsync(x => x.Id == item.Id);

        if (existing == null)
            throw new Exception("Business Function not found.");

        var normalizedName = item.BusinessFunctionName
            .Trim()
            .Replace(" ", "")
            .ToLowerInvariant();

        var duplicate = await _context.BusinessFunctionMasters.AnyAsync(x =>
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
        existing.BusinessTagValue = item.BusinessTagValue;
        existing.Remarks = item.Remarks;
        existing.DateModified = DateTime.UtcNow;

        //_context.BusinessTags.RemoveRange(existing.BusinessTags);

        //existing.BusinessTags = item.BusinessTags.Select(x => new BusinessTag
        //{
        //    TagName = x.TagName,
        //    TagValue = x.TagValue,
        //    DateCreated = DateTime.UtcNow,
        //    DateModified = DateTime.UtcNow
        //}).ToList();

        await _context.SaveChangesAsync();
    }
}