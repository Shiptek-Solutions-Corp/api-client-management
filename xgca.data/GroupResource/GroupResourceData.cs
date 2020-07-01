using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using xgca.entity;

namespace xgca.data.GroupResource
{
    public interface IGroupResourceData
    {
        Task<bool> Create(entity.Models.GroupResource obj);
        Task<List<entity.Models.GroupResource>> List();
        Task<entity.Models.GroupResource> Retrieve(int key);

    }
    public class GroupResourceData : IGroupResourceData, IMaintainable<entity.Models.GroupResource>
    {
        private readonly IXGCAContext _context;
        public GroupResourceData(IXGCAContext context)
        {
            _context = context;
        }

        public async Task<bool> Create(entity.Models.GroupResource obj)
        {
            await _context.GroupResources.AddAsync(obj);
            var result = await _context.SaveChangesAuditable();
            return result > 0 ? true : false;
        }

        public async Task<List<entity.Models.GroupResource>> List()
        {
            var moduleGroups = await _context
                .GroupResources
                .Include(x => x.ModuleGroup)
                .Include(x => x.CompanyGroupResources).ThenInclude(x => x.CompanyServiceRole)
                .AsNoTracking()
                .ToListAsync();

            return moduleGroups;
        }

        public async Task<entity.Models.GroupResource> Retrieve(int key)
        {
            var moduleGroup = await _context
                .GroupResources
                .Include(x => x.ModuleGroup)
                .Include(x => x.CompanyGroupResources)
                .SingleOrDefaultAsync(x => x.GroupResourceId == key);

            return moduleGroup;
        }

        public Task<bool> Update(entity.Models.GroupResource obj)
        {
            throw new NotImplementedException();
        }
    }
}
