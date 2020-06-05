using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using xgca.entity;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace xgca.data.CompanyService
{
    public class CompanyService : IMaintainable<entity.Models.CompanyService>, ICompanyService
    {
        private readonly IXGCAContext _context;

        public CompanyService(IXGCAContext context)
        {
            _context = context;
        }

        public async Task<bool> Create(List<entity.Models.CompanyService> obj)
        {
            foreach (entity.Models.CompanyService companyService in obj)
            {
                await _context.CompanyServices.AddAsync(companyService);
            }
            var result = await _context.SaveChangesAsync();
            return result > 0 ? true : false;
        }
        public async Task<List<entity.Models.CompanyService>> List()
        {
            var data = await _context.CompanyServices
                .Include(c => c.Companies)
                .Where(c => c.IsDeleted == 0).ToListAsync();
            return data;
        }

        public async Task<List<entity.Models.CompanyService>> ListByCompanyId(int companyId)
        {
            var data = await _context.CompanyServices
                .Where(c => c.CompanyId == companyId && c.IsDeleted == 0).ToListAsync();
            return data;
        }
        public async Task<entity.Models.CompanyService> Retrieve(int key)
        { 
            var data = await _context.CompanyServices
                .Include(c => c.Companies)
                .Where(cs => cs.CompanyServiceId == key && cs.IsDeleted == 0).FirstOrDefaultAsync();
            return data;
        }
        public async Task<int> GetIdByGuid(Guid guid)
        {
            var data = await _context.CompanyServices
                .Where(c => c.Guid == guid && c.IsDeleted == 0).FirstOrDefaultAsync();
            if (data == null) { return 0; }
            return data.CompanyServiceId;
        }
        public async Task<bool> Update(List<entity.Models.CompanyService> obj)
        {
            foreach (entity.Models.CompanyService companyService in obj)
            {
                var data = await _context.CompanyServices
                    .Where(c => c.CompanyServiceId == companyService.CompanyServiceId && c.IsDeleted == 0)
                    .FirstOrDefaultAsync();
                if (data == null)
                {
                    return false;
                }
                data.CompanyId = companyService.CompanyId;
                data.ServiceId = companyService.ServiceId;
                data.Status = companyService.Status;
                data.ModifiedBy = companyService.ModifiedBy;
                data.ModifiedOn = DateTime.Now;
            }
            var result = await _context.SaveChangesAsync();
            return result > 0 ? true : false;
        }

        public async Task<bool> ChangeStatus(entity.Models.CompanyService obj)
        {
            var data = await _context.CompanyServices
                .Where(c => c.CompanyServiceId == obj.CompanyServiceId && c.IsDeleted == 0).FirstOrDefaultAsync();
            if (data == null)
            { return false; }
            data.Status = obj.Status;
            data.ModifiedBy = obj.ModifiedBy;
            data.ModifiedOn = obj.ModifiedOn;
            var result = await _context.SaveChangesAsync();
            return result > 0 ? true : false;
        }
        public async Task<bool> Delete(int key)
        {
            var data = await _context.CompanyServices.Where(c => c.CompanyServiceId == key && c.IsDeleted == 0).FirstOrDefaultAsync();
            if (data == null)
            {
                return false;
            }
            data.IsDeleted = 1;
            data.ModifiedOn = DateTime.Now;
            var result = await _context.SaveChangesAsync();
            return result > 0 ? true : false;
        }

        public Task<bool> Create(entity.Models.CompanyService obj)
        {
            throw new NotImplementedException();
        }
        public Task<bool> Update(entity.Models.CompanyService obj)
        {
            throw new NotImplementedException();
        }
    }
}
