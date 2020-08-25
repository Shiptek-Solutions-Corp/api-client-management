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
using xgca.data.ContactDetail;
using xgca.data.AuditLog;
using xgca.core.AuditLog;
using xgca.core.Helpers;
using xgca.core.Helpers.Token;
using xgca.core.Validators.User;
using xgca.core.Constants;
using xgca.core.Helpers.Http;
using xgca.entity.Migrations;
using xgca.core.Models.AuditLog;
using xgca.data.CompanyServiceUser;
using xgca.core.Models.CompanyService;
using xgca.core.Models.CompanyServiceRole;
using xgca.data.Company;
using Microsoft.IdentityModel.Tokens;
using Castle.Core.Internal;

namespace xgca.core.User
{
    public interface IUser
    {
        Task<IGeneralModel> List();
        Task<IGeneralModel> List(string columnFilter, string isActive, string isLock);
        Task<IGeneralModel> Create(CreateUserModel obj, string companyId, string auth, string CreatedBy);
        Task<int> CreateAndReturnId(CreateUserModel obj);
        Task<dynamic> CreateMasterUser(CreateUserModel obj, int createdBy);
        Task<IGeneralModel> Update(UpdateUserModel obj, string modifiedBy);
        Task<IGeneralModel> UpdateStatus(UpdateUserStatusModel obj, string modifiedBy, string auth);
        Task<IGeneralModel> UpdateLock(UpdateUserLockModel obj, string modifiedBy, string auth);
        Task<IGeneralModel> UpdateMultipleLock(UpdateMultipleLockModel obj, string modifiedBy, string auth);
        Task<IGeneralModel> DeleteMultipleUser(DeleteMultipleUserModel obj, string modifiedBy, string auth);
        Task<IGeneralModel> UpdateMultipleStatus(UpdateMultipleStatusModel obj, string modifiedBy, string auth);
        Task<IGeneralModel> SetUsername(SetUsernameModel obj);
        Task<IGeneralModel> Retrieve(string key);
        Task<IGeneralModel> RetrieveByUsername(string username);
        Task<IGeneralModel> Delete(string key, string modifiedBy, string auth);
        Task<IGeneralModel> GetIdByGuid(string key);
        Task<int> GetIdByGuid(Guid key);
        Task<int> GetIdByUsername(string username);
        Task<IGeneralModel> GetUserByReferenceId(int id);
        Task<IGeneralModel> ListUserLogs(string? userKey, string? username);
        Task<IGeneralModel> GetUserCounts(List<int> userIds);
    }
    public class User : 
        IUser
    {
        private readonly xgca.data.User.IUserData _userData;
        private readonly xgca.core.ContactDetail.IContactDetail _coreContactDetail;
        private readonly xgca.core.CompanyUser.ICompanyUser _coreCompanyUser;
        private readonly xgca.data.AuditLog.IAuditLogData _auditLog;
        private readonly IAuditLogCore _coreAuditLog;
        private readonly ITokenHelper _tokenHelper;
        private readonly IHttpHelper _httpHelper;
        private readonly IOptions<GlobalCmsService> _options;
        private readonly IOptions<OptimusAuthService> _optimusAuthService;
        private readonly IGeneral _general;
        private readonly xgca.data.CompanyServiceUser.ICompanyServiceUser _companyServiceUser;
        private readonly xgca.core.CompanyServiceUser.ICompanyServiceUser _coreCompanyServiceUser;
        private readonly xgca.data.CompanyService.ICompanyService _companyService;
        private readonly xgca.data.CompanyServiceRole.ICompanyServiceRole _companyServiceRole;
        private readonly ICompanyData _companyData;

        public User(xgca.data.User.IUserData userData,
            xgca.core.ContactDetail.IContactDetail coreContactDetail,
            xgca.core.CompanyUser.ICompanyUser coreCompanyUser,
            xgca.data.AuditLog.IAuditLogData auditLog,
            IAuditLogCore coreAuditLog,
            ITokenHelper tokenHelper,
            IOptions<GlobalCmsService> options,
            IHttpHelper httpHelper,
            IOptions<OptimusAuthService> optimusAuthService,
            xgca.data.CompanyServiceUser.ICompanyServiceUser companyServiceUser,
            xgca.core.CompanyServiceUser.ICompanyServiceUser coreCompanyServiceUser,
            xgca.data.CompanyService.ICompanyService companyService,
            xgca.data.CompanyServiceRole.ICompanyServiceRole companyServiceRole,
            ICompanyData companyData,
        IGeneral general)
        {
            _userData = userData;
            _coreContactDetail = coreContactDetail;
            _coreCompanyUser = coreCompanyUser;
            _auditLog = auditLog;
            _coreAuditLog = coreAuditLog;
            _tokenHelper = tokenHelper;
            _httpHelper = httpHelper;
            _options = options;
            _general = general;
            _optimusAuthService = optimusAuthService;
            _companyServiceUser = companyServiceUser;
            _coreCompanyServiceUser = coreCompanyServiceUser;
            _companyService = companyService;
            _companyServiceRole = companyServiceRole;
            _companyData = companyData;
        }

        public async Task<IGeneralModel> List()
        {
            var user = await _userData.List();
            var totalUsers = await _userData.GetTotalUsers();
            var activeUsers = await _userData.GetTotalActiveUsers();
            var inactiveUsers = await _userData.GetTotalInactiveUsers();
            var lockUsers = await _userData.GetTotalLockedUsers();
            var unlockUsers = await _userData.GetTotalUnlockedUsers();
            var data = new { user = user.Select(u => new { UserId = u.Guid, 
                FullName = u.FirstName + " " + u.MiddleName + " " + u.LastName, 
                u.FirstName, 
                u.LastName.Length, 
                u.MiddleName, 
                u.ImageURL, 
                u.EmailAddress, 
                u.Status, 
                u.IsLocked,
                u.Username,
                Role = "Admin", 
                Service = "Shipping Line" 
            }), TotalUsersCount = totalUsers, TotalActiveUsers = activeUsers,TotalInactiveUsers = inactiveUsers, TotalLockUsers = lockUsers, TotalUnlockUsers = unlockUsers };
            
            return _general.Response(data, 200, "Configurable companies has been listed", true);
        }

        public async Task<IGeneralModel> List(string query, string isActive, string isLock)
        {
            //Start: Please move this on helpers
            var queryParams = query.Split(",")
                                    .Select(p => p.Split(':'))
                                    .ToDictionary(p => p[0], p => p.Length > 1 ? Uri.EscapeDataString(p[1]) : null);

            var queryString = "";

            foreach (var v in queryParams)
            {
                queryString += $"{v.Key} = '{v.Value}' and ";
            }
            //End
            
            var user = await _userData.List(queryString, isActive, isLock);
            var totalUsers = await _userData.GetTotalUsers();
            var activeUsers = await _userData.GetTotalActiveUsers();
            var inactiveUsers = await _userData.GetTotalInactiveUsers();
            var lockUsers = await _userData.GetTotalLockedUsers();
            var unlockUsers = await _userData.GetTotalUnlockedUsers();

            var data = new { user = user.Select(u => new { UserId = u.Guid, 
                FullName = u.FirstName + " " + u.MiddleName + " " + u.LastName, 
                u.ImageURL, 
                u.EmailAddress, 
                u.Status, 
                u.IsLocked, 
                Role = "Admin", 
                Service = "Shipping Line",
                u.Username,
            }), TotalUsersCount = totalUsers, TotalActiveUsers = activeUsers, TotalInactiveUsers = inactiveUsers, TotalLockUsers = lockUsers, TotalUnlockUsers = unlockUsers };

            return _general.Response(data, 200, "Configurable companies has been listed", true);
        }
        
        public async Task<IGeneralModel> Create(CreateUserModel obj, string companyId, string auth, string CreatedBy)
        {
            if (obj == null)
            {
                return _general.Response(false, 400, "Data cannot be null", false);
            }

            int createdById = GlobalVariables.SystemUserId;
            if (CreatedBy != null)
            { createdById = await _userData.GetIdByUsername(CreatedBy); }

            int contactDetailId = await _coreContactDetail.CreateAndReturnId(obj, GlobalVariables.SystemUserId);
            if (contactDetailId <= 0)
            {
                return _general.Response(false, 400, "Data cannot be null", false);
            }

            bool emailAddressIsExists = await _userData.EmailAddressExists(obj.EmailAddress);
            if (emailAddressIsExists)
            {
                var errors = new List<ErrorField>();
                errors.Add(new ErrorField("EmailAddress", "Email address already exists."));
                //return _general.Response(null, errors, 400, "Email address already exists.", false);
                return _general.Response(null, 400, "Email address already exists.", false);
            }

            var user = new xgca.entity.Models.User
            {
                FirstName = obj.FirstName,
                LastName = obj.LastName,
                MiddleName = obj.MiddleName,
                Title = obj.Title,
                ContactDetailId = contactDetailId,
                ImageURL = obj.ImageURL,
                EmailAddress = obj.EmailAddress,
                CreatedBy = createdById,
                CreatedOn = DateTime.UtcNow,
                ModifiedBy = createdById,
                ModifiedOn = DateTime.UtcNow,
                Status = 1,
                Guid = Guid.NewGuid()
            };

            int newUserId = await _userData.CreateAndReturnId(user);
            var newUserGuid = await _userData.GetGuidById(newUserId);
            var getCompany = await _companyData.Retrieve(Convert.ToInt32(companyId));
            // integration of registration to auth
            var postvals = new { Email = obj.EmailAddress, ReferenceId = newUserId, CompanyReferenceId = Convert.ToInt32(companyId), FirstName = obj.FirstName, LastName = obj.LastName, CompanyName = getCompany.CompanyName };
            string url = _optimusAuthService.Value.BaseUrl + _optimusAuthService.Value.SingleRegisterUser;

            string token = _tokenHelper.RemoveBearer(auth);
            var serviceResponse = await _httpHelper.Post(url, postvals, token);
            var json = (JObject)serviceResponse;

            var companyUser = await _coreCompanyUser.CreateAndReturnId(new xgca.core.Models.CompanyUser.CreateCompanyUserModel { CompanyId = Convert.ToInt32(companyId), UserId = newUserGuid.ToString() });
            int companyUserId = companyUser.data.companyUserId;


            //var companyServices = await _companyService.ListByCompanyId(companyId);
            List<entity.Models.CompanyServiceUser> companyServiceUsers = new List<entity.Models.CompanyServiceUser>();
            foreach (var role in obj.Roles)
            {
                //    //int companyServiceRoleId = await _companyServiceRole.RetrieveAdministratorId(companyService.CompanyServiceId);
                int companyServiceId = await _companyService.GetIdByGuid(Guid.Parse(role.companyServiceId));
                int companyServiceRoleId = await _companyServiceRole.GetIdByGuid(Guid.Parse(role.companyServiceRoleId));

                companyServiceUsers.Add(new entity.Models.CompanyServiceUser
                {
                    CompanyServiceId = companyServiceId,
                    CompanyServiceRoleId = companyServiceRoleId,
                    CompanyUserId = companyUserId,
                    CreatedBy = createdById,
                    CreatedOn = DateTime.UtcNow,
                    ModifiedBy = createdById,
                    ModifiedOn = DateTime.UtcNow,
                    Guid = Guid.NewGuid()
                });
            }
            var result = await _companyServiceUser.Create(companyServiceUsers);

            // Create audit log
            await _coreAuditLog.CreateAuditLog("Create", user.GetType().Name, newUserId, createdById, obj, null);

            return newUserId > 0
                ? _general.Response(new { userGuid = newUserGuid }, 200, "User created", false)
                : _general.Response(false, 400, "Data cannot be null", false);
        }
        public async Task<dynamic> CreateMasterUser(CreateUserModel obj, int createdBy)
        {
            if (obj == null)
            { return 0; }

            int contactDetailId = await _coreContactDetail.CreateAndReturnId(obj, GlobalVariables.SystemUserId);
            if (contactDetailId <= 0)
            { return 0; }

            //var userTypeRepsonse = await _httpHelper.CustomGet(_options.Value.BaseUrl, $"{_options.Value.GetUserType}/" + "Master_User/userTypeId", AuthToken.Contra);
            //var userTypeJson = (JObject)userTypeRepsonse;
            //int userTypeId = Convert.ToInt32((userTypeJson)["data"]["userTypeId"]);

            var user = new xgca.entity.Models.User
            {
                FirstName = obj.FirstName,
                LastName = obj.LastName,
                Title = obj.Title,
                ContactDetailId = contactDetailId,
                ImageURL = obj.ImageURL,
                EmailAddress = obj.EmailAddress,
                CreatedBy = createdBy,
                CreatedOn = DateTime.UtcNow,
                ModifiedBy = createdBy,
                ModifiedOn = DateTime.UtcNow,
                Guid = Guid.NewGuid()
            };

            var masterUserId = await _userData.CreateAndReturnId(user);
            var masterUserGuid = await _userData.GetGuidById(masterUserId);
            var userLog = UserHelper.BuildUserLogValue(user, masterUserId, GlobalVariables.SystemUserId);

            // Create audit log
            await _coreAuditLog.CreateAuditLog("Create", user.GetType().Name, masterUserId, GlobalVariables.SystemUserId, userLog, null);

            return new { MasterUserId = masterUserId, MasterUserGuid = masterUserGuid };
        }
        public async Task<int> CreateAndReturnId(CreateUserModel obj)
        {
            if (obj == null)
            { return 0; }

            int contactDetailId = await _coreContactDetail.CreateAndReturnId(obj, GlobalVariables.SystemUserId);
            if (contactDetailId <= 0)
            { return 0; }

            string json = JsonConvert.SerializeObject(obj);
            var middleName = json.Contains("middleName") ? obj.MiddleName : null;

            var user = new xgca.entity.Models.User
            {
                FirstName = obj.FirstName,
                LastName = obj.LastName,
                MiddleName = middleName,
                Title = obj.Title,
                ContactDetailId = contactDetailId,
                ImageURL = obj.ImageURL,
                EmailAddress = obj.EmailAddress,
                CreatedBy = GlobalVariables.SystemUserId,
                CreatedOn = DateTime.UtcNow,
                ModifiedBy = GlobalVariables.SystemUserId,
                ModifiedOn = DateTime.UtcNow,
                Guid = Guid.NewGuid()
            };

            var newUserId = await _userData.CreateAndReturnId(user);

            // Create audit log
            await _coreAuditLog.CreateAuditLog("Create", user.GetType().Name, newUserId, GlobalVariables.SystemUserId, obj, null);

            return newUserId;
        }

        public async Task<IGeneralModel> Update(UpdateUserModel obj, string modifiedBy)
        {
            if (obj == null)
            { return _general.Response(null, 400, "Data cannot be null", false); }

            int userId = await _userData.GetIdByGuid(Guid.Parse(obj.UserId));
            if (userId <= 0)
            {
                return _general.Response(null, 400, "User may have been deleted or does not exists!", false);
            }
            int modifiedById = await _userData.GetIdByUsername(modifiedBy);

            var validator = new UpdateUserValidator(_userData);
            var validationResult = validator.Validate(obj);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(error => new ErrorField(error.PropertyName, error.ErrorMessage)).ToList();
                return _general.Response(null, errors, 400, "Error on updating user", false);
            }

            //bool emailAddressIsExists = await _userData.EmailAddressExists(obj.EmailAddress, userId);
            //if (emailAddressIsExists)
            //{
            //    var errors = new List<ErrorField>();
            //    errors.Add(new ErrorField("EmailAddress", "Email address already exists."));
            //    return _general.Response(null, errors, 400, "Error updating user", false);
            //}

            var oldUser = await _userData.Retrieve(userId);
            var oldValue = UserHelper.BuildUserLogValue(oldUser, userId, modifiedById);

            int contactDetailId = await _coreContactDetail.UpdateAndReturnId(obj, modifiedById);
            if (contactDetailId <= 0)
            {
                return _general.Response(false, 400, "Error on updating contact detail", false);
            }

            var user = new xgca.entity.Models.User
            {
                UserId = userId,
                FirstName = obj.FirstName,
                LastName = obj.LastName,
                MiddleName = obj.MiddleName,
                Title = obj.Title,
                ContactDetailId = contactDetailId,
                ImageURL = obj.ImageURL,
                //EmailAddress = obj.EmailAddress,
                ModifiedBy = modifiedById,
                ModifiedOn = DateTime.UtcNow
            };
            var userResult = await _userData.Update(user);

            //Update company service user
            List<entity.Models.CompanyServiceUser> companyServiceUsers = new List<entity.Models.CompanyServiceUser>();
            if (obj.Roles != null)
            {
                foreach (var role in obj.Roles)
                {
                    //    //int companyServiceRoleId = await _companyServiceRole.RetrieveAdministratorId(companyService.CompanyServiceId);
                    int companyServiceId = await _companyService.GetIdByGuid(Guid.Parse(role.companyServiceId));
                    int companyServiceRoleId = await _companyServiceRole.GetIdByGuid(Guid.Parse(role.companyServiceRoleId));
                    int companyServiceUserId = await _companyServiceUser.GetIdByGuid(Guid.Parse(role.companyServiceUserId));

                    var result = await _companyServiceUser.UpdateServiceAndRole(companyServiceUserId, companyServiceId, companyServiceRoleId, modifiedById);

                }
            }


            var newValue = UserHelper.BuildUserLogValue(oldUser, userId, modifiedById);

            // Create audit log
            await _coreAuditLog.CreateAuditLog("Update", user.GetType().Name, userId, modifiedById, oldValue, newValue);

            return userResult
                ? _general.Response(null, 200, "User updated", true)
                : _general.Response(null, 400, "Error on updating user", false);
        }

