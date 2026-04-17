using CloudAccountsProject.AuditModel;
using CloudAccountsShared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CloudAccountsProjects.Data;

public partial class CloudAccountsDbContext : DbContext
{
    public CloudAccountsDbContext()
    {
    }

    public CloudAccountsDbContext(DbContextOptions<CloudAccountsDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AuditTableMaster> AuditTableMasters { get; set; }

    public virtual DbSet<AuditTableTransaction> AuditTableTransactions { get; set; }

    public virtual DbSet<BusinessFunction> BusinessFunctions { get; set; }

    public virtual DbSet<CloudAccount> CloudAccounts { get; set; }

    public virtual DbSet<CloudAccountManualDetail> CloudAccountManualDetails { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AuditTableMaster>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Audit_Table_Master");

            entity.ToTable("AuditTableMaster");

            entity.Property(e => e.CloudAccountId).HasMaxLength(200);
            entity.Property(e => e.ModifiedByUser).HasMaxLength(200);
            entity.Property(e => e.NewValues).HasColumnType("json");
            entity.Property(e => e.OldValues).HasColumnType("json");
            entity.Property(e => e.PrimaryKey).HasColumnType("json");
            entity.Property(e => e.TableName).HasMaxLength(200);
            entity.Property(e => e.Type).HasMaxLength(200);
        });

        modelBuilder.Entity<AuditTableTransaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Audit_Table_Transaction");

            entity.ToTable("AuditTableTransaction");

            entity.Property(e => e.ModifiedByUser).HasMaxLength(200);
            entity.Property(e => e.NewValues).HasColumnType("json");
            entity.Property(e => e.OldValues).HasColumnType("json");
            entity.Property(e => e.PrimaryKey).HasColumnType("json");
            entity.Property(e => e.TableName).HasMaxLength(200);
            entity.Property(e => e.Type).HasMaxLength(200);
        });

        modelBuilder.Entity<BusinessFunction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Business_Function");

            entity.ToTable("BusinessFunction");

            entity.Property(e => e.BusinessFunctionGroupDl).HasColumnName("BusinessFunctionGroupDL");
            entity.Property(e => e.BusinessFunctionLtMember).HasMaxLength(255);
            entity.Property(e => e.BusinessFunctionName).HasMaxLength(255);
            entity.Property(e => e.BusinessFunctionOwner).HasMaxLength(255);
            entity.Property(e => e.BusinessTagValue).HasMaxLength(255);
        });

        modelBuilder.Entity<CloudAccount>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Cloud_Accounts");

            entity.Property(e => e.CloudAccountId).HasMaxLength(250);
            entity.Property(e => e.CloudName).HasMaxLength(250);
            entity.Property(e => e.CloudOrgId).HasMaxLength(250);
            entity.Property(e => e.CloudRootAccountId)
                .HasMaxLength(250)
                .HasColumnName("CloudRootAccountID");
            entity.Property(e => e.DeploymentMethod).HasMaxLength(150);
            entity.Property(e => e.Dspmstatus)
                .HasMaxLength(100)
                .HasColumnName("DSPMStatus");
            entity.Property(e => e.IdentityProtectionStatus).HasMaxLength(100);
            entity.Property(e => e.Iomstatus)
                .HasMaxLength(100)
                .HasColumnName("IOMStatus");
            entity.Property(e => e.LastUpdatedAtCrwd).HasColumnName("Last_UpdatedAtCRWD");
            entity.Property(e => e.OneClickSensorStatus).HasMaxLength(100);
            entity.Property(e => e.Provider).HasMaxLength(50);
            entity.Property(e => e.RawJson).HasColumnType("json");
            entity.Property(e => e.RealTimeVisibilityAndDetectionStatus).HasMaxLength(100);
            entity.Property(e => e.RegisteredAtCrwd).HasColumnName("RegisteredAtCRWD");
            entity.Property(e => e.RegistrationType).HasMaxLength(150);
            entity.Property(e => e.VulnerabilityScanningStatus).HasMaxLength(100);
        });

        modelBuilder.Entity<CloudAccountManualDetail>(entity =>
        {
            entity.Property(e => e.AccountType).HasMaxLength(100);
            entity.Property(e => e.AttachmentPath).HasMaxLength(500);
            entity.Property(e => e.CloudTagEmail).HasMaxLength(255);
            entity.Property(e => e.OverallStatus).HasMaxLength(100);

            entity.HasOne(d => d.BusFuncRefNavigation).WithMany(p => p.CloudAccountManualDetails)
                .HasForeignKey(d => d.BusFuncRef)
                .HasConstraintName("FK_CloudAccountManualDetails_BusinessFunction");

            entity.HasOne(d => d.CloudAccRefNavigation).WithMany(p => p.CloudAccountManualDetails)
                .HasForeignKey(d => d.CloudAccRef)
                .HasConstraintName("FK_CloudAccountManualDetails_CloudAccounts");
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
        typeof(CloudAccount),
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
        //ChangeTracker.DetectChanges();
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
            auditEntry.TableName = entry.Metadata.GetTableName();
            auditEntry.IsMaster = MasterTables.Any(t => t.IsAssignableFrom(entry.Entity.GetType()));
            auditEntry.Reference = GetReference(entry);

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

        // Save audit entities that have all the modifications
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
            // Get the final value of the temporary properties
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
