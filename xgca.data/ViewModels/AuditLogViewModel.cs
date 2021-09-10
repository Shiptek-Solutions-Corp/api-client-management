using System;
using System.Collections.Generic;
using System.Text;

namespace xgca.data.ViewModels
{
    public class AuditLogViewModel
    {
        public DateTime CreatedOn { get; set; }
        public string Module { get; set; }
        public string SubModule { get; set; }
        public string AuditLogAction { get; set; }
        public string Description { get; set; }
        public string Username { get; set; }
    }

    public class GetAuditLogListingViewModel : AuditLogViewModel
    {
        public string NewValue { get; set; }
        public string OldValue { get; set; }
    }
    public class ExportAuditLogData
    {
        public int No { get; set; }
        public string DateTime { get; set; }
        public string Module { get; set; }
        public string SubModule { get; set; }
        public string Action { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
    }
}
