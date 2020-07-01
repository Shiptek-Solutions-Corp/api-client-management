using Microsoft.EntityFrameworkCore;
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
        Task<entity.Models.CompanyServiceUserRole> Retrieve(int key);
        Task<List<entity.Models.CompanyServiceUserRole>> List();
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

        public async Task<List<entity.Models.CompanyServiceUserRole>> List()
        {
            var companyServiceUserRoles = await _context
                .CompanyServiceUserRoles
                .Include(x => x.CompanyServiceUser)
                .Include(x => x.CompanyServiceRole)
                .AsNoTracking()
                .ToListAsync();

            return companyServiceUserRoles;
        }

        public async Task<entity.Models.CompanyServiceUserRole> Retrieve(int key)
        {
            var moduleGroup = await _context.CompanyServiceUserRoles
                .Include(x => x.CompanyServiceUser)
                .Include(x => x.CompanyServiceRole)
                .SingleOrDefaultAsync(x => x.CompanyServiceUserRoleID == key);

            return moduleGroup;
        }

        public Task<bool> Update(entity.Models.CompanyServiceUserRole obj)
        {
            throw new NotImplementedException();
        }
    }
}
