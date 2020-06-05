using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace xgca.data.CompanyUser
{
    public interface ICompanyUser
    {
        Task<bool> Create(entity.Models.CompanyUser obj);
        Task<int> CreateAndReturnId(entity.Models.CompanyUser obj);
        Task<List<entity.Models.CompanyUser>> List();
        Task<List<entity.Models.CompanyUser>> ListByCompanyId(int companyId);
        Task<entity.Models.CompanyUser> Retrieve(int key);
        Task<int> GetIdByGuid(Guid guid);
        Task<int> GetCompanyIdByUserId(int key);
        Task<bool> Update(entity.Models.CompanyUser obj);
        Task<bool> ChangeStatus(entity.Models.CompanyUser obj);
        Task<bool> Delete(int key);
    }
}
