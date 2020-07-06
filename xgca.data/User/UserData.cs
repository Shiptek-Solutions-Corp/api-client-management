using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using xgca.entity;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using xgca.entity.Migrations;

namespace xgca.data.User
{
    public class UserData : IMaintainable<xgca.entity.Models.User>, IUserData
    {
        private readonly IXGCAContext _context;
        public UserData(IXGCAContext context)
        {
            _context = context;
        }

        public Task<bool> Create(entity.Models.User obj)
        {
            throw new NotImplementedException();
        }

        public async Task<int> CreateAndReturnId(entity.Models.User obj)
        {
            await _context.Users.AddAsync(obj);
            var result = await _context.SaveChangesAsync();
            return obj.UserId;
        }
        public async Task<List<entity.Models.User>> List()
        {
            var data = await _context.Users
                .Include(cn => cn.ContactDetails)
                .Where(u => u.IsDeleted == 0).ToListAsync();
            return data;
        }

        public async Task<List<entity.Models.User>> List(string columnFilter, string isActive, string isLock)
        {
            var queryString = $"Select * from [Users].[User] where {columnFilter} isDeleted = 0";

            var data = await _context.Users.FromSqlRaw(queryString).ToListAsync();
            return data;
        }

        public async Task<entity.Models.User> Retrieve(int key)
        {
            var data = await _context.Users
                .Include(cn => cn.ContactDetails)
                .Where(u => u.UserId == key && u.IsDeleted == 0).FirstOrDefaultAsync();
            return data;
        }
        public async Task<entity.Models.User> RetrieveByUsername(string username)
        {
            var data = await _context.Users
                .Include(cn => cn.ContactDetails)
                .Include(cu => cu.CompanyUsers)
                .Where(u => u.Username == username).FirstOrDefaultAsync();
            return data;
        }
        public async Task<bool> Update(entity.Models.User obj)
        {
            var data = await _context.Users.Where(u => u.UserId == obj.UserId && u.IsDeleted == 0).FirstOrDefaultAsync();
            if (data == null)
            {
                return false;
            }
            data.FirstName = obj.FirstName;
            data.LastName = obj.LastName;
            data.MiddleName = obj.MiddleName;
            data.Title = obj.Title;
            data.ImageURL = obj.ImageURL;
            data.ModifiedBy = obj.ModifiedBy;
            data.ModifiedOn = DateTime.UtcNow;
            var result = await _context.SaveChangesAsync();
            return result > 0 ? true : false;
        }

        public async Task<bool> UpdateStatus(entity.Models.User obj)
        {
            var data = await _context.Users.Where(u => u.UserId == obj.UserId && u.IsDeleted == 0).FirstOrDefaultAsync();
            if (data == null)
            {
                return false;
            }
            data.Status = obj.Status;
            data.ModifiedBy = obj.ModifiedBy;
            data.ModifiedOn = DateTime.UtcNow;
            var result = await _context.SaveChangesAsync();
            return result > 0 ? true : false;
        }

        public async Task<bool> UpdateStatus(List<int> userIds, int modifiedBy, byte status)
        {
            var data = await _context.Users.Where(u => userIds.Contains(u.UserId) && u.IsDeleted == 0)
                .ToListAsync();
            if (data == null)
            {
                return false;
            }
            data.ForEach(t => {
                t.Status = status;
                t.ModifiedBy = modifiedBy;
                t.ModifiedOn = DateTime.UtcNow;
            });
            var result = await _context.SaveChangesAsync();
            return result > 0 ? true : false;
        }

        public async Task<bool> UpdateLock(entity.Models.User obj)
        {
            var data = await _context.Users.Where(u => u.UserId == obj.UserId && u.IsDeleted == 0).FirstOrDefaultAsync();
            if (data == null)
            {
                return false;
            }
            data.IsLocked = obj.IsLocked;
            data.ModifiedBy = obj.ModifiedBy;
            data.ModifiedOn = DateTime.UtcNow;
            var result = await _context.SaveChangesAsync();
            return result > 0 ? true : false;
        }

        public async Task<bool> UpdateLock(List<int> userIds, int modifiedBy, byte isLock)
        {
            var data = await _context.Users.Where(u => userIds.Contains(u.UserId))
                .ToListAsync();
            if (data == null)
            {
                return false;
            }
            data.ForEach(t => {
                t.IsLocked = isLock;
                t.ModifiedBy = modifiedBy;
                t.ModifiedOn = DateTime.UtcNow;
            });
            var result = await _context.SaveChangesAsync();
            return result > 0 ? true : false;
        }

