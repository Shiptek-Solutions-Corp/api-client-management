using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace xgca.data.AuditLog
{
    public interface IAuditLogData
    {
        Task<bool> Create(entity.Models.AuditLog obj);
        Task<List<entity.Models.AuditLog>> List();
        Task<entity.Models.AuditLog> Retrieve(int key);
        Task<int> GetIdByGuid(Guid key);
        Task<List<entity.Models.AuditLog>> ListByTableName(string tableName);
        Task<List<entity.Models.AuditLog>> ListByTableNameAndKeyFieldId(string tableName, int keyFieldId);

    }
}
