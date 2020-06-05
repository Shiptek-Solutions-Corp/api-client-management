using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using xgca.entity;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace xgca.data.AddressType
{
    public class AddressType : IMaintainable<entity.Models.AddressType>, IAddressType
    {
        private readonly IXGCAContext _context;
        public AddressType(IXGCAContext context)
        {
            _context = context;
        }
        public async Task<bool> Create(entity.Models.AddressType obj)
        {
            await _context.AddressTypes.AddAsync(obj);
            var result = await _context.SaveChangesAsync();
            return result > 0 ? true : false;
        }

        public async Task<List<entity.Models.AddressType>> List()
        {
            var data = await _context.AddressTypes.Where(c => c.IsDeleted == 0).ToListAsync();
            return data;
        }

        public async Task<bool> Update(entity.Models.AddressType obj)
        {
            var data = await _context.AddressTypes.Where(c => c.AddressTypeId == obj.AddressTypeId).FirstOrDefaultAsync();
            if (data == null)
            {
                return false;
            }
            data.ModifiedOn = new DateTime();
            data.Name = obj.Name;
            var result = await _context.SaveChangesAsync();
            return result > 0 ? true : false;
        }

        public async Task<entity.Models.AddressType> Retrieve(int key)
        {
            var data = await _context.AddressTypes.Where(c => c.AddressTypeId == key && c.IsDeleted == 0).FirstOrDefaultAsync();
            return data;
        }
        public async Task<int> GetIdByGuid(Guid guid)
        {
            var data = await _context.AddressTypes
                .Where(c => c.Guid == guid && c.IsDeleted == 0).FirstOrDefaultAsync();
            return data.AddressTypeId;
        }

        public async Task<int> RetrieveIdByName(string key)
        {
            var data = await _context.AddressTypes.Where(c => c.Name == key && c.IsDeleted == 0).FirstOrDefaultAsync();
            return data.AddressTypeId;
        }

        public async Task<bool> Delete(int key)
        {
            var data = await _context.AddressTypes.Where(c => c.AddressTypeId == key).FirstOrDefaultAsync();
            if (data == null)
            {
                return false;
            }
            data.IsDeleted = 1;
            data.ModifiedOn = new DateTime();
            var result = await _context.SaveChangesAsync();
            return result > 0 ? true : false;
        }
    }
}