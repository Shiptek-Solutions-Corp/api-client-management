using System;
using System.Collections.Generic;
using System.Text;
using xgca.data.AuditLog;
using Newtonsoft.Json;

namespace xgca.core.Helpers
{
    public interface IAuditLogHelper
    {
        entity.Models.AuditLog BuildAuditLog(string auditLogAction, string tableName, int keyFieldId, int createdBy, dynamic oldObj, dynamic newObj = null);
    }
    public class AuditLogHelper : IAuditLogHelper
    {

        public entity.Models.AuditLog BuildAuditLog(string auditLogAction, string tableName, int keyFieldId, int createdBy, dynamic oldObj, dynamic newObj = null)
        {
            var auditLog = new entity.Models.AuditLog
            {
                AuditLogAction = auditLogAction,
                TableName = tableName,
                KeyFieldId = keyFieldId,
                NewValue = JsonConvert.SerializeObject(newObj),
                OldValue = JsonConvert.SerializeObject(oldObj),
                CreatedBy = createdBy,
                CreatedOn = DateTime.UtcNow,
                Guid = Guid.NewGuid()
            };

            return auditLog;
        }
    }
}
