using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudAccountsShared.Models.DTOs
{
    public class CloudAccountColumnMetadata
    {
        public string GenericColumn { get; set; } = string.Empty;
        public string AllColumn { get; set; } = string.Empty;
        public string AwsColumn { get; set; } = string.Empty;
        public string AzureColumn { get; set; } = string.Empty;
        public string GcpColumn { get; set; } = string.Empty;
    }
}
