using CloudAccountsShared.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;

namespace CloudAccountsProject.AuditModel;

public class AuditEntry
{
    public AuditEntry(EntityEntry entry)
    {
        Entry = entry;
    }
    public EntityEntry Entry { get; }
    public string UserId { get; set; }
    public string TableName { get; set; }
    public string Reference { get; set; }
    public Dictionary<string, object> KeyValues { get; } = new Dictionary<string, object>();
    public Dictionary<string, object> OldValues { get; } = new Dictionary<string, object>();
    public Dictionary<string, object> NewValues { get; } = new Dictionary<string, object>();
    public AuditType AuditType { get; set; }
    public List<string> ChangedColumns { get; } = new List<string>();

    public bool IsMaster { get; set; }

    public List<PropertyEntry> TemporaryProperties { get; } = new List<PropertyEntry>();

    public bool HasTemporaryProperties => TemporaryProperties.Any();

    public AuditTableMaster ToAuditMaster()
    {
        var auditMaster = new AuditTableMaster();

        auditMaster.PrimaryKey = JsonConvert.SerializeObject(KeyValues);
        auditMaster.TableName = TableName;
        auditMaster.CloudAccountId = Reference;
        auditMaster.DateTime = DateTime.UtcNow;     
        auditMaster.OldValues = OldValues.Count == 0 ? null : JsonConvert.SerializeObject(OldValues);
        auditMaster.NewValues = NewValues.Count == 0 ? null : JsonConvert.SerializeObject(NewValues);
        auditMaster.Type = AuditType.ToString();
        auditMaster.AffectedColumns = ChangedColumns.Count == 0 ? null : JsonConvert.SerializeObject(ChangedColumns);
        return auditMaster;
    }

    public AuditTableTransaction ToAuditTransaction()
    {
        var auditTransaction = new AuditTableTransaction();

        auditTransaction.PrimaryKey = JsonConvert.SerializeObject(KeyValues);
        auditTransaction.TableName = TableName;
        auditTransaction.Reference = Reference;
        auditTransaction.DateTime = DateTime.UtcNow;
        auditTransaction.OldValues = OldValues.Count == 0 ? null : JsonConvert.SerializeObject(OldValues);
        auditTransaction.NewValues = NewValues.Count == 0 ? null : JsonConvert.SerializeObject(NewValues);
        auditTransaction.Type = AuditType.ToString();
        auditTransaction.AffectedColumns = ChangedColumns.Count == 0 ? null : JsonConvert.SerializeObject(ChangedColumns);
        return auditTransaction;
    }
}

public enum AuditType
{
    None = 0,
    Added = 1,
    Modified = 2,
    Removed = 3
}