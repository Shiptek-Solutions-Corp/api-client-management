using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using xgca.core.Models.Company;
using xgca.core.Response;

namespace xgca.core.Company
{
    public interface ICompany
    {
        Task<IGeneralModel> List();
        Task<IGeneralModel> Create(CreateCompanyModel obj);
        Task<IGeneralModel> CreateAndReturnId(CreateCompanyModel obj);
        Task<IGeneralModel> InitialRegistration(InitialRegistrationModel obj);
        Task<IGeneralModel> Update(UpdateCompanyModel obj);
        Task<IGeneralModel> Retrieve(string key);
        Task<IGeneralModel> Delete(string key);
        Task<IGeneralModel> GetIdByGuid(string key);
        Task<int> GetIdByGuid(Guid key);
    }
}
