using CloudAccountsProject.AuditModel;
using CloudAccountsShared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CloudAccountsProjects.Data;

public partial class CloudAccountsDbContext : DbContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CloudAccountsDbContext(DbContextOptions<CloudAccountsDbContext> options, IHttpContextAccessor httpContextAccessor)
        : base(options)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public virtual DbSet<AuditTableMaster> AuditTableMasters { get; set; }

    public virtual DbSet<AuditTableTransaction> AuditTableTransactions { get; set; }

    public virtual DbSet<BusinessFunctionMaster> BusinessFunctionMasters { get; set; }

    public virtual DbSet<CloudAccountsMaster> CloudAccountsMasters { get; set; }

    public virtual DbSet<CloudAccountsTransaction> CloudAccountsTransactions { get; set; }

    public virtual DbSet<CrowdGroupMaster> CrowdGroupMasters { get; set; }

    public virtual DbSet<UserTable> UserTables { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AuditTableMaster>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Audit_Table_Master");

            entity.ToTable("AuditTableMaster");

            entity.Property(e => e.CloudAccountId).HasMaxLength(300);
            entity.Property(e => e.ModifiedByUser).HasMaxLength(250);
            entity.Property(e => e.NewValues).HasColumnType("json");
            entity.Property(e => e.OldValues).HasColumnType("json");
            entity.Property(e => e.PrimaryKey).HasColumnType("json");
            entity.Property(e => e.TableName).HasMaxLength(250);
            entity.Property(e => e.Type).HasMaxLength(200);
        });

        modelBuilder.Entity<AuditTableTransaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Audit_Table_Transaction");

            entity.ToTable("AuditTableTransaction");

            entity.Property(e => e.ModifiedByUser).HasMaxLength(250);
            entity.Property(e => e.NewValues).HasColumnType("json");
            entity.Property(e => e.OldValues).HasColumnType("json");
            entity.Property(e => e.PrimaryKey).HasColumnType("json");
            entity.Property(e => e.TableName).HasMaxLength(250);
            entity.Property(e => e.Type).HasMaxLength(200);
        });

        modelBuilder.Entity<BusinessFunctionMaster>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Business_Function");

            entity.ToTable("BusinessFunctionMaster");

            entity.Property(e => e.BusinessFunctionGroupDl).HasColumnName("BusinessFunctionGroupDL");
            entity.Property(e => e.BusinessFunctionLtMember).HasMaxLength(255);
            entity.Property(e => e.BusinessFunctionName).HasMaxLength(300);
            entity.Property(e => e.BusinessFunctionOwner).HasMaxLength(255);
            entity.Property(e => e.BusinessTagValue).HasMaxLength(255);
        });

        modelBuilder.Entity<CloudAccountsMaster>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Cloud_Accounts");

            entity.ToTable("CloudAccountsMaster");

            entity.Property(e => e.CloudAccountId).HasMaxLength(350);
            entity.Property(e => e.CloudName).HasMaxLength(350);
            entity.Property(e => e.CloudOrgId).HasMaxLength(350);
            entity.Property(e => e.CloudRootAccountId)
                .HasMaxLength(350)
                .HasColumnName("CloudRootAccountID");
            entity.Property(e => e.DeploymentMethod).HasMaxLength(250);
            entity.Property(e => e.Dspmstatus)
                .HasMaxLength(250)
                .HasColumnName("DSPMStatus");
            entity.Property(e => e.IdentityProtectionStatus).HasMaxLength(250);
            entity.Property(e => e.Iomstatus)
                .HasMaxLength(250)
                .HasColumnName("IOMStatus");
            entity.Property(e => e.LastUpdatedAtCrwd).HasColumnName("LastUpdatedAtCRWD");
            entity.Property(e => e.OneClickSensorStatus).HasMaxLength(250);
            entity.Property(e => e.Provider).HasMaxLength(60);
            entity.Property(e => e.RawJson).HasColumnType("json");
            entity.Property(e => e.RealTimeVisibilityAndDetectionStatus).HasMaxLength(250);
            entity.Property(e => e.RegisteredAtCrwd).HasColumnName("RegisteredAtCRWD");
            entity.Property(e => e.RegistrationType).HasMaxLength(300);
            entity.Property(e => e.VulnerabilityScanningStatus).HasMaxLength(250);
        });

        modelBuilder.Entity<CloudAccountsTransaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_CloudAccountManualDetails");

            entity.ToTable("CloudAccountsTransaction");

            entity.Property(e => e.AccountType).HasMaxLength(100);
            entity.Property(e => e.OverallStatus).HasMaxLength(100);

            entity.HasOne(d => d.BusFuncRefNavigation).WithMany(p => p.CloudAccountsTransactions)
                .HasForeignKey(d => d.BusFuncRef)
                .HasConstraintName("FK_CloudAccountManualDetails_BusinessFunction");

            entity.HasOne(d => d.CloudAccRefNavigation).WithMany(p => p.CloudAccountsTransactions)
                .HasForeignKey(d => d.CloudAccRef)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CloudAccountManualDetails_CloudAccounts");
        });

        modelBuilder.Entity<CrowdGroupMaster>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Crowd_Group_Master");

            entity.ToTable("CrowdGroupMaster");

            entity.Property(e => e.AllAccountIds).HasColumnName("AllAccountIDs");
            entity.Property(e => e.CrwdgroupName)
                .HasMaxLength(300)
                .HasColumnName("CRWDGroupName");
            entity.Property(e => e.FilterBy)
                .HasMaxLength(500)
                .HasColumnName("FilterBY");
            entity.Property(e => e.GroupId)
                .HasMaxLength(200)
                .HasColumnName("GroupID");
            entity.Property(e => e.GroupType).HasMaxLength(100);
            entity.Property(e => e.Provider).HasMaxLength(60);

            entity.HasOne(d => d.BusinessFunction).WithMany(p => p.CrowdGroupMasters)
                .HasForeignKey(d => d.BusinessFunctionId)
                .HasConstraintName("FK_CrowdGroupMaster_BusinessFunction");
        });

        modelBuilder.Entity<UserTable>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_User_Table");

            entity.ToTable("UserTable");

            entity.Property(e => e.Username).HasMaxLength(50);
        });


        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);


    private static readonly HashSet<string> IgnoredColumns = new()
    {
        "RawJson"
    };

    private static readonly HashSet<Type> IgnoredTables = new()
    {
        typeof(AuditTableMaster),
        typeof(AuditTableTransaction),
    };

    private static readonly HashSet<Type> MasterTables = new()
    {
        typeof(CloudAccountsMaster),
    };
    private string? GetReference(EntityEntry entry)
    {
        var prop = entry.Metadata.FindProperty("CloudAccountId");
        if (prop != null)
        {
            return entry.Property("CloudAccountId").CurrentValue?.ToString();
        }
        return null;
    }

    private List<AuditEntry> OnBeforeSaveChanges()
    {
        var auditEntries = new List<AuditEntry>();
        foreach (var entry in ChangeTracker.Entries())
        {
            if (IgnoredTables.Any(t => t.IsAssignableFrom(entry.Entity.GetType())) || 
                entry.State == EntityState.Detached || 
                entry.State == EntityState.Unchanged)
            {
                continue;
            }

            var auditEntry = new AuditEntry(entry);
            var context = _httpContextAccessor.HttpContext;
            //Console.WriteLine(context);
            //var isAuth = _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated;
            //Console.WriteLine(isAuth);
            //var claims = _httpContextAccessor.HttpContext?.User?.Claims;

            //foreach (var claim in claims)
            //{
            //    Console.WriteLine($"{claim.Type} : {claim.Value}");
            //}
            auditEntry.TableName = entry.Metadata.GetTableName();
            auditEntry.IsMaster = MasterTables.Any(t => t.IsAssignableFrom(entry.Entity.GetType()));
            auditEntry.Reference = GetReference(entry);
            auditEntry.ModifiedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name;

            auditEntries.Add(auditEntry);
            foreach (var property in entry.Properties)
            {
                string propertyName = property.Metadata.Name;
                if (IgnoredColumns.Contains(propertyName))
                    continue;

                if (property.IsTemporary)
                {
                    auditEntry.TemporaryProperties.Add(property);
                    continue;
                }
                
                if (property.Metadata.IsPrimaryKey())
                {
                    auditEntry.KeyValues[propertyName] = property.CurrentValue;
                    continue;
                }
                switch (entry.State)
                {
                    case EntityState.Added:
                        auditEntry.AuditType = AuditType.Added;
                        auditEntry.NewValues[propertyName] = property.CurrentValue;
                        break;

                    case EntityState.Deleted:
                        auditEntry.AuditType = AuditType.Removed;
                        auditEntry.OldValues[propertyName] = property.OriginalValue;
                        break;

                    case EntityState.Modified:
                        if (property.IsModified)
                        {
                            auditEntry.ChangedColumns.Add(propertyName);
                            auditEntry.AuditType = AuditType.Modified;
                            auditEntry.OldValues[propertyName] = property.OriginalValue;
                            auditEntry.NewValues[propertyName] = property.CurrentValue;
                        }
                        break;
                }
            }
            if (auditEntry.AuditType == AuditType.Modified && !auditEntry.ChangedColumns.Any())
                continue;
        }

        foreach (var auditEntry in auditEntries.Where(_ => !_.HasTemporaryProperties))
        {
            if (auditEntry.IsMaster) AuditTableMasters.Add(auditEntry.ToAuditMaster());
            else AuditTableTransactions.Add(auditEntry.ToAuditTransaction());
        }

        return (auditEntries.Where(_ => _.HasTemporaryProperties).ToList());
    }

    private async Task OnAfterSaveChanges(List<AuditEntry> auditEntries)
    {
        if (auditEntries == null || auditEntries.Count == 0)
            return;

        foreach (var auditEntry in auditEntries)
        {
            foreach (var prop in auditEntry.TemporaryProperties)
            {
                if (prop.Metadata.IsPrimaryKey())
                {
                    auditEntry.KeyValues[prop.Metadata.Name] = prop.CurrentValue;
                }
                else
                {
                    auditEntry.NewValues[prop.Metadata.Name] = prop.CurrentValue;
                }
            }

            if (auditEntry.IsMaster) AuditTableMasters.Add(auditEntry.ToAuditMaster());
            else AuditTableTransactions.Add(auditEntry.ToAuditTransaction());
        }

        await base.SaveChangesAsync();
    }

    public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        ChangeTracker.DetectChanges();
        var auditEntries = OnBeforeSaveChanges();

        var result = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);

        if (auditEntries.Any())
        {
            await OnAfterSaveChanges(auditEntries);
        }

        return result;
    }
}