        public async Task<IGeneralModel> UpdateStatus(UpdateUserStatusModel obj, string modifiedBy, string auth)
        {
            if (obj == null)
            { return _general.Response(null, 400, "Data cannot be null", false); }

            int userId = await _userData.GetIdByGuid(Guid.Parse(obj.UserId));

            var getUserName = await _userData.Retrieve(userId);
            if (!getUserName.Username.IsNullOrEmpty())
            {
                string url = "";
                if (obj.Status == 1)
                {
                    url = _optimusAuthService.Value.BaseUrl + _optimusAuthService.Value.EnableUser;
                }
                else
                {
                    url = _optimusAuthService.Value.BaseUrl + _optimusAuthService.Value.DisableUser;
                }
                string token = _tokenHelper.RemoveBearer(auth);
                var serviceResponse = await _httpHelper.Put($"{url}/{userId}", null, token);
                var json = (JObject)serviceResponse;
                var statusCode = json["statusCode"];
            }

            int modifiedById = await _userData.GetIdByUsername(modifiedBy);

            var user = new xgca.entity.Models.User
            {
                UserId = userId,
                Status = obj.Status,
                ModifiedBy = modifiedById,
                ModifiedOn = DateTime.UtcNow
            };

            var userResult = await _userData.UpdateStatus(user);

            return userResult
                ? _general.Response(null, 200, "User Status updated", true)
                : _general.Response(null, 400, "Error on updating user", false);
        }

