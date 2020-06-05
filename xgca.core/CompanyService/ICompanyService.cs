using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using xgca.core.Models.CompanyService;
using xgca.core.Response;

namespace xgca.core.CompanyService
{
    public interface ICompanyService
    {
        Task<IGeneralModel> ListByCompanyId(string key);
        Task<IGeneralModel> ListByCompanyUserId(string key);
        Task<IGeneralModel> Create(CreateCompanyServiceModel obj);
        Task<bool> CreateBatch(dynamic services, int companyId, int userId);
        Task<IGeneralModel> Update(UpdateCompanyServiceModel obj);
        Task<bool> UpdateBatch(dynamic services, int companyId, int userId);
    }
}
