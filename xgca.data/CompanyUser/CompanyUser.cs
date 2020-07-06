using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using xgca.entity;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Z.EntityFramework.Plus;

namespace xgca.data.CompanyUser
{
    public class CompanyUser : IMaintainable<entity.Models.CompanyUser>, ICompanyUser
    {
        private readonly IXGCAContext _context;

        public CompanyUser(IXGCAContext context)
        {
            _context = context;
        }

        public async Task<bool> ChangeStatus(entity.Models.CompanyUser obj)
        {
            var data = await _context.CompanyUsers
                .Where(cu => cu.CompanyUserId == obj.CompanyUserId && cu.IsDeleted == 0)
                .FirstOrDefaultAsync();
            if (data == null) { return false; }
            data.Status = obj.Status;
            data.ModifiedBy = obj.ModifiedBy;
            data.ModifiedOn = obj.ModifiedOn;
            var result = await _context.SaveChangesAsync();
            return result > 0 ? true : false;
        }

        public async Task<bool> Create(entity.Models.CompanyUser obj)
        {
            await _context.CompanyUsers.AddAsync(obj);
            var result = await _context.SaveChangesAsync();
            return result > 0 ? true : false;
        }

        public async Task<int> CreateAndReturnId(entity.Models.CompanyUser obj)
        {
            await _context.CompanyUsers.AddAsync(obj);
            var result = await _context.SaveChangesAsync();
            return obj.CompanyUserId;
        }

        public async Task<bool> Delete(int key)
        {
            var data = await _context.CompanyUsers
                .Where(cu => cu.CompanyUserId == key && cu.IsDeleted == 0)
                .FirstOrDefaultAsync();
            if (data == null) { return false; }
            data.IsDeleted = 1;
            var result = await _context.SaveChangesAsync();
            return result > 0 ? true : false;
        }

        public async Task<int> GetIdByGuid(Guid guid)
        {
            var result = await _context.CompanyUsers
                .Where(cu => cu.Guid == guid && cu.IsDeleted == 0)
                .FirstOrDefaultAsync();
            return result.CompanyUserId;
        }

        public async Task<int> GetCompanyIdByUserId(int key)
        {
            var result = await _context.CompanyUsers
                .Where(cu => cu.UserId == key && cu.IsDeleted == 0)
                .FirstOrDefaultAsync();
            return result.CompanyId;
        }

        public async Task<List<entity.Models.CompanyUser>> List()
        {
            var data = await _context.CompanyUsers
                .Where(cu => cu.IsDeleted == 0)
                .ToListAsync();
            return data;
        }

        public async Task<List<entity.Models.CompanyUser>> ListByCompanyId(int companyId)
        {
            var data = await _context.CompanyUsers
                .Include(u => u.Users)
                .Where(cu => cu.CompanyId == companyId && cu.IsDeleted == 0)
                .ToListAsync();
            return data;
        }

        public async Task<entity.Models.CompanyUser> Retrieve(int key)
        {
            var data = await _context.CompanyUsers
                .Include(c => c.Companies)
                .Include(u => u.Users)
                .Where(cu => cu.CompanyUserId == key && cu.IsDeleted == 0)
                .FirstOrDefaultAsync();
            return data;
        }

        public async Task<bool> Update(entity.Models.CompanyUser obj)
        {
            var data = await _context.CompanyUsers
                .Where(cu => cu.CompanyUserId == obj.CompanyUserId && cu.IsDeleted == 0)
                .FirstOrDefaultAsync();
            if (data == null) { return false; }
            data.CompanyId = obj.CompanyId;
            data.UserId = obj.UserId;
            data.UserTypeId = obj.UserTypeId;
            data.ModifiedBy = obj.ModifiedBy;
            data.ModifiedOn = obj.ModifiedOn;
            var result = await _context.SaveChangesAsync();
            return result > 0 ? true : false;
        }

        public async Task<int> GetIdByUserId(int userId)
        {
            var data = await _context.CompanyUsers
                .Where(cu => cu.UserId == userId)
                .FirstOrDefaultAsync();
            return data.CompanyUserId;
        }
        public async Task<List<entity.Models.CompanyUser>> GetAllIdByUserId(int userId)
        {
            var data = await _context.CompanyUsers
                .Include(u => u.CompanyServiceUsers)
                .Where(cu => cu.UserId == userId && cu.IsDeleted == 0)
                .ToListAsync();
            return data;
        }
    }
}
