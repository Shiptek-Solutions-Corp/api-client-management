using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using xgca.entity.Models;
using xgca.data.AuditLog;
using xgca.core.Response;

namespace xgca.core.AuditLog
{
    public class AuditLog : IAuditLog
    {
        private readonly xgca.data.AuditLog.IAuditLogData _auditLog;
        private readonly IGeneral _general;

        public AuditLog(xgca.data.AuditLog.IAuditLogData auditLog, IGeneral general)
        {
            _auditLog = auditLog;
            _general = general;
        }

        public async Task<IGeneralModel> ListByTableNameAndKeyFieldId(string tableName, int keyFieldId)
        {
            var data = await _auditLog.ListByTableNameAndKeyFieldId(tableName, keyFieldId);

            var auditLogs = data.Select(logs => new
            {
                AuditLogId = logs.Guid,
                logs.AuditLogAction,
                logs.CreatedBy,
                logs.CreatedOn
            });

            return _general.Response(new { Logs = auditLogs }, 200, "Configurable audit logs has been listed", true);
        }

        public async Task<IGeneralModel> RetrieveDetails(string key)
        {
            int logId = await _auditLog.GetIdByGuid(Guid.Parse(key));
            var data = await _auditLog.Retrieve(logId);

            var log = new
            {
                AuditLogId = data.Guid,
                data.AuditLogAction,
                KeyFieldId = String.Concat(data.TableName,"Id : ", data.KeyFieldId),
                OldValue = !(data.OldValue is null) ? JsonConvert.DeserializeObject(data.OldValue) : null,
                NewValue = JsonConvert.DeserializeObject(data.NewValue),
                data.CreatedBy,
                data.CreatedOn
            };

            return _general.Response(new { AuditLog = log }, 200, "Audit log details has been displayed", true);
        }
    }
}
