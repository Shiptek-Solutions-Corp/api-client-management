using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using xgca.core.Models.GroupResource;
using xgca.core.Response;
using xgca.data.GroupResource;

namespace xgca.core.GroupResource
{
    public interface IGroupResource
    {
        Task<IGeneralModel> Create(CreateGroupResource createGroupResource);
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
    }
}
