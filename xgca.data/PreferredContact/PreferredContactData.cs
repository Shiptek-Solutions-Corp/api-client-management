using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using xgca.entity;

namespace xgca.data.PreferredContact
{
    public class PreferredContactData : IMaintainable<entity.Models.PreferredContact>, IPreferredContactData
    {
        private readonly IXGCAContext _context;

        public PreferredContactData(IXGCAContext context)
        {
            _context = context;
        }

        public async Task<bool> Create(entity.Models.PreferredContact obj)
        {
            await _context.PreferredContacts.AddAsync(obj);
            var result = await _context.SaveChangesAsync();
            return result > 0 ? true : false;
        }

        public async Task<int> GetRecordCount()
        {
            int recordCount = await _context.PreferredContacts
                .Where(pc => pc.IsDeleted == 0)
                .CountAsync();

            return recordCount;
        }

        public async Task<int> GetRecordCount(int profileId)
        {
            int recordCount = await _context.PreferredContacts
                .Where(pc => pc.ProfileId == profileId && pc.IsDeleted == 0)
                .CountAsync();

            return recordCount;
        }

        public async Task<List<entity.Models.PreferredContact>> List()
        {
            var contacts = await _context.PreferredContacts
                .Where(pc => pc.IsDeleted == 0)
                .ToListAsync();

            return contacts;
        }

        public async Task<List<entity.Models.PreferredContact>> List(int profileId)
        {
            var contacts = await _context.PreferredContacts
                .Where(pc => pc.ProfileId == profileId && pc.IsDeleted == 0)
                .ToListAsync();

            return contacts;

        }

        public async Task<List<entity.Models.PreferredContact>> List(int profileId, int pageNumber, int pageSize)
        {
            var contacts = await _context.PreferredContacts
                .Where(pc => pc.ProfileId == profileId && pc.IsDeleted == 0)
                .Skip(pageSize * pageNumber)
                .Take(pageSize)
                .ToListAsync();

            return contacts;
        }

        public Task<List<entity.Models.PreferredContact>> List(int profileId, string columnFilter, int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task<entity.Models.PreferredContact> Retrieve(int key)
        {
            throw new NotImplementedException();
        }

        public async Task<entity.Models.PreferredContact> Retrieve(Guid guid)
        {
            var contact = await _context.PreferredContacts
                .Where(pc => pc.Guid == guid && pc.IsDeleted == 0)
                .FirstOrDefaultAsync();

            return contact;
        }

        public Task<bool> Update(entity.Models.PreferredContact obj)
        {
            throw new NotImplementedException();
        }
    }
}
