using System;
using System.Collections.Generic;
using System.Text;

namespace xgca.core.Models.AuditLog
{
    public class CreatedAuditLog
    {
        public string AuditLogAction { get; set; }
        public string TableName { get; set; }
        public int KeyFieldId { get; set; }
        public string? OldValue { get; set; }
        public string NewValue { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid Guid { get; set; }
    }
}
