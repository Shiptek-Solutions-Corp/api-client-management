using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using xgca.core.Models.CompanyService;
using xgca.core.Response;
using xgca.core.Company;
using xgca.core.Helpers;
using xgca.core.CompanyUser;
using xgca.core.Helpers.Http;
using xgca.core.Constants;
using xgca.core.Models.Service;
using xgca.data.PreferredProvider;
using Castle.Core.Internal;

namespace xgca.core.CompanyService
{
    public class CompanyService : ICompanyService
    {
        private readonly xgca.data.CompanyService.ICompanyService _companyService;
        private readonly xgca.data.Company.ICompanyData _company;
        private readonly xgca.core.User.IUser _coreUser;
        private readonly ICompanyUser _coreCompanyUser;

        private readonly IHttpHelper _httpHelpers;
        private readonly IOptions<GlobalCmsService> _options;
        private readonly IPagedResponse _pagedResponse;
        private readonly IPreferredProviderData _preferredProvider;
        private readonly IGeneral _general;

        public CompanyService(xgca.data.CompanyService.ICompanyService companyService,
            xgca.data.Company.ICompanyData company,
            xgca.core.User.IUser coreUser, ICompanyUser coreCompanyUser,
            IHttpHelper httpHelpers,
            IOptions<GlobalCmsService> options, IPagedResponse pagedResponse,
            IPreferredProviderData preferredProvider, IGeneral general)
        {
            _companyService = companyService;
            _company = company;
            _coreUser = coreUser;
            _coreCompanyUser = coreCompanyUser;
            _httpHelpers = httpHelpers;
            _options = options;
            _pagedResponse = pagedResponse;
            _preferredProvider = preferredProvider;
            _general = general;
        }
        public async Task<IGeneralModel> ListByCompanyId(string key)
        {
            int companyId = await _company.GetIdByGuid(Guid.Parse(key));
            var result = await _companyService.ListByCompanyId(companyId);
            var companyServices = result.Select(t => new { CompanyServiceId = t.Guid, t.ServiceId, t.Status });

            List<ListCompanyServiceModel> data = new List<ListCompanyServiceModel>();
            foreach(var companyService in companyServices)
            {
                var serviceKey = await _httpHelpers.GetGuidById(_options.Value.BaseUrl, $"{_options.Value.GetService}/", companyService.ServiceId, AuthToken.Contra);
                string serviceId = serviceKey.data.serviceId;
                var serviceResponse = await _httpHelpers.CustomGet(_options.Value.BaseUrl, _options.Value.GetServiceDetails.Replace("{serviceId}", serviceId), AuthToken.Contra);
                var json = (JObject)serviceResponse;
                data.Add(new ListCompanyServiceModel
                {
                    CompanyServiceId = companyService.CompanyServiceId.ToString(),
                    ServiceId = (json)["data"]["service"]["serviceId"].ToString(),
                    Code = (json)["data"]["service"]["code"].ToString(),
                    Name = (json)["data"]["service"]["name"].ToString(),
                    ImageURL = (json)["data"]["service"]["imageUrl"].ToString(),
                    StaticId = Convert.ToByte((json)["data"]["service"]["staticId"]),
                    Status = companyService.Status
                });
            }

            return _general.Response(new { companyService = data }, 200, "Configurable services for selected company has been listed", true);
        }
        public async Task<IGeneralModel> ListByCompanyUserId(int referenceId)
        {
            int companyId = await _coreCompanyUser.GetCompanyIdByUserId(referenceId);
            var result = await _companyService.ListByCompanyId(companyId, GlobalVariables.ActiveServices);
            var companyServices = result.Select(t => new { CompanyServiceId = t.Guid, t.ServiceId, t.Status });

            List<ListCompanyServiceModel> data = new List<ListCompanyServiceModel>();

            foreach (var companyService in companyServices)
            {
                var serviceKey = await _httpHelpers.GetGuidById(_options.Value.BaseUrl, $"{_options.Value.GetService}/", companyService.ServiceId, AuthToken.Contra);
                string serviceId = serviceKey.data.serviceId;
                var serviceResponse = await _httpHelpers.CustomGet(_options.Value.BaseUrl, _options.Value.GetServiceDetails.Replace("{serviceId}", serviceId), AuthToken.Contra);

                var json = (JObject)serviceResponse;

                data.Add(new ListCompanyServiceModel
                {
                    CompanyServiceId = companyService.CompanyServiceId.ToString(),
                    ServiceId = (json)["data"]["service"]["serviceId"].ToString(),
                    Code = (json)["data"]["service"]["code"].ToString(),
                    Name = (json)["data"]["service"]["name"].ToString(),
                    ImageURL = (json)["data"]["service"]["imageUrl"].ToString(),
                    StaticId = Convert.ToByte((json)["data"]["service"]["staticId"])
                });
            }

            return _general.Response(new { companyServices = data }, 200, "Configurable services for selected company has been listed", true);
        }
        public async Task<IGeneralModel> Create(CreateCompanyServiceModel obj)
        {
            int companyId = await _company.GetIdByGuid(Guid.Parse(obj.CompanyId));
            var service = await _httpHelpers.CustomGet(_options.Value.BaseUrl, _options.Value.GetServiceDetails.Replace("{serviceId}", obj.ServiceId), AuthToken.Contra);
            int userId = 1;
            var companyService = new entity.Models.CompanyService
            {
                CompanyId = companyId,
                ServiceId = service.data.ServiceId,
                Status = 1,
                CreatedBy = userId,
                CreatedOn = DateTime.UtcNow,
                ModifiedBy = userId,
                ModifiedOn = DateTime.UtcNow,
                Guid = Guid.NewGuid()
            };

            var result = await _companyService.Create(companyService);
            return result
                ? _general.Response(true, 200, "Company service created", true)
                : _general.Response(false, 400, "Error on creating company service", true);
        }
        public async Task<bool> CreateBatch(dynamic services, int companyId, int userId)
        {
            List<entity.Models.CompanyService> companyServices = new List<entity.Models.CompanyService>();
            foreach(string serviceKey in services)
            {
                var service = await _httpHelpers.GetIdByGuid(_options.Value.BaseUrl, $"{_options.Value.GetService}/" , serviceKey, AuthToken.Contra);
                companyServices.Add(new entity.Models.CompanyService
                {
                    ServiceId = service.data.serviceId,
                    CompanyId = companyId,
                    Status = 1,
                    CreatedBy = userId,
                    CreatedOn = DateTime.UtcNow,
                    ModifiedBy = userId,
                    ModifiedOn = DateTime.UtcNow,
                    Guid = Guid.NewGuid()
                });
            }
            var result = await _companyService.Create(companyServices);
            return result; ;
        }
        public async Task<IGeneralModel> Update(UpdateCompanyServiceModel obj)
        {
            if (obj == null)
            { return _general.Response(null, 400, "Data cannot be null", true); }

            int userId = 1;
            int companyServiceId = await _companyService.GetIdByGuid(Guid.Parse(obj.CompanyServiceId));
            int companyId = await _company.GetIdByGuid(Guid.Parse(obj.CompanyId));

            var companyService = new entity.Models.CompanyService
            {
                CompanyServiceId = companyServiceId,
                CompanyId = companyId,
                Status = obj.Status,
                CreatedBy = userId,
                CreatedOn = DateTime.UtcNow
            };

            var companyServiceResult = await _companyService.Update(companyService);
            return companyServiceResult
                ? _general.Response(true, 200, "Company service updated", true)
                : _general.Response(false, 400, "Error on updating company service", true);
        }
        public async Task<bool> UpdateBatch(dynamic services, int companyId, int userId)
        {
            bool result = false;
            List<entity.Models.CompanyService> createServices = new List<entity.Models.CompanyService>();
            List<entity.Models.CompanyService> updateServices = new List<entity.Models.CompanyService>();
            foreach (dynamic service in services)
            {
                var serviceObj = (JObject)service;
                string serviceKey = (serviceObj)["serviceId"].ToString();
                var serviceResponse = await _httpHelpers.GetIdByGuid(_options.Value.BaseUrl, $"{_options.Value.GetService}/", serviceKey, AuthToken.Contra);
                var serviceJson = (JObject)serviceResponse;
                string companyServiceKey = (serviceObj)["companyServiceId"].ToString();
                if (companyServiceKey == "NEW")
                {
                    createServices.Add(new entity.Models.CompanyService
                    {
                        CompanyId = companyId,
                        ServiceId = Convert.ToInt32((serviceJson)["data"]["serviceId"]),
                        Guid = Guid.NewGuid(),
                        Status = 1,
                        CreatedBy = userId,
                        CreatedOn = DateTime.UtcNow,
                        ModifiedBy = userId,
                        ModifiedOn = DateTime.UtcNow,
                    });
                }
                else
                {
                    int companyServiceId = await _companyService.GetIdByGuid(Guid.Parse(companyServiceKey));
                    updateServices.Add(new entity.Models.CompanyService
                    {
                        CompanyServiceId = companyServiceId,
                        CompanyId = companyId,
                        ServiceId = Convert.ToInt32((serviceJson)["data"]["serviceId"]),
                        Status = Convert.ToByte((serviceObj)["status"]),
                        ModifiedBy = userId,
                        ModifiedOn = DateTime.UtcNow,
                    });
                }
            }

            if (createServices != null)
            { result = await _companyService.Create(createServices); }
            if (updateServices != null)
            { result = await _companyService.Update(updateServices); }
            return result;
        }

