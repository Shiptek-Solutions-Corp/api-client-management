using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace xgca.data.User
{
    public interface IUserData
    {
        Task<bool> Create(entity.Models.User obj);
        Task<int> CreateAndReturnId(entity.Models.User obj);
        Task<List<entity.Models.User>> List();
        Task<entity.Models.User> Retrieve(int key);
        Task<bool> Update(entity.Models.User obj);
        Task<bool> Delete(int key);
        Task<int> GetIdByGuid(Guid key);
        Task<Guid> GetGuidById(int key);
    }
}