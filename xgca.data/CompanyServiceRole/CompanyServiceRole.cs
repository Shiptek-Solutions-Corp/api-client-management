using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using xgca.entity;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace xgca.data.CompanyServiceRole
{
    public class CompanyServiceRole : IMaintainable<entity.Models.CompanyServiceRole>, ICompanyServiceRole
    {
        private readonly IXGCAContext _context;

        public CompanyServiceRole(IXGCAContext context)
        {
            _context = context;
        }

        public Task<bool> ChangeStatus(entity.Models.CompanyServiceRole obj)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Create(List<entity.Models.CompanyServiceRole> obj)
        {
            foreach (entity.Models.CompanyServiceRole companyServiceRole in obj)
            {
                await _context.CompanyServiceRoles.AddAsync(companyServiceRole);
            }
            var result = await _context.SaveChangesAsync();
            return result > 0 ? true : false;
        }

        public async Task<bool> Create(entity.Models.CompanyServiceRole obj)
        {
            await _context.CompanyServiceRoles.AddAsync(obj);
            var result = await _context.SaveChangesAsync();
            return result > 0 ? true : false;
        }

        public Task<bool> Delete(int key)
        {
            throw new NotImplementedException();
        }

        public async Task<int> GetIdByGuid(Guid guid)
        {
            var data = await _context.CompanyServiceRoles
                .Where(csr => csr.Guid == guid && csr.IsDeleted == 0)
                .FirstOrDefaultAsync();
            return data.CompanyServiceRoleId;
        }

        public Task<List<entity.Models.CompanyServiceRole>> List()
        {
            throw new NotImplementedException();
        }

        public async Task<List<entity.Models.CompanyServiceRole>> ListByCompanyId(int companyID)
        {
            var data = await _context.CompanyServiceRoles
                .Where(c => c.CompanyServices.CompanyId == companyID)
                .Include(c => c.CompanyServices).ThenInclude(c => c.Companies)
                .ToListAsync();

            return data;
        }

        public async Task<List<entity.Models.CompanyServiceRole>> ListByCompanyServiceId(int companyServiceId)
        {
            var data = await _context.CompanyServiceRoles
                .Include(cs => cs.CompanyServices)
                .Where(csr => csr.CompanyServiceId == companyServiceId && csr.IsDeleted == 0)
                .ToListAsync();
            return data;
        }

        public async Task<entity.Models.CompanyServiceRole> Retrieve(int key)
        {
            var data = await _context.CompanyServiceRoles
                .Where(cs => cs.CompanyServiceRoleId == key && cs.IsDeleted == 0).FirstOrDefaultAsync();
            return data;
        }

        public async Task<int> RetrieveAdministratorId(int key)
        {
            var data = await _context.CompanyServiceRoles
                .Where(csr => csr.CompanyServiceId == key && csr.Name == "Administrator" && csr.IsDeleted == 0)
                .FirstOrDefaultAsync();
            return data.CompanyServiceRoleId;
        }

        public Task<bool> Update(entity.Models.CompanyServiceRole obj)
        {
            throw new NotImplementedException();
        }
    }
}
