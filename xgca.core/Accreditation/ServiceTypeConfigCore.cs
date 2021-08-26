using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xas.core._Helpers;
using xas.core._Helpers.IOptionModels;
using xas.core._ResponseModel;
using xas.data.DataModel.ServiceRoleConfig;
using xgca.core.Helpers;
using xgca.core.Helpers.Http;
using xgca.core.Response;

namespace xas.core.ServiceRoleConfig
{
    public interface IServiceRoleConfigCore
    {
        Task<GeneralModel> ServiceRoleConfig(Guid serviceRoleId);
    }
    public class ServiceRoleConfigCore : IServiceRoleConfigCore
    {
        private readonly IServiceRoleConfigData _serviceRoleConfigData;
        private readonly IGeneralResponse _generalResponse;
        private readonly IMapper _mapper;
        private readonly IHttpHelper _httpHelper;
        private readonly IOptions<ClientManagement> _clientOptions;
        private readonly IOptions<GlobalCmsService> _globalOptions;

        public ServiceRoleConfigCore(
            IServiceRoleConfigData serviceRoleConfigData,
            IGeneralResponse generalResponse,
            IMapper mapper,
            IHttpHelper httpHelper,
            IOptions<ClientManagement> clientOptions,
            IOptions<GlobalCmsService> globalOptions)
        {
            _serviceRoleConfigData = serviceRoleConfigData;
            _generalResponse = generalResponse;
            _mapper = mapper;
            _httpHelper = httpHelper;
            _clientOptions = clientOptions;
            _globalOptions = globalOptions;
        }

        public async Task<GeneralModel> ServiceRoleConfig(Guid serviceRoleId)
        {
            var response = await _serviceRoleConfigData.GetAllowedServices(serviceRoleId);
            var baseRequest = _globalOptions.Value.BaseUrl + _globalOptions.Value.ServiceList;

            var guidParam = "";
            response.ForEach(t => {
                guidParam += "ids=" + t.ServiceRoleIdAllowed + "&";
            });


            guidParam = guidParam.Length == 0 ? guidParam : guidParam.Remove(guidParam.Length - 1, 1);
            var data = (JObject)await _httpHelper.Get(baseRequest, "?" + guidParam);
            var serviceListing = new List<object>();

            if (guidParam.Length != 0)
            {
                data["data"]["services"].ToList().ForEach(t => {
                    serviceListing.Add(new
                    {
                        serviceId = t["serviceId"].ToString(),
                        serviceName = t["serviceName"].ToString()
                    });
                });
            }


            return _generalResponse.Response(serviceListing, StatusCodes.Status200OK, "Allowed Service role for accreditation has been listed!", true);

        }
    }
}