        public async Task<IGeneralModel> UpdateLock(UpdateUserLockModel obj, string modifiedBy, string auth)
        {
            if (obj == null)
            { return _general.Response(null, 400, "Data cannot be null", false); }

            int userId = await _userData.GetIdByGuid(Guid.Parse(obj.UserId));

            var getUserName = await _userData.Retrieve(userId);
            if (!getUserName.Username.IsNullOrEmpty())
            {
                string url = "";
                if (obj.IsLocked == 1)
                {
                    url = _optimusAuthService.Value.BaseUrl + _optimusAuthService.Value.DisableUser;
                }
                else
                {
                    url = _optimusAuthService.Value.BaseUrl + _optimusAuthService.Value.EnableUser;
                }
                string token = _tokenHelper.RemoveBearer(auth);
                var serviceResponse = await _httpHelper.Put($"{url}/{userId}", null, token);
                var json = (JObject)serviceResponse;
                var statusCode = json["statusCode"];
            }

            int modifiedById = await _userData.GetIdByUsername(modifiedBy);

            var user = new xgca.entity.Models.User
            {
                UserId = userId,
                IsLocked = obj.IsLocked,
                ModifiedBy = modifiedById,
                ModifiedOn = DateTime.UtcNow
            };

            var userResult = await _userData.UpdateLock(user);

            return userResult
                ? _general.Response(null, 200, "User Lock Status updated", true)
                : _general.Response(null, 400, "Error on updating user", false);
        }

