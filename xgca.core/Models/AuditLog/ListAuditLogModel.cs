namespace xgca.core.Models.AuditLog
{
    public class ListAuditLogModel
    {
        public string AuditLogId { get; set; }
        public string AuditLogAction { get; set; }
        public string CreatedBy { get; set; }
        public string Username { get; set; }
        public string CreatedOn { get; set; }
    }
}
