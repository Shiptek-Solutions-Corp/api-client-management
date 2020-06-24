using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using xgca.core.Models.Company;
using xgca.core.Response;
using xgca.entity.Models;
using xgca.data.Company;
using xgca.data.CompanyService;
using xgca.data.AuditLog;
using xgca.data.User;
using xgca.core.User;
using xgca.core.CompanyServiceRole;
using xgca.core.CompanyServiceUser;
using xgca.core.CompanyUser;
using xgca.core.Helpers;
using xgca.core.Helpers.Http;
using xgca.core.Constants;
using xgca.core.Models.CompanyService;
using xgca.core.Models.AuditLog;
using xgca.core.Helpers.Token;
using Amazon.Runtime.Internal;

namespace xgca.core.Company
{
    public class Company : ICompany
    {
        private readonly ICompanyData _companyData;
        private readonly xgca.core.Address.IAddress _coreAddress;
        private readonly xgca.core.ContactDetail.IContactDetail _coreContactDetail;
        private readonly IUser _coreUser;
        private readonly IAuditLogData _auditLog;        
        private readonly xgca.core.CompanyService.ICompanyService _coreCompanyService;
        private readonly ICompanyServiceRole _coreCompanyServiceRole;
        private readonly ICompanyServiceUser _coreCompanyServiceUser;
        private readonly ICompanyUser _coreCompanyUser;

        private readonly IUserData _userData;

        private readonly IHttpHelper _httpHelper;
        private readonly ITokenHelper _tokenHelper;
        private readonly IOptions<GlobalCmsApi> _options;
        private readonly IGeneral _general;

        public Company(ICompanyData companyData,
            xgca.core.Address.IAddress coreAddress,
            xgca.core.ContactDetail.IContactDetail coreContactDetail,
            xgca.core.User.IUser coreUser,
            xgca.data.AuditLog.IAuditLogData auditLog,
            xgca.core.CompanyService.ICompanyService coreCompanyService,
            ICompanyServiceRole coreCompanyServiceRole,
            ICompanyServiceUser coreCompanyServiceUser,
            ICompanyUser coreCompanyUser,
            IUserData userData,
            IHttpHelper httpHelper,
            ITokenHelper tokenHelper,
            IOptions<GlobalCmsApi> options,
            IGeneral general)
        {
            _companyData = companyData;
            _coreAddress = coreAddress;
            _coreContactDetail = coreContactDetail;
            _coreUser = coreUser;
            _auditLog = auditLog;
            _coreCompanyService = coreCompanyService;
            _coreCompanyServiceRole = coreCompanyServiceRole;
            _coreCompanyServiceUser = coreCompanyServiceUser;
            _coreCompanyUser = coreCompanyUser;
            _userData = userData;
            _httpHelper = httpHelper;
            _tokenHelper = tokenHelper;
            _options = options;
            _general = general;
        }
        
        public async Task<IGeneralModel> List()
        {
            var company = await _companyData.List();
            var data = company.Select(c => new { CompanyId = c.Guid, c.CompanyName, c.Status });
            return _general.Response(new { companty = data }, 200, "Configurable companies has been listed", true);
        }

