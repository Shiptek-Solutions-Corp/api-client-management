using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace xgca.data.CompanyService
{
    public interface ICompanyService
    {
        Task<bool> Create(List<entity.Models.CompanyService> obj);
        Task<bool> Create(entity.Models.CompanyService obj);
        Task<List<entity.Models.CompanyService>> List();
        Task<List<entity.Models.CompanyService>> ListByCompanyId(int companyId);
        Task<entity.Models.CompanyService> Retrieve(int key);
        Task<int> GetIdByGuid(Guid guid);
        Task<bool> Update(entity.Models.CompanyService obj);
        Task<bool> Update(List<entity.Models.CompanyService> obj);
        Task<bool> ChangeStatus(entity.Models.CompanyService obj);
        Task<bool> Delete(int key);
    }
}
