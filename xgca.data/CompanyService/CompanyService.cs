using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using xgca.entity;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using LinqKit;

namespace xgca.data.CompanyService
{
    public interface ICompanyService
    {
        Task<int> GetRecordCount();
        Task<int> GetRecordCount(int nonProviderId);
        Task<int> GetOtherProvidersRecordCount(int nonProviderId, int serviceId, List<Guid> existingIds, string search);
        Task<bool> Create(List<entity.Models.CompanyService> obj);
        Task<bool> Create(entity.Models.CompanyService obj);
        Task<List<entity.Models.CompanyService>> List(int pageNumber, int pageSize);
        Task<List<entity.Models.CompanyService>> ListServiceProviders(int nonProviderId, int pageNumber, int pageSize);
        Task<List<entity.Models.CompanyService>> ListServiceProviders(int serviceId, int nonProviderId, int pageNumber, int pageSize);
        Task<List<entity.Models.CompanyService>> ListServiceProviders(string search, int serviceId, int nonProviderId, int pageNumber, int pageSize, List<Guid> existingIds);
        Task<List<entity.Models.CompanyService>> ListPreferredProviders(string search, int serviceId, int nonProviderId, int pageNumber, int pageSize, List<Guid> existingIds);
        Task<List<entity.Models.CompanyService>> List(string search, int pageNumber, int pageSize);
        Task<List<entity.Models.CompanyService>> ListByCompanyId(int companyId);
        Task<List<entity.Models.CompanyService>> ListByCompanyId(int companyId, List<string> companyServiceGuids);
        Task<List<entity.Models.CompanyService>> ListByCompanyId(int companyId, int status);
        Task<entity.Models.CompanyService> Retrieve(int key);
        Task<entity.Models.CompanyService> Retrieve(string key);
        Task<int> GetIdByGuid(Guid guid);
        Task<bool> Update(entity.Models.CompanyService obj);
        Task<bool> Update(List<entity.Models.CompanyService> obj);
        Task<bool> ChangeStatus(entity.Models.CompanyService obj);
        Task<bool> Delete(int key);
        Task<int[]> GetUserByCompanyServiceGuid(Guid guid);
        Task<List<string>> QuickSearch(string search, List<string> companyServiceId);
        Task<List<entity.Models.CompanyService>> ListCompanyServicesByGuids(List<string> guids, List<KeyValuePair<string, string>> filterList, List<int> serviceIds);
        Task<List<string>> SearchAndFilterProvider(string search, List<KeyValuePair<string, string>> filterList, List<Guid> guids, List<int> serviceIds);
    }

    public class CompanyService : IMaintainable<entity.Models.CompanyService>, ICompanyService
    {
        private readonly IXGCAContext _context;

        public CompanyService(IXGCAContext context)
        {
            _context = context;
        }

        public async Task<bool> Create(List<entity.Models.CompanyService> obj)
        {
            foreach (entity.Models.CompanyService companyService in obj)
            {
                await _context.CompanyServices.AddAsync(companyService);
            }
            var result = await _context.SaveChangesAsync();
            return result > 0 ? true : false;
        }
        public async Task<List<entity.Models.CompanyService>> List()
        {
            var data = await _context.CompanyServices
                .Include(c => c.Companies)
                .Where(c => c.IsDeleted == 0).ToListAsync();
            return data;
        }