        public async Task<IGeneralModel> Create(CreateCompanyModel obj, string createdBy)
        {
            if (obj == null)
            { return _general.Response(null, 400, "Data cannot be null", true); }

            int createdById = await _coreUser.GetIdByUsername(createdBy);

            int addressId = await _coreAddress.CreateAndReturnId(obj, createdById);
            if (addressId <= 0)
            { return _general.Response(false, 400, "Error on creating company", true); }

            int contactDetailId = await _coreContactDetail.CreateAndReturnId(obj, createdById);
            if (contactDetailId <= 0)
            { return _general.Response(false, 400, "Error on creating company", true); }

            var company = new xgca.entity.Models.Company
            {
                ClientId = 1,
                CompanyName = obj.CompanyName,
                AddressId = addressId,
                ContactDetailId = contactDetailId,
                ImageURL = obj.ImageURL,
                WebsiteURL = obj.WebsiteURL,
                EmailAddress = obj.EmailAddress,
                TaxExemption = obj.TaxExemption,
                TaxExemptionStatus = obj.TaxExemptionStatus,
                CreatedBy = createdById,
                CreatedOn = DateTime.Now,
                ModifiedBy = createdById,
                ModifiedOn = DateTime.Now,
                Guid = Guid.NewGuid(),
                Status = 1
            };

            var companyId = await _companyData.CreateAndReturnId(company);
            if (companyId <= 0)
            { return _general.Response(null, 400, "Error on creating company", true); }
            await _coreCompanyService.CreateBatch(obj.Services, companyId, createdById);
            var auditLog = AuditLogHelper.BuildAuditLog(obj, "Create", company.GetType().Name, companyId, createdById);
            await _auditLog.Create(auditLog);
            return companyId > 0
                ? _general.Response(new { companyId = companyId }, 200, "Company created", true)
                : _general.Response(null, 400, "Error on creating company", true);
        }
        public async Task<IGeneralModel> InitialRegistration(InitialRegistrationModel obj)
        {
            if (obj == null)
            { return _general.Response(null, 400, "Data cannot be null", true); }

            int addressId = await _coreAddress.CreateAndReturnId(obj, GlobalVariables.SystemUserId);
            if (addressId <= 0)
            { return _general.Response(false, 400, "Error on creating company", true); }

            int contactDetailId = await _coreContactDetail.CreateAndReturnId(obj, GlobalVariables.SystemUserId);
            if (contactDetailId <= 0)
            { return _general.Response(false, 400, "Error on creating company", true); }

            var company = new xgca.entity.Models.Company
            {
                ClientId = 1,
                CompanyName = obj.CompanyName,
                AddressId = addressId,
                ContactDetailId = contactDetailId,
                ImageURL = obj.ImageURL,
                WebsiteURL = obj.WebsiteURL,
                EmailAddress = obj.EmailAddress,
                CreatedBy = GlobalVariables.SystemUserId,
                CreatedOn = DateTime.Now,
                ModifiedBy = GlobalVariables.SystemUserId,
                ModifiedOn = DateTime.Now,
                Guid = Guid.NewGuid(),
                Status = 1
            };

            var companyId = await _companyData.CreateAndReturnId(company);
            if (companyId <= 0)
            { return _general.Response(null, 400, "Error on creating company", true); }

            await _coreCompanyService.CreateBatch(obj.Services, companyId, GlobalVariables.SystemUserId);
            await _coreCompanyServiceRole.CreateDefault(companyId, GlobalVariables.SystemUserId);

            var masterUserObj = obj.MasterUser;
            dynamic masterUser = await _coreUser.CreateMasterUser(masterUserObj, GlobalVariables.SystemUserId);
            int companyUserId = await _coreCompanyUser.CreateDefaultCompanyUser(companyId, masterUser.MasterUserId, GlobalVariables.SystemUserId);
            await _coreCompanyServiceUser.CreateDefault(companyId, companyUserId, GlobalVariables.SystemUserId);

            var newCompany = await _companyData.Retrieve(companyId);
            var newCompanyServicesResponse = await _coreCompanyService.ListByCompanyId(newCompany.Guid.ToString());
            var newCompanyServices = newCompanyServicesResponse.data.companyService;
            var companyLog = CompanyHelper.BuildCompanyValue(newCompany, newCompanyServices);

            var auditLog = AuditLogHelper.BuildAuditLog(companyLog, "Create", company.GetType().Name, companyId, GlobalVariables.SystemUserId);
            await _auditLog.Create(auditLog);
            return companyId > 0
                ? _general.Response(new { CompanyId = companyId, MasterUserId = masterUser.MasterUserId }, 200, "Company registration successful", true)
                : _general.Response(false, 400, "Error on registration", true);
        }

        public async Task<IGeneralModel> Update(UpdateCompanyModel obj, string modifiedBy)
        {
            if (obj == null)
            { return _general.Response(null, 400, "Data cannot be null", true); }

            int modifiedById = await _coreUser.GetIdByUsername(modifiedBy);

            int oldCompanyId = await _companyData.GetIdByGuid(Guid.Parse(obj.CompanyId));
            var oldCompany = await _companyData.Retrieve(oldCompanyId);
            var oldCompanyServicesResponse = await _coreCompanyService.ListByCompanyId(oldCompany.Guid.ToString());
            var oldCompanyServices = oldCompanyServicesResponse.data.companyService;
            var oldValue = CompanyHelper.BuildCompanyValue(oldCompany, oldCompanyServices);

            
            int addressId = await _coreAddress.UpdateAndReturnId(obj, modifiedById);
            if (addressId <= 0)
            { return _general.Response(false, 400, "Error on updating company", true); }
           
            int contactDetailId  = await _coreContactDetail.UpdateAndReturnId(obj, modifiedById);
            if (contactDetailId <= 0)
            { return _general.Response(false, 400, "Error on updating company", true); }

            int companyId = await _companyData.GetIdByGuid(Guid.Parse(obj.CompanyId));
            var company = new xgca.entity.Models.Company
            {
                CompanyId = companyId,
                CompanyName = obj.CompanyName,
                AddressId = addressId,
                ContactDetailId = contactDetailId,
                ImageURL = obj.ImageURL,
                WebsiteURL = obj.WebsiteURL,
                EmailAddress = obj.EmailAddress,
                TaxExemption = obj.TaxExemption,
                TaxExemptionStatus = obj.TaxExemptionStatus,
                ModifiedBy = modifiedById,
                ModifiedOn = DateTime.Now,
                Guid = Guid.Parse(obj.CompanyId),
            };

            var companyResult = await _companyData.Update(company);

            await _coreCompanyService.UpdateBatch(obj.CompanyServices, companyId, modifiedById);

            // Return updated company detail
            var companyServicesResponse = await _coreCompanyService.ListByCompanyId(obj.CompanyId);
            var companyServices = companyServicesResponse.data.companyService;
            var newCompany = await _companyData.Retrieve(companyId);
            var updatedCompany = CompanyHelper.ReturnUpdatedValue(newCompany, companyServices);

            var newValue = CompanyHelper.BuildCompanyValue(newCompany, companyServices);
            var auditLog = AuditLogHelper.BuildAuditLog(oldValue, newValue, "Update", company.GetType().Name, company.CompanyId, modifiedById);
            await _auditLog.Create(auditLog);

            return companyResult
                ? _general.Response(new { company = updatedCompany }, 200, "Company updated", true)
                : _general.Response(null, 400, "Error on updating company", true);
        }
        public async Task<IGeneralModel> Retrieve(int companyId)
        {
            string companyKey = await _companyData.GetGuidById(companyId);
            if (companyKey is null)
            { return _general.Response(null, 400, "Selected company might have been deleted or does not exists", false); }
            var result = await _companyData.Retrieve(companyId);
            if (result == null)
            { return _general.Response(null, 400, "Selected company might have been deleted or does not exists", false); }

            var cityResponse = await _httpHelper.GetGuidById(_options.Value.BaseUrl, ApiEndpoints.cmsGetCity, result.Addresses.CityId, AuthToken.Contra);
            var cityJson = (JObject)cityResponse;
            var stateResponse = await _httpHelper.GetGuidById(_options.Value.BaseUrl, ApiEndpoints.cmsGetState, result.Addresses.StateId, AuthToken.Contra);
            var stateJson = (JObject)stateResponse;

            var companyServices = await _coreCompanyService.ListByCompanyId(companyKey);

            var data = new
            {
                CompanyId = result.Guid,
                result.CompanyName,
                result.ImageURL,
                AddressId = result.Addresses.Guid,
                result.Addresses.AddressLine,
                City = new
                {
                    CityId = (cityJson)["data"]["cityId"],
                    result.Addresses.CityName,
                },
                State = new
                {
                    StateId = (stateJson)["data"]["stateId"],
                    result.Addresses.StateName,
                },
                Country = new
                {
                    result.Addresses.CountryId,
                    result.Addresses.CountryName,
                },
                result.Addresses.ZipCode,
                result.Addresses.FullAddress,
                result.Addresses.Longitude,
                result.Addresses.Latitude,
                result.WebsiteURL,
                result.EmailAddress,
                ContactDetailId = result.ContactDetails.Guid,
                Phone = new
                {
                    result.ContactDetails.PhonePrefixId,
                    result.ContactDetails.PhonePrefix,
                    result.ContactDetails.Phone,
                },
                Mobile = new
                {
                    result.ContactDetails.MobilePrefixId,
                    result.ContactDetails.MobilePrefix,
                    result.ContactDetails.Mobile,
                },
                Fax = new
                {
                    result.ContactDetails.FaxPrefixId,
                    result.ContactDetails.FaxPrefix,
                    result.ContactDetails.Fax,
                },
                result.TaxExemption,
                result.TaxExemptionStatus,
                CompanyServices = companyServices.data.companyService,
            };

            return _general.Response(new { company = data }, 200, "Configurable information for selected company has been displayed", true);
        }
        public async Task<IGeneralModel> Delete(string key)
        {
            int companyId = await _companyData.GetIdByGuid(Guid.Parse(key));
            if (companyId == 0)
            { return _general.Response(false, 400, "Error on deleting company", true); }
            var result = await _companyData.Delete(companyId);
            return result
                ? _general.Response(true, 200, "Company deleted", true)
                : _general.Response(false, 400, "Error on deleting company", true);
        }

