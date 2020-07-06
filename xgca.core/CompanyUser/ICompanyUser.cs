using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using xgca.core.Models.CompanyUser;
using xgca.core.Response;

namespace xgca.core.CompanyUser
{
    public interface ICompanyUser
    {
        Task<IGeneralModel> ListByCompanyId(string key);
        Task<IGeneralModel> ListByCompanyId(int companyId);
        Task<IGeneralModel> Create(CreateCompanyUserModel obj);
        Task<int> CreateDefaultCompanyUser(int companyId, int masterUserId, int createdBy);
        Task<IGeneralModel> Update(UpateCompanyUserModel obj);
        Task<int> GetCompanyIdByUserId(int key);
        Task<IGeneralModel> CreateAndReturnId(CreateCompanyUserModel obj);
        Task<int> GetIdByUserId(int key);

    }
}
