using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using xgca.core.Models.CompanyServiceUser;
using xgca.core.Response;
using xgca.core.Helpers;
using xgca.core.Models.CompanyService;
using xgca.core.User;
using xgca.core.Helpers.Http;
using xgca.core.Constants;
using System.Runtime.InteropServices.ComTypes;
using xgca.data.Company;
using xgca.data.User;

namespace xgca.core.CompanyServiceUser
{
    public class CompanyServiceUser : ICompanyServiceUser
    {
        private readonly ICompanyData _companyData;
        private readonly xgca.data.CompanyServiceRole.ICompanyServiceRole _companyServiceRole;
        private readonly xgca.data.CompanyServiceUser.ICompanyServiceUser _companyServiceUser;
        private readonly xgca.data.CompanyService.ICompanyService _companyService;
        private readonly IUserData _userData;
        private readonly IHttpHelper _httpHelper;
        private readonly IOptions<GlobalCmsService> _options;
        private readonly IUserHelper _userHelper;
        private readonly IGeneral _general;

        public CompanyServiceUser(ICompanyData companyData, xgca.data.CompanyServiceRole.ICompanyServiceRole companyServiceRole,
            xgca.data.CompanyServiceUser.ICompanyServiceUser companyServiceUser,
            xgca.data.CompanyService.ICompanyService companyService, IUserData userData,
            IHttpHelper httpHelper, IOptions<GlobalCmsService> options, IUserHelper userHelper, IGeneral general)
        {
            _companyData = companyData;
            _companyServiceRole = companyServiceRole;
            _companyServiceUser = companyServiceUser;
            _companyService = companyService;
            _userData = userData;
            _httpHelper = httpHelper;
            _options = options;
            _userHelper = userHelper;
            _general = general;
        }

        public async Task<bool> CreateDefault(int companyId, int companyUserId, int createdBy)
        {
            var companyServices = await _companyService.ListByCompanyId(companyId);
            List<entity.Models.CompanyServiceUser> companyServiceUsers = new List<entity.Models.CompanyServiceUser>();
            foreach (entity.Models.CompanyService companyService in companyServices)
            {
                int companyServiceRoleId = await _companyServiceRole.RetrieveAdministratorId(companyService.CompanyServiceId);
                companyServiceUsers.Add(new entity.Models.CompanyServiceUser
                {
                    CompanyServiceId = companyService.CompanyServiceId,
                    CompanyServiceRoleId = companyServiceRoleId,
                    CompanyUserId = companyUserId,
                    CompanyId = companyId,
                    IsActive = 1,
                    IsLocked = 0,
                    CreatedBy = createdBy,
                    CreatedOn = DateTime.UtcNow,
                    ModifiedBy = createdBy,
                    ModifiedOn = DateTime.UtcNow,
                    Guid = Guid.NewGuid()
                });
            }

            var result = await _companyServiceUser.Create(companyServiceUsers);
            return result;
        }

