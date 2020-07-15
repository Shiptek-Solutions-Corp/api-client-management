using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using xgca.entity;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;
using LinqKit;

namespace xgca.data.CompanyServiceUser
{
    public interface ICompanyServiceUser
    {
        Task<bool> Create(List<entity.Models.CompanyServiceUser> obj);
        Task<bool> Create(entity.Models.CompanyServiceUser obj);
        Task<List<entity.Models.CompanyServiceUser>> List();
        Task<List<entity.Models.CompanyServiceUser>> ListByCompanyServiceId(int companyServiceId);
        Task<List<entity.Models.CompanyServiceUser>> ListByCompanyId(int companyId);
        Task<List<entity.Models.CompanyServiceUser>> ListByCompanyUserId(int companyUserId);
        Task<entity.Models.CompanyServiceUser> Retrieve(int companyServiceUserId);
        Task<entity.Models.CompanyServiceUser> Retrieve(int companyUserId, int companyServiceId);
        Task<int> GetIdByGuid(Guid guid);
        Task<bool> Update(entity.Models.CompanyServiceUser obj);
        Task<bool> UpdateServiceAndRole(int companyServiceUserId, int companyServiceId, int companyServiceRoleId, int modifiedById);
        Task<bool> Delete(int key);
        Task<List<entity.Models.CompanyServiceUser>> ListUserWithNoDuplicateRole(int companyId, int companyServiceRoleId = 0, string groupName = null, int companyServiceId = 0);
    }
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

        public async Task<List<entity.Models.CompanyServiceUser>> ListByCompanyId(int companyId)
        {
            var companyServiceUsers = await _context.CompanyServiceUsers
                .Include(cs => cs.CompanyServices)
                .Include(cu => cu.CompanyUsers)
                    .ThenInclude(u => u.Users)
                .Include(csr => csr.CompanyServiceRoles)
                .Where(csu => csu.CompanyId == companyId)
                .ToListAsync();

            return companyServiceUsers;
        }

        public async Task<List<entity.Models.CompanyServiceUser>> ListByCompanyUserId(int companyUserId)
        {
            var companyServiceUsers = await _context.CompanyServiceUsers
                .Include(cs => cs.CompanyServices)
                .Include(cu => cu.CompanyUsers)
                    .ThenInclude(u => u.Users)
                .Include(csr => csr.CompanyServiceRoles)
                .Where(csu => csu.CompanyUserId == companyUserId)
                .ToListAsync();

            return companyServiceUsers;
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

        public async Task<entity.Models.CompanyServiceUser> Retrieve(int companyServiceUserId)
        {
            var data = await _context.CompanyServiceUsers
                .Include(csr => csr.CompanyServiceRoles)
                .Where(csu => csu.CompanyServiceUserId == companyServiceUserId)
                .FirstOrDefaultAsync();
            return data;
        }

        public async Task<entity.Models.CompanyServiceUser> Retrieve(int companyUserId, int companyServiceId)
        {
            var data = await _context.CompanyServiceUsers
                .Include(csr => csr.CompanyServiceRoles)
                .Where(csu => csu.CompanyUserId == companyUserId && csu.CompanyServiceId == companyServiceId)
                .FirstOrDefaultAsync();
            return data;
        }

        public Task<bool> Update(entity.Models.CompanyServiceUser obj)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateServiceAndRole(int companyServiceUserId, int companyServiceId, int companyServiceRoleId, int modifiedById)
        {
            var data = await _context.CompanyServiceUsers.Where(u => u.CompanyServiceId == companyServiceUserId).FirstOrDefaultAsync();
            if (data == null)
            {
                return false;
            }

            data.CompanyServiceId = companyServiceId;
            data.CompanyServiceRoleId = companyServiceRoleId;
            data.ModifiedBy = modifiedById;
            data.ModifiedOn = DateTime.UtcNow;
            var result = await _context.SaveChangesAsync();

            return result > 0 ? true : false;
        }

        public async Task<List<entity.Models.CompanyServiceUser>> ListUserWithNoDuplicateRole(int companyId, int companyServiceRoleId = 0, string groupName = null, int companyServiceId = 0)
        {
            var data = _context.CompanyServiceUsers;
            var predicate = PredicateBuilder.New<entity.Models.CompanyServiceUser>();

            if (companyServiceId > 1)
            {
                predicate = predicate.And(c => c.CompanyServiceId == companyServiceId);
            }

            if (groupName != null && groupName != "")
            {
                predicate = predicate.And(c => c.CompanyServiceRoles.Name != groupName);
            }

            if (companyServiceRoleId > 1)
            {
                predicate = predicate.And(c => c.CompanyServiceRoleId != companyServiceRoleId);
            }

            return await data
                .Where(predicate)
                .Where(c => c.CompanyUsers.CompanyId == companyId)
                .Where(c => c.CompanyUsers.Users.IsDeleted == 0)
                .Include(c => c.CompanyServices)
                .Include(c => c.CompanyUsers).ThenInclude(c => c.Users)
                .ToListAsync();
        }
    }
}
