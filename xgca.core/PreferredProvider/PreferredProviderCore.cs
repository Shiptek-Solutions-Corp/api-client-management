using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using xgca.data.CompanyService;
using xgca.core.Response;
using xgca.data.PreferredProvider;
using xgca.core.Helpers.Http;
using Microsoft.Extensions.Options;
using xgca.core.Helpers;
using xgca.core.Constants;
using AutoMapper;
using xgca.core.Models.PreferredProvider;
using xgca.data.User;
using xgca.core.Models.Service;
using xgca.data.Company;
using System.Linq;
using xgca.core.Helpers.QueryFilter;
using xgca.core.Helpers.PreferredProvider;

namespace xgca.core.PreferredProvider
{
    public class PreferredProviderCore : IPreferredProviderCore
    {
        private readonly IPreferredProviderData _preferredProvider;
        private readonly ICompanyData _company;
        private readonly ICompanyService _companyService;
        private readonly IUserData _user;
        private readonly IHttpHelper _httpHelper;
        private readonly IMapper _mapper;
        private readonly IOptions<GlobalCmsService> _options;
        private readonly IPagedResponse _pagedResponse;
        private readonly IGeneral _general;
        private readonly IQueryFilterHelper _query;
        private readonly IPreferredProviderHelper _prefProv;
        public PreferredProviderCore(IPreferredProviderData preferredProvider, ICompanyData company, ICompanyService companyService, IUserData user,
            IHttpHelper httpHelper, IMapper mapper, IOptions<GlobalCmsService> options, IPagedResponse pagedResponse, IGeneral general, IQueryFilterHelper query,
            IPreferredProviderHelper prefProv)
        {
            _company = company;
            _companyService = companyService;
            _preferredProvider = preferredProvider;
            _user = user;
            _httpHelper = httpHelper;
            _mapper = mapper;
            _options = options;
            _pagedResponse = pagedResponse;
            _general = general;
            _query = query;
            _prefProv = prefProv;
        }

        public async Task<IGeneralModel> AddPreferredProviders(BatchCreatePreferredProvider providers, int profileId, string createdBy)
        {
            var companyService = await _companyService.Retrieve(providers.Providers[0].CompanyServiceId);
            if (companyService == null)
            {
                return _general.Response(null, 400, "Provider does not exists", false);
            }

            foreach(var provider in providers.Providers)
            {
                var isExists = await _preferredProvider.CheckIfExists(profileId, provider.ServiceId, provider.CompanyId, provider.CompanyServiceId);

                if (isExists)
                {
                    return _general.Response(null, 400, "Provider already exists in preferred list", false);
                }
            }

            List<entity.Models.PreferredProvider> newProviders = new List<entity.Models.PreferredProvider>();

            foreach(var provider in providers.Providers)
            {
                int createdById = await _user.GetIdByUsername(createdBy);

                var mappedModel = _mapper.Map<entity.Models.PreferredProvider>(provider);
                mappedModel.ProfileId = profileId;
                mappedModel.CreatedBy = (createdById == 0) ? GlobalVariables.SystemUserId : createdById;
                mappedModel.CreatedOn = DateTime.UtcNow;
                mappedModel.ModifiedBy = (createdById == 0) ? GlobalVariables.SystemUserId : createdById;
                mappedModel.ModifiedOn = DateTime.UtcNow;
                mappedModel.Guid = Guid.NewGuid();

                newProviders.Add(mappedModel);
            }

            var result = await _preferredProvider.Create(newProviders);

            return result
                ? _general.Response(null, 200, "Provider added on preferred list", true)
                : _general.Response(null, 400, "Error on adding provider to preferred list", false);

        }

        public async Task<IGeneralModel> DeleteProvider(string key)
        {
            var result = await _preferredProvider.Delete(key);
            if(!result)
            {
                return _general.Response(null, 400, "Preferred provider does not exists or may have already been deleted", false);
            }

            return _general.Response(null, 200, "Preferred provider deleted successfully", true);
        }

