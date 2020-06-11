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
        Task<IGeneralModel> Create(CreateUserModel obj);
        Task<int> CreateAndReturnId(CreateUserModel obj);
        Task<dynamic> CreateMasterUser(CreateUserModel obj, int createdBy);
        Task<IGeneralModel> Update(UpdateUserModel obj);
        Task<IGeneralModel> SetUsername(SetUsernameModel obj);
        Task<IGeneralModel> Retrieve(string key);
        Task<IGeneralModel> Delete(string key);
        Task<IGeneralModel> GetIdByGuid(string key);
        Task<int> GetIdByGuid(Guid key);
    }
}