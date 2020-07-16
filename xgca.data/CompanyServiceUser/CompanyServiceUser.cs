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
        Task<bool> BulkCreate(List<entity.Models.CompanyServiceUser> obj);
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
        Task<List<entity.Models.CompanyUser>> ListUserWithNoDuplicateRole(
            int companyId, 
            int companyServiceRoleId = 0, 
            string groupName = null, 
            int companyServiceId = 0, 
            string fullName = "");
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
            obj.IsActive = 1;
            obj.CreatedBy = 1;
            obj.CreatedOn = DateTime.UtcNow;
            obj.ModifiedBy = 1;
            obj.ModifiedOn = DateTime.UtcNow;
            obj.Guid = Guid.NewGuid();
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

        public async Task<List<entity.Models.CompanyUser>> ListUserWithNoDuplicateRole(
            int companyId, 
            int companyServiceRoleId = 0, 
            string groupName = null, 
            int companyServiceId = 0,
            string fullName = "")
        {
            var data = _context.CompanyUsers;

            var predicate = PredicateBuilder.New<entity.Models.CompanyUser>();

            if (fullName != "")
            {
                //predicate = predicate.And(c => c.Users.FullName.Contains(fullName.ToLower()));
                predicate = predicate.And(c => ((c.Users.FirstName ?? "") + " " + (c.Users.LastName ?? "")).Contains(fullName));
            }
            if (companyServiceId > 1)
            {
                predicate = predicate.And(c => !c.CompanyServiceUsers.Any(c => c.CompanyServiceId == companyServiceId));
            }

            if (companyServiceRoleId > 1)
            {
                predicate = predicate.And(c => c.CompanyServiceUsers.Any(c => c.CompanyServiceRoleId != companyServiceRoleId));
            }

            return await data
                .Where(predicate)
                .Where(c => c.CompanyId == companyId)
                .Where(c => c.Users.IsDeleted == 0)
                .Include(c => c.Users)
                .Include(c => c.CompanyServiceUsers)
                .ToListAsync();
        }

        public async Task<bool> BulkCreate(List<entity.Models.CompanyServiceUser> obj)
        {
            _context.CompanyServiceUsers.BulkInsert(obj);
            var result = await _context.SaveChangesAsync();

            return result > 0 ? true : false;
        }
    }
}
