using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace xgca.data.CompanyServiceUser
{
    public interface ICompanyServiceUser
    {
        Task<bool> Create(List<entity.Models.CompanyServiceUser> obj);
        Task<bool> Create(entity.Models.CompanyServiceUser obj);
        Task<List<entity.Models.CompanyServiceUser>> List();
        Task<List<entity.Models.CompanyServiceUser>> ListByCompanyServiceId(int companyServiceId);
        Task<entity.Models.CompanyServiceUser> Retrieve(int key);
        Task<int> GetIdByGuid(Guid guid);
        Task<bool> Update(entity.Models.CompanyServiceUser obj);
        Task<bool> Delete(int key);
    }
}