        public async Task<List<entity.Models.CompanyService>> ListByCompanyId(int companyId)
        {
            var data = await _context.CompanyServices
                .Where(c => c.CompanyId == companyId && c.IsDeleted == 0).ToListAsync();
            return data;
        }
        public async Task<List<entity.Models.CompanyService>> ListByCompanyId(int companyId, int status)
        {
            var data = await _context.CompanyServices
                .Where(c => c.CompanyId == companyId && c.Status == 1 && c.IsDeleted == 0).ToListAsync();
            return data;
        }
        public async Task<entity.Models.CompanyService> Retrieve(int key)
        { 
            var data = await _context.CompanyServices
                .Include(c => c.Companies)
                .Where(cs => cs.CompanyServiceId == key && cs.IsDeleted == 0).FirstOrDefaultAsync();
            return data;
        }
        public async Task<int> GetIdByGuid(Guid guid)
        {
            var data = await _context.CompanyServices
                .Where(c => c.Guid == guid && c.IsDeleted == 0).FirstOrDefaultAsync();
            if (data == null) { return 0; }
            return data.CompanyServiceId;
        }
        public async Task<bool> Update(List<entity.Models.CompanyService> obj)
        {
            foreach (entity.Models.CompanyService companyService in obj)
            {
                var data = await _context.CompanyServices
                    .Where(c => c.CompanyServiceId == companyService.CompanyServiceId && c.IsDeleted == 0)
                    .FirstOrDefaultAsync();
                if (data == null)
                {
                    return false;
                }
                data.CompanyId = companyService.CompanyId;
                data.ServiceId = companyService.ServiceId;
                data.Status = companyService.Status;
                data.ModifiedBy = companyService.ModifiedBy;
                data.ModifiedOn = DateTime.UtcNow;
            }
            var result = await _context.SaveChangesAsync();
            return result > 0 ? true : false;
        }
        public async Task<bool> ChangeStatus(entity.Models.CompanyService obj)
        {
            var data = await _context.CompanyServices
                .Where(c => c.CompanyServiceId == obj.CompanyServiceId && c.IsDeleted == 0).FirstOrDefaultAsync();
            if (data == null)
            { return false; }
            data.Status = obj.Status;
            data.ModifiedBy = obj.ModifiedBy;
            data.ModifiedOn = obj.ModifiedOn;
            var result = await _context.SaveChangesAsync();
            return result > 0 ? true : false;
        }
        public async Task<bool> Delete(int key)
        {
            var data = await _context.CompanyServices.Where(c => c.CompanyServiceId == key && c.IsDeleted == 0).FirstOrDefaultAsync();
            if (data == null)
            {
                return false;
            }
            data.IsDeleted = 1;
            data.ModifiedOn = DateTime.UtcNow;
            var result = await _context.SaveChangesAsync();
            return result > 0 ? true : false;
        }
        public async Task<bool> Create(entity.Models.CompanyService obj)
        {
            obj.Guid = Guid.NewGuid();
            await _context.CompanyServices.AddAsync(obj);
            var result = await _context.SaveChangesAsync();
            return result > 0 ? true : false;
        }
        public Task<bool> Update(entity.Models.CompanyService obj)
        {
            throw new NotImplementedException();
        }
        public async Task<int[]> GetUserByCompanyServiceGuid(Guid guid)
        {
            var result = await _context.CompanyServices
                .Where(cs => cs.Guid == guid)
                .SelectMany(cs => cs.CompanyServiceUsers)
                .Select(cs => cs.CompanyUsers)
                .Select(cu => cu.Users)
                .Select(cu => cu.UserId)
                .ToArrayAsync();

            return result;
        }
        public async Task<List<entity.Models.CompanyService>> List(int pageNumber, int pageSize)
        {
            var companyServices = await _context.CompanyServices.AsNoTracking()
                .Include(x => x.Companies)
                    .ThenInclude(a => a.Addresses)
                .Where(x => x.IsDeleted == 0 && x.Status == 1)
                .Skip(pageSize * pageNumber)
                .Take(pageSize)
                .ToListAsync();

            return companyServices;
        }

        public Task<List<entity.Models.CompanyService>> List(string columnFilter, int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }

        public async Task<int> GetRecordCount()
        {
            int count = await _context.CompanyServices.AsNoTracking()
                .Where(x => x.IsDeleted == 0 && x.Status == 1)
                .CountAsync();

            return count;
        }

        public async Task<int> GetRecordCount(int nonProviderId)
        {
            int count = await _context.CompanyServices.AsNoTracking()
                .Where(x => x.ServiceId != nonProviderId && x.IsDeleted == 0 && x.Status == 1)
                .CountAsync();

            return count;
        }

