using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace xgca.data
{
    public interface IMaintainable<T>
    {
        Task<bool> Create(T obj);
        Task<T> Retrieve(int key);
        Task<bool> Update(T obj);
        Task<List<T>> List();
    }
}
