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
using xgca.core.Helpers;
using xgca.core.Response;
using xgca.core.Constants;
using System.ComponentModel.Design;
using Amazon.Runtime.Internal.Transform;
using Microsoft.VisualBasic;

namespace xgca.core.AuditLog
{
    public class AuditLogCore : IAuditLogCore
    {
        private readonly IAuditLogData _auditLog;
        private readonly IAuditLogHelper _auditLogHelper;
        private readonly IUserData _user;
        private readonly IGeneral _general;

        public AuditLogCore(IAuditLogData auditLog, IAuditLogHelper auditLogHelper, IUserData user, IGeneral general)
        {
            _auditLog = auditLog;
            _auditLogHelper = auditLogHelper;
            _user = user;
            _general = general;
        }

        public async Task<IGeneralModel> CreateAuditLog(string auditLogAction, string tableName, int keyFieldId, int createdBy, dynamic oldObj, dynamic newObj = null)
        {
            string createdByName = "";
            if (createdBy == 0)
            { createdByName = "System"; }
            else
            {
                var user = await _user.Retrieve(createdBy);
                createdByName = $"{user.FirstName} {user.LastName}";
            }
            
            var auditLog = new entity.Models.AuditLog
            {
                AuditLogAction = auditLogAction,
                TableName = tableName,
                KeyFieldId = keyFieldId,
                OldValue = oldObj,
                NewValue = newObj,
                CreatedBy = createdBy,
                CreatedByName = createdByName,
                CreatedOn = DateTime.UtcNow,
            };

            var result = await _auditLog.Create(auditLog);
            return result
                ? _general.Response(null, 200, "Audit log data created!", true)
                : _general.Response(null, 400, "Failed in creating audit log data!", false);
        }

        public async Task<IGeneralModel> ListByTableNameAndKeyFieldId(string tableName, int keyFieldId)
        {
            var data = await _auditLog.ListByTableNameAndKeyFieldId(tableName, keyFieldId);

            var auditLogs = data.Select(logs => new
            {
                AuditLogId = logs.Guid,
                logs.AuditLogAction,
                logs.CreatedBy,
                logs.CreatedByName,
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
                    CreatedBy = (auditLog.CreatedBy == 0) ? "System" : auditLog.CreatedByName,
                    Username = !(user.Username is null) ? (auditLog.CreatedBy == 0 ? "system" : user.Username) : "Not Set",
                    CreatedOn = auditLog.CreatedOn.ToString(GlobalVariables.AuditLogTimeFormat)
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
                createdBy = data.CreatedByName;
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
                CratedOn = data.CreatedOn.ToString(GlobalVariables.AuditLogTimeFormat)
            };

            return _general.Response(new { AuditLog = log }, 200, "Audit log details has been displayed", true);
        }
    }
}
