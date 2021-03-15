using System;
using System.Collections.Generic;
using System.Text;

namespace xgca.core.Models.AuditLog
{
    public class DetailsAuditLogModel
    {
        public string AuditLogId { get; set; }
        public string AuditLogAction { get; set; }
        public string KeyFieldId { get; set; }
        public dynamic OldValue { get; set; }
        public dynamic NewValue { get; set; }
        public string CreatedBy { get; set; }
        public string Username { get; set; }
        public string CreatedOn { get; set; }
    }
}
