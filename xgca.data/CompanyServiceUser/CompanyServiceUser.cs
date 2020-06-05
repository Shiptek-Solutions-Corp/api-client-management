using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using xgca.entity;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace xgca.data.CompanyServiceUser
{
    public class CompanyServiceUser : IMaintainable<entity.Models.CompanyServiceUser>, ICompanyServiceUser
    {
        private readonly IXGCAContext _context;

        public CompanyServiceUser(IXGCAContext context)
        {
            _context = context;
        }

        public async Task<bool> Create(List<entity.Models.CompanyServiceUser> obj)
        {
            foreach (entity.Models.CompanyServiceUser companyServiceUser in obj)
            {
                await _context.CompanyServiceUsers.AddAsync(companyServiceUser);
            }
            var result = await _context.SaveChangesAsync();
            return result > 0 ? true : false;
        }

        public async Task<bool> Create(entity.Models.CompanyServiceUser obj)
        {
            await _context.CompanyServiceUsers.AddAsync(obj);
            var result = await _context.SaveChangesAsync();
            return result > 0 ? true : false;
        }

        public Task<bool> Delete(int key)
        {
            throw new NotImplementedException();
        }

        public async Task<int> GetIdByGuid(Guid guid)
        {
            var data = await _context.CompanyServiceUsers
                .Where(csu => csu.Guid == guid && csu.IsDeleted == 0)
                .FirstOrDefaultAsync();
            return data.CompanyServiceUserId;
        }

        public async Task<List<entity.Models.CompanyServiceUser>> List()
        {
            var data = await _context.CompanyServiceUsers
                .Include(cr => cr.CompanyServiceRoles)
                    .ThenInclude(cs => cs.CompanyServices)
                    .ThenInclude(c => c.Companies)
                .Include(cs => cs.CompanyServices)
                .Include(cu => cu.CompanyUsers)
                    .ThenInclude(u => u.Users)
                .Where(csu => csu.IsDeleted == 0)
                .ToListAsync();
            return data;
        }

        public async Task<List<entity.Models.CompanyServiceUser>> ListByCompanyServiceId(int companyServiceId)
        {
            var data = await _context.CompanyServiceUsers
                .Include(cr => cr.CompanyServiceRoles)
                    .ThenInclude(cs => cs.CompanyServices)
                    .ThenInclude(c => c.Companies)
                .Include(cs => cs.CompanyServices)
                .Include(cu => cu.CompanyUsers)
                    .ThenInclude(u => u.Users)
                .Where(csu => csu.CompanyServiceId == companyServiceId && csu.IsDeleted == 0)
                .ToListAsync();
            return data;
        }

        public Task<entity.Models.CompanyServiceUser> Retrieve(int key)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(entity.Models.CompanyServiceUser obj)
        {
            throw new NotImplementedException();
        }
    }
}
