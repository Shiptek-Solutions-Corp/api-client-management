using AutoMapper;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xgca.entity;

namespace xgca.data.PreferredProvider
{
    public class PreferredProviderData : IPreferredProviderData
    {
        private readonly IXGCAContext _context;

        public PreferredProviderData(IXGCAContext context)
        {
            _context = context;
        }

        public async Task<bool> Create(entity.Models.PreferredProvider obj)
        {
            _context.PreferredProviders.Add(obj);
            var result = await _context.SaveChangesAsync();
            return result > 0 ? true : false;
        }

        public async Task<bool> Delete(string key)
        {
            var provider = _context.PreferredProviders
                .Where(x => x.Guid == Guid.Parse(key)).FirstOrDefault();

            if (provider is null) { return false; }

            _context.PreferredProviders.Remove(provider);
            var result = await _context.SaveChangesAsync();

            return result > 0 ? true : false;
        }

        public async Task<List<entity.Models.PreferredProvider>> List()
        {
            var providers = await _context.PreferredProviders.AsNoTracking()
                .ToListAsync();

            return providers;
        }

        public async Task<List<entity.Models.PreferredProvider>> ListByServiceId(string serviceId, int pageNumber, int pageSize)
        {
            var providers = await _context.PreferredProviders.AsNoTracking()
                .Where(x => x.ServiceId == serviceId)
                .Skip(pageSize * pageNumber)
                .Take(pageSize)
                .ToListAsync();

            return providers;
        }

        public async Task<List<entity.Models.PreferredProvider>> ListByProfileId(int profileId)
        {
            var providers = await _context.PreferredProviders.AsNoTracking()
                .Where(x => x.ProfileId == profileId)
                .ToListAsync();

            return providers;
        }

        public async Task<List<entity.Models.PreferredProvider>> ListByProfileId(int profileId, int pageNumber, int pageSize)
        {
            var providers = await _context.PreferredProviders.AsNoTracking()
                .Where(x => x.ProfileId == profileId)
                .Skip(pageSize * pageNumber)
                .Take(pageSize)
                .ToListAsync();

            return providers;
        }

        public async Task<entity.Models.PreferredProvider> Retrieve(int key)
        {
            var provider = await _context.PreferredProviders
                .Where(x => x.ProfileId == key)
                .FirstOrDefaultAsync();

            return provider;
        }

        public async Task<bool> Update(entity.Models.PreferredProvider obj)
        {
            var provider = _context.PreferredProviders
                .Where(x => x.PreferredProviderId == obj.PreferredProviderId)
                .FirstOrDefault();

            if (provider is null) return false;

            provider.ServiceId = obj.ServiceId;
            provider.CompanyId = obj.CompanyId;
            provider.CompanyServiceId = obj.CompanyServiceId;
            provider.ProfileId = obj.ProfileId;
            provider.ModifiedBy = obj.ModifiedBy;
            provider.ModifiedOn = obj.ModifiedOn;

            var result = await _context.SaveChangesAsync();
            return result > 0 ? true : false;
        }

        public Task<List<entity.Models.PreferredProvider>> CreateAndReturnList(entity.Models.PreferredProvider provider)
        {
            throw new NotImplementedException();
        }

        public async Task<int> GetRecordCount()
        {
            int count = await _context.PreferredProviders.AsNoTracking().Where(x => x.IsDeleted == 0).CountAsync();
            return count;
        }

        public async Task<int> GetRecordCount(int profileId)
        {
            int count = await _context.PreferredProviders.AsNoTracking().Where(x => x.ProfileId == profileId && x.IsDeleted == 0).CountAsync();
            return count;
        }

        public async Task<List<string>> ListCompanyIdsByProfileId(int profileId)
        {
            var companyIds = await _context.PreferredProviders.AsNoTracking()
                .Where(x => x.ProfileId == profileId && x.IsDeleted == 0)
                .Select(i => i.CompanyId)
                .ToListAsync();

            return companyIds;
        }

        public async Task<bool> Create(List<entity.Models.PreferredProvider> providers)
        {
            _context.PreferredProviders.AddRange(providers);
            var result = await _context.SaveChangesAsync();
            return result > 0 ? true : false;
        }

        public async Task<bool> CheckIfExists(int profileId, string serviceId, string companyId, string companyServiceId)
        {
            var provider = await _context.PreferredProviders.AsNoTracking()
                .Where(x => x.ProfileId == profileId && x.ServiceId == serviceId && x.CompanyId == companyId && x.CompanyServiceId == companyServiceId)
                .FirstOrDefaultAsync();

            return (provider is null) ? false : true;
        }

        public async Task<List<string>> GetCompanyServiceIdByProfileId(int profileId)
        {
            var companyServiceIds = await _context.PreferredProviders.AsNoTracking()
                .Where(x => x.ProfileId == profileId)
                .Select(o => o.CompanyServiceId)
                .ToListAsync();

            return companyServiceIds;
        }

        public async Task<List<entity.Models.PreferredProvider>> GetPreferredProvidersByQuickSearch(int profileId, List<string> companyServiceIds, int pageNumber, int pageSize)
        {
            var providers = await _context.PreferredProviders.AsNoTracking()
                .Where(x => x.ProfileId == profileId && companyServiceIds.Contains(x.CompanyServiceId))
                .Skip(pageSize * pageNumber)
                .Take(pageSize)
                .ToListAsync();

            return providers;
        }

        public async Task<int> GetRecordCount(int profileId, List<string> compayServiceIds)
        {
            int recordCount = await _context.PreferredProviders.AsNoTracking()
                .Where(x => x.ProfileId == profileId && compayServiceIds.Contains(x.CompanyServiceId))
                .Select(o => o.PreferredProviderId)
                .CountAsync();

            return recordCount;
        }

        public async Task<List<string>> ListCompanyServiceIdsByProfileId(int profileId, List<entity.Models.PreferredProvider> filteredProviders)
        {
            List<int> filteredProviderIds = filteredProviders.Select(x => x.PreferredProviderId).ToList<int>();

            var predicate = PredicateBuilder.New<entity.Models.PreferredProvider>();

            if (filteredProviderIds.Count != 0)
            {
                predicate.And(x => filteredProviderIds.Contains(x.PreferredProviderId));
            }

            predicate = predicate.And(x => x.ProfileId == profileId);

            List<string> companyServiceIds = await _context.PreferredProviders.AsNoTracking()
                .Where(predicate)
                .Select(o => o.CompanyServiceId)
                .ToListAsync();

            return companyServiceIds;
        }

        public async Task<List<entity.Models.PreferredProvider>> List(int profileId, string companyGuid)
        {
            var providers = await _context.PreferredProviders.AsNoTracking()
                .Where(x => x.ProfileId == profileId && x.CompanyId == companyGuid)
                .ToListAsync();

            return providers;
        }
    }
}
