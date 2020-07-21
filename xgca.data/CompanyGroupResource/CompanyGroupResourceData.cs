using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xgca.entity;
using xgca.entity.Models;

namespace xgca.data.CompanyGroupResource
{
    public interface ICompanyGroupResourceData
    {
        Task<bool> Create(entity.Models.CompanyGroupResource obj);
        Task<List<entity.Models.CompanyGroupResource>> List(int companyServiceRoleGuid);
        Task<entity.Models.CompanyGroupResource> Retrieve(int key);
        Task<bool> BlukCreate(List<entity.Models.CompanyGroupResource> obj);
        Task<bool> BulkDeleteByCompanyServiceRole(int companyServiceRoleId);


    }
    public class CompanyGroupResourceData : IMaintainable<entity.Models.CompanyGroupResource>, ICompanyGroupResourceData
    {
        private readonly IXGCAContext _context;
        public CompanyGroupResourceData(IXGCAContext context)
        {
            _context = context;
        }

        public async Task<bool> BlukCreate(List<entity.Models.CompanyGroupResource> obj)
        {
            _context.CompanyGroupResources.AddRange(obj);

            var result = await _context.SaveChangesAsync();

            return result > 0 ? true : false;
        }

        public async Task<bool> BulkDeleteByCompanyServiceRole(int companyServiceRoleId)
        {
            var data = _context.CompanyGroupResources
                .Where(c => c.CompanyServiceRoleId == companyServiceRoleId);

            var result = await _context.SaveChangesAsync();

            return result > 0 ? true : false;
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

        public async Task<List<entity.Models.CompanyGroupResource>> List(int companyServiceRoleGuid)
        {
            var predicate = PredicateBuilder.New<entity.Models.CompanyGroupResource>();
            if (companyServiceRoleGuid > 0)
            {
                predicate = predicate.And(cgr => cgr.CompanyServiceRoleId == companyServiceRoleGuid);
            }
            var companyGroupResources = await _context
                .CompanyGroupResources
                .Where(predicate)
                .Include(x => x.CompanyServiceRole)
                .AsNoTracking()
                .ToListAsync();

            return companyGroupResources;
        }

        public Task<List<entity.Models.CompanyGroupResource>> List()
        {
            throw new NotImplementedException();
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
