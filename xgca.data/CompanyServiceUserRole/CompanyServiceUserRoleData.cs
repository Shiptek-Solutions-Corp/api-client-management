using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using xgca.entity;

namespace xgca.data.CompanyServiceUserRole
{
    public interface ICompanyServiceUserRoleData
    {
        Task<bool> Create(entity.Models.CompanyServiceUserRole obj);
    }
    public class CompanyServiceUserRoleData : ICompanyServiceUserRoleData, IMaintainable<entity.Models.CompanyServiceUserRole>
    {
        private readonly IXGCAContext _context;
        public CompanyServiceUserRoleData(IXGCAContext context)
        {
            _context = context;
        }

        public async Task<bool> Create(entity.Models.CompanyServiceUserRole obj)
        {
            await _context.CompanyServiceUserRoles.AddAsync(obj);
            var result = await _context.SaveChangesAuditable();
            return result > 0 ? true : false;
        }

        public Task<List<entity.Models.CompanyServiceUserRole>> List()
        {
            throw new NotImplementedException();
        }

        public Task<entity.Models.CompanyServiceUserRole> Retrieve(int key)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(entity.Models.CompanyServiceUserRole obj)
        {
            throw new NotImplementedException();
        }
    }
}
