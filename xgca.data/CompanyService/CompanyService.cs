using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using xgca.entity;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace xgca.data.CompanyService
{
    public interface ICompanyService
    {
        Task<bool> Create(List<entity.Models.CompanyService> obj);
        Task<bool> Create(entity.Models.CompanyService obj);
        Task<List<entity.Models.CompanyService>> List();
        Task<List<entity.Models.CompanyService>> ListByCompanyId(int companyId);
        Task<List<entity.Models.CompanyService>> ListByCompanyId(int companyId, int status);
        Task<entity.Models.CompanyService> Retrieve(int key);
        Task<int> GetIdByGuid(Guid guid);
        Task<bool> Update(entity.Models.CompanyService obj);
        Task<bool> Update(List<entity.Models.CompanyService> obj);
        Task<bool> ChangeStatus(entity.Models.CompanyService obj);
        Task<bool> Delete(int key);
        Task<int[]> GetUserByCompanyServiceGuid(Guid guid);

    }

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
        public async Task<List<entity.Models.CompanyService>> ListByCompanyId(int companyId, int status)
        {
            var data = await _context.CompanyServices
                .Where(c => c.CompanyId == companyId && c.Status == 1 && c.IsDeleted == 0).ToListAsync();
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
                data.ModifiedOn = DateTime.UtcNow;
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
            data.ModifiedOn = DateTime.UtcNow;
            var result = await _context.SaveChangesAsync();
            return result > 0 ? true : false;
        }

        public async Task<bool> Create(entity.Models.CompanyService obj)
        {
            obj.Guid = Guid.NewGuid();
            await _context.CompanyServices.AddAsync(obj);
            var result = await _context.SaveChangesAsync();
            return result > 0 ? true : false;
        }
        public Task<bool> Update(entity.Models.CompanyService obj)
        {
            throw new NotImplementedException();
        }

        public async Task<int[]> GetUserByCompanyServiceGuid(Guid guid)
        {
            var result = await _context.CompanyServices
                .Where(cs => cs.Guid == guid)
                .SelectMany(cs => cs.CompanyServiceUsers)
                .Select(cs => cs.CompanyUsers)
                .Select(cu => cu.Users)
                .Select(cu => cu.UserId)
                .ToArrayAsync();

            return result;
        }
    }
}
