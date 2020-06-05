using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using xgca.entity;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace xgca.data.Company
{
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
            data.ModifiedOn = DateTime.Now;
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
            data.ModifiedOn = DateTime.Now;
            var result = await _context.SaveChangesAsync();
            return result > 0 ? true : false;
        }
    }
}
