using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using xgca.core.Response;

namespace xgca.core.AuditLog
{
    public interface IAuditLog
    {
        Task<IGeneralModel> ListByTableNameAndKeyFieldId(string tableName, int keyFieldId);

        Task<IGeneralModel> RetrieveDetails(string key);
    }
}