        /*public async Task<IGeneralModel> ListProviders(int companyId, string serviceId, int pageNumber, int pageSize, int recordCount)
        {
            var serviceResponse = await _httpHelpers.Get(_options.Value.BaseUrl, _options.Value.GetService, null, AuthToken.Contra);
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
                    ServiceImageURL = service.imageURL,
                    ServiceStaticId = service.serviceStaticId
                });
            }

            var existingProviders = await _preferredProvider.GetCompanyServiceIdByProfileId(companyId);

            int shipperConsigneeId = services.Find(x => x.ServiceStaticId == 1).IntServiceId;
            var result = new List<entity.Models.CompanyService>();

            int serviceIdFilter = 0;
            if (!(serviceId.IsNullOrEmpty()))
            {
                var serviceFilter = services.Find(x => x.ServiceId.ToString() == serviceId.ToString());
                serviceIdFilter = (services is null) ? 0 : serviceFilter.IntServiceId;
            }
            recordCount = await _companyService.GetRecordCount(shipperConsigneeId, serviceIdFilter, existingProviders);
            result = await _companyService.ListServiceProviders(null, serviceIdFilter, shipperConsigneeId, pageNumber, pageSize, existingProviders);

            List<ListProvidersModel> providers = new List<ListProvidersModel>();
            foreach (var provider in result)
            {
                var service = services.Find(x => x.IntServiceId == provider.ServiceId);

                providers.Add(new ListProvidersModel
                {
                    CompanyServiceId = provider.Guid.ToString(),
                    CompanyId = provider.Companies.Guid.ToString(),
                    CompanyName = provider.Companies.CompanyName,
                    CompanyImageURL = (provider.Companies.ImageURL is null) ? "No Image" : provider.Companies.ImageURL,
                    CompanyAddress = CompanyHelper.ParseCompanydAddress(provider.Companies),
                    ServiceId = (service is null) ? "N/A" : service.ServiceId,
                    ServiceName = (service is null) ? "N/A" : service.ServiceName,
                    ServiceImageURL = (service != null) ? ((service.ServiceImageURL is null) ? "No Image" : service.ServiceImageURL) : "N/A"
                });
            }

            var pagedResponse = _pagedResponse.Paginate(providers, recordCount, pageNumber, pageSize);
            return _general.Response(pagedResponse, 200, "Configurable providers has been listed", true);
        }*/

