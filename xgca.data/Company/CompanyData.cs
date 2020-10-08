using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using xgca.entity;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace xgca.data.Company
{
    public interface ICompanyData
    {
        Task<bool> Create(entity.Models.Company obj);
        Task<int> CreateAndReturnId(entity.Models.Company obj);
        Task<List<entity.Models.Company>> List();
        Task<entity.Models.Company> Retrieve(int key);
        Task<int> GetIdByGuid(Guid guid);
        Task<string> GetGuidById(int id);
        Task<bool> Update(entity.Models.Company obj);
        Task<bool> Delete(int key);
        Task<List<ActorReturn>> GetAll();
        Task<YourEDIActorReturn> GetCompanyAndMasterUserDetails(string companyKey);
        Task<entity.Models.Company> Retrieve(Guid key);
        Task<dynamic> ListByService(int serviceId, string companyName, int page, int rowCount);
        Task<dynamic> ListByService(int serviceId, int page, int rowCount);
        Task<List<entity.Models.Company>> ListCompaniesByGuids(List<string> guids);

        Task<List<entity.Models.Company>> ListRegisteredCompanies(List<string> registeredIds);
        Task<entity.Models.Company> GetCompanyByEmail(string email);
        Task<bool> CheckCompanyCode(string code);
        Task<List<entity.Models.Company>> GetCompanyWithNullCompanyCodes();
        Task<List<entity.Models.Company>> GetCompanyWithCompanyCodes();

        Task<List<string>> QuickSearch(string search, List<string> companyIds);

        Task<bool> SetAccreditedBy(string companyId, string accreditedBy, int modifiedBy);
        Task<List<entity.Models.Company>> ListByCompanyName(string companyName);
        Task<bool> CheckIfExistsByCompanyName(string companyName);
        Task<string[]> BulkCheckIfExistsByCompanyName(string[] companyName);
        Task<bool> SetCUCCodeByCompanyGuid(string companyKey, string CUCC);
    }

    public class ActorReturn
    {
        public Guid Guid { get; set; }
        public string CompanyName { get; set; }
        public string ImageUrl { get; set; }
        public string Addresses { get; set; }
        public dynamic ContactDetails { get; set; }
    }

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

    public class CompanyData : IMaintainable<entity.Models.Company>, ICompanyData
    {
        private readonly IXGCAContext _context;

        public CompanyData(IXGCAContext context)
        {
            _context = context;
        }

        public Task<bool> Create(entity.Models.Company obj)
        {
            throw new NotImplementedException();
        }

        public async Task<int> CreateAndReturnId(entity.Models.Company obj)
        {
            await _context.Companies.AddAsync(obj);
            var result = await _context.SaveChangesAsync();
            return obj.CompanyId;
        }
        public async Task<List<entity.Models.Company>> List()
        {
            var data = await _context.Companies
                .Include(a => a.Addresses)
                .Include(cn => cn.ContactDetails)
                .Where(c => c.IsDeleted == 0).ToListAsync();
            return data;
        }

        public async Task<dynamic> ListByService(int serviceId, string companyName, int page, int rowCount)
        {
            var data = await _context.CompanyServices.AsNoTracking()
                .Include(c => c.Companies)
                    .ThenInclude(a => a.Addresses)
                .Where(t => t.ServiceId == serviceId && EF.Functions.Like(t.Companies.CompanyName, $"%{companyName}%"))
                .Select(t => new
                { 
                    CompanyId = t.Companies.Guid, 
                    CompanyName = t.Companies.CompanyName,
                    CompanyAddress = t.Companies.Addresses.FullAddress,
                    CompanyImage = t.Companies.ImageURL,
                    isDeleted = t.Companies.IsDeleted
                })
                .Where(a => a.isDeleted == 0)
                .OrderBy(t => t.CompanyName).ToListAsync();

            var response = new
            {
                pages = $"Pages {page + 1} of " + Math.Ceiling(Convert.ToDecimal(data.Count / rowCount)),
                recordCount = data.Count,
                companies = data.Skip(page * rowCount).Take(rowCount)
            };

            return response;                
        }

        public async Task<dynamic> ListByService(int serviceId, int page, int rowCount)
        {
            var data = await _context.CompanyServices.AsNoTracking()
                .Include(c => c.Companies)
                    .ThenInclude(a => a.Addresses)
                .Where(t => t.ServiceId == serviceId)
                .Select(t => new
                { 
                    CompanyId = t.Companies.Guid, 
                    CompanyName = t.Companies.CompanyName,
                    CompanyAddress = t.Companies.Addresses.FullAddress,
                    CompanyImage = t.Companies.ImageURL,
                    isDeleted = t.Companies.IsDeleted
                })
                .Where(a => a.isDeleted == 0)
                .OrderBy(t => t.CompanyName).ToListAsync();

            var response = new
            {
                pages = $"Pages {page} of " + Math.Ceiling(Convert.ToDecimal(data.Count / rowCount)),
                recordCount = data.Count,
                companies = data.Skip(page * rowCount).Take(rowCount)
            };

            return response;                
        }

        public async Task<entity.Models.Company> Retrieve(int key)
        {
            var data = await _context.Companies
                .Include(a => a.Addresses)
                    .ThenInclude(at => at.AddressTypes)
                .Include(cn => cn.ContactDetails)
                .Include(cs => cs.CompanyServices)
                .Where(c => c.CompanyId == key && c.IsDeleted == 0).FirstOrDefaultAsync();
            return data;
        }
        public async Task<int> GetIdByGuid(Guid guid)
        {
            var data = await _context.Companies
                .Where(c => c.Guid == guid && c.IsDeleted == 0).FirstOrDefaultAsync();
            if (data == null) { return 0; }
            return data.CompanyId;
        }
        public async Task<string> GetGuidById(int id)
        {
            var data = await _context.Companies
                .Where(c => c.CompanyId == id && c.IsDeleted == 0).FirstOrDefaultAsync();
            if (data == null) { return null; }
            return data.Guid.ToString();
        }
        public async Task<bool> Update(entity.Models.Company obj)
        {
            var data = await _context.Companies.Where(c => c.CompanyId == obj.CompanyId && c.IsDeleted == 0).FirstOrDefaultAsync();
            if (data == null)
            {
                return false;
            }
            data.CompanyName = obj.CompanyName;
            data.ImageURL = obj.ImageURL;
            data.EmailAddress = obj.EmailAddress;
            data.WebsiteURL = obj.WebsiteURL;
            data.TaxExemption = obj.TaxExemption;
            data.TaxExemptionStatus = obj.TaxExemptionStatus;
            data.ModifiedOn = DateTime.UtcNow;
            data.CUCC = obj.CUCC;
            var result = await _context.SaveChangesAsync();
            return result > 0 ? true : false;
        }
        public async Task<bool> Delete(int key)
        {
            var data = await _context.Companies.Where(c => c.CompanyId == key && c.IsDeleted == 0).FirstOrDefaultAsync();
            if (data == null)
            {
                return false;
            }
            data.IsDeleted = 1;
            data.ModifiedOn = DateTime.UtcNow;
            var result = await _context.SaveChangesAsync();
            return result > 0 ? true : false;
        }

        public async Task<List<ActorReturn>> GetAll()
        {
            var data = await _context.Companies
                .Include(a => a.Addresses)
                .Include(a => a.ContactDetails)
                .Select(
                c => new ActorReturn { Guid = c.Guid, CompanyName = c.CompanyName, ImageUrl = c.ImageURL, Addresses = c.Addresses.FullAddress, ContactDetails = c.ContactDetails })
                .AsNoTracking()
                .ToListAsync();

            var guests = await 
                _context
                .Guests
                .Select(
                g => new ActorReturn 
                { Guid = g.Id, 
                    CompanyName = g.GuestName, 
                    ImageUrl = g.Image, 
                    Addresses = g.AddressLine, 
                    ContactDetails = new { 
                        PhonePrefixId = g.PhoneNumberPrefix ?? "",
                        Phone = g.PhoneNumber,
                        MobilePrefixId = g.MobileNumberPrefix ?? "",
                        Mobile = g.MobileNumber ?? "",
                        FaxPrefix = g.FaxNumberPrefix ?? "",
                        Fax = g.FaxNumber ?? ""
                    }
                }
                )
                .AsNoTracking()
                .ToListAsync();

            data.AddRange(guests);

            return data;
        }

        public async Task<entity.Models.Company> Retrieve(Guid key)
        {
            var data = await _context.Companies
                .Include(a => a.Addresses)
                    .ThenInclude(at => at.AddressTypes)
                .Include(cn => cn.ContactDetails)
                .Include(cs => cs.CompanyServices)
                .Where(c => c.Guid == key && c.IsDeleted == 0).FirstOrDefaultAsync();
            return data;
        }

        public async Task<List<entity.Models.Company>> ListCompaniesByGuids(List<string> guids)
        {
            var data = await _context.Companies.AsNoTracking()
                .Include(a => a.Addresses)
                .Include(c => c.ContactDetails)
                .Where(c => guids.Contains(c.Guid.ToString()))
                .ToListAsync();

            return data;
        }

        public async Task<List<entity.Models.Company>> ListRegisteredCompanies(List<string> registeredIds)
        {
            var companies = await _context.Companies.AsNoTracking()
                .Include(a => a.Addresses)
                .Include(cd => cd.ContactDetails)
                .Where(c => registeredIds.Contains(c.Guid.ToString()))
                .ToListAsync();

            return companies;
        }

        public async Task<entity.Models.Company> GetCompanyByEmail(string email)
        {
            var company = await _context.Companies
                .SingleOrDefaultAsync(x => x.EmailAddress == email);
            return company;
        }

        public async Task<bool> CheckCompanyCode(string code)
        {
            var company = await _context.Companies.AsNoTracking()
                .Where(c => c.CompanyCode == code)
                .Select(x => x.CompanyCode)
                .FirstOrDefaultAsync();

            return (company is null) ? false : true;
        }

        public async Task<List<entity.Models.Company>> GetCompanyWithNullCompanyCodes()
        {
            var companies = await _context.Companies
                .Where(x => x.CompanyCode == null)
                .ToListAsync();

            return companies;
        }

        public async Task<List<entity.Models.Company>> GetCompanyWithCompanyCodes()
        {
            var companies = await _context.Companies
                .Where(x => x.CompanyCode != null)
                .ToListAsync();

            return companies;
        }

        public async Task<List<string>> QuickSearch(string search)
        {
            var companies = await _context.Companies.AsNoTracking()
                .Include(a => a.Addresses)
                .Where(x => EF.Functions.Like(x.CompanyCode, $"%{search}%")
                    || EF.Functions.Like(x.CompanyName, $"%{search}%")
                    || EF.Functions.Like(x.Addresses.AddressLine, $"%{search}%")
                    || EF.Functions.Like(x.Addresses.CityName, $"%{search}%")
                    || EF.Functions.Like(x.Addresses.StateName, $"%{search}%")
                    || EF.Functions.Like(x.Addresses.CountryName, $"%{search}%"))
                .Select(x => x.Guid.ToString())
                .ToListAsync();

            return companies;
        }

        public async Task<List<string>> QuickSearch(string search, List<string> companyIds)
        {
            var companies = await _context.Companies.AsNoTracking()
                .Include(a => a.Addresses)
                .Where(x => (EF.Functions.Like(x.CompanyCode, $"%{search}%")
                    || EF.Functions.Like(x.CompanyName, $"%{search}%")
                    || EF.Functions.Like(x.Addresses.AddressLine, $"%{search}%")
                    || EF.Functions.Like(x.Addresses.CityName, $"%{search}%")
                    || EF.Functions.Like(x.Addresses.StateName, $"%{search}%")
                    || EF.Functions.Like(x.Addresses.CountryName, $"%{search}%"))
                    && companyIds.Contains(x.Guid.ToString()))
                .Select(x => x.Guid.ToString())
                .ToListAsync();

            return companies;
        }

        public async Task<bool> SetAccreditedBy(string companyId, string accreditedBy, int modifiedBy)
        {
            var company = await _context.Companies
                .Where(x => x.Guid.ToString() == companyId)
                .FirstOrDefaultAsync();

            if (company is null) return false;

            company.AccreditedBy = accreditedBy;
            company.ModifiedBy = modifiedBy;
            company.ModifiedOn = DateTime.UtcNow;

            var result = await _context.SaveChangesAsync();
            return result > 0 ? true : false;
        }

        public async Task<List<entity.Models.Company>> ListByCompanyName(string companyName)
        {
            var companies = await _context.Companies.AsNoTracking()
                .Where(x => EF.Functions.Like(x.CompanyName, $"%{companyName}%") && x.IsDeleted == 0)
                .Select(x => new entity.Models.Company
                {
                    Guid = x.Guid,
                    CompanyName = x.CompanyName
                })
                .ToListAsync();

            return companies;
        }

        public async Task<bool> CheckIfExistsByCompanyName(string companyName)
        {
            //var company = await _context.Companies.AsNoTracking().SingleOrDefaultAsync(x => x.CompanyName == companyName);
            var company = await _context.Companies.Where(x => x.CompanyName == companyName).FirstOrDefaultAsync();
            return (company is null) ? false : true;
        }

        public async Task<string[]> BulkCheckIfExistsByCompanyName(string[] companyName)
        {
            var data = await _context.Companies
                .Where(c => companyName.Contains(c.CompanyName))
                .Select(c => c.CompanyName)
                .AsNoTracking()
                .ToArrayAsync();

            return data;
        }

        public async Task<bool> SetCUCCodeByCompanyGuid(string companyKey, string CUCC)
        {
            var company = await _context.Companies.Where(c => c.Guid.ToString() == companyKey).FirstOrDefaultAsync();

            if (company is null)
            {
                return false;
            }

            company.CUCC = CUCC;

            var result = await _context.SaveChangesAsync();
            return result > 0 ? true : false;
        }

        public async Task<YourEDIActorReturn> GetCompanyAndMasterUserDetails(string companyKey)
        {
            var data = await _context.Companies
                .Include(a => a.Addresses)
                .Include(a => a.ContactDetails)
                .Where(x => x.Guid.ToString() == companyKey)
                .Select(
                c => new YourEDIActorReturn
                {
                    Guid = c.Guid,
                    CUCC = (c.CUCC == null) ? "N/A" : c.CUCC,
                    CompanyName = c.CompanyName,
                    Email = (c.EmailAddress == null) ? "N/A" : c.EmailAddress,
                    ImageUrl = (c.ImageURL == null) ? "N/A" : c.ImageURL,
                    ContactDetails = new
                    {
                        MobileNoPrefix = (c.ContactDetails.MobilePrefix == null) ? "N/A" : c.ContactDetails.MobilePrefix,
                        MobileNo = (c.ContactDetails.Mobile == null) ? "N/A" : c.ContactDetails.Mobile,
                        PhoneNoPrefix = (c.ContactDetails.PhonePrefix == null) ? "N/A" : c.ContactDetails.PhonePrefix,
                        PhoneNo = (c.ContactDetails.Phone == null) ? "N/A" : c.ContactDetails.Phone,
                        FaxNoPrefix = (c.ContactDetails.FaxPrefix == null) ? "N/A" : c.ContactDetails.FaxPrefix,
                        FaxNo = (c.ContactDetails.Fax == null) ? "N/A" : c.ContactDetails.Fax
                    },
                    Address = new
                    {
                        CompleteAddress = (c.Addresses.FullAddress == null) ? "N/A" : c.Addresses.FullAddress,
                        AddressLine = (c.Addresses.AddressLine == null) ? "N/A" : c.Addresses.AddressLine,
                        City = (c.Addresses.CityName == null) ? "N/A" : c.Addresses.CityName,
                        ProvinceState = (c.Addresses.StateName == null) ? "N/A" : c.Addresses.StateName,
                        Country = (c.Addresses.CountryName == null) ? " N/A" : c.Addresses.CountryName,
                        ZipCode = (c.Addresses.ZipCode == null) ? "N/A" : c.Addresses.ZipCode
                    }
                })
                .AsNoTracking()
                .SingleOrDefaultAsync();
            
            if (data is null)
            {
                return null;
            }

            int companyId = await _context.Companies.AsNoTracking()
                .Where(x => x.Guid.ToString() == companyKey)
                .Select(x => x.CompanyId)
                .FirstOrDefaultAsync();

            var masterUser = await _context.CompanyUsers.AsNoTracking()
                .Include(a => a.Users)
                .Where(x => x.CompanyId == companyId && x.UserTypeId == 1) // UserTypeId == Master User
                .Select(m => new 
                { 
                    Firstname = (m.Users.FirstName == null) ? "Master" : m.Users.FirstName, 
                    Lastname = (m.Users.LastName == null) ? "User" : m.Users.LastName 
                })
                .FirstOrDefaultAsync();


            data.MasterUser = masterUser;

            return data;
        }
    }
}
