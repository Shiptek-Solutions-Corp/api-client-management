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
using xgca.core.Models.AuditLog;
using xgca.data.AuditLog;
using xgca.data.User;
using xgca.core.Response;

namespace xgca.core.AuditLog
{
    public class AuditLog : IAuditLog
    {
        private readonly IAuditLogData _auditLog;
        private readonly IUserData _user;
        private readonly IGeneral _general;

        public AuditLog(IAuditLogData auditLog, IUserData user, IGeneral general)
        {
            _auditLog = auditLog;
            _user = user;
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

            List<ListAuditLogModel> logs = new List<ListAuditLogModel>();

            foreach (var auditLog in auditLogs)
            {
                var user = await _user.Retrieve(auditLog.CreatedBy);

                logs.Add(new ListAuditLogModel
                {
                    AuditLogId = auditLog.AuditLogId.ToString(),
                    AuditLogAction = auditLog.AuditLogAction,
                    CreatedBy = (auditLog.CreatedBy == 0) ? "System" : String.Concat(user.FirstName, " ", user.LastName),
                    Username = !(user.Username is null) ? (auditLog.CreatedBy == 0 ? "system" : user.Username) : "Not Set",
                    CreatedOn = auditLog.CreatedOn
                });
            }

            return _general.Response(new { Logs = logs }, 200, "Configurable audit logs has been listed", true);
        }

        public async Task<IGeneralModel> RetrieveDetails(string key)
        {
            int logId = await _auditLog.GetIdByGuid(Guid.Parse(key));
            var data = await _auditLog.Retrieve(logId);
            
            string username = "";
            string createdBy = "";
            if (data.CreatedBy == 0)
            {
                username = "system";
                createdBy = "System";
            }
            else
            {
                var user = await _user.Retrieve(data.CreatedBy);
                username = !(user.Username is null) ? user.Username : "Not Set";
                createdBy = String.Concat(user.FirstName, " ", user.LastName);
            }
            
            var log = new
            {
                AuditLogId = data.Guid,
                data.AuditLogAction,
                KeyFieldId = String.Concat(data.TableName,"Id : ", data.KeyFieldId),
                OldValue = !(data.OldValue is null) ? JsonConvert.DeserializeObject(data.OldValue) : null,
                NewValue = JsonConvert.DeserializeObject(data.NewValue),
                CreatedBy = createdBy,
                Username = username,
                data.CreatedOn
            };

            return _general.Response(new { AuditLog = log }, 200, "Audit log details has been displayed", true);
        }
    }
}
