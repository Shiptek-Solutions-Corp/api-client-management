using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xgca.core.Models.MenuModule;
using xgca.core.Response;
using xgca.data.MenuModule;

namespace xgca.core.MenuModule
{
    public interface IMenuModule
    {
        Task<IGeneralModel> Create(CreateMenuModule createMenuModules);
        Task<IGeneralModel> Get(int id);
        Task<IGeneralModel> GetAll();
    }
    public class MenuModule : IMenuModule
    {
        private readonly IMenuModuleData _menuModuleData;
        private readonly IGeneral _general;
        private readonly IMapper _mapper;
        public MenuModule(IMenuModuleData menuModuleData, IGeneral general, IMapper mapper)
        {
            _menuModuleData = menuModuleData;
            _general = general;
            _mapper = mapper;
        }

        public async Task<IGeneralModel> Create(CreateMenuModule createMenuModule)
        {
            var menuModule = _mapper.Map<entity.Models.MenuModule>(createMenuModule);
            var result = await _menuModuleData.Create(menuModule);
            var viewMenuModule = _mapper.Map<GetMenuModule>(menuModule);

            return _general.Response(viewMenuModule, 200, "Created successfuly", true);
        }

        public async Task<IGeneralModel> Get(int id)
        {
            var moduleGroup = await _menuModuleData.Retrieve(id);
            if (moduleGroup != null)
            {
                var viewModuleGroup = _mapper.Map<GetMenuModule>(moduleGroup);
                return _general.Response(viewModuleGroup, 200, "Retreived successfuly", true);
            }
            return _general.Response(null, 400, "Invalid Menu Module", false);
        }

        public async Task<IGeneralModel> GetAll()
        {
            var result = await _menuModuleData.List();
            var viewModuleGroups = result.Select(d => _mapper.Map<GetMenuModule>(d)).ToList();

            return _general.Response(viewModuleGroups, 200, "Menu Modules listed successfuly", true);
        }
    }
}
