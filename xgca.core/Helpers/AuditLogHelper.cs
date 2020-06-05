using System;
using System.Collections.Generic;
using System.Text;
using xgca.data.AuditLog;
using Newtonsoft.Json;

namespace xgca.core.Helpers
{
    public class AuditLogHelper
    {
        public static entity.Models.AuditLog BuilCreateLog(dynamic obj, string auditLogAction, string tableName, int keyFieldId)
        {
            var auditLog = new entity.Models.AuditLog
            {
                AuditLogAction = auditLogAction,
                TableName = tableName,
                KeyFieldId = keyFieldId,
                NewValue = JsonConvert.SerializeObject(obj),
                OldValue = null,
                CreatedOn = DateTime.Now,
            };

            return auditLog;
        }

        public static entity.Models.AuditLog BuildUpdateLog(dynamic oldObj, dynamic newObj, string auditLogAction, string tableName, int keyFieldId)
        {
            var auditLog = new entity.Models.AuditLog
            {
                AuditLogAction = auditLogAction,
                TableName = tableName,
                KeyFieldId = keyFieldId,
                NewValue = JsonConvert.SerializeObject(newObj),
                OldValue = JsonConvert.SerializeObject(oldObj),
                CreatedOn = DateTime.Now,
            };

            return auditLog;
        }
    }
}