        public async Task<IGeneralModel> DeleteMultipleUser(DeleteMultipleUserModel obj, string modifiedBy, string auth)
        {
            if (obj == null)
            { return _general.Response(null, 400, "Data cannot be null", false); }

            List<int> Ids = new List<int>();
            List<int> NoUserNameList = new List<int>();
            foreach (string UserId in obj.UserId)
            {
                int userId = await _userData.GetIdByGuid(Guid.Parse(UserId));
                var getUserName = await _userData.Retrieve(userId);
                if (getUserName.Username == null || getUserName.Username == "")
                {
                    NoUserNameList.Add(userId);
                }
                Ids.Add(userId);
            }

            var arrIds = new { Ids = Ids };
            string url = _optimusAuthService.Value.BaseUrl + _optimusAuthService.Value.DisableUserBatch;

            string token = _tokenHelper.RemoveBearer(auth);
            var serviceResponse = await _httpHelper.Put(url, arrIds, token);
            var json = (JObject)serviceResponse;
            var IdsSuccessList = json["data"]["success"];


            List<int> newIdsList = new List<int>(NoUserNameList);
            foreach (int successUserId in IdsSuccessList)
            {
                newIdsList.Add(successUserId);
            }


            int modifiedById = await _userData.GetIdByUsername(modifiedBy);

            var userResult = await _userData.Delete(
                newIdsList,
                modifiedById);

            return userResult
                ? _general.Response(null, 200, "Users has been deleted", true)
                : _general.Response(null, 400, "Error on updating user", false);
        }



