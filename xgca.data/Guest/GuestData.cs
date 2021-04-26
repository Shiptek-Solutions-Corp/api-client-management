using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using xgca.entity;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using LinqKit;
using xgca.data.Company;

namespace xgca.data.Guest
{
    public class YourEDIActorReturn
    {
        public Guid Guid { get; set; }
        public string CUCC { get; set; }
        public dynamic MasterUser { get; set; }
        public string CompanyName { get; set; }
        public string Email { get; set; }
        public string ImageUrl { get; set; }
        public dynamic ContactDetails { get; set; }
        public dynamic Address { get; set; }

    }
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

        public async Task<List<entity.Models.Guest>> QuickSearch(string search, List<string> guids)
        {
            var predicate = PredicateBuilder.New<entity.Models.Guest>();

            if (!(search is null))
            {
                predicate = predicate.Or(x => (EF.Functions.Like(x.GuestName, $"%{search}%")));
                predicate = predicate.Or(x => (EF.Functions.Like(x.CountryName, $"%{search}%")));
                predicate = predicate.Or(x => (EF.Functions.Like(x.CityName, $"%{search}%")));
                predicate = predicate.Or(x => (EF.Functions.Like(x.StateName, $"%{search}%")));
            }

            predicate = predicate.And(x => guids.Contains(x.Id.ToString()) && x.IsDeleted == 0);

            var guests = await _context.Guests.AsNoTracking()
                .Where(predicate)
                .ToListAsync();

            return guests;
        }

        public async Task<List<entity.Models.Guest>> QuickSearch(string search)
        {
            string searcHValue = $"%{search}%";

            var guests = await _context.Guests.AsNoTracking()
                .Where(x => EF.Functions.Like(x.AddressLine, searcHValue)
                    || EF.Functions.Like(x.CityName, searcHValue)
                    || EF.Functions.Like(x.CountryName, searcHValue)
                    || EF.Functions.Like(x.EmailAddress, searcHValue)
                    || EF.Functions.Like(x.FirstName, searcHValue)
                    || EF.Functions.Like(x.GuestName, searcHValue)
                    || EF.Functions.Like(x.LastName, searcHValue)
                    || EF.Functions.Like(x.StateName, searcHValue))
                .ToListAsync();

            return guests;
        }

        public async Task<YourEDIActorReturn> GetGuestAndMasterUserDetails(string guestKey)
        {
            var data = await _context.Guests
                .Where(x => x.Id.ToString() == guestKey)
                .Select(x => new YourEDIActorReturn
                {
                    Guid = x.Id,
                    CUCC = (x.CUCC == null) ? "N/A" : x.CUCC,
                    CompanyName = x.GuestName,
                    Email = (x.EmailAddress == null) ? "N/A" : x.EmailAddress,
                    ImageUrl = (x.Image == null) ? "N/A" : x.Image,
                    ContactDetails = new
                    {
                        MobileNoPrefix = (x.MobileNumberPrefix == null) ? "N/A" : x.MobileNumberPrefix,
                        MobileNo = (x.MobileNumber == null) ? "N/A" : x.MobileNumber,
                        PhoneNoPrefix = (x.PhoneNumberPrefix == null) ? "N/A" : x.PhoneNumberPrefix,
                        PhoneNo = (x.PhoneNumber == null) ? "N/A" : x.PhoneNumber,
                        FaxNoPrefix = (x.FaxNumberPrefix == null) ? "N/A" : x.FaxNumberPrefix,
                        FaxNo = (x.FaxNumber == null) ? "N/A" : x.FaxNumber
                    },
                    Address = new
                    {
                        CompleteAddress = $"{x.AddressLine}, {x.CityName}, {x.StateName}, {x.CountryName}, {x.ZipCode}",
                        AddressLine = (x.AddressLine == null) ? "N/A" : x.AddressLine,
                        City = (x.CityName == null) ? "N/A" : x.CityName,
                        ProvinceState = (x.StateName == null) ? "N/A" : x.StateName,
                        Country = (x.CountryName == null) ? "N/A" : x.CountryName,
                        ZipCode = (x.ZipCode == null) ? "N/A" : x.ZipCode
                    },
                    MasterUser = new
                    {
                        Firstname = (x.FirstName == null) ? "N/A" : x.FirstName,
                        Lastname = (x.LastName == null) ? "N/A" : x.LastName
                    }
                })
                .AsNoTracking()
                .FirstOrDefaultAsync();

            return data;
        }