        public async Task<int> GetOtherProvidersRecordCount(int nonProviderId, int serviceId, List<Guid> existingIds, string search)
        {
            var predicate = PredicateBuilder.New<entity.Models.CompanyService>();

            if (!(search is null || search.Equals("")))
            {
                predicate = predicate.Or(x => (EF.Functions.Like(x.Companies.CompanyCode, $"%{search}%")));
                predicate = predicate.Or(x => (EF.Functions.Like(x.Companies.CompanyName, $"%{search}%")));
                predicate = predicate.Or(x => (EF.Functions.Like(x.Companies.Addresses.AddressLine, $"%{search}%")));
                predicate = predicate.Or(x => (EF.Functions.Like(x.Companies.Addresses.CityName, $"%{search}%")));
                predicate = predicate.Or(x => (EF.Functions.Like(x.Companies.Addresses.StateName, $"%{search}%")));
                predicate = predicate.Or(x => (EF.Functions.Like(x.Companies.Addresses.CountryName, $"%{search}%")));
            }

            if (nonProviderId != 0)
            {
                predicate = predicate.And(x => x.ServiceId != nonProviderId);
            }

            if (serviceId != 0)
            {
                predicate = predicate.And(x => x.ServiceId == serviceId);
            }

            if (!(existingIds is null) || existingIds.Count != 0)
            {
                predicate = predicate.And(x => !existingIds.Contains(x.Guid));
            }

            predicate = predicate.And(x => x.IsDeleted == 0 && x.Status == 1);

            int count = await _context.CompanyServices.AsNoTracking()
                .Include(i => i.Companies)
                    .ThenInclude(h => h.Addresses)
                .Where(predicate)
                .CountAsync();

            return count;
        }

        public async Task<entity.Models.CompanyService> Retrieve(string key)
        {
            var data = await _context.CompanyServices
                .Include(c => c.Companies)
                .Where(cs => cs.Guid == Guid.Parse(key) && cs.IsDeleted == 0).FirstOrDefaultAsync();
            return data;
        }

        public async Task<List<entity.Models.CompanyService>> ListServiceProviders(int nonProviderId, int pageNumber, int pageSize)
        {
            var companyServices = await _context.CompanyServices.AsNoTracking()
                .Include(x => x.Companies)
                    .ThenInclude(a => a.Addresses)
                .Where(x => x.ServiceId != nonProviderId && x.IsDeleted == 0 && x.Status == 1)
                .Skip(pageSize * pageNumber)
                .Take(pageSize)
                .ToListAsync();

            return companyServices;
        }

        public async Task<List<entity.Models.CompanyService>> ListServiceProviders(int serviceId, int nonProviderId, int pageNumber, int pageSize)
        {
            var companyServices = await _context.CompanyServices.AsNoTracking()
                .Include(x => x.Companies)
                    .ThenInclude(a => a.Addresses)
                .Where(x => x.ServiceId != nonProviderId && x.ServiceId == serviceId && x.IsDeleted == 0 && x.Status == 1)
                .Skip(pageSize * pageNumber)
                .Take(pageSize)
                .ToListAsync();

            return companyServices;
        }
        public async Task<List<entity.Models.CompanyService>> ListServiceProviders(string search, int serviceId, int nonProviderId, int pageNumber, int pageSize, List<Guid> existingIds)
        {
            var predicate = PredicateBuilder.New<entity.Models.CompanyService>();

            if (!(search is null || search.Equals("")))
            {
                predicate = predicate.Or(x => (EF.Functions.Like(x.Companies.CompanyCode, $"%{search}%")));
                predicate = predicate.Or(x => (EF.Functions.Like(x.Companies.CompanyName, $"%{search}%")));
                predicate = predicate.Or(x => (EF.Functions.Like(x.Companies.Addresses.AddressLine, $"%{search}%")));
                predicate = predicate.Or(x => (EF.Functions.Like(x.Companies.Addresses.CityName, $"%{search}%")));
                predicate = predicate.Or(x => (EF.Functions.Like(x.Companies.Addresses.StateName, $"%{search}%")));
                predicate = predicate.Or(x => (EF.Functions.Like(x.Companies.Addresses.CountryName, $"%{search}%")));
            }

            if (!(existingIds is null) || existingIds.Count != 0)
            {
                predicate = predicate.And(x => !existingIds.Contains(x.Guid));
            }

            if (serviceId != 0)
            {
                predicate = predicate.And(x => x.ServiceId == serviceId);
            }

            predicate = predicate.And(x => x.ServiceId != nonProviderId && x.IsDeleted == 0 && x.Status == 1);

            List<entity.Models.CompanyService> companyServices = await _context.CompanyServices.AsNoTracking()
                .Include(x => x.Companies)
                    .ThenInclude(a => a.Addresses)
                .Include(x => x.Companies)
                    .ThenInclude(c => c.ContactDetails)
                .Where(predicate)
                .Skip(pageSize * pageNumber)
                .Take(pageSize)
                .ToListAsync();

            return companyServices;
        }