        public async Task<IGeneralModel> UpdateMultipleLock(UpdateMultipleLockModel obj, string modifiedBy, string auth)
        {
            if (obj == null)
            { return _general.Response(null, 400, "Data cannot be null", false); }

            List<int> Ids = new List<int>();
            List<int> NoUserNameList = new List<int>();
            foreach (string UserId in obj.UserId)
            {
                int userId = await _userData.GetIdByGuid(Guid.Parse(UserId));
                var getUserName = await _userData.Retrieve(userId);
                if (getUserName.Username == null || getUserName.Username == "")
                {
                    NoUserNameList.Add(userId);
                }
                Ids.Add(userId);
            }

            //var serviceKey = await _httpHelpers.GetGuidById(_options.Value.BaseUrl, $"{_options.Value.GetService}/", companyService.ServiceId, AuthToken.Contra);
            var arrIds = new { Ids = Ids };
            string url = "";
            if (obj.IsLocked == 1)
            {
                url = _optimusAuthService.Value.BaseUrl + _optimusAuthService.Value.DisableUserBatch;
            }
            else
            {
                url = _optimusAuthService.Value.BaseUrl + _optimusAuthService.Value.EnableUserBatch;
            }
            string token = _tokenHelper.RemoveBearer(auth);
            var serviceResponse = await _httpHelper.Put(url, arrIds, token);
            var json = (JObject)serviceResponse;
            var IdsSuccessList = json["data"]["success"];


            List<int> newIdsList = new List<int>(NoUserNameList);
            foreach (int successUserId in IdsSuccessList)
            {
                newIdsList.Add(successUserId);
            }


            int modifiedById = await _userData.GetIdByUsername(modifiedBy);

            var userResult = await _userData.UpdateLock(
                newIdsList,
                modifiedById,
                obj.IsLocked);

            return userResult
                ? _general.Response(null, 200, "User Multiple Lock Status updated", true)
                : _general.Response(null, 400, "Error on updating user", false);
        }


