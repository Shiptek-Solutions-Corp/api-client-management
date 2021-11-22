using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace xgca.data
{
    public interface IBulkRepository<T>
    {
        Task<(List<T>, string)> BulkCreate(List<T> list);
        Task<(List<T>, string)> BulkUpdate(List<T> list);
        Task<(bool, string)> BulkDelete(List<string> ids, string username);
    }
}