        public async Task<List<entity.Models.CompanyService>> ListPreferredProviders(string search, int serviceId, int nonProviderId, int pageNumber, int pageSize, List<Guid> existingIds)
        {
            var predicate = PredicateBuilder.New<entity.Models.CompanyService>();

            if (!(search is null || search.Equals("")))
            {
                predicate = predicate.Or(x => (EF.Functions.Like(x.Companies.CompanyCode, $"%{search}%")));
                predicate = predicate.Or(x => (EF.Functions.Like(x.Companies.CompanyName, $"%{search}%")));
                predicate = predicate.Or(x => (EF.Functions.Like(x.Companies.Addresses.AddressLine, $"%{search}%")));
                predicate = predicate.Or(x => (EF.Functions.Like(x.Companies.Addresses.CityName, $"%{search}%")));
                predicate = predicate.Or(x => (EF.Functions.Like(x.Companies.Addresses.StateName, $"%{search}%")));
                predicate = predicate.Or(x => (EF.Functions.Like(x.Companies.Addresses.CountryName, $"%{search}%")));
            }

            if (!(existingIds is null) || existingIds.Count != 0)
            {
                predicate = predicate.And(x => existingIds.Contains(x.Guid));
            }

            if (serviceId != 0)
            {
                predicate = predicate.And(x => x.ServiceId == serviceId);
            }

            predicate = predicate.And(x => x.ServiceId != nonProviderId && x.IsDeleted == 0 && x.Status == 1);

            List<entity.Models.CompanyService> companyServices = await _context.CompanyServices.AsNoTracking()
                .Include(x => x.Companies)
                    .ThenInclude(a => a.Addresses)
                .Include(x => x.Companies)
                    .ThenInclude(c => c.ContactDetails)
                .Where(predicate)
                .ToListAsync();

            return companyServices;
        }

        public async Task<List<string>> QuickSearch(string search, List<string> companyServiceId)
        {
            var predicate = PredicateBuilder.New<entity.Models.CompanyService>();

            if (!(search is null || search.Equals("")))
            {
                predicate = predicate.Or(x => (EF.Functions.Like(x.Companies.CompanyCode, $"%{search}%")));
                predicate = predicate.Or(x => (EF.Functions.Like(x.Companies.CompanyName, $"%{search}%")));
                predicate = predicate.Or(x => (EF.Functions.Like(x.Companies.Addresses.AddressLine, $"%{search}%")));
                predicate = predicate.Or(x => (EF.Functions.Like(x.Companies.Addresses.CityName, $"%{search}%")));
                predicate = predicate.Or(x => (EF.Functions.Like(x.Companies.Addresses.StateName, $"%{search}%")));
                predicate = predicate.Or(x => (EF.Functions.Like(x.Companies.Addresses.CountryName, $"%{search}%")));
            }

            if (companyServiceId.Count != 0)
            {
                predicate = predicate.And(x => companyServiceId.Contains(x.Guid.ToString()));
            }

            predicate = predicate.And(x => x.IsDeleted == 0 && x.Status == 1);

            var data = await _context.CompanyServices.AsNoTracking()
                .Include(c => c.Companies)
                .Where(predicate)
                .Select(o => o.Guid.ToString())
                .ToListAsync();

            return data;
        }