        public async Task<IGeneralModel> UpdateMultipleStatus(UpdateMultipleStatusModel obj, string modifiedBy, string auth)
        {
            if (obj == null)
            { return _general.Response(null, 400, "Data cannot be null", false); }

            List<int> Ids = new List<int>();
            List<int> NoUserNameList = new List<int>();
            foreach (string UserId in obj.UserId)
            {
                int userId = await _userData.GetIdByGuid(Guid.Parse(UserId));
                var getUserName = await _userData.Retrieve(userId);
                if (getUserName.Username == null || getUserName.Username == "") {
                    NoUserNameList.Add(userId);
                }
                Ids.Add(userId);
            }

            //var serviceKey = await _httpHelpers.GetGuidById(_options.Value.BaseUrl,  $"{_options.Value.GetService}/", companyService.ServiceId, AuthToken.Contra);
            var arrIds = new { Ids = Ids };
            string url = "";
            if (obj.Status == 1)
            {
                url = _optimusAuthService.Value.BaseUrl + _optimusAuthService.Value.EnableUserBatch;
            }
            else {
                url = _optimusAuthService.Value.BaseUrl + _optimusAuthService.Value.DisableUserBatch;
            }
            string token = _tokenHelper.RemoveBearer(auth);
            var serviceResponse = await _httpHelper.Put(url, arrIds, token);
            var json = (JObject)serviceResponse;
            var IdsSuccessList = json["data"]["success"];


            List<int> newIdsList = new List<int>(NoUserNameList);
            foreach (int successUserId in IdsSuccessList)
            {
                newIdsList.Add(successUserId);
            }


            int modifiedById = await _userData.GetIdByUsername(modifiedBy);

            var userResult = await _userData.UpdateStatus(
                newIdsList,
                modifiedById,
                obj.Status);

            return userResult
                ? _general.Response(null, 200, "User Multiple Status updated", true)
                : _general.Response(null, 400, "Error on updating user", false);
        }

