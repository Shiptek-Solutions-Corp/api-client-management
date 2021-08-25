using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using xas.core._Helpers;
using xas.core._Helpers.IOptionModels;
using xas.core._ResponseModel;
using xgca.core.Helpers;
using xgca.core.Helpers.Http;
using xgca.core.Response;
using xgca.data.Company;

namespace xas.core.ServiceProvider
{
    public interface ICServiceProvider
    {
        Task<GeneralModel> ServiceProviderByServiceId(Guid guid, int page, int rows, string token = null, string search = null);
    }
    public class CServiceProvider : ICServiceProvider
    {
        private readonly IOptions<ClientManagement> _optionsClient;
        private readonly IOptions<GlobalCmsService> _optionsGlobal;
        private readonly IGeneralResponse _generalResponse;
        private readonly IHttpHelper _httpHelper;
        private readonly ICompanyData _companyData;
        public CServiceProvider(
            IOptions<ClientManagement> optionsClient,
            IOptions<GlobalCmsService> optionsGlobal,
            IGeneralResponse generalResponse,
            IHttpHelper httpHelper,
            ICompanyData companyData)
        {
            _optionsClient = optionsClient;
            _optionsGlobal = optionsGlobal;
            _generalResponse = generalResponse;
            _httpHelper = httpHelper;
            _companyData = companyData;
        }

        public async Task<GeneralModel> ServiceProviderByServiceId(Guid guid, int page, int rows, string token = null , string search = null)
        {
            //FETCH CONVERT Guid To int ID from Global
            var objectResponse = (JObject)await _httpHelper.Get(_optionsGlobal.Value.BaseUrl + _optionsGlobal.Value.ServiceList, "?ids=" + guid);
            var serviceId = objectResponse["data"]["services"][0]["intServiceId"].ToString();

            //FETCH ALL COMPANY BY SERVICE
            var companies = await _companyData.ListByService(int.Parse(serviceId), page, rows);
            return _generalResponse.Response(companies, StatusCodes.Status200OK, "Request deletion has been succesful!", true);
        }
    }
}