        public async Task<IGeneralModel> GetIdByGuid(string key)
        {
            int companyId = await _companyData.GetIdByGuid(Guid.Parse(key));
            if (companyId == 0)
            { return _general.Response(new { CompanyId = companyId }, 400, "Error on retrieving company id", true); }
            return _general.Response(new { CompanyId = companyId }, 200, "Company id retrieved", true);
        }

        public async Task<int> GetIdByGuid(Guid key)
        {
            int companyId = await _companyData.GetIdByGuid(key);
            return companyId;
        }

        public Task<IGeneralModel> CreateAndReturnId(CreateCompanyModel obj)
        {
            throw new NotImplementedException();
        }

        public async Task<IGeneralModel> Update(UpdateCompanyModel obj)
        {
            if (obj == null)
            { return _general.Response(null, 400, "Data cannot be null", true); }

            int oldCompanyId = await _companyData.GetIdByGuid(Guid.Parse(obj.CompanyId));
            var oldCompany = await _companyData.Retrieve(oldCompanyId);
            var oldCompanyServicesResponse = await _coreCompanyService.ListByCompanyId(oldCompany.Guid.ToString());
            var oldCompanyServices = oldCompanyServicesResponse.data.companyService;
            var oldValue = CompanyHelper.BuildCompanyValue(oldCompany, oldCompanyServices);


            int addressId = await _coreAddress.UpdateAndReturnId(obj, GlobalVariables.SystemUserId);
            if (addressId <= 0)
            { return _general.Response(false, 400, "Error on updating company", true); }

            int contactDetailId = await _coreContactDetail.UpdateAndReturnId(obj, GlobalVariables.SystemUserId);
            if (contactDetailId <= 0)
            { return _general.Response(false, 400, "Error on updating company", true); }

            int companyId = await _companyData.GetIdByGuid(Guid.Parse(obj.CompanyId));
            var company = new xgca.entity.Models.Company
            {
                CompanyId = companyId,
                CompanyName = obj.CompanyName,
                AddressId = addressId,
                ContactDetailId = contactDetailId,
                ImageURL = obj.ImageURL,
                WebsiteURL = obj.WebsiteURL,
                EmailAddress = obj.EmailAddress,
                TaxExemption = obj.TaxExemption,
                TaxExemptionStatus = obj.TaxExemptionStatus,
                ModifiedBy = GlobalVariables.SystemUserId,
                ModifiedOn = DateTime.Now,
                Guid = Guid.Parse(obj.CompanyId),
            };

            var companyResult = await _companyData.Update(company);
            var newCompany = await _companyData.Retrieve(companyId);
            var newCompanyServicesResponse = await _coreCompanyService.ListByCompanyId(newCompany.Guid.ToString());
            var newCompanyServices = newCompanyServicesResponse.data.companyService;
            var newValue = CompanyHelper.BuildCompanyValue(company, newCompanyServices);
            await _coreCompanyService.UpdateBatch(obj.CompanyServices, companyId, GlobalVariables.SystemUserId);
            var auditLog = AuditLogHelper.BuildAuditLog(oldValue, newValue, "Update", company.GetType().Name, company.CompanyId, GlobalVariables.SystemUserId);
            await _auditLog.Create(auditLog);

            // Return updated company detail
            var companyServicesResponse = await _coreCompanyService.ListByCompanyId(obj.CompanyId);
            var companyServices = companyServicesResponse.data.companyService;
            var updatedCompany = CompanyHelper.ReturnUpdatedValue(obj, companyServices);

            return companyResult
                ? _general.Response(new { company = updatedCompany }, 200, "Company updated", true)
                : _general.Response(null, 400, "Error on updating company", true);
        }

