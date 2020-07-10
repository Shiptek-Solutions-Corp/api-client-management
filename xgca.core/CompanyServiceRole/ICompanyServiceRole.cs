using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using xgca.core.Models.CompanyServiceRole;
using xgca.core.Response;

namespace xgca.core.CompanyServiceRole
{
    public interface ICompanyServiceRole
    {
        Task<IGeneralModel> Create(CreateCompanyServiceRoleModel obj);
        Task<IGeneralModel> CreateDefault(int companyId, int userId);
        Task<IGeneralModel> ListByCompanyServiceId(string key);
        Task<IGeneralModel> ListByCompany(string key);
        Task<IGeneralModel> Show(Guid companyServiceRoleId);
        Task<IGeneralModel> Update(UpdateCompanyServiceRoleModel updateCompanyServiceRoleModel, Guid companyServiceRoleId);
        Task<IGeneralModel> CreateWithCompanyService(CreateCompanyServiceRoleWithCompanyService obj);
    }
}