        public async Task<bool> SetCUCCByGuestGuid(string guestKey, string CUCC)
        {
            var guest = await _context.Guests.Where(x => x.Id == Guid.Parse(guestKey)).FirstOrDefaultAsync();

            if (guest is null)
            { return false; }

            guest.CUCC = CUCC;

            var result = await _context.SaveChangesAsync();
            return result > 0 ? true : false;

        }

        public async Task<List<entity.Models.Guest>> SearchGuest(string search, string name, string country, string stateCity, string contact, List<string> guids)
        {
            var predicate = PredicateBuilder.New<entity.Models.Guest>();

            if (!(search is null))
            {
                predicate = predicate.Or(x => (EF.Functions.Like(x.GuestName, $"%{search}%")));
                predicate = predicate.Or(x => (EF.Functions.Like(x.CountryName, $"%{search}%")));
                predicate = predicate.Or(x => (EF.Functions.Like(x.CityName, $"%{search}%")));
                predicate = predicate.Or(x => (EF.Functions.Like(x.StateName, $"%{search}%")));
                predicate = predicate.Or(x => (EF.Functions.Like(x.PhoneNumber, $"%{search}%")));
                predicate = predicate.Or(x => (EF.Functions.Like(x.MobileNumber, $"%{search}%")));
                predicate = predicate.Or(x => (EF.Functions.Like(x.FaxNumber, $"%{search}%")));
            }

            if (!(name is null))
            {
                predicate = predicate.And(x => x.GuestName == name);
            }

            if (!(country is null))
            {
                predicate = predicate.And(x => x.CountryName == country);
            }

            if (!(stateCity is null))
            {
                predicate = predicate.And(x => EF.Functions.Like(x.StateName, $"%{stateCity}%"));
                predicate = predicate.And(x => EF.Functions.Like(x.CityName, $"%{stateCity}%"));
            }

            if (!(contact is null))
            {
                predicate = predicate.And(x => EF.Functions.Like(x.PhoneNumber, $"%{contact}%"));
                predicate = predicate.And(x => EF.Functions.Like(x.MobileNumber, $"%{contact}%"));
                predicate = predicate.And(x => EF.Functions.Like(x.FaxNumber, $"%{contact}%"));
            }

            predicate = predicate.And(x => guids.Contains(x.Id.ToString()) && x.IsDeleted == 0);

            var guests = await _context.Guests.AsNoTracking()
                .Where(predicate)
                .ToListAsync();

            return guests;
        }

        public async Task<Biller> GetGuestBiller(string guestBillerId)
        {
            var guestBiller = await _context.Guests.AsNoTracking()
                .Where(x => x.Id == Guid.Parse(guestBillerId))
                .Select(c => new Biller
                {
                    BillerId = c.Id.ToString(),
                    BillerName = c.GuestName,
                    BillerImage = c.Image,
                    BillerAddress = (c.AddressLine == null) ? "" : $"{c.AddressLine}, {c.CityName}, {c.StateName}, {c.CountryName}",
                    BillerLandline = (c.PhoneNumber == null) ? "" : $"{c.PhoneNumberPrefix}{c.PhoneNumber}",
                    BillerFax = (c.FaxNumber == null) ? "" : $"{c.FaxNumberPrefix}{c.FaxNumber}",
                    BillerCode = "GUEST",
                    BillerCountryId = c.CountryId
                })
                .FirstOrDefaultAsync();

            return guestBiller;
        }

        public async Task<Customer> GetGuestCustomer(string guestCustomerId)
        {
            var guestCustomer = await _context.Guests.AsNoTracking()
                .Where(x => x.Id == Guid.Parse(guestCustomerId))
                .Select(c => new Customer
                {
                    CustomerId = c.Id.ToString(),
                    CustomerName = c.GuestName,
                    CustomerImage = c.Image,
                    CustomerAddress = (c.AddressLine == null) ? "" : $"{c.AddressLine}, {c.CityName}, {c.StateName}, {c.CountryName}",
                    CustomerLandline = (c.PhoneNumber == null) ? "" : $"{c.PhoneNumberPrefix}{c.PhoneNumber}",
                    CustomerFax = (c.FaxNumber == null) ? "" : $"{c.FaxNumberPrefix}{c.FaxNumber}",
                    CustomerCode = "GUEST",
                    CustomerCountryId = c.CountryId
                })
                .FirstOrDefaultAsync();

            return guestCustomer;
        }
    }
}