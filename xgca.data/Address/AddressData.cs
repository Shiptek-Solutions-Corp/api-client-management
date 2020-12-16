using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using xgca.entity;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace xgca.data.Address
{
    public class AddressData : IMaintainable<entity.Models.Address>, IAddressData
    {
        private readonly IXGCAContext _context;
        public AddressData(IXGCAContext context)
        {
            _context = context;
        }

        public async Task<int> CreateAndReturnId(entity.Models.Address obj)
        {
            await _context.Addresses.AddAsync(obj);
            var result = await _context.SaveChangesAsync();
            return result > 0 ? obj.AddressId : 0;
        }
        public Task<bool> Create(entity.Models.Address obj)
        {
            throw new NotImplementedException();
        }
        public Task<List<entity.Models.Address>> List()
        {
            throw new NotImplementedException();
        }
        public async Task<entity.Models.Address> Retrieve(int key)
        {
            var data = await _context.Addresses
                .Include(at => at.AddressTypes)
                .Where(c => c.AddressId == key && c.IsDeleted == 0)
                .FirstOrDefaultAsync();
            return data;
        }
        public async Task<int> GetIdByGuid(Guid guid)
        {
            var data = await _context.Addresses
                .Where(c => c.Guid == guid && c.IsDeleted == 0).FirstOrDefaultAsync();
            if (data == null) { return 0; }
            return data.AddressId;
        }
        public async Task<bool> Update(entity.Models.Address obj)
        {
            var data = await _context.Addresses.Where(c => c.AddressId == obj.AddressId).FirstOrDefaultAsync();
            if (data == null)
            {
                return false;
            }
            data.AddressTypeId = obj.AddressTypeId;
            data.AddressLine = obj.AddressLine;
            data.CityId = obj.CityId;
            data.CityName = obj.CityName;
            data.StateName = obj.StateName;
            data.StateId = obj.StateId;
            data.ZipCode = obj.ZipCode;
            data.CountryId = obj.CountryId;
            data.CountryName = obj.CountryName;
            data.FullAddress = obj.FullAddress;
            data.Longitude = obj.Longitude;
            data.Latitude = obj.Latitude;
            data.ModifiedBy = obj.ModifiedBy;
            data.ModifiedOn = obj.ModifiedOn;
            data.AddressAdditionalInformation = obj.AddressAdditionalInformation;
            var result = await _context.SaveChangesAsync();
            return result > 0 ? true : false;
        }
        public Task<bool> Delete(int key)
        {
            throw new NotImplementedException();
        }
    }
}