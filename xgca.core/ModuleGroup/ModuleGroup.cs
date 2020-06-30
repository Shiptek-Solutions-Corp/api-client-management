using AutoMapper;
using System;
using System.Collections.Generic;
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
    }
}