        public async Task<IGeneralModel> ListProviders(int companyId, string search, string serviceId, int otherProviderPageNumber, int otherProviderPageSize, int otherProviderRecordCount, int preferredProviderPageNumber, int preferredProviderPageSize, int preferredProviderRecordCount)
        {
            var serviceResponse = await _httpHelpers.Get(_options.Value.BaseUrl, _options.Value.GetService, null, AuthToken.Contra);
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
                    ServiceImageURL = service.imageURL,
                    ServiceStaticId = service.serviceStaticId
                });
            }

            int shipperConsigneeId = services.Find(x => x.ServiceStaticId == 1).IntServiceId;
            int serviceIdFilter = 0;
            if (!(serviceId.IsNullOrEmpty()))
            {
                var serviceFilter = services.Find(x => Guid.Parse(x.ServiceId) == Guid.Parse(serviceId));
                serviceIdFilter = (services is null) ? 0 : serviceFilter.IntServiceId;
            }

            var preferredProviderCompanyServiceGuids = await _preferredProvider.GetCompanyServiceIdByProfileId(companyId);
            var (preferredProvidersData, preferredProviderDataCount) = await _companyService.ListPreferredProviders(search, serviceIdFilter, shipperConsigneeId, preferredProviderPageNumber, preferredProviderPageSize, preferredProviderCompanyServiceGuids);
            List<ListProvidersModel> preferredProviders = new List<ListProvidersModel>();
            if (!(preferredProvidersData is null))
            {
                foreach (var provider in preferredProvidersData)
                {
                    var service = services.Find(x => x.IntServiceId == provider.ServiceId);

                    preferredProviders.Add(new ListProvidersModel
                    {
                        CompanyServiceId = provider.Guid.ToString(),
                        CompanyId = provider.Companies.Guid.ToString(),
                        CompanyName = provider.Companies.CompanyName,
                        CompanyImageURL = (provider.Companies.ImageURL is null) ? "No Image" : provider.Companies.ImageURL,
                        CompanyAddress = CompanyHelper.ParseCompanydAddress(provider.Companies),
                        ServiceId = (service is null) ? "N/A" : service.ServiceId,
                        ServiceName = (service is null) ? "N/A" : service.ServiceName,
                        ServiceImageURL = (service != null) ? ((service.ServiceImageURL is null) ? "No Image" : service.ServiceImageURL) : "N/A",
                        PhoneNumber = (provider.Companies.ContactDetails.Phone is null) ? "-" : $"{provider.Companies.ContactDetails.PhonePrefix}{provider.Companies.ContactDetails.Phone}",
                        MobileNumber = (provider.Companies.ContactDetails.Mobile is null) ? "-" : $"{provider.Companies.ContactDetails.MobilePrefix}{provider.Companies.ContactDetails.Mobile}",
                        FaxNumber = (provider.Companies.ContactDetails.Fax is null) ? "-" : $"{provider.Companies.ContactDetails.FaxPrefix}{provider.Companies.ContactDetails.Fax}",
                        Email = (provider.Companies.EmailAddress is null) ? "-" : provider.Companies.EmailAddress
                    });
                }
            }

            //recordCount = await _companyService.GetOtherProvidersRecordCount(shipperConsigneeId, serviceIdFilter, preferredProviderCompanyServiceGuids, search);
            var (otherProvidersData, otherProviderDataCount) = await _companyService.ListServiceProviders(search, serviceIdFilter, shipperConsigneeId, otherProviderPageNumber, otherProviderPageSize, preferredProviderCompanyServiceGuids);
            List<ListProvidersModel> otherProviders = new List<ListProvidersModel>();

            if (!(otherProvidersData is null))
            {
                foreach (var provider in otherProvidersData)
                {
                    var service = services.Find(x => x.IntServiceId == provider.ServiceId);

                    string state = provider.Companies.Addresses.StateName ?? null;
                    string country = provider.Companies.Addresses.CountryName;
                    string address = (state is null) ? country : $"{state}, {country}";

                    otherProviders.Add(new ListProvidersModel
                    {
                        CompanyServiceId = provider.Guid.ToString(),
                        CompanyId = provider.Companies.Guid.ToString(),
                        CompanyName = provider.Companies.CompanyName,
                        CompanyImageURL = (provider.Companies.ImageURL is null) ? "No Image" : provider.Companies.ImageURL,
                        CompanyAddress = address,
                        ServiceId = (service is null) ? "N/A" : service.ServiceId,
                        ServiceName = (service is null) ? "N/A" : service.ServiceName,
                        ServiceImageURL = (service != null) ? ((service.ServiceImageURL is null) ? "No Image" : service.ServiceImageURL) : "N/A",
                        PhoneNumber = (provider.Companies.ContactDetails.Phone is null) ? "-" : $"{provider.Companies.ContactDetails.PhonePrefix}{provider.Companies.ContactDetails.Phone}",
                        MobileNumber = (provider.Companies.ContactDetails.Mobile is null) ? "-" : $"{provider.Companies.ContactDetails.MobilePrefix}{provider.Companies.ContactDetails.Mobile}",
                        FaxNumber = (provider.Companies.ContactDetails.Fax is null) ? "-" : $"{provider.Companies.ContactDetails.FaxPrefix}{provider.Companies.ContactDetails.Fax}",
                        Email = (provider.Companies.EmailAddress is null) ? "-" : provider.Companies.EmailAddress
                    });
                }
            }

            // Paginate result data
            var pagedPreferredProvider = _pagedResponse.Paginate(preferredProviders, preferredProviderDataCount, preferredProviderPageNumber, preferredProviderPageSize);
            var pagedOtherProviders = _pagedResponse.Paginate(otherProviders, otherProviderDataCount, otherProviderPageNumber, otherProviderPageSize);
            return _general.Response(new { PreferredProviders = pagedPreferredProvider, OtherProviders = pagedOtherProviders }, 200, "Configurable providers has been listed", true);
        }
    }
}
