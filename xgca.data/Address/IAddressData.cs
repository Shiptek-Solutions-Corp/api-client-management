using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace xgca.data.Address
{
    public interface IAddressData
    {
        Task<bool> Create(entity.Models.Address obj);
        Task<int> CreateAndReturnId(entity.Models.Address obj);
        Task<List<entity.Models.Address>> List();
        Task<entity.Models.Address> Retrieve(int key);
        Task<int> GetIdByGuid(Guid guid);
        Task<bool> Update(entity.Models.Address obj);
        Task<bool> Delete(int key);
    }
}
