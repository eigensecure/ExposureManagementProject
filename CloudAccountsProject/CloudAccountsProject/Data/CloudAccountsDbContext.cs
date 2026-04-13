using CloudAccountsProject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;

namespace CloudAccountsProject.Data;

public partial class CloudAccountsDbContext : DbContext
{
    public CloudAccountsDbContext()
    {
    }

    public CloudAccountsDbContext(DbContextOptions<CloudAccountsDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AuditTable> AuditTables { get; set; }

    public virtual DbSet<CloudAccount> CloudAccounts { get; set; }

    public virtual DbSet<CloudAccountManualDetail> CloudAccountManualDetails { get; set; }

    public virtual DbSet<BusinessFunctionMaster> BusinessFunctionMasters { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=testingenvsql.database.windows.net;Initial Catalog=CloudAccountsDB;Persist Security Info=True;User ID=testuser;Password=3F0&TJS72Of!123;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AuditTable>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Audit_Table");

            entity.ToTable("AuditTable");

            entity.Property(e => e.CloudAccountId)
                .HasMaxLength(200)
                .HasColumnName("Cloud_Account_ID");
            entity.Property(e => e.ModifiedByUser).HasMaxLength(200);
            entity.Property(e => e.NewValues).HasColumnType("json");
            entity.Property(e => e.OldValues).HasColumnType("json");
            entity.Property(e => e.PrimaryKey).HasColumnType("json");
            entity.Property(e => e.TableName).HasMaxLength(200);
            entity.Property(e => e.Type).HasMaxLength(200);
        });

        modelBuilder.Entity<CloudAccount>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Cloud_Accounts");

