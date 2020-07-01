using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xgca.core.Models.CompanyGroupResource;
using xgca.core.Models.GroupResource;
using xgca.core.Response;
using xgca.data.GroupResource;

namespace xgca.core.GroupResource
{
    public interface IGroupResource
    {
        Task<IGeneralModel> Create(CreateGroupResource createGroupResource);
        Task<IGeneralModel> Get(int id);
        Task<IGeneralModel> GetAll();

    }
    public class GroupResource : IGroupResource
    {
        private readonly IMapper _mapper;
        private readonly IGroupResourceData _groupResourceData;
        private readonly IGeneral _general;
        public GroupResource(IMapper mapper, IGroupResourceData groupResourceData, IGeneral general)
        {
            _mapper = mapper;
            _groupResourceData = groupResourceData;
            _general = general;
        }

        public async Task<IGeneralModel> Create(CreateGroupResource createGroupResource)
        {
            var groupResource = _mapper.Map<entity.Models.GroupResource>(createGroupResource);
            var result = await _groupResourceData.Create(groupResource);
            var viewGroupResource = _mapper.Map<GetGroupResource>(groupResource);

            return _general.Response(viewGroupResource, 200, "Created successfuly.", true);
        }

        public async Task<IGeneralModel> Get(int id)
        {
            var moduleGroup = await _groupResourceData.Retrieve(id);
            if (moduleGroup != null)
            {
                var viewModuleGroup = _mapper.Map<GetGroupResource>(moduleGroup);
                return _general.Response(viewModuleGroup, 200, "Retreived successfuly", true);
            }
            return _general.Response(null, 400, "Invalid Group Resource", false);
        }

        public async Task<IGeneralModel> GetAll()
        {
            var result = await _groupResourceData.List();
            var viewModuleGroups = result.Select(d => _mapper.Map<GetGroupResource>(d)).ToList();

            return _general.Response(viewModuleGroups, 200, "Group Resources listed successfuly", true);
        }
    }
}
