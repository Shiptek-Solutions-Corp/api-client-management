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
using xgca.core.Helpers;
using xgca.core.Helpers.Token;

namespace xgca.core.User
{
    public class User : IUser
    {
        private readonly xgca.data.User.IUserData _userData;
        private readonly xgca.core.ContactDetail.IContactDetail _coreContactDetail;
        private readonly xgca.core.CompanyUser.ICompanyUser _coreCompanyUser;
        private readonly xgca.data.AuditLog.IAuditLog _auditLog;
        private readonly ITokenHelper _tokenHelper;
        private readonly IGeneral _general;

        public User(xgca.data.User.IUserData userData,
            xgca.core.ContactDetail.IContactDetail coreContactDetail,
            xgca.core.CompanyUser.ICompanyUser coreCompanyUser,
            xgca.data.AuditLog.IAuditLog auditLog,
            ITokenHelper tokenHelper,
            IGeneral general)
        {
            _userData = userData;
            _coreContactDetail = coreContactDetail;
            _coreCompanyUser = coreCompanyUser;
            _auditLog = auditLog;
            _tokenHelper = tokenHelper;
            _general = general;
        }

        public async Task<IGeneralModel> List()
        {
            var user = await _userData.List();
            var data = new { user = user.Select(u => new { UserId = u.Guid, u.FirstName, u.LastName.Length, u.MiddleName, u.ImageURL, u.EmailAddress, u.Status }) };
            return _general.Response(data, 200, "Configurable companies has been listed", true);
        }

        public async Task<IGeneralModel> Create(CreateUserModel obj)
        {
            if (obj == null)
            { 
                return _general.Response(false, 400, "Data cannot be null", false); 
            }

            int userId = 1;
            if (obj.CreatedBy != null)
            { userId = await _userData.GetIdByGuid(Guid.Parse(obj.CreatedBy)); }

            int contactDetailId = await _coreContactDetail.CreateAndReturnId(obj);
            if (contactDetailId <= 0)
            {
                return _general.Response(false, 400, "Data cannot be null", false);
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
                CreatedBy = userId,
                CreatedOn = DateTime.Now,
                ModifiedBy = userId,
                ModifiedOn = DateTime.Now,
                Status = 1,
                Guid = Guid.NewGuid()
            };

            int newUserId = await _userData.CreateAndReturnId(user);
            var newUserGuid = await _userData.GetGuidById(newUserId);
            var companyUser = CompanyHelper.BuildCompanyValue(obj);
            await _coreCompanyUser.Create(companyUser);
            
            var auditLog = AuditLogHelper.BuilCreateLog(obj, "Create", user.GetType().Name, newUserId);
            await _auditLog.Create(auditLog);
            return newUserId > 0
                ? _general.Response(new { userGuid = newUserGuid }, 200, "User created", false)
                : _general.Response(false, 400, "Data cannot be null", false);
        }
        public async Task<dynamic> CreateMasterUser(CreateUserModel obj, int createdBy)
        {
            if (obj == null)
            { return 0; }

            int contactDetailId = await _coreContactDetail.CreateAndReturnId(obj);
            if (contactDetailId <= 0)
            { return 0; }

            var user = new xgca.entity.Models.User
            {
                FirstName = obj.FirstName,
                LastName = obj.LastName,
                Title = obj.Title,
                ContactDetailId = contactDetailId,
                ImageURL = obj.ImageURL,
                EmailAddress = obj.EmailAddress,
                CreatedBy = createdBy,
                CreatedOn = DateTime.Now,
                ModifiedBy = createdBy,
                ModifiedOn = DateTime.Now,
                Guid = Guid.NewGuid()
            };

            var masterUserId = await _userData.CreateAndReturnId(user);
            var masterUserGuid = await _userData.GetGuidById(masterUserId);
            var auditLog = AuditLogHelper.BuilCreateLog(obj, "Create", user.GetType().Name, masterUserId);
            await _auditLog.Create(auditLog);
            return new { MasterUserId = masterUserId, MasterUserGuid = masterUserGuid };
        }
        public async Task<int> CreateAndReturnId(CreateUserModel obj)
        {
            if (obj == null)
            { return 0; }

            int userId = await _userData.GetIdByGuid(Guid.Parse(obj.CreatedBy));

            int contactDetailId = await _coreContactDetail.CreateAndReturnId(obj);
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
                CreatedBy = userId,
                CreatedOn = DateTime.Now,
                ModifiedBy = userId,
                ModifiedOn = DateTime.Now,
                Guid = Guid.NewGuid()
            };

