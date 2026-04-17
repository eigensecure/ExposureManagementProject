using CloudAccountsShared.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace CloudAccountsProject.Data;

public partial class CloudAccountsDbSPContext : DbContext
{
    public CloudAccountsDbSPContext()
    {
    }

    public CloudAccountsDbSPContext(DbContextOptions<CloudAccountsDbSPContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CloudAccountDetailsDTO> CloudAccountDetailsDTOs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CloudAccountDetailsDTO>(entity =>
        entity.HasNoKey());

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}