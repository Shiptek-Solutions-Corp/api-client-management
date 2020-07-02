using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using xgca.entity;
using xgca.entity.Models;

namespace xgca.data.CompanyGroupResource
{
    public interface ICompanyGroupResourceData
    {
        Task<bool> Create(entity.Models.CompanyGroupResource obj);
        Task<List<entity.Models.CompanyGroupResource>> List();
        Task<entity.Models.CompanyGroupResource> Retrieve(int key);
    }
    public class CompanyGroupResourceData : IMaintainable<entity.Models.CompanyGroupResource>, ICompanyGroupResourceData
    {
        private readonly IXGCAContext _context;
        public CompanyGroupResourceData(IXGCAContext context)
        {
            _context = context;
        }

        public async Task<bool> Create(entity.Models.CompanyGroupResource obj)
        {
            obj.Guid = Guid.NewGuid();
            obj.CreatedBy = 1;
            obj.CreatedOn = DateTime.UtcNow;
            obj.ModifiedOn = DateTime.UtcNow;

            _context.CompanyGroupResources.Add(obj);
            var result = await _context.SaveChangesAsync();

            return result > 0 ? true : false;
        }

        public async Task<List<entity.Models.CompanyGroupResource>> List()
        {
            var companyGroupResources = await _context
                .CompanyGroupResources
                .Include(x => x.CompanyServiceRole)
                .AsNoTracking()
                .ToListAsync();

            return companyGroupResources;
        }

        public async Task<entity.Models.CompanyGroupResource> Retrieve(int key)
        {
            var companyGroupResource = await _context.CompanyGroupResources
                .Include(x => x.CompanyServiceRole)
                .SingleOrDefaultAsync(x => x.CompanyGroupResourceId == key);

            return companyGroupResource;
        }

        public Task<bool> Update(entity.Models.CompanyGroupResource obj)
        {
            throw new NotImplementedException();
        }
    }
}
