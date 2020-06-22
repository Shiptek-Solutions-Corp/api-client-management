using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using xgca.core.Models.User;
using xgca.core.Response;

namespace xgca.core.User
{
    public interface IUser
    {
        Task<IGeneralModel> List();
        Task<IGeneralModel> List(string columnFilter, string isActive, string isLock);        
        Task<IGeneralModel> Create(CreateUserModel obj, string companyId);
        Task<int> CreateAndReturnId(CreateUserModel obj);
        Task<dynamic> CreateMasterUser(CreateUserModel obj, int createdBy);
        Task<IGeneralModel> Update(UpdateUserModel obj, string modifiedBy);
        Task<IGeneralModel> SetUsername(SetUsernameModel obj);
        Task<IGeneralModel> Retrieve(string key);
        Task<IGeneralModel> RetrieveByUsername(string username);
        Task<IGeneralModel> Delete(string key);
        Task<IGeneralModel> GetIdByGuid(string key);
        Task<int> GetIdByGuid(Guid key);
        Task<int> GetIdByUsername(string username);
    }
}