using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using xgca.entity;

namespace xgca.data.ModuleGroup
{
    public interface IModuleGroupData
    {
        Task<bool> Create(entity.Models.ModuleGroup obj);
        Task<List<entity.Models.ModuleGroup>> List();
        Task<entity.Models.ModuleGroup> Retrieve(int key);

    }
    public class ModuleGroupData : IModuleGroupData, IMaintainable<entity.Models.ModuleGroup>
    {
        private readonly IXGCAContext _xGCAContext;
        public ModuleGroupData(IXGCAContext xGCAContext)
        {
            _xGCAContext = xGCAContext;
        }

        public async Task<bool> Create(entity.Models.ModuleGroup obj)
        {
            _xGCAContext.ModuleGroups.Add(obj);
            var result = await _xGCAContext.SaveChangesAuditable();

            return result > 0 ? true : false;
        }

        public async Task<List<entity.Models.ModuleGroup>> List()
        {
            var moduleGroups = await _xGCAContext
                .ModuleGroups
                .AsNoTracking()
                .ToListAsync();

            return moduleGroups;
        }

        public async Task<entity.Models.ModuleGroup> Retrieve(int key)
        {
            var moduleGroup = await _xGCAContext.ModuleGroups.SingleOrDefaultAsync(x => x.ModuleGroupsId == key);
            
            return moduleGroup;
        }

        public Task<bool> Update(entity.Models.ModuleGroup obj)
        {
            throw new NotImplementedException();
        }
    }
}