        public async Task<List<entity.Models.CompanyService>> ListCompanyServicesByGuids(List<string> guids, List<KeyValuePair<string, string>> filterList, List<int> serviceIds)
        {
            var predicate = PredicateBuilder.New<entity.Models.CompanyService>();

            if (filterList.Count != 0)
            {
                foreach(var element in filterList)
                {
                    if (element.Key.ToLower().Equals("companyname") && !(element.Value.Equals("") || element.Value is null))
                    {
                        predicate = predicate.And(x => EF.Functions.Like(x.Companies.CompanyName, $"%{element.Value}%"));
                    }

                    if (element.Key.ToLower().Equals("country") && !(element.Value.Equals("") || element.Value is null))
                    {
                        predicate = predicate.And(x => EF.Functions.Like(x.Companies.Addresses.CountryName, $"%{element.Value}%"));
                    }

                    if (element.Key.ToLower().Equals("state") && !(element.Value.Equals("") || element.Value is null))
                    {
                        predicate = predicate.And(x => EF.Functions.Like(x.Companies.Addresses.StateName, $"%{element.Value}%") || EF.Functions.Like(x.Companies.Addresses.CityName, $"%{element.Value}%"));
                    }
                }
            }

            if (serviceIds.Count != 0)
            {
                predicate = predicate.Or(x => serviceIds.Contains(x.ServiceId));
            }

            predicate = predicate.And(x => guids.Contains(x.Guid.ToString()));

            List<entity.Models.CompanyService> providers = await _context.CompanyServices
                .Include(c => c.Companies)
                    .ThenInclude(cd => cd.ContactDetails)
                .Include(c => c.Companies)
                    .ThenInclude(a => a.Addresses)
                .Where(predicate)
                .ToListAsync();

            return providers;
        }

        public async Task<List<entity.Models.CompanyService>> ListByCompanyId(int companyId, List<string> companyServiceGuids)
        {
            var companyServices = await _context.CompanyServices.AsNoTracking()
                .Where(x => x.CompanyId == companyId && companyServiceGuids.Contains(x.Guid.ToString()))
                .ToListAsync();

            return companyServices;
        }

        public async Task<List<string>> SearchAndFilterProvider(string search, List<KeyValuePair<string, string>> filterList, List<Guid> guids, List<int> serviceIds)
        {
            var predicate = PredicateBuilder.New<entity.Models.CompanyService>();

            if (!(search is null || search.Equals("")))
            {
                predicate = predicate.Or(x => (EF.Functions.Like(x.Companies.CompanyCode, $"%{search}%")));
                predicate = predicate.Or(x => (EF.Functions.Like(x.Companies.CompanyName, $"%{search}%")));
                predicate = predicate.Or(x => (EF.Functions.Like(x.Companies.Addresses.AddressLine, $"%{search}%")));
                predicate = predicate.Or(x => (EF.Functions.Like(x.Companies.Addresses.CityName, $"%{search}%")));
                predicate = predicate.Or(x => (EF.Functions.Like(x.Companies.Addresses.StateName, $"%{search}%")));
                predicate = predicate.Or(x => (EF.Functions.Like(x.Companies.Addresses.CountryName, $"%{search}%")));
            }

            if (filterList.Count != 0)
            {
                foreach (var element in filterList)
                {
                    if (element.Key.ToLower().Equals("companyname") && !(element.Value.Equals("") || element.Value is null))
                    {
                        predicate = predicate.And(x => EF.Functions.Like(x.Companies.CompanyName, $"%{element.Value}%"));
                    }

                    if (element.Key.ToLower().Equals("country") && !(element.Value.Equals("") || element.Value is null))
                    {
                        predicate = predicate.And(x => EF.Functions.Like(x.Companies.Addresses.CountryName, $"%{element.Value}%"));
                    }

                    if (element.Key.ToLower().Equals("state") && !(element.Value.Equals("") || element.Value is null))
                    {
                        predicate = predicate.And(x => (EF.Functions.Like(x.Companies.Addresses.StateName, $"%{element.Value}%") || EF.Functions.Like(x.Companies.Addresses.CityName, $"%{element.Value}%")));
                    }
                }
            }

            if (serviceIds.Count != 0)
            {
                predicate = predicate.And(x => serviceIds.Contains(x.ServiceId));
            }

            predicate = predicate.And(x => guids.Contains(x.Guid));

            List<string> providers = await _context.CompanyServices
                .Include(c => c.Companies)
                    .ThenInclude(a => a.Addresses)
                .Where(predicate)
                .Select(x => x.Guid.ToString())
                .ToListAsync();

            return providers;

        }
    }
}
