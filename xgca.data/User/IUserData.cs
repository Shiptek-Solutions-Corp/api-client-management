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
        Task<int> GetTotalActiveUsers();
        Task<int> GetTotalInactiveUsers();
        Task<int> GetTotalLockedUsers();
        Task<int> GetTotalUnlockedUsers();
        Task<int> GetTotalUsers();
        Task<int> GetTotalActiveUsers(List<int> userIds);
        Task<int> GetTotalInactiveUsers(List<int> userIds);
        Task<int> GetTotalLockedUsers(List<int> userIds);
        Task<int> GetTotalUnlockedUsers(List<int> userIds);
        Task<int> GetTotalUsers(List<int> userIds);
        Task<entity.Models.User> Retrieve(int key);
        Task<entity.Models.User> RetrieveByUsername(string username);
        Task<bool> Update(entity.Models.User obj);
        Task<bool> UpdateStatus(entity.Models.User obj);
        Task<bool> UpdateStatus(List<int> userIds, int modifiedBy, byte status);
        Task<bool> UpdateLock(entity.Models.User obj);
        Task<bool> UpdateLock(List<int> userIds, int modifiedBy, byte isLock);
        Task<bool> SetUsername(entity.Models.User obj);
        Task<bool> Delete(int key);
        Task<int> GetIdByGuid(Guid key);
        Task<Guid> GetGuidById(int key);
        Task<int> GetIdByUsername(string username);
        bool UsernameExists(string username);
        Task<bool> EmailAddressExists(string emailAddress);
    }
}