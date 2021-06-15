using System.Collections.Generic;
using System.Threading.Tasks;

namespace xgca.data
{
    public interface IRepository<T>
    {
        Task<(T, string)> Create(T obj);
        Task<(T, string)> Get(string id);
        Task<(T, string)> Update(T obj);
        Task<(List<T>, string)> List();
        Task<(bool, string)> Delete(T obj);
    }
}
