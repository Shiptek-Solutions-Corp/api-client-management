using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace xgca.data.AddressType
{
    public interface IAddressType
    {
        Task<bool> Create(entity.Models.AddressType obj);
        Task<List<entity.Models.AddressType>> List();
        Task<entity.Models.AddressType> Retrieve(int key);
        Task<int> RetrieveIdByName(string key);
        Task<int> GetIdByGuid(Guid guid);
        Task<bool> Update(entity.Models.AddressType obj);
        Task<bool> Delete(int key);
    }
}
