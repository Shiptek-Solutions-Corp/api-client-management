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
using DocumentFormat.OpenXml.Office2013.PowerPoint.Roaming;
using xgca.data.CompanyService;
using xgca.data.CompanyServiceRole;
using System.Data;
using ClosedXML.Excel;
using System.IO;

namespace xgca.core.AuditLog
{
    public interface IAuditLogCore
    {
        Task<IGeneralModel> ListByTableNameAndKeyFieldId(string tableName, int keyFieldId);
        Task<IGeneralModel> ListByTableNameAndKeyFieldId(string tableName, string keyFieldId);
        Task<byte[]> DownloadByTableNameAndKeyFieldId(string tableName, string keyFieldId);
        Task<IGeneralModel> RetrieveDetails(string key);
        Task<IGeneralModel> CreateAuditLog(string auditLogAction, string tableName, int keyFieldId, int createdBy, dynamic oldObj, dynamic newObj = null);
        Task<IGeneralModel> GetCompanyServiceRoleLogs(string type, string companyServiceGuid, string keyGuid);
        Task<byte[]> DownloadCompanyServiceRoleLogs(string type, string companyServiceGuid, string keyGuid);
        Task<IGeneralModel> BatchCreateAuditLog(List<CreateAuditLog> obj, int modifiedById);
    }

    public class AuditLogCore : IAuditLogCore
    {
        private readonly IAuditLogData _auditLog;
        private readonly IAuditLogHelper _auditLogHelper;
        private readonly IUserData _user;
        private readonly IGeneral _general;
        private readonly ICompanyService companyService;
        private readonly ICompanyServiceRole companyServiceRole;

        public AuditLogCore(
            IAuditLogData auditLog, 
            IAuditLogHelper auditLogHelper, 
            IUserData user, 
            IGeneral general, 
            ICompanyService companyService,
            ICompanyServiceRole companyServiceRole)
        {
            _auditLog = auditLog;
            _auditLogHelper = auditLogHelper;
            _user = user;
            _general = general;
            this.companyService = companyService;
            this.companyServiceRole = companyServiceRole;
        }

        public async Task<IGeneralModel> BatchCreateAuditLog(List<CreateAuditLog> obj, int createdById)
        {
            string createdByName = "";
            if (createdById == 0)
            { createdByName = "System"; }
            else
            {
                var user = await _user.Retrieve(createdById);
                createdByName = $"{user.FirstName} {user.LastName}";
            }

            List<entity.Models.AuditLog> auditLogs = new List<entity.Models.AuditLog>();
            foreach(var o in obj)
            {
                auditLogs.Add(new entity.Models.AuditLog
                {
                    CreatedBy = createdById,
                    CreatedByName = createdByName,
                    CreatedOn = DateTime.UtcNow,
                    TableName = o.TableName,
                    KeyFieldId = o.KeyFieldId,
                    NewValue = o.NewValue,
                    OldValue = o.OldValue,
                    AuditLogAction = o.AuditLogAction,
                    Guid = Guid.NewGuid()
                });
            }

            var result = await _auditLog.Create(auditLogs);

            return result
                ? _general.Response(null, 200, "Audit logs created successfully", true)
                : _general.Response(null, 400, "Error in creating audit logs,", false);

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
            
            var data = new entity.Models.AuditLog
            {
                AuditLogAction = auditLogAction,
                TableName = tableName,
                KeyFieldId = keyFieldId,
                OldValue = JsonConvert.SerializeObject(oldObj),
                NewValue = JsonConvert.SerializeObject(newObj),
                CreatedBy = createdBy,
                CreatedByName = createdByName,
                CreatedOn = DateTime.UtcNow,
                Guid = Guid.NewGuid()
            };

            var result = await _auditLog.Create(data);
            return result
                ? _general.Response(null, 200, "Audit log data created!", true)
                : _general.Response(null, 400, "Failed in creating audit log data!", false);
        }

        public async Task<byte[]> DownloadByTableNameAndKeyFieldId(string tableName, string keyFieldId)
        {
            var data = await ListByTableNameAndKeyFieldId(tableName, keyFieldId);

            return await GenerateExcelFile(data);
        }

        public async Task<byte[]> DownloadCompanyServiceRoleLogs(string type, string companyServiceGuid, string keyGuid)
        {
            var logs = await GetCompanyServiceRoleLogs(type, companyServiceGuid, keyGuid);

            return await GenerateExcelFile(logs);
        }

        public async Task<IGeneralModel> GetCompanyServiceRoleLogs(string type, string companyServiceGuid, string keyGuid)
        {
            int[] ids = new int[] { };
            int companyServiceRoleId = 0;

            if (!companyServiceGuid.Equals(""))
            {
                ids = await companyService.GetUserByCompanyServiceGuid(Guid.Parse(companyServiceGuid));
            }

            if (!keyGuid.Equals(""))
            {
                companyServiceRoleId = await companyServiceRole.GetIdByGuid(Guid.Parse(keyGuid));
            }

            var result = await _auditLog.GetCompanyServiceRoleLogs(
                type,
                ids,
                companyServiceRoleId);


            List<ListAuditLogModel> logs = new List<ListAuditLogModel>();

            foreach (var d in result)
            {
                var user = await _user.Retrieve(d.CreatedBy);

                logs.Add(new ListAuditLogModel
                {
                    AuditLogId = d.Guid.ToString(),
                    AuditLogAction = d.AuditLogAction,
                    CreatedBy = (d.CreatedBy == 0) ? "System" : d.CreatedByName,
                    Username = !(user.Username is null) ? (d.CreatedBy == 0 ? "system" : user.Username) : "Not Set",
                    CreatedOn = d.CreatedOn.ToString(GlobalVariables.AuditLogTimeFormat),
                    OldValue = d.OldValue,
                    NewValue = d.NewValue
                });
            }

            // TODO: Use automapper to map result
            return _general.Response(new { Logs = logs }, 200, "Success", false);
        }

