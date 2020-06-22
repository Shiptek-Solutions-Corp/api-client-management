using System;
using System.Collections.Generic;
using System.Text;
using xgca.data.AuditLog;
using Newtonsoft.Json;

namespace xgca.core.Helpers
{
    public class AuditLogHelper
    {
        public static entity.Models.AuditLog BuildAuditLog(dynamic obj, string auditLogAction, string tableName, int keyFieldId, int createdBy)
        {
            var auditLog = new entity.Models.AuditLog
            {
                AuditLogAction = auditLogAction,
                TableName = tableName,
                KeyFieldId = keyFieldId,
                NewValue = JsonConvert.SerializeObject(obj),
                OldValue = null,
                CreatedBy = createdBy,
                CreatedOn = DateTime.Now,
                Guid = Guid.NewGuid()
            };

            return auditLog;
        }

        public static entity.Models.AuditLog BuildAuditLog(dynamic oldObj, dynamic newObj, string auditLogAction, string tableName, int keyFieldId, int createdBy)
        {
            var auditLog = new entity.Models.AuditLog
            {
                AuditLogAction = auditLogAction,
                TableName = tableName,
                KeyFieldId = keyFieldId,
                NewValue = JsonConvert.SerializeObject(newObj),
                OldValue = JsonConvert.SerializeObject(oldObj),
                CreatedBy = createdBy,
                CreatedOn = DateTime.Now,
                Guid = Guid.NewGuid()
            };

            return auditLog;
        }
    }
}
