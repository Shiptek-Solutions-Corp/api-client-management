using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace xgca.data.ContactDetail
{
    public interface IContactDetail
    {
        Task<bool> Create(entity.Models.ContactDetail obj);
        Task<int> CreateAndReturnId(entity.Models.ContactDetail obj);
        Task<List<entity.Models.ContactDetail>> List();
        Task<entity.Models.ContactDetail> Retrieve(int key);
        Task<int> GetIdByGuid(Guid guid);
        Task<bool> Update(entity.Models.ContactDetail obj);
        Task<bool> Delete(int key);
    }
}
