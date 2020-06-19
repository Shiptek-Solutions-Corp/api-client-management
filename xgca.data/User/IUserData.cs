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
        Task<List<entity.Models.User>> List(string columnFilter, string isActive, string isLock);        
        Task<entity.Models.User> Retrieve(int key);
        Task<entity.Models.User> RetrieveByUsername(string username);
        Task<bool> Update(entity.Models.User obj);
        Task<bool> SetUsername(entity.Models.User obj);
        Task<bool> Delete(int key);
        Task<int> GetIdByGuid(Guid key);
        Task<Guid> GetGuidById(int key);
        Task<int> GetIdByUsername(string username);
        bool UsernameExists(string username);
        Task<bool> EmailAddressExists(string emailAddress, int userId);
    }
}