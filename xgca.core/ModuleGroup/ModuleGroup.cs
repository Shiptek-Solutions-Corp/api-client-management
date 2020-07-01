using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xgca.core.Models.ModuleGroup;
using xgca.core.Response;
using xgca.data.ModuleGroup;

namespace xgca.core.ModuleGroup
{
    public interface IModuleGroup
    {
        Task<IGeneralModel> Create(CreateModuleGroup obj);
        Task<IGeneralModel> Get(int id);
        Task<IGeneralModel> GetAll();
    }

    public class ModuleGroup : IModuleGroup
    {
        private readonly IModuleGroupData _moduleGroupData;
        private readonly IMapper _mapper;
        private readonly IGeneral _general;
        public ModuleGroup(IModuleGroupData moduleGroupData, IMapper mapper, IGeneral general)
        {
            _moduleGroupData = moduleGroupData;
            _mapper = mapper;
            _general = general;
        }

        public async Task<IGeneralModel> Create(CreateModuleGroup createModuleGroup)
        {
            var companyModuleGroup = _mapper.Map<entity.Models.ModuleGroup>(createModuleGroup);
            var result = await _moduleGroupData.Create(companyModuleGroup);
            var viewCompanyModelGroup = _mapper.Map<GetModuleGroup>(companyModuleGroup);

            return _general.Response(viewCompanyModelGroup, 200, "Created successfuly", true);
        }

        public async Task<IGeneralModel> Get(int id)
        {
            var moduleGroup = await _moduleGroupData.Retrieve(id);
            if (moduleGroup != null)
            {
                var viewModuleGroup = _mapper.Map<GetModuleGroup>(moduleGroup);
                return _general.Response(viewModuleGroup, 200, "Retreived successfuly", true);
            }
            return _general.Response(null, 400, "Invalid Module Group", false);
        }

        public async Task<IGeneralModel> GetAll()
        {
            var result = await _moduleGroupData.List();
            var viewModuleGroups = result.Select(d => _mapper.Map<GetModuleGroup>(d)).ToList();

            return _general.Response(viewModuleGroups, 200, "Module Groups listed successfuly", true);
        }
    }
}
