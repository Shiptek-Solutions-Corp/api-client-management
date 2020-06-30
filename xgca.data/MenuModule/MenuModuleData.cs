using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using xgca.entity;

namespace xgca.data.MenuModule
{
    public interface IMenuModuleData
    {
        Task<bool> Create(entity.Models.MenuModule obj);
    }

    public class MenuModuleData : IMenuModuleData, IMaintainable<entity.Models.MenuModule>
    {
        private readonly IXGCAContext _context;
        public MenuModuleData(IXGCAContext context)
        {
            _context = context;
        }

        public async Task<bool> Create(entity.Models.MenuModule obj)
        {
            _context.MenuModules.Add(obj);
            var result = await _context.SaveChangesAuditable();

            return result > 0 ? true : false;
        }

        public Task<List<entity.Models.MenuModule>> List()
        {
            throw new NotImplementedException();
        }

        public Task<entity.Models.MenuModule> Retrieve(int key)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(entity.Models.MenuModule obj)
        {
            throw new NotImplementedException();
        }
    }
}