        public async Task<IGeneralModel> ListByTableNameAndKeyFieldId(string tableName, int keyFieldId)
        {
            var data = await _auditLog.ListByTableNameAndKeyFieldId(tableName, keyFieldId);

            List<ListAuditLogModel> logs = new List<ListAuditLogModel>();

            foreach (var d in data)
            {
                var user = await _user.Retrieve(d.CreatedBy);

                logs.Add(new ListAuditLogModel
                {
                    AuditLogId = d.AuditLogId.ToString(),
                    AuditLogAction = d.AuditLogAction,
                    CreatedBy = (d.CreatedBy == 0) ? "System" : d.CreatedByName,
                    Username = !(user.Username is null) ? (d.CreatedBy == 0 ? "system" : user.Username) : "Not Set",
                    CreatedOn = d.CreatedOn.ToString(GlobalVariables.AuditLogTimeFormat)
                });
            }

            return _general.Response(new { Logs = logs }, 200, "Configurable audit logs has been listed", true);
        }

        public async Task<IGeneralModel> ListByTableNameAndKeyFieldId(string tableName, string keyFieldId)
        {
            int keyId = 0;

            switch (tableName)
            {
                case "User":
                    keyId = await _user.GetIdByGuid(Guid.Parse(keyFieldId));
                    break;
                default:
                    break;
            }

            var data = await _auditLog.ListByTableNameAndKeyFieldId(tableName, keyId);

            List<ListAuditLogModel> logs = new List<ListAuditLogModel>();

            foreach (var d in data)
            {
                var user = await _user.Retrieve(d.CreatedBy);
                string username = (user is null) ? "System" : (user.Username is null ? "Not Set" : user.Username); 

                logs.Add(new ListAuditLogModel
                {
                    AuditLogId = d.Guid.ToString(),
                    AuditLogAction = d.AuditLogAction,
                    CreatedBy = (d.CreatedBy == 0) ? "System" : d.CreatedByName,
                    Username = username,
                    CreatedOn = d.CreatedOn.ToString(GlobalVariables.AuditLogTimeFormat),
                    OldValue = d.OldValue,
                    NewValue = d.NewValue
                });
            }

            return _general.Response(new { Logs = logs }, 200, "Configurable audit logs has been listed", true);
        }

        private async Task<byte[]>  GenerateExcelFile(dynamic logs)
        {

            var table = new DataTable { TableName = "ServiceRates" };
            table.Columns.Add("Action", typeof(string));
            table.Columns.Add("Old Value", typeof(string));
            table.Columns.Add("New Value", typeof(string));
            table.Columns.Add("Created By", typeof(string));
            table.Columns.Add("Created On", typeof(string));
            table.Columns.Add("Username", typeof(string));


            for (int i = 0; i < logs.data?.Logs.Count; i++)
            {
                table.Rows.Add(
                    logs.data?.Logs[i]?.AuditLogAction,
                    logs.data?.Logs[i]?.NewValue,
                    logs.data?.Logs[i]?.OldValue,
                    logs.data?.Logs[i]?.CreatedBy,
                    logs.data?.Logs[i]?.CreatedOn,
                    logs.data?.Logs[i]?.Username
                );
            }

            var wb = new XLWorkbook();
            wb.Worksheets.Add(table);
            await using var memoryStream = new MemoryStream();
            wb.SaveAs(memoryStream);
            return memoryStream.ToArray();
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
                username = (user is null) ? "System" : (user.Username is null ? "Not Set" : user.Username);
                createdBy = data.CreatedByName;
            }

            var log = new DetailsAuditLogModel
            {
                AuditLogId = data.Guid.ToString(),
                AuditLogAction=  data.AuditLogAction,
                KeyFieldId = String.Concat(data.TableName, "Id : ", data.KeyFieldId),
                CreatedBy = createdBy,
                Username = username,
                CreatedOn = data.CreatedOn.ToString(GlobalVariables.AuditLogTimeFormat)
            };

            dynamic oldValue = null;
            dynamic newValue = null;
            if (!(data.OldValue is null))
            {
                if (!(data.OldValue.Contains("{")))
                {
                    log.OldValue = (data.OldValue.Contains("\"")) ? JsonConvert.DeserializeObject(data.OldValue) : JsonConvert.DeserializeObject(JsonConvert.SerializeObject(data.OldValue));
                }
                else
                {
                    log.OldValue = JsonConvert.DeserializeObject(data.OldValue);
                }
            }

            if (!(data.NewValue is null))
            {
                if (!(data.NewValue.Contains("{")))
                {
                    log.NewValue = (data.NewValue.Contains("\"")) ? JsonConvert.DeserializeObject(data.NewValue) : JsonConvert.DeserializeObject(JsonConvert.SerializeObject(data.NewValue));
                }
                else
                {
                    log.NewValue = JsonConvert.DeserializeObject(data.NewValue);
                }
            }

            return _general.Response(new { AuditLog = log }, 200, "Audit log details has been displayed", true);
        }
    }
}