        public async Task<IGeneralModel> ListUserServiceRolesByCompanyId(int companyId)
        {
            var companyServiceUsers = await _companyServiceUser.ListByCompanyId(companyId);
            if (companyServiceUsers.Count == 0)
            {
                return _general.Response(null, 400, "No configurable user service roles found!", false);
            }
            List<ListCompanyServiceUsers> lists = new List<ListCompanyServiceUsers>();
            List<int> userIds = new List<int>();

            foreach(var companyServiceUser in companyServiceUsers)
            {
                /** For Localhost testing with Dev DB Environment **/
                //string getDetails = "service/{serviceId}";
                //string url = "https://localhost:44380/global/cms/api/v1/";
                //var serviceGuid = await _httpHelper.CustomGet(url, getDetails.Replace("{serviceId}", companyServiceUser.CompanyServices.ServiceId.ToString()) + "/guid", AuthToken.Contra);

                var serviceGuid = await _httpHelper.Get(_options.Value.BaseUrl, _options.Value.GetServiceDetails.Replace("{serviceId}", companyServiceUser.CompanyServices.ServiceId.ToString()) + "/guid");
                var serviceGuidObj = (JObject)serviceGuid;
                string serviceKey = (serviceGuidObj)["data"]["serviceId"].ToString();

                /** For Localhost testing with Dev DB Environment **/
                //var serviceResponse = await _httpHelper.CustomGet(url, getDetails.Replace("{serviceId}", serviceKey), AuthToken.Contra);
                
                var serviceResponse = await _httpHelper.Get(_options.Value.BaseUrl, _options.Value.GetServiceDetails.Replace("{serviceId}", serviceKey));
                var serviceObj = (JObject)serviceResponse;


                lists.Add(new ListCompanyServiceUsers
                {
                    CompanyServiceuserId = companyServiceUser.Guid.ToString(),
                    Fullname = _userHelper.GetUserFullname(companyServiceUser.CompanyUsers.Users),
                    EmailAddress = companyServiceUser.CompanyUsers.Users.EmailAddress,
                    ImageURL = companyServiceUser.CompanyUsers.Users.ImageURL,
                    Username = companyServiceUser.CompanyUsers.Users.Username,
                    Role = companyServiceUser.CompanyServiceRoles.Name,
                    Service = (serviceObj)["data"]["service"]["name"].ToString(),
                    IsActive = companyServiceUser.IsActive,
                    IsLocked = companyServiceUser.IsLocked
                });

                userIds.Add(companyServiceUser.CompanyUsers.Users.UserId);
            }

            int activeUsers = await _userData.GetTotalActiveUsers(userIds);
            int inactiveUsers = await _userData.GetTotalInactiveUsers(userIds);
            int lockedUsers = await _userData.GetTotalLockedUsers(userIds);
            int unlockedUsers = await _userData.GetTotalUnlockedUsers(userIds);
            int totalUsers = await _userData.GetTotalUsers(userIds);

            var responseData = new
            {
                user = lists,
                TotalUsersCount = totalUsers,
                TotalActiveUsers = activeUsers,
                TotalInactiveUsers = inactiveUsers,
                TotalLockUsers = lockedUsers,
                TotalUnlockUsers = unlockedUsers
            };

            return _general.Response(new { data = responseData }, 200, "Configurable user service roles have been listed", true);
        }

        public async Task<IGeneralModel> ListUserServiceRolesByCompanyId(string companyKey)
        {
            int companyId = await _companyData.GetIdByGuid(Guid.Parse(companyKey));
            var companyServiceUsers = await _companyServiceUser.ListByCompanyId(companyId);
            List<ListCompanyServiceUsers> lists = new List<ListCompanyServiceUsers>();
            List<int> userIds = new List<int>();
            if (companyServiceUsers.Count == 0)
            {
                return _general.Response(null, 400, "No configurable user service roles found!", false);
            }
            foreach (var companyServiceUser in companyServiceUsers)
            {
                var servicesGuid = await _httpHelper.Get(_options.Value.BaseUrl, _options.Value.GetServiceDetails.Replace("{serviceId}", companyServiceUser.CompanyServices.ServiceId.ToString()) + "/guid");
                var serviceGuidObj = (JObject)servicesGuid;
                string serviceKey = (serviceGuidObj)["data"]["serviceId"].ToString();

                var serviceResponse = await _httpHelper.Get(_options.Value.BaseUrl, _options.Value.GetServiceDetails.Replace("{serviceId}", serviceKey));
                var serviceObj = (JObject)serviceResponse;


                lists.Add(new ListCompanyServiceUsers
                {
                    CompanyServiceuserId = companyServiceUser.Guid.ToString(),
                    Fullname = _userHelper.GetUserFullname(companyServiceUser.CompanyUsers.Users),
                    EmailAddress = companyServiceUser.CompanyUsers.Users.EmailAddress,
                    ImageURL = companyServiceUser.CompanyUsers.Users.ImageURL,
                    Username = companyServiceUser.CompanyUsers.Users.Username,
                    Role = companyServiceUser.CompanyServiceRoles.Name,
                    Service = (serviceObj)["data"]["service"]["name"].ToString(),
                    IsActive = companyServiceUser.IsActive,
                    IsLocked = companyServiceUser.IsLocked
                });

                userIds.Add(companyServiceUser.CompanyUsers.Users.UserId);
            }

            int activeUsers = await _userData.GetTotalActiveUsers(userIds);
            int inactiveUsers = await _userData.GetTotalInactiveUsers(userIds);
            int lockedUsers = await _userData.GetTotalLockedUsers(userIds);
            int unlockedUsers = await _userData.GetTotalUnlockedUsers(userIds);
            int totalUsers = await _userData.GetTotalUsers(userIds);

            var responseData = new
            {
                user = lists,
                TotalUsersCount = totalUsers,
                TotalActiveUsers = activeUsers,
                TotalInactiveUsers = inactiveUsers,
                TotalLockUsers = lockedUsers,
                TotalUnlockUsers = unlockedUsers
            };

            return _general.Response(new { data = responseData }, 200, "Configurable user service roles have been listed", true);
        }
    }
}
