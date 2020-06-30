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

        public Task<List<entity.Models.ModuleGroup>> List()
        {
            throw new NotImplementedException();
        }

        public Task<entity.Models.ModuleGroup> Retrieve(int key)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(entity.Models.ModuleGroup obj)
        {
            throw new NotImplementedException();
        }
    }
}
