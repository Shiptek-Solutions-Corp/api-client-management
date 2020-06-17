using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using xgca.core.Models.User;
using xgca.core.Response;
using xgca.entity.Models;
using xgca.data.User;
using xgca.data.Company;
using xgca.core.Helpers;
using xgca.core.Helpers.Token;
using xgca.core.Helpers.Http;
using xgca.core.Constants;

namespace xgca.core.Profile
{
    public class Profile : IProfile
    {
        private readonly IUserData _userData;
        private readonly ICompanyData _companyData;
        private readonly xgca.data.CompanyUser.ICompanyUser _companyUserData;
        private readonly xgca.data.CompanyService.ICompanyService _companyServiceData;

        private readonly IHttpHelper _httpHelper;
        private readonly ITokenHelper _tokenHelper;
        private readonly IOptions<GlobalCmsApi> _options;
        private readonly IGeneral _general;

        public Profile(xgca.data.User.IUserData userData,
            ICompanyData companyData,
            xgca.data.CompanyUser.ICompanyUser companyUserData,
            xgca.data.CompanyService.ICompanyService companyServiceData,
            IHttpHelper httpHelper,
            ITokenHelper tokenHelper,
            IOptions<GlobalCmsApi> options,
            IGeneral general)
        {
            _userData = userData;
            _companyData = companyData;
            _companyUserData = companyUserData;
            _companyServiceData = companyServiceData;
            _httpHelper = httpHelper;
            _tokenHelper = tokenHelper;
            _options = options;
            _general = general;
        }

        public async Task<dynamic> LoadProfile(string username, string companyServiceKey)
        {
            int userId = await _userData.GetIdByUsername(username);
            int companyId = await _companyUserData.GetCompanyIdByUserId(userId);

            var user = await _userData.RetrieveByUsername(username);
            var company = await _companyData.Retrieve(companyId);
            int companyServiceId = await _companyServiceData.GetIdByGuid(Guid.Parse(companyServiceKey));
            var companyService = await _companyServiceData.Retrieve(companyServiceId);
            var serviceResponse = await _httpHelper.GetGuidById(_options.Value.BaseUrl, ApiEndpoints.cmsGetService, companyService.ServiceId);
            var json = (JObject)serviceResponse;
            string serviceKey = json["data"]["serviceId"].ToString();
            var service = await _httpHelper.Get(_options.Value.BaseUrl, ApiEndpoints.cmsGetService, serviceKey);
            var serviceJson = (JObject)service;

            dynamic data = new
            {
                Company = new
                {
                    CompanyId = company.Guid,
                    Name = company.CompanyName,
                    Image = company.ImageURL,
                    ServiceId = serviceKey,
                    Service = serviceJson["data"]["service"]["name"],
                },
                User = new
                {
                    UserId = user.Guid,
                    Name = String.Concat(user.FirstName, " ", user.LastName),
                    Image = user.ImageURL,
                    Email = user.EmailAddress,
                }
            };

            return _general.Response(new { Profile = data }, 200, "Profile loaded", true);
        }
    }
}
