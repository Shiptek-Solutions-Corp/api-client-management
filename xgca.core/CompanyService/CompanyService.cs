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

namespace xgca.core.CompanyService
{
    public class CompanyService : ICompanyService
    {
        private readonly xgca.data.CompanyService.ICompanyService _companyService;
        private readonly xgca.data.Company.ICompanyData _company;
        private readonly xgca.core.User.IUser _coreUser;
        private readonly ICompanyUser _coreCompanyUser;

        private readonly IHttpHelpers _httpHelpers;
        private readonly IOptions<GlobalCmsApi> _options;
        private readonly IGeneral _general;

        public CompanyService(xgca.data.CompanyService.ICompanyService companyService,
            xgca.data.Company.ICompanyData company,
            xgca.core.User.IUser coreUser, ICompanyUser coreCompanyUser,
            IHttpHelpers httpHelpers,
            IOptions<GlobalCmsApi> options,
            IGeneral general)
        {
            _companyService = companyService;
            _company = company;
            _coreUser = coreUser;
            _coreCompanyUser = coreCompanyUser;
            _httpHelpers = httpHelpers;
            _options = options;
            _general = general;
        }
        public async Task<IGeneralModel> ListByCompanyId(string key)
        {
            int companyId = await _company.GetIdByGuid(Guid.Parse(key));
            var result = await _companyService.ListByCompanyId(companyId);
            var data = result.Select(t => new { CompanyServiceId = t.Guid, t.Status });
            return _general.Response(new { companyService = data }, 200, "Configurable services for selected company has been listed", true);
        }
        public async Task<IGeneralModel> ListByCompanyUserId(string key)
        {
            int userId = await _coreUser.GetIdByGuid(Guid.Parse(key));
            int companyId = await _coreCompanyUser.GetCompanyIdByUserId(userId);
            var result = await _companyService.ListByCompanyId(companyId);
            var companyServices = result.Select(t => new { CompanyServiceId = t.Guid, t.ServiceId, t.Status });

            List<ListCompanyServiceModel> data = new List<ListCompanyServiceModel>();

            foreach (var companyService in companyServices)
            {
                var serviceKey = await _httpHelpers.GetGuidById(_options.Value.BaseUrl, ApiEndpoints.cmsGetService, companyService.ServiceId);
                string serviceId = serviceKey.data.serviceId;
                var serviceResponse = await _httpHelpers.Get(_options.Value.BaseUrl, ApiEndpoints.cmsGetService, serviceId);

                var json = (JObject)serviceResponse;

                data.Add(new ListCompanyServiceModel
                {
                    CompanyServiceId = companyService.CompanyServiceId.ToString(),
                    ServiceId = (json)["data"]["service"]["serviceId"].ToString(),
                    Name = (json)["data"]["service"]["name"].ToString(),
                    ImageURL = (json)["data"]["service"]["imageUrl"].ToString(),
                });
            }

            return _general.Response(new { companyServices = data }, 200, "Configurable services for selected company has been listed", true);
        }
        public async Task<IGeneralModel> Create(CreateCompanyServiceModel obj)
        {
            int companyId = await _company.GetIdByGuid(Guid.Parse(obj.CompanyId));
            var service = await _httpHelpers.Get(_options.Value.BaseUrl, ApiEndpoints.cmsGetService, obj.ServiceId);
            int userId = 1;
            var companyService = new entity.Models.CompanyService
            {
                CompanyId = companyId,
                ServiceId = service.data.ServiceId,
                Status = 1,
                CreatedBy = userId,
                CreatedOn = DateTime.Now,
                ModifiedBy = userId,
                ModifiedOn = DateTime.Now,
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
                var service = await _httpHelpers.GetIdByGuid(_options.Value.BaseUrl, ApiEndpoints.cmsGetService, serviceKey);
                companyServices.Add(new entity.Models.CompanyService
                {
                    ServiceId = service.data.serviceId,
                    CompanyId = companyId,
                    Status = 1,
                    CreatedBy = userId,
                    CreatedOn = DateTime.Now,
                    ModifiedBy = userId,
                    ModifiedOn = DateTime.Now,
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
            //int serviceId = await _service.GetIdByGuid(Guid.Parse(obj.Serviceid));
            int companyId = await _company.GetIdByGuid(Guid.Parse(obj.CompanyId));

            var companyService = new entity.Models.CompanyService
            {
                CompanyServiceId = companyServiceId,
                //ServiceId = serviceId,
                CompanyId = companyId,
                Status = obj.Status,
                CreatedBy = userId,
                CreatedOn = DateTime.Now
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
                //int serviceId = await _service.GetIdByGuid(Guid.Parse(service.ServiceId));
                if (service.CompanyServiceId == 0)
                {
                    createServices.Add(new entity.Models.CompanyService
                    {
                        CompanyId = companyId,
                        //ServiceId = serviceId,
                        Status = 1,
                        CreatedBy = userId,
                        CreatedOn = DateTime.Now,
                        ModifiedBy = userId,
                        ModifiedOn = DateTime.Now,
                    });
                }
                else
                {
                    updateServices.Add(new entity.Models.CompanyService
                    {
                        CompanyId = companyId,
                        //ServiceId = serviceId,
                        Status = 1,
                        ModifiedBy = userId,
                        ModifiedOn = DateTime.Now,
                    });
                }
            }

            if (createServices != null)
            { result = await _companyService.Create(createServices); }
            if (updateServices != null)
            { result = await _companyService.Update(updateServices); }
            return result;
        }
    }
}