        public async Task<IGeneralModel> Retrieve(string key)
        {
            int userId = await _userData.GetIdByGuid(Guid.Parse(key));
            var data = await _userData.Retrieve(userId);
            int companyUsersId = await _coreCompanyUser.GetIdByUserId(userId);
            var companyServiceUsers = await _coreCompanyServiceUser.ListUserServiceRolesByCompanyUserId(companyUsersId);


            if (data == null)
            {
                return _general.Response(null, 400, "Selected user might have been deleted or does not exists", false);
            }



            var result = new
            {
                UserId = data.Guid,
                data.Username,
                data.FirstName,
                data.LastName,
                data.MiddleName,
                data.Title,
                data.Status,
                data.ImageURL,
                data.EmailAddress,
                ContactDetailId = data.ContactDetails.Guid,
                Phone = new
                {
                    data.ContactDetails.PhonePrefixId,
                    data.ContactDetails.PhonePrefix,
                    data.ContactDetails.Phone,
                },
                Mobile = new
                {
                    data.ContactDetails.MobilePrefixId,
                    data.ContactDetails.MobilePrefix,
                    data.ContactDetails.Mobile,
                },
                Roles = new { companyServiceUsers.data.data }
            };



            return _general.Response(result, 200, "Configurable information for selected user has been displayed", true);
        }
        public async Task<IGeneralModel> RetrieveByUsername(string username)
        {
            var data = await _userData.RetrieveByUsername(username);
            var companyServiceUsers = await _coreCompanyServiceUser.ListUserServiceRolesByCompanyUserId(data.CompanyUsers.CompanyUserId);
            if (data == null)
            {
                return _general.Response(null, 400, "Selected user might have been deleted or does not exists", false);
            }

            var result = new
            {
                UserId = data.Guid,
                data.Username,
                data.FirstName,
                data.LastName,
                data.MiddleName,
                data.Title,
                data.Status,
                data.ImageURL,
                data.EmailAddress,
                data.IsLocked,
                ContactDetailId = data.ContactDetails.Guid,
                Phone = new
                {
                    data.ContactDetails.PhonePrefixId,
                    data.ContactDetails.PhonePrefix,
                    data.ContactDetails.Phone,
                },
                Mobile = new
                {
                    data.ContactDetails.MobilePrefixId,
                    data.ContactDetails.MobilePrefix,
                    data.ContactDetails.Mobile,
                },
                Roles = new { companyServiceUsers.data.data }
            };

            return _general.Response(result, 200, "Configurable information for selected user has been displayed", true);
        }
        public async Task<IGeneralModel> Delete(string key, string modifiedBy, string auth)
        {
            int userId = await _userData.GetIdByGuid(Guid.Parse(key));
            var user = await _userData.Retrieve(userId);

            if (!user.Username.IsNullOrEmpty())
            {
                string url = _optimusAuthService.Value.BaseUrl + _optimusAuthService.Value.DisableUser;
                string token = _tokenHelper.RemoveBearer(auth);
                var serviceResponse = await _httpHelper.Put($"{url}/{userId}", null, token);
                var json = (JObject)serviceResponse;
                var statusCode = json["statusCode"];
            }

            var result = await _userData.Delete(userId);
            return result
                ? _general.Response(true, 200, "User deleted", true)
                : _general.Response(false, 400, "Error on deleting user", true);
        }

