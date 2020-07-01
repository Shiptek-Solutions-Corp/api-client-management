using Microsoft.EntityFrameworkCore;
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
        Task<entity.Models.MenuModule> Retrieve(int key);
        Task<List<entity.Models.MenuModule>> List();
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

        public async Task<List<entity.Models.MenuModule>> List()
        {
            var menuModules = await _context
                .MenuModules
                .AsNoTracking()
                .ToListAsync();

            return menuModules;
        }

        public async Task<entity.Models.MenuModule> Retrieve(int key)
        {
            var menuModule = await _context.MenuModules.SingleOrDefaultAsync(x => x.MenuModulesId == key);

            return menuModule;
        }

        public Task<bool> Update(entity.Models.MenuModule obj)
        {
            throw new NotImplementedException();
        }
    }
}