        public async Task<IGeneralModel> List(int profileId, string filters, string sortBy, string sortOrder, int pageNumber, int pageSize, int recordCount)
        {
            recordCount = await _preferredProvider.GetRecordCount(profileId);
            var result = await _preferredProvider.ListByProfileId(profileId, pageNumber, pageSize);

            if (result is null)
            {
                return _general.Response(null, 200, "No preferred providers found", true);
            }

            var serviceResponse = await _httpHelper.Get(_options.Value.BaseUrl, _options.Value.GetService, null, AuthToken.Contra);
            string statusCode = serviceResponse.statusCode;

            if (!(statusCode.Equals("200")))
            {
                return _general.Response(null, 400, "Error in fetching services", false);
            }

            List<ListServiceModel> services = new List<ListServiceModel>();
            foreach (var service in serviceResponse.data.services)
            {
                services.Add(new ListServiceModel
                {
                    IntServiceId = service.intServiceId,
                    ServiceId = service.serviceId,
                    ServiceCode = service.serviceCode,
                    ServiceName = service.serviceName,
                    ServiceImageURL = service.imageURL
                });
            }

            List<string> companyServiceGuids = await _preferredProvider.ListCompanyServiceIdsByProfileId(profileId, result);
            List<KeyValuePair<string, string>> filterList = new List<KeyValuePair<string, string>>();
            filterList = _query.ParseFilter(filters);
            int serviceId = 0;
            foreach (var filter in filterList)
            {
                if (filter.Key.ToLower().Equals("service"))
                {
                    var service = services.Find(x => x.ServiceName == filter.Value);
                    serviceId = (service is null) ? 0 : service.IntServiceId;
                }
            }
            var companiesServices = await _companyService.ListCompanyServicesByGuids(companyServiceGuids, filterList, serviceId);

            List<ListPreferredProvider> providers = new List<ListPreferredProvider>();
            foreach (var provider in companiesServices)
            {
                var service = services.Find(x => x.IntServiceId == provider.ServiceId);
                var company = companiesServices.Find(x => x.Companies.Guid.ToString().ToLower() == provider.Companies.Guid.ToString().ToLower());

                var preferredProvider = result.Find(x => x.CompanyServiceId.ToLower() == provider.Guid.ToString().ToLower()
                    && x.CompanyId.ToLower() == provider.Companies.Guid.ToString().ToLower()
                    && x.ServiceId.ToLower() == service.ServiceId.ToLower()
                    && x.ProfileId == profileId);

                providers.Add(new ListPreferredProvider
                {
                    PreferredProvider = preferredProvider.Guid.ToString(),
                    CompanyServiceId = provider.Guid.ToString(),
                    CompanyId = provider.Companies.Guid.ToString(),
                    CompanyName = company.Companies.CompanyName,
                    CompanyImageURL = (company.Companies.ImageURL is null) ? "No Image" : company.Companies.ImageURL,
                    CompanyAddress = CompanyHelper.ParseCompanydAddress(company.Companies),
                    CityProvince = AddressHelper.GetCityState(company.Companies.Addresses),
                    Country = company.Companies.Addresses.CountryName,
                    ServiceId = (service is null) ? "N/A" : service.ServiceId,
                    ServiceName = (service is null) ? "N/A" : service.ServiceName,
                });
            }

            providers = _prefProv.SortProviders(providers, sortBy, sortOrder);

            pageSize = (pageSize > recordCount) ? recordCount : pageSize;

            var pagedResponse = _pagedResponse.Paginate(providers, recordCount, pageNumber, pageSize);
            return _general.Response(pagedResponse, 200, "Configurable providers has been listed", true);
        }

