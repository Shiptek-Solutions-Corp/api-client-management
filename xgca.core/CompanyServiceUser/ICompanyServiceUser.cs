using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using xgca.core.Models.CompanyServiceUser;
using xgca.core.Response;

namespace xgca.core.CompanyServiceUser
{
    public interface ICompanyServiceUser
    {
        Task<bool> CreateDefault(int companyId, int companyUserId, int createdBy);
        Task<IGeneralModel> ListUserServiceRolesByCompanyId(int companyId);
        Task<IGeneralModel> ListUserServiceRolesByCompanyId(string companyKey);
    }
}