            var newUserId = await _userData.CreateAndReturnId(user);
            var auditLog = AuditLogHelper.BuilCreateLog(obj, "Create", user.GetType().Name, newUserId);
            await _auditLog.Create(auditLog);
            return newUserId;
        }

        public async Task<IGeneralModel> Update(UpdateUserModel obj)
        {
            if (obj == null)
            { return _general.Response(null, 400, "Data cannot be null", true); }

            int userId = await _userData.GetIdByGuid(Guid.Parse(obj.UserId));
            var oldUser = await _userData.Retrieve(userId);
            var oldValue = UserHelper.BuildUserValue(oldUser);

            int contactDetailId = await _coreContactDetail.UpdateAndReturnId(obj);
            if (contactDetailId <= 0)
            {
                return _general.Response(false, 400, "Error on updating company", true);
            }

            var user = new xgca.entity.Models.User
            {
                UserId = Convert.ToInt32(obj.UserId),
                FirstName = obj.FirstName,
                LastName = obj.LastName,
                MiddleName = obj.MiddleName,
                Title = obj.Title,
                ContactDetailId = contactDetailId,
                ImageURL = obj.ImageURL,
                EmailAddress = obj.EmailAddress,
                ModifiedBy = Convert.ToInt32(obj.ModifiedBy),
                ModifiedOn = DateTime.Now
            };

            var userResult = await _userData.Update(user);
            var newValue = UserHelper.BuildUserValue(user);
            var auditLog = AuditLogHelper.BuildUpdateLog(oldValue, newValue, "Update", user.GetType().Name, user.UserId);
            await _auditLog.Create(auditLog);
            return userResult
                ? _general.Response(true, 200, "User updated", true)
                : _general.Response(false, 400, "Error on updating user", true);
        }
        public async Task<IGeneralModel> Retrieve(string key)
        {
            int userId = await _userData.GetIdByGuid(Guid.Parse(key));
            var data = await _userData.Retrieve(userId);
            if (data == null)
            {
                return _general.Response(null, 400, "Selected user might have been deleted or does not exists", false);
            }

            var result = new
            {
                UserId = data.Guid,
                data.FirstName,
                data.LastName,
                data.MiddleName,
                data.Title,
                data.Status,
                data.ImageURL,
                data.EmailAddress,
                ContactDetailId = data.ContactDetails.Guid,
                data.ContactDetails.PhonePrefixId,
                data.ContactDetails.PhonePrefix,
                data.ContactDetails.Phone,
                data.ContactDetails.MobilePrefixId,
                data.ContactDetails.MobilePrefix,
                data.ContactDetails.Mobile,
                data.ContactDetails.FaxPrefixId,
                data.ContactDetails.FaxPrefix,
                data.ContactDetails.Fax
            };

            return _general.Response(result, 200, "Configurable information for selected user has been displayed", true);
        }
        public async Task<IGeneralModel> RetrieveByToken(string token)
        {
            var decodedToken = _tokenHelper.DecodeJWT(token);
            var tokenUsername = decodedToken.Payload["cognito:username"];

            var data = await _userData.RetrieveByUsername(tokenUsername);
            if (data == null)
            {
                return _general.Response(null, 400, "Selected user might have been deleted or does not exists", false);
            }

            var result = new
            {
                UserId = data.Guid,
                data.FirstName,
                data.LastName,
                data.MiddleName,
                data.Title,
                data.Status,
                data.ImageURL,
                data.EmailAddress,
                ContactDetailId = data.ContactDetails.Guid,
                data.ContactDetails.PhonePrefixId,
                data.ContactDetails.PhonePrefix,
                data.ContactDetails.Phone,
                data.ContactDetails.MobilePrefixId,
                data.ContactDetails.MobilePrefix,
                data.ContactDetails.Mobile,
                data.ContactDetails.FaxPrefixId,
                data.ContactDetails.FaxPrefix,
                data.ContactDetails.Fax
            };

            return _general.Response(result, 200, "Configurable information for selected user has been displayed", true);
        }
        public async Task<IGeneralModel> Delete(string key)
        {
            int userId = await _userData.GetIdByGuid(Guid.Parse(key));
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
            var data = new entity.Models.User
            {
                UserId = obj.UserId,
                Username = obj.Username,
                ModifiedBy = 0,
                ModifiedOn = DateTime.Now
            };

            var result = await _userData.SetUsername(data);
            return result
                ? _general.Response(true, 200, "Username updated", true)
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
    }
}
