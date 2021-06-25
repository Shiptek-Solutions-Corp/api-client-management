using System.Collections.Generic;
using System.Threading.Tasks;

namespace xgca.data
{
    public interface IRetrievableRepository<T>
    {
        Task<(int, string)> GetIdByGuid(string id);
        Task<(string, string)> GetGuidById(int id);
    }
}
