using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using xgca.entity;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using xgca.entity.Migrations;

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
        Task<entity.Models.Company> Retrieve(Guid key);
        Task<dynamic> ListByService(int serviceId, int page, int rowCount);
        Task<List<entity.Models.Company>> ListCompaniesByIDs(List<string> IDs);
    }

    public class ActorReturn
    {
        public Guid Guid { get; set; }
        public string CompanyName { get; set; }
        public string ImageUrl { get; set; }
        public string Addresses { get; set; }
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

        public async Task<dynamic> ListByService(int serviceId, int page, int rowCount)
        {
            var data = await _context.CompanyServices.AsNoTracking().Where(t => t.ServiceId == serviceId)
                .Include(c => c.Companies)
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
            var data = await _context.Companies.Include(a => a.Addresses).Select(
                c => new ActorReturn { Guid = c.Guid, CompanyName = c.CompanyName, ImageUrl = c.ImageURL, Addresses = c.Addresses.FullAddress })
                .AsNoTracking()
                .ToListAsync();

            var guests = await _context.Guests.Select(
                g => new ActorReturn { Guid = g.Id, CompanyName = g.GuestName, ImageUrl = g.Image, Addresses = g.AddressLine})
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

        public async Task<List<entity.Models.Company>> ListCompaniesByIDs(List<string> IDs)
        {
            var data = await _context.Companies.AsNoTracking()
                .Include(a => a.Addresses)
                .Include(c => c.ContactDetails)
                .Where(c => IDs.Contains(c.Guid.ToString()))
                .ToListAsync();

            return data;
        }
    }
}