            entity.Property(e => e.CloudAccountId)
                .HasMaxLength(250)
                .HasColumnName("Cloud_Account_ID");
            entity.Property(e => e.CloudName)
                .HasMaxLength(250)
                .HasColumnName("Cloud_Name");
            entity.Property(e => e.CloudOrgId)
                .HasMaxLength(250)
                .HasColumnName("Cloud_ORG_ID");
            entity.Property(e => e.CloudRootAccountId)
                .HasMaxLength(250)
                .HasColumnName("Cloud_RootAccount_ID");
            entity.Property(e => e.DeploymentMethod)
                .HasMaxLength(150)
                .HasColumnName("Deployment_Method");
            entity.Property(e => e.Dspmstatus)
                .HasMaxLength(100)
                .HasColumnName("DSPMStatus");
            entity.Property(e => e.IdentityProtectionStatus).HasMaxLength(100);
            entity.Property(e => e.Iomstatus)
                .HasMaxLength(100)
                .HasColumnName("IOMStatus");
            entity.Property(e => e.LastUpdatedAtCrwd).HasColumnName("Last_Updated_At_CRWD");
            entity.Property(e => e.OneClickSensorStatus).HasMaxLength(100);
            entity.Property(e => e.Provider).HasMaxLength(50);
            entity.Property(e => e.RawJson).HasColumnType("json");
            entity.Property(e => e.RealTimeVisibilityAndDetectionStatus).HasMaxLength(100);
            entity.Property(e => e.RegisteredAtCrwd).HasColumnName("Registered_At_CRWD");
            entity.Property(e => e.RegistrationType)
                .HasMaxLength(150)
                .HasColumnName("Registration_Type");
            entity.Property(e => e.VulnerabilityScanningStatus).HasMaxLength(100);
        });

        modelBuilder.Entity<CloudAccountManualDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CloudAcc__3214EC078C0C72C8");

            entity.HasIndex(e => e.CloudAccountId, "UQ__CloudAcc__B8D5FE8F805CBBBB").IsUnique();

            entity.Property(e => e.AccountType).HasMaxLength(100);
            entity.Property(e => e.AttachmentPath).HasMaxLength(500);
            entity.Property(e => e.BusinessFunctionId).HasMaxLength(255);
            entity.Property(e => e.CloudTagEmail).HasMaxLength(255);
            entity.Property(e => e.DateCreated).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DateModified).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.FirstUpdatedBy).HasMaxLength(255);
            entity.Property(e => e.LastUpdatedBy).HasMaxLength(255);
            entity.Property(e => e.OverallStatus).HasMaxLength(100);

            entity.HasOne(d => d.CloudAccount).WithOne(p => p.CloudAccountManualDetail)
                .HasForeignKey<CloudAccountManualDetail>(d => d.CloudAccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CloudAccountManualDetails_CloudAccounts");
        });

        modelBuilder.Entity<BusinessFunctionMaster>(entity =>
        {
            entity.ToTable("BusinessFunctionMaster");

            entity.HasKey(e => e.Id);

            entity.HasIndex(e => e.BusinessFunctionName)
                .IsUnique()
                .HasDatabaseName("UX_BusinessFunctionMaster_Name");

            entity.HasIndex(e => e.BusinessTagValue)
                .IsUnique()
                .HasDatabaseName("UX_BusinessFunctionMaster_Tag");

            entity.Property(e => e.BusinessFunctionName)
                .HasMaxLength(255);

            entity.Property(e => e.BusinessFunctionLtMember)
                .HasMaxLength(255);

            entity.Property(e => e.BusinessFunctionOwner)
                .HasMaxLength(255);

            entity.Property(e => e.BusinessTagValue)
                .HasMaxLength(255);

            entity.Property(e => e.DateCreated)
                .HasColumnType("datetime2");

            entity.Property(e => e.DateModified)
                .HasColumnType("datetime2");
        });

        OnModelCreatingPartial(modelBuilder);
    }


    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    private List<AuditTable> OnBeforeSaveScim()
    {
        var auditLogs = new List<AuditTable>();

        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.Entity is AuditTable ||
                entry.State == EntityState.Detached ||
                entry.State == EntityState.Unchanged)
            {
                continue;
            }

            var tableName = entry.Metadata.GetTableName() ?? entry.Entity.GetType().Name;

            var primaryKeyDictionary = new Dictionary<string, object?>();

            var primaryKey = entry.Metadata.FindPrimaryKey();

            if (primaryKey != null)
            {
                foreach (var property in primaryKey.Properties)
                {
                    if (property.Name.Equals("RawJson", StringComparison.OrdinalIgnoreCase))
                        continue;
                    var value =
                        entry.Property(property.Name).CurrentValue ??
                        entry.Property(property.Name).OriginalValue;

                    primaryKeyDictionary[property.Name] = value;
                }
            }

            var oldValues = new Dictionary<string, object?>();
            var newValues = new Dictionary<string, object?>();
            var affectedColumns = new List<string>();

            switch (entry.State)
            {
                case EntityState.Added:

                    foreach (var property in entry.CurrentValues.Properties)
                    {
                        if (property.Name.Equals("RawJson", StringComparison.OrdinalIgnoreCase))
                            continue;
                        var value = entry.CurrentValues[property];

                        if (value != null)
                        {
                            newValues[property.Name] = value;
                            affectedColumns.Add(property.Name);
                        }
                    }

                    auditLogs.Add(new AuditTable
                    {
                        TableName = tableName,
                        PrimaryKey = System.Text.Json.JsonSerializer.Serialize(primaryKeyDictionary),
                        CloudAccountId = GetCloudAccountId(entry),
                        Type = "Create",
                        DateTime = DateTime.UtcNow,
                        AffectedColumns = string.Join(",", affectedColumns),
                        OldValues = null,
                        NewValues = System.Text.Json.JsonSerializer.Serialize(newValues)
                    });

                    break;

                case EntityState.Modified:

                    foreach (var property in entry.OriginalValues.Properties)
                    {
                        if (property.Name.Equals("RawJson", StringComparison.OrdinalIgnoreCase))
                            continue;
                        var oldValue = entry.OriginalValues[property];
                        var newValue = entry.CurrentValues[property];

                        if (!Equals(oldValue, newValue))
                        {
                            oldValues[property.Name] = oldValue;
                            newValues[property.Name] = newValue;
                            affectedColumns.Add(property.Name);
                        }
                    }

                    if (affectedColumns.Any())
                    {
                        auditLogs.Add(new AuditTable
                        {
                            TableName = tableName,
                            PrimaryKey = System.Text.Json.JsonSerializer.Serialize(primaryKeyDictionary),
                            CloudAccountId = GetCloudAccountId(entry),
                            Type = "Update",
                            DateTime = DateTime.UtcNow,
                            AffectedColumns = string.Join(",", affectedColumns),
                            OldValues = System.Text.Json.JsonSerializer.Serialize(oldValues),
                            NewValues = System.Text.Json.JsonSerializer.Serialize(newValues)
                        });
                    }

                    break;

                case EntityState.Deleted:

                    foreach (var property in entry.OriginalValues.Properties)
                    {
                        if (property.Name.Equals("RawJson", StringComparison.OrdinalIgnoreCase))
                            continue;
                        var oldValue = entry.OriginalValues[property];

                        oldValues[property.Name] = oldValue;
                        affectedColumns.Add(property.Name);
                    }

                    auditLogs.Add(new AuditTable
                    {
                        TableName = tableName,
                        PrimaryKey = System.Text.Json.JsonSerializer.Serialize(primaryKeyDictionary),
                        CloudAccountId = GetCloudAccountId(entry),
                        Type = "Delete",
                        DateTime = DateTime.UtcNow,
                        AffectedColumns = string.Join(",", affectedColumns),
                        OldValues = System.Text.Json.JsonSerializer.Serialize(oldValues),
                        NewValues = null
                    });

                    break;
            }
        }

        return auditLogs;
    }


    private string? GetCloudAccountId(EntityEntry entry)
    {
        try
        {
            // First try direct property on the entity
            var cloudAccountProperty = entry.Properties.FirstOrDefault(p =>
                string.Equals(p.Metadata.Name, "CloudAccountId", StringComparison.OrdinalIgnoreCase));

            if (cloudAccountProperty != null)
            {
                return cloudAccountProperty.CurrentValue?.ToString()
                       ?? cloudAccountProperty.OriginalValue?.ToString();
            }

            // Fallback: check primary key properties in case CloudAccountId is part of key
            var pk = entry.Metadata.FindPrimaryKey();

            if (pk != null)
            {
                var accountProp = pk.Properties.FirstOrDefault(p =>
                    string.Equals(p.Name, "CloudAccountId", StringComparison.OrdinalIgnoreCase));

                if (accountProp != null)
                {
                    return entry.Property(accountProp.Name).CurrentValue?.ToString()
                           ?? entry.Property(accountProp.Name).OriginalValue?.ToString();
                }
            }

            return null;
        }
        catch
        {
            throw;
        }
    }

    public override async Task<int> SaveChangesAsync(
    CancellationToken cancellationToken = default)
    {
        ChangeTracker.DetectChanges();

        var auditLogs = OnBeforeSaveScim();

        var result = await base.SaveChangesAsync(cancellationToken);

        if (auditLogs.Any())
        {
            foreach (var audit in auditLogs)
            {
                // If PrimaryKey was empty during Add, populate it now
                if (string.IsNullOrWhiteSpace(audit.PrimaryKey) ||
                    audit.PrimaryKey == "{}")
                {
                    var matchingEntry = ChangeTracker.Entries()
                        .FirstOrDefault(e =>
                            e.Entity is not AuditTable &&
                            (e.Metadata.GetTableName() ?? e.Entity.GetType().Name) == audit.TableName);

                    if (matchingEntry != null)
                    {
                        var pk = matchingEntry.Metadata.FindPrimaryKey();

                        if (pk != null)
                        {
                            var pkDict = new Dictionary<string, object?>();

                            foreach (var prop in pk.Properties)
                            {
                                pkDict[prop.Name] =
                                    matchingEntry.Property(prop.Name).CurrentValue;
                            }

                            audit.PrimaryKey =
                                System.Text.Json.JsonSerializer.Serialize(pkDict);
                        }
                    }
                }
            }

            AuditTables.AddRange(auditLogs);

            await base.SaveChangesAsync(cancellationToken);
        }

        return result;
    }

}
