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

        public Task<List<entity.Models.GroupResource>> List()
        {
            throw new NotImplementedException();
        }

        public Task<entity.Models.GroupResource> Retrieve(int key)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(entity.Models.GroupResource obj)
        {
            throw new NotImplementedException();
        }
    }
}
