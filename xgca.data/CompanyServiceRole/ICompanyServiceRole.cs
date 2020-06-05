using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace xgca.data.CompanyServiceRole
{
    public interface ICompanyServiceRole
    {
        Task<bool> Create(List<entity.Models.CompanyServiceRole> obj);
        Task<List<entity.Models.CompanyServiceRole>> List();
        Task<List<entity.Models.CompanyServiceRole>> ListByCompanyServiceId(int companyServiceId);
        Task<entity.Models.CompanyServiceRole> Retrieve(int key);
        Task<int> RetrieveAdministratorId(int key);
        Task<int> GetIdByGuid(Guid guid);
        Task<bool> Update(entity.Models.CompanyServiceRole obj);
        Task<bool> ChangeStatus(entity.Models.CompanyServiceRole obj);
        Task<bool> Delete(int key);
    }
}
