using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using xgca.entity;
using System.Linq;
using Microsoft.EntityFrameworkCore;


namespace xgca.data.User
{
    public interface IUserData
    {
        Task<bool> Create(entity.Models.User obj);
        Task<int> CreateAndReturnId(entity.Models.User obj);
        Task<List<entity.Models.User>> List();
        Task<List<entity.Models.User>> List(string columnFilter, string isActive, string isLock);
        Task<List<entity.Models.User>> FilterWithCompanyUsersId(string columnFilter, string userIds);
        Task<int> GetTotalActiveUsers();
        Task<int> GetTotalInactiveUsers();
        Task<int> GetTotalLockedUsers();
        Task<int> GetTotalUnlockedUsers();
        Task<int> GetTotalUsers();
        Task<int> GetTotalActiveUsers(List<int> userIds);
        Task<int> GetTotalInactiveUsers(List<int> userIds);
        Task<int> GetTotalLockedUsers(List<int> userIds);
        Task<int> GetTotalUnlockedUsers(List<int> userIds);
        Task<int> GetTotalUsers(List<int> userIds);
        Task<entity.Models.User> Retrieve(int key);
        Task<entity.Models.User> RetrieveByUsername(string username);
        Task<entity.Models.User> RetrieveByEmail(string emailAddress);
        Task<bool> Update(entity.Models.User obj);
        Task<bool> UpdateStatus(entity.Models.User obj);
        Task<bool> UpdateStatus(List<int> userIds, int modifiedBy, byte status);
        Task<bool> UpdateLock(entity.Models.User obj);
        Task<bool> UpdateLock(List<int> userIds, int modifiedBy, byte isLock);
        Task<SetUserNameReturnObject> SetUsername(entity.Models.User obj);
        Task<bool> Delete(int key);
        Task<bool> Delete(List<int> userIds, int modifiedBy);
        Task<int> GetIdByGuid(Guid key);
        Task<Guid> GetGuidById(int key);
        Task<int> GetIdByUsername(string username);
        Task<xgca.entity.Models.User> ActivateCompanyUser(string emailAddress);
        bool UsernameExists(string username);
        Task<bool> EmailAddressExists(string emailAddress);
        Task<entity.Models.User> GetUserByEmail(string email);
        Task<entity.Models.User> GetMasterUser(int userId);
        Task<List<xgca.entity.Models.User>> GetUsernamesByIds(List<int> userIds);
    }
    public class SetUserNameReturnObject
    {
        public bool Result { get; set; }
        public List<int> IsUserMaster { get; set; }
        public string CompanyId { get; set; }
    }
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
                .ThenInclude(c => c.Companies)
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
        public async Task<SetUserNameReturnObject> SetUsername(entity.Models.User obj)
        {
            var data = await _context.Users.Where(u => u.UserId == obj.UserId && u.IsDeleted == 0).FirstOrDefaultAsync();
            if (data == null)
            {
                return new SetUserNameReturnObject { Result = false, IsUserMaster = null };
            }
            data.Username = obj.Username;
            data.ModifiedBy = obj.ModifiedBy;
            data.ModifiedOn = DateTime.UtcNow;
            data.Status = obj.Status;
            var result = await _context.SaveChangesAsync();
            List<int> isUserMaster = await _context.Users.Where(u => u.UserId == obj.UserId && u.IsDeleted == 0)
                .Select(u => u.CompanyUsers).SelectMany(c => c.CompanyServiceUsers).Select(c => c.IsMasterUser).ToListAsync();

            var companyId = await _context.Users.Where(u => u.UserId == obj.UserId && u.IsDeleted == 0)
                .Select(u => u.CompanyUsers)
                .Select(c => c.Companies)
                .Select(c => c.Guid)
                .FirstOrDefaultAsync();

            return new SetUserNameReturnObject { Result = result > 0 ? true : false, IsUserMaster = isUserMaster, CompanyId = companyId.ToString()};

            //return new { result = result > 0 ? true : false, isUserMaster = isUserMaster };
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

        public async Task<bool> Delete(List<int> userIds, int modifiedBy)
        {
            var data = await _context.Users.Where(u => userIds.Contains(u.UserId))
                .ToListAsync();
            if (data == null)
            {
                return false;
            }
            data.ForEach(t => {
                t.IsDeleted = 1;
                t.ModifiedBy = modifiedBy;
                t.ModifiedOn = DateTime.UtcNow;
            });
            var result = await _context.SaveChangesAsync();
            return result > 0 ? true : false;
        }
        public async Task<int> GetIdByGuid(Guid key)
        {
            var data = await _context.Users
                .Where(u => u.Guid == key && u.IsDeleted == 0).FirstOrDefaultAsync();
            return (data is null) ? 0 : data.UserId;
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
        public async Task<List<entity.Models.User>> FilterWithCompanyUsersId(string columnFilter, string userIds)
        {
            var queryString = $"Select * from [Users].[User] where {columnFilter} isDeleted = 0 AND userId IN ({userIds})";

            var data = await _context.Users.FromSqlRaw(queryString).ToListAsync();
            return data;
        }

        public async Task<entity.Models.User> GetUserByEmail(string email)
        {
            var user = await _context.Users.AsNoTracking()
                .Where(x => x.EmailAddress == email)
                .FirstOrDefaultAsync();

            return user;
        }

        public async Task<entity.Models.User> GetMasterUser(int userId)
        {
            var masterUser = await _context.Users.AsNoTracking()
                .Where(x => x.UserId == userId)
                .FirstOrDefaultAsync();

            return masterUser;
        }

        public async Task<List<xgca.entity.Models.User>> GetUsernamesByIds(List<int> userIds)
        {
            var users = await _context.Users.AsNoTracking()
                .Where(x => userIds.Contains(x.UserId))
                .Select(c => new xgca.entity.Models.User
                {
                    UserId = c.UserId,
                    Username = c.Username
                })
                .ToListAsync();

            return users;
        }

        public async Task<xgca.entity.Models.User> ActivateCompanyUser(string emailAddress)
        {
            var user = await _context.Users.Where(u => u.EmailAddress == emailAddress)
                .Include(u => u.CompanyUsers)
                .ThenInclude(u => u.Companies)
                .ThenInclude(u => u.Addresses)
                .Include(u => u.ContactDetails)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return null;
            }

            user.CompanyUsers.Status = 1;
            user.CompanyUsers.Companies.Status = 1;
            user.Status = 1;
            try
            {
                var result = await _context.SaveChangesAuditable();
                return user;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<entity.Models.User> RetrieveByEmail(string emailAddress)
        {
            var data = await _context.Users
                .Include(cn => cn.ContactDetails)
                .Include(cu => cu.CompanyUsers)
                .ThenInclude(c => c.Companies)
                .Where(u => u.EmailAddress == emailAddress).FirstOrDefaultAsync();
            return data;
        }
    }
}