        public async Task<IGeneralModel> GetIdByGuid(string key)
        {
            int userId = await _userData.GetIdByGuid(Guid.Parse(key));
            if (userId == 0)
            {
                return _general.Response(null, 400, "Error on retrieving user id", true);
            }
            return _general.Response(new { UserId = userId }, 200, "User Id retrieved", true);
        }
        public async Task<IGeneralModel> SetUsername(SetUsernameModel obj)
        {
            bool isUserMaster = false;
            var data = new entity.Models.User
            {
                UserId = obj.UserId,
                Username = obj.Username,
                ModifiedBy = GlobalVariables.SystemUserId,
                ModifiedOn = DateTime.UtcNow,
                Status = 1
            };

            var result = await _userData.SetUsername(data);

            foreach (int item in result.IsUserMaster)
            {
                if (item == 1)
                {
                    isUserMaster = true;
                }
            }

            return result.Result
                ? _general.Response(new { isUserMaster = isUserMaster}, 200, "Username updated", true)
                : _general.Response(false, 400, "Error on updating username", true);
        }
        public async Task<int> GetIdByGuid(Guid key)
        {
            int userId = await _userData.GetIdByGuid(key);
            return userId;
        }

        public Task<bool> Create(xgca.entity.Models.User obj)
        {
            throw new NotImplementedException();
        }

        public async Task<int> GetIdByUsername(string username)
        {
            var user = await _userData.RetrieveByUsername(username);
            return user.UserId;
        }

        public async Task<IGeneralModel> GetUserByReferenceId(int id)
        {
            var user = await _userData.Retrieve(id);

            return user == null 
                ? _general.Response(null, 400, "Invalid User", true) 
                : _general.Response(new { name = user.FirstName + " " + user.LastName}, 200, "User details retrieved", true);
        }

        public async Task<IGeneralModel> ListUserLogs(string? userKey, string? username)
        {
            int userId = 0;
            if(!(username is null))
            {
                userId = await _userData.GetIdByUsername(username);
            }
            else if(!(userKey is null ))
            {
                userId = await _userData.GetIdByGuid(Guid.Parse(userKey));
            }

            var data = await _auditLog.ListByTableNameAndKeyFieldId("User", userId);

            List<ListAuditLogModel> logs = new List<ListAuditLogModel>();

            foreach (var d in data)
            {
                var user = await _userData.Retrieve(d.CreatedBy);

                logs.Add(new ListAuditLogModel
                {
                    AuditLogId = d.Guid.ToString(),
                    AuditLogAction = d.AuditLogAction,
                    CreatedBy = (d.CreatedBy == 0) ? "System" : d.CreatedByName,
                    Username = d.CreatedBy != 0 ? (!(user.Username is null) ? user.Username : "Not Set") : "system",
                    //Username = !(user.Username is null) ? (auditLog.CreatedBy == 0 ? "system" : user.Username) : "Not Set",
                    CreatedOn = d.CreatedOn.ToString(GlobalVariables.AuditLogTimeFormat)
                });
            }

            return _general.Response(new { Logs = logs }, 200, "Company audit logs has been listed", true);
        }

        public async Task<IGeneralModel> GetUserCounts(List<int> userIds)
        {
            int totalUsers = await _userData.GetTotalUsers(userIds);
            int totalActiveUsers = await _userData.GetTotalActiveUsers(userIds);
            int totalInactiveUsers = await _userData.GetTotalInactiveUsers(userIds);
            int totalLockedUsers = await _userData.GetTotalLockedUsers(userIds);
            int totalUnlockedUsers = await _userData.GetTotalUnlockedUsers(userIds);

            return _general.Response(new
            {
                TotalUsers = totalUsers,
                TotalLockedUsers = totalLockedUsers,
                TotalUnlockedUsers = totalUnlockedUsers,
                TotalActiveUsers = totalActiveUsers,
                TotalInactiveUsers = totalInactiveUsers,
            }, 200, "Total user counts displayed", true);
        }
    }
}
