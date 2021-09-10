using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xgca.entity;
using xgca.entity.Models;

namespace xgca.data.Repositories
{
    public interface ISectionRepository : IRepository<Section>
    {

    }
    public class SectionRepository : ISectionRepository
    {
        private readonly IXGCAContext _context;
        public SectionRepository(IXGCAContext _context)
        {
            this._context = _context;
        }

        public Task<(Section, string)> Create(Section obj)
        {
            throw new NotImplementedException();
        }

        public Task<(bool, string)> Delete(Section obj)
        {
            throw new NotImplementedException();
        }

        public Task<(Section, string)> Get(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<(List<Section>, string)> List()
        {
            var records = await _context.Sections.AsNoTracking()
                .Where(x => x.IsDeleted == false && x.IsActive == true)
                .ToListAsync();

            return (records, "Sections listed successfully");
        }

        public Task<(Section, string)> Update(Section obj)
        {
            throw new NotImplementedException();
        }
    }
}
