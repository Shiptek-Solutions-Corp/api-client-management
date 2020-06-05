using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using xgca.entity;
using System.Linq;
using Microsoft.EntityFrameworkCore;

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
        public async Task<entity.Models.User> Retrieve(int key)
        {
            var data = await _context.Users
                .Include(cn => cn.ContactDetails)
                .Where(u => u.UserId == key && u.IsDeleted == 0).FirstOrDefaultAsync();
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
            data.EmailAddress = obj.EmailAddress;
            data.ImageURL = obj.ImageURL;
            data.ModifiedBy = obj.ModifiedBy;
            data.ModifiedOn = DateTime.Now;
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
            data.ModifiedOn = DateTime.Now;
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
    }
}
