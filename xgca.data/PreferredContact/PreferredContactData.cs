using Castle.Core.Internal;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using xgca.entity;
using AutoMapper;

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

        public async Task<bool> Delete(string key)
        {
            var contact = await _context.PreferredContacts
                .SingleOrDefaultAsync(x => x.Guid == Guid.Parse(key));

            if (contact is null) { return false; }

            _context.PreferredContacts.Remove(contact);
            var result = await _context.SaveChangesAsync();

            return result > 0 ? true : false;
        }

        public async Task<List<string>> GetGuestIds(int profileId)
        {
            var guestIds = await _context.PreferredContacts
                .Where(pc => pc.ProfileId == profileId && pc.ContactType == 2)
                .Select(x => x.GuestId)
                .ToListAsync();

            return guestIds;
        }

        public async Task<int> GetRecordCount()
        {
            int recordCount = await _context.PreferredContacts
                .Where(pc => pc.IsDeleted == 0)
                .Select(x => x.PreferredContactId)
                .CountAsync();

            return recordCount;
        }

        public async Task<int> GetRecordCount(int profileId)
        {
            int recordCount = await _context.PreferredContacts
                .Where(pc => pc.ProfileId == profileId && pc.IsDeleted == 0)
                .Select(x => x.PreferredContactId)
                .CountAsync();

            return recordCount;
        }

        public async Task<List<string>> GetRegisteredIds(int profileId)
        {
            var registeredIds = await _context.PreferredContacts
                .Where(pc => pc.ProfileId == profileId && pc.ContactType == 1)
                .Select(x => x.CompanyId)
                .ToListAsync();

            return registeredIds;
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

        public async Task<List<entity.Models.PreferredContact>> GetContactsByQuickSearch(int profileId, List<string> guestIds, List<string> registeredIds, int pageNumber, int pageSize)
        {
            var contacts = await _context.PreferredContacts.AsNoTracking()
                .Where(x => x.ProfileId == profileId &&  guestIds.Contains(x.GuestId) || registeredIds.Contains(x.CompanyId) && x.IsDeleted == 0)
                .Skip(pageSize * pageNumber)
                .Take(pageSize)
                .ToListAsync();

            return contacts;
        }

        public async Task<bool> CheckIfContactAlreadyAdded(string companyGuid, int profileId)
        {
            var contact = await _context.PreferredContacts.AsNoTracking().SingleOrDefaultAsync(x => x.CompanyId == companyGuid && x.ProfileId == profileId);

            return (contact is null) ? false : true;
        }
    }
}
