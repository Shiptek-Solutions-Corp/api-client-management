using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using xgca.core.Response;

namespace xgca.core.AuditLog
{
    public interface IAuditLogCore
    {
        Task<IGeneralModel> ListByTableNameAndKeyFieldId(string tableName, int keyFieldId);

        Task<IGeneralModel> RetrieveDetails(string key);

        Task<IGeneralModel> CreateAuditLog(string auditLogAction, string tableName, int keyFieldId, int createdBy, dynamic oldObj, dynamic newObj = null);
    }
}
