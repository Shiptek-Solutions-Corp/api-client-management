using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace xgca.data.Company
{
    public interface ICompanyData
    {
        Task<bool> Create(entity.Models.Company obj);
        Task<int> CreateAndReturnId(entity.Models.Company obj);
        Task<List<entity.Models.Company>> List();
        Task<entity.Models.Company> Retrieve(int key);
        Task<int> GetIdByGuid(Guid guid);
        Task<string> GetGuidById(int id);
        Task<bool> Update(entity.Models.Company obj);
        Task<bool> Delete(int key);
    }
}
