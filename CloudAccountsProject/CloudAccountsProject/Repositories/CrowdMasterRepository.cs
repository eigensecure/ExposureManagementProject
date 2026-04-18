using CloudAccountsProject.Repositories.Contracts;
using CloudAccountsProjects.Data;
using CloudAccountsShared.Models;
using Microsoft.EntityFrameworkCore;

namespace CloudAccountsProject.Repositories
{
    public class CrowdGroupMasterRepository(CloudAccountsDbContext context) : ICrowdGroupMasterRepository
    {
        private readonly CloudAccountsDbContext _context = context;

        public async Task<List<CrowdGroupMaster>> GetAllAsync()
        {
            return await _context.CrowdGroupMasters
                .OrderByDescending(x => x.DateCreated)
                .ToListAsync();
        }

        public async Task<CrowdGroupMaster?> GetByIdAsync(int id)
        {
            return await _context.CrowdGroupMasters
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<CrowdGroupMaster> CreateAsync(CrowdGroupMaster group)
        {
            group.DateCreated = DateTime.UtcNow;
            group.DateModified = DateTime.UtcNow;

            _context.CrowdGroupMasters.Add(group);
            await _context.SaveChangesAsync();

            return group;
        }

        public async Task<CrowdGroupMaster> UpdateAsync(CrowdGroupMaster group)
        {
            var existing = await _context.CrowdGroupMasters
                .FirstOrDefaultAsync(x => x.Id == group.Id);

            if (existing == null)
                throw new Exception("Crowd Group not found");

            existing.CrwdgroupName = group.CrwdgroupName;
            existing.GroupType = group.GroupType;
            existing.GroupId = group.GroupId;
            existing.FilterBy = group.FilterBy;
            existing.BusinessFunctionId = group.BusinessFunctionId;
            existing.Provider = group.Provider;
            existing.Remarks = group.Remarks;
            existing.AllAccountIds = group.AllAccountIds;
            existing.DateModified = DateTime.UtcNow;
            existing.LastsuccessfulDateofapi = group.LastsuccessfulDateofapi;
            existing.CommentsLogs = group.CommentsLogs;

            await _context.SaveChangesAsync();

            return existing;
        }

        public async Task DeleteAsync(int id)
        {
            var existing = await _context.CrowdGroupMasters
                .FirstOrDefaultAsync(x => x.Id == id);

            if (existing == null)
                return;

            _context.CrowdGroupMasters.Remove(existing);
            await _context.SaveChangesAsync();
        }
    }
}