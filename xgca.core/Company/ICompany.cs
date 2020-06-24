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
        Task<IGeneralModel> Create(CreateCompanyModel obj, string createdBy);
        Task<IGeneralModel> CreateAndReturnId(CreateCompanyModel obj);
        Task<IGeneralModel> InitialRegistration(InitialRegistrationModel obj);
        Task<IGeneralModel> Update(UpdateCompanyModel obj);
        Task<IGeneralModel> Update(UpdateCompanyModel obj, string modifiedBy);
        Task<IGeneralModel> Retrieve(int companyId);
        Task<IGeneralModel> Delete(string key);
        Task<IGeneralModel> GetIdByGuid(string key);
        Task<int> GetIdByGuid(Guid key);
        Task<IGeneralModel> ListCompanyLogs(int companyId);
        Task<IGeneralModel> ListCompanyLogs(string companyId);
    }
}