        public async Task<IGeneralModel> ListCompanyLogs(int companyId)
        {
            var data = await _auditLog.ListByTableNameAndKeyFieldId("Company", companyId);

            var auditLogs = data.Select(logs => new
            {
                AuditLogId = logs.Guid,
                logs.AuditLogAction,
                logs.CreatedBy,
                logs.CreatedOn
            });

            List<ListAuditLogModel> logs = new List<ListAuditLogModel>();

            foreach (var auditLog in auditLogs)
            {
                var user = await _userData.Retrieve(auditLog.CreatedBy);

                logs.Add(new ListAuditLogModel
                {
                    AuditLogId = auditLog.AuditLogId.ToString(),
                    AuditLogAction = auditLog.AuditLogAction,
                    CreatedBy = (auditLog.CreatedBy == 0) ? "System" : String.Concat(user.FirstName, " ", user.LastName),
                    Username = auditLog.CreatedBy != 0 ? (!(user.Username is null) ? user.Username : "Not Set") : "system",
                    //Username = !(user.Username is null) ? (auditLog.CreatedBy == 0 ? "system" : user.Username) : "Not Set",
                    CreatedOn = auditLog.CreatedOn
                });
            }

            return _general.Response(new { Logs = logs }, 200, "Company audit logs has been listed", true);
        }

        public async Task<IGeneralModel> ListCompanyLogs(string companyId)
        {
            int companyKey = await _companyData.GetIdByGuid(Guid.Parse(companyId));
            var data = await _auditLog.ListByTableNameAndKeyFieldId("Company", companyKey);

            var auditLogs = data.Select(logs => new
            {
                AuditLogId = logs.Guid,
                logs.AuditLogAction,
                logs.CreatedBy,
                logs.CreatedOn
            });

            List<ListAuditLogModel> logs = new List<ListAuditLogModel>();

            foreach (var auditLog in auditLogs)
            {
                var user = await _userData.Retrieve(auditLog.CreatedBy);

                logs.Add(new ListAuditLogModel
                {
                    AuditLogId = auditLog.AuditLogId.ToString(),
                    AuditLogAction = auditLog.AuditLogAction,
                    CreatedBy = (auditLog.CreatedBy == 0) ? "System" : String.Concat(user.FirstName, " ", user.LastName),
                    Username = auditLog.CreatedBy != 0 ? (!(user.Username is null) ? user.Username : "Not Set") : "system",
                    //Username = !(user.Username is null) ? (auditLog.CreatedBy == 0 ? "system" : user.Username) : "Not Set",
                    CreatedOn = auditLog.CreatedOn
                });
            }

            return _general.Response(new { Logs = logs }, 200, "Company audit logs has been listed", true);
        }
    }
}
