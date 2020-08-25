using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using xgca.entity;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace xgca.data.Guest
{
    public class GuestData : IMaintainable<entity.Models.Guest>, IGuestData
    {
        private readonly IXGCAContext _context;

        public GuestData(IXGCAContext context)
        {
            _context = context;
        }

        public async Task<bool> Create(entity.Models.Guest obj)
        {
            await _context.Guests.AddAsync(obj);
            var result = await _context.SaveChangesAsync();
            return result > 0 ? true : false;
        }

        public async Task<int> CreateAndReturnId(entity.Models.Guest obj)
        {
            await _context.Guests.AddAsync(obj);
            var result = await _context.SaveChangesAsync();
            return result > 0 ? obj.GuestId : 0;
        }

        public async Task<string> CreateAndReturnGuid(entity.Models.Guest obj)
        {
            await _context.Guests.AddAsync(obj);
            var result = await _context.SaveChangesAsync();
            return result > 0 ? obj.Id.ToString() : "";
        }

        public async Task<bool> Delete(entity.Models.Guest obj)
        {
            var guest = await _context.Guests
                .Where(g => g.GuestId == obj.GuestId && g.IsDeleted == 0)
                .FirstOrDefaultAsync();

            guest.IsDeleted = obj.IsDeleted;
            guest.DeletedBy = obj.DeletedBy;
            guest.DeletedOn = obj.DeletedOn;

            var result = await _context.SaveChangesAsync();
            return result > 0 ? true : false;
        }

        public async Task<string> GetGuidById(int id)
        {
            var guest = await _context.Guests
                .Where(g => g.GuestId == id && g.IsDeleted == 0)
                .FirstOrDefaultAsync();

            return (guest is null) ? null : guest.Id.ToString();

        }

        public async Task<int> GetIdByGuid(Guid guid)
        {
            var guest = await _context.Guests
                .Where(g => g.Id == guid && g.IsDeleted == 0)
                .FirstOrDefaultAsync();

            return (guest is null) ? 0 : guest.GuestId;
        }

        public async Task<List<entity.Models.Guest>> List()
        {
            var guests = await _context.Guests
            .Where(g => g.IsDeleted == 0)
            .ToListAsync();

            return guests;
        }

        public Task<List<entity.Models.Guest>> List(string columnFilter)
        {
            throw new NotImplementedException();
        }

        public async Task<List<entity.Models.Guest>> List(int companyId)
        {
            var guests = await _context.Guests
                .Where(g => g.CompanyId == companyId && g.IsDeleted == 0)
                .ToListAsync();

            return guests;
        }

        public Task<List<entity.Models.Guest>> List(int companyId, string columnFilter)
        {
            throw new NotImplementedException();
        }

        public async Task<entity.Models.Guest> Retrieve(int key)
        {
            var guest = await _context.Guests
                .Where(g => g.GuestId == key)
                .FirstOrDefaultAsync();

            return guest;
        }

        public async Task<entity.Models.Guest> Retrieve(Guid key)
        {

            var guest = await _context.Guests
                .Where(g => g.Id == key && g.IsDeleted == 0)
                .FirstOrDefaultAsync();
                

            return guest;
        }

        public async Task<bool> Update(entity.Models.Guest obj)
        {
            var guest = await _context.Guests
                .Where(g => g.GuestId == obj.GuestId && g.IsDeleted == 0)
                .FirstOrDefaultAsync();

            guest.AddressLine = obj.AddressLine;
            guest.CityId = obj.CityId;
            guest.CityName = obj.CityName;
            guest.CompanyId = obj.CompanyId;
            guest.EmailAddress = obj.EmailAddress;
            guest.FaxNumber = obj.FaxNumber;
            guest.FaxNumberPrefix = obj.FaxNumberPrefix;
            guest.FaxNumberPrefixId = obj.FaxNumberPrefixId;
            guest.FirstName = obj.FirstName;
            guest.GuestName = obj.GuestName;
            guest.GuestType = obj.GuestType;
            guest.Image = obj.Image;
            guest.IsGuest = obj.IsGuest;
            guest.LastName = obj.LastName;
            guest.MobileNumber = obj.MobileNumber;
            guest.MobileNumberPrefix = obj.MobileNumberPrefix;
            guest.MobileNumberPrefixId = obj.MobileNumberPrefixId;
            guest.ModifiedBy = obj.ModifiedBy;
            guest.ModifiedOn = obj.ModifiedOn;
            guest.PhoneNumber = obj.PhoneNumber;
            guest.PhoneNumberPrefix = obj.PhoneNumberPrefix;
            guest.PhoneNumberPrefixId = obj.PhoneNumberPrefixId;
            guest.StateId = obj.StateId;
            guest.StateName = obj.StateName;
            guest.CountryId = obj.CountryId;
            guest.CountryName = obj.CountryName;
            guest.ZipCode = obj.ZipCode;

            var result = await _context.SaveChangesAsync();
            return result > 0 ? true : false;
        }
    }
}