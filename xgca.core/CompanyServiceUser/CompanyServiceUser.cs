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
using AutoMapper;

namespace xgca.core.CompanyServiceUser
{
    public interface ICompanyServiceUser
    {
        Task<bool> CreateDefault(int companyId, int companyUserId, int createdBy);
        Task<IGeneralModel> ListUserServiceRolesByCompanyId(int companyId);
        Task<IGeneralModel> ListUserServiceRolesByCompanyId(string companyKey);
        Task<IGeneralModel> ListUserServiceRolesByCompanyUserId(int companyUserId);
        Task<IGeneralModel> ListUserWithNoDuplicateRole(
            string companyGuidId, 
            string companyServiceRoleGuid = "", 
            string groupName = "",
            string companyServiceGuid = "",
            string fullname = "");


    }
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
        private readonly IMapper mapper;

        public CompanyServiceUser(ICompanyData companyData, xgca.data.CompanyServiceRole.ICompanyServiceRole companyServiceRole,
            xgca.data.CompanyServiceUser.ICompanyServiceUser companyServiceUser,
            xgca.data.CompanyService.ICompanyService companyService, IUserData userData,
            IHttpHelper httpHelper, IOptions<GlobalCmsService> options, IUserHelper userHelper, IGeneral general,
            IMapper mapper)
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
            this.mapper = mapper;
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

        public async Task<IGeneralModel> ListUserServiceRolesByCompanyUserId(int companyUserId)
        {
            var companyServiceUsers = await _companyServiceUser.ListByCompanyUserId(companyUserId);
            if (companyServiceUsers.Count == 0)
            {
                return _general.Response(null, 400, "No configurable user service roles found!", false);
            }
            List<ListCompanyServiceUsers> lists = new List<ListCompanyServiceUsers>();
            foreach (var companyServiceUser in companyServiceUsers)
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
                var companyServiceData = await _companyService.Retrieve(companyServiceUser.CompanyServiceId); //companyServiceUser.CompanyServiceId;
                var CompanyServiceRoleData = await _companyServiceRole.Retrieve(companyServiceUser.CompanyServiceRoleId); // companyServiceUser.CompanyServiceRoleId;

                lists.Add(new ListCompanyServiceUsers
                {
                    CompanyServiceuserId = companyServiceUser.Guid.ToString(),
                    CompanyServiceId = companyServiceData.Guid.ToString(),
                    CompanyServiceRoleId = CompanyServiceRoleData.Guid.ToString(),
                    ServiceId = (serviceObj)["data"]["service"]["serviceId"].ToString(),
                    //Fullname = _userHelper.GetUserFullname(companyServiceUser.CompanyUsers.Users),
                    //EmailAddress = companyServiceUser.CompanyUsers.Users.EmailAddress,
                    //ImageURL = companyServiceUser.CompanyUsers.Users.ImageURL,
                    //Username = companyServiceUser.CompanyUsers.Users.Username,
                    Role = companyServiceUser.CompanyServiceRoles.Name,
                    Service = (serviceObj)["data"]["service"]["name"].ToString(),
                    //IsActive = companyServiceUser.IsActive,
                    //IsLocked = companyServiceUser.IsLocked
                });
            }

            return _general.Response(new { data = lists }, 200, "Configurable user service roles have been listed", true);
        }

        public async Task<IGeneralModel> ListUserWithNoDuplicateRole(
            string companyGuidId, 
            string companyServiceRoleGuid = "", 
            string groupName = "",
            string companyServiceGuid = "", 
            string fullName = "")
        {
            int companyServiceRoleId = 0;
            int companyServiceId = 0;

            var companyId = await _companyData.GetIdByGuid(Guid.Parse(companyGuidId));

            if (companyId > 0 == false)
            {
                return _general.Response(null, 400, "Invalid company guid", false);
            }

            if (companyServiceRoleGuid != null && companyServiceRoleGuid != "")
            {
                companyServiceRoleId = await _companyServiceRole.GetIdByGuid(Guid.Parse(companyServiceRoleGuid));
            }
            if (companyServiceGuid != null && companyServiceGuid != "")
            {
                companyServiceId = await _companyService.GetIdByGuid(Guid.Parse(companyServiceGuid));
            }
            var result = await _companyServiceUser.ListUserWithNoDuplicateRole(
                companyId, 
                companyServiceRoleId, 
                groupName, 
                companyServiceId,
                fullName);

            var list = result.Select(u => mapper.Map<Models.CompanyUser.GetCompanyUserModel>(u)).ToList();

            return _general.Response(list, 200, "List of Company Service user with no existing role", true);
        }
    }
}