        public async Task<IGeneralModel> QuickSearch(string search, int profileId, string filters, string sortBy, string sortOrder, int pageNumber, int pageSize, int recordCount)
        {
            var companyServiceIds = await _preferredProvider.GetCompanyServiceIdByProfileId(profileId);

            var filteredIds = await _companyService.QuickSearch(search, companyServiceIds);

            if (filteredIds is null)
            {
                return _general.Response(null, 200, "Configurable preferred providers have been listed", true);
            }

            var serviceResponse = await _httpHelper.Get(_options.Value.BaseUrl, _options.Value.GetService, null, AuthToken.Contra);
            string statusCode = serviceResponse.statusCode;

            if (!(statusCode.Equals("200")))
            {
                return _general.Response(null, 400, "Error in fetching services", false);
            }

            List<ListServiceModel> services = new List<ListServiceModel>();
            foreach (var service in serviceResponse.data.services)
            {
                services.Add(new ListServiceModel
                {
                    IntServiceId = service.intServiceId,
                    ServiceId = service.serviceId,
                    ServiceCode = service.serviceCode,
                    ServiceName = service.serviceName,
                    ServiceImageURL = service.imageURL
                });
            }

            recordCount = await _preferredProvider.GetRecordCount(profileId, filteredIds);
            var filteredProviders = await _preferredProvider.GetPreferredProvidersByQuickSearch(profileId, filteredIds, pageNumber, pageSize);

            if (filteredProviders is null)
            {
                return _general.Response(null, 200, "Configurable preferred providers have been listed", true);
            }

            List<string> companyServiceGuids = filteredProviders.Select(x => x.CompanyServiceId).ToList<string>(); //await _preferredProvider.ListCompanyServiceIdsByProfileId(profileId, filteredProviders);
            List<KeyValuePair<string, string>> filterList = new List<KeyValuePair<string, string>>();
            filterList = _query.ParseFilter(filters);
            int serviceId = 0;
            foreach( var filter in filterList)
            {
                if (filter.Key.ToLower().Equals("service"))
                {
                    var service = services.Find(x => x.ServiceName == filter.Value);
                    serviceId = (service is null) ? 0 : service.IntServiceId;
                }
            }
            var companyServices = await _companyService.ListCompanyServicesByGuids(companyServiceGuids, filterList, serviceId);

            List<ListPreferredProvider> providers = new List<ListPreferredProvider>();
            foreach (var provider in companyServices)
            {
                var service = services.Find(x => x.IntServiceId == provider.ServiceId);
                var companyService = filteredProviders.Find(x => x.CompanyServiceId.ToString().ToLower() == provider.Guid.ToString().ToLower());

                var preferredProvider = filteredProviders.Find(x => x.CompanyServiceId.ToLower() == provider.Guid.ToString().ToLower()
                    && x.CompanyId.ToLower() == provider.Companies.Guid.ToString().ToLower()
                    && x.ServiceId.ToLower() == service.ServiceId.ToLower()
                    && x.ProfileId == profileId);

                providers.Add(new ListPreferredProvider
                {
                    PreferredProvider = preferredProvider.Guid.ToString(),
                    CompanyServiceId = provider.Guid.ToString(),
                    CompanyId = provider.Companies.Guid.ToString(),
                    CompanyName = provider.Companies.CompanyName,
                    CompanyImageURL = (provider.Companies.ImageURL is null) ? "No Image" : provider.Companies.ImageURL,
                    CompanyAddress = CompanyHelper.ParseCompanydAddress(provider.Companies),
                    CityProvince = AddressHelper.GetCityState(provider.Companies.Addresses),
                    State = provider.Companies.Addresses.StateName,
                    Country = provider.Companies.Addresses.CountryName,
                    ServiceId = (service is null) ? "N/A" : service.ServiceId,
                    ServiceName = (service is null) ? "N/A" : service.ServiceName,
                });
            }

            providers = _prefProv.SortProviders(providers, sortBy, sortOrder);

            pageSize = (pageSize > recordCount) ? recordCount : pageSize;

            var pagedResponse = _pagedResponse.Paginate(providers, recordCount, pageNumber, pageSize);
            return _general.Response(pagedResponse, 200, "Configurable providers has been listed", true);
        }
    }
}