        public async Task<bool> SetUsername(entity.Models.User obj)
        {
            var data = await _context.Users.Where(u => u.UserId == obj.UserId && u.IsDeleted == 0).FirstOrDefaultAsync();
            if (data == null)
            {
                return false;
            }

            data.Username = obj.Username;
            data.ModifiedBy = obj.ModifiedBy;
            data.ModifiedOn = DateTime.UtcNow;
            var result = await _context.SaveChangesAsync();
            return result > 0 ? true : false;
        }
        public async Task<bool> Delete(int key)
        {
            var data = await _context.Users.Where(u => u.UserId == key && u.IsDeleted == 0).FirstOrDefaultAsync();
            if (data == null)
            {
                return false;
            }
            data.IsDeleted = 1;
            data.ModifiedOn = DateTime.UtcNow;
            var result = await _context.SaveChangesAsync();
            return result > 0 ? true : false;
        }
        public async Task<int> GetIdByGuid(Guid key)
        {
            var data = await _context.Users
                .Where(u => u.Guid == key && u.IsDeleted == 0).FirstOrDefaultAsync();
            return data.UserId;
        }
        public async Task<Guid> GetGuidById(int key)
        {
            var data = await _context.Users
                .Where(u => u.UserId == key && u.IsDeleted == 0).FirstOrDefaultAsync();
            return data.Guid;
        }

        public async Task<int> GetIdByUsername(string username)
        {
            var data = await _context.Users
                .Where(u => u.Username == username)
                .FirstOrDefaultAsync();
            return data.UserId;
        }

        public bool UsernameExists(string username)
        {
            return _context.Users.Any(u => u.Username == username);
        }

        public async Task<bool> EmailAddressExists(string emailAddress)
        {
            var user = await _context.Users
                .Where(u => u.EmailAddress == emailAddress)
                .FirstOrDefaultAsync();

            return !(user is null) ? true : false;
        }

        public async Task<int> GetTotalActiveUsers()
        {
            var data = await _context.Users
                .Where(u => u.Status == 1 && u.IsDeleted == 0)
                .ToListAsync();
            return data.Count;
        }

        public async Task<int> GetTotalInactiveUsers()
        {
            var data = await _context.Users
                .Where(u => u.Status == 0 && u.IsDeleted == 0)
                .ToListAsync();
            return data.Count;
        }

        public async Task<int> GetTotalLockedUsers()
        {
            var data = await _context.Users
                .Where(u => u.IsLocked == 1 && u.IsDeleted == 0)
                .ToListAsync();
            return data.Count;
        }

        public async Task<int> GetTotalUnlockedUsers()
        {
            var data = await _context.Users
                .Where(u => u.IsLocked == 0 && u.IsDeleted == 0)
                .ToListAsync();
            return data.Count;
        }

        public async Task<int> GetTotalUsers()
        {
            var data = await _context.Users
                .Where(u => u.IsDeleted == 0)
                .ToListAsync();
            return data.Count;
        }

        public async Task<int> GetTotalActiveUsers(List<int> userIds)
        {
            var data = await _context.Users
                .Where(u => userIds.Contains(u.UserId) && u.Status == 1 && u.IsDeleted == 0)
                .ToListAsync();
            return data.Count;
        }

        public async Task<int> GetTotalInactiveUsers(List<int> userIds)
        {
            var data = await _context.Users
                .Where(u => userIds.Contains(u.UserId) && u.Status == 0 && u.IsDeleted == 0)
                .ToListAsync();
            return data.Count;
        }

        public async Task<int> GetTotalLockedUsers(List<int> userIds)
        {
            var data = await _context.Users
                .Where(u => userIds.Contains(u.UserId) && u.IsLocked == 1 && u.IsDeleted == 0)
                .ToListAsync();
            return data.Count;
        }

        public async Task<int> GetTotalUnlockedUsers(List<int> userIds)
        {
            var data = await _context.Users
                .Where(u => userIds.Contains(u.UserId) && u.IsLocked == 0 && u.IsDeleted == 0)
                .ToListAsync();
            return data.Count;
        }

        public async Task<int> GetTotalUsers(List<int> userIds)
        {
            var data = await _context.Users
                .Where(u => userIds.Contains(u.UserId) && u.IsDeleted == 0)
                .ToListAsync();
            return data.Count;
        }
    }
}
