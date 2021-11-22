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
using xgca.data.Guest;
using xgca.core.AuditLog;
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
using System.Data;
using System.Globalization;
using ClosedXML.Excel;
using System.IO;
using xgca.data.Repositories;
using xgca.core.Services;
using GlobalCmsService = xgca.core.Helpers.GlobalCmsService;

namespace xgca.core.Company
{
    public interface ICompany
    {
        Task<IGeneralModel> List();

        Task<IGeneralModel> ListByService(int serviceId, string companyName, int page, int rowCount);
        Task<IGeneralModel> Create(CreateCompanyModel obj, string createdBy);
        Task<IGeneralModel> CreateAndReturnId(CreateCompanyModel obj);
        Task<IGeneralModel> InitialRegistration(InitialRegistrationModel obj);
        Task<IGeneralModel> Update(UpdateCompanyModel obj);
        Task<IGeneralModel> Update(UpdateCompanyModel obj, string modifiedBy);
        Task<IGeneralModel> Retrieve(int companyId);
        Task<IGeneralModel> Retrieve(string companyId);
        Task<IGeneralModel> RetrieveCompanyName(GetCompanyNamesModel obj);
        Task<IGeneralModel> Delete(string key, string username);
        Task<IGeneralModel> GetIdByGuid(string key);
        Task<int> GetIdByGuid(Guid key);
        Task<IGeneralModel> GetGuidById(int key);

        Task<IGeneralModel> ListCompanyLogs(int companyId);
        Task<IGeneralModel> ListCompanyLogs(string companyId);
        Task<IGeneralModel> GetReservationActors(List<GetReservationActorsModel> getReservationActorsModel);
        Task<IGeneralModel> GetCompanyAndGuestByIds(List<string> guids);
        Task<IGeneralModel> ListCompaniesByIDs(GetCompanyIDs obj);
        Task<IGeneralModel> ListCompanyDetailsByIds(GetCompanyIDs obj);

        Task<IGeneralModel> BatchUpdateCompanyCode(string username);
        Task<IGeneralModel> UpdateAccreditedBy(UpdateAccreditedByModel obj, string username);

        Task<IGeneralModel> ListByCompanyName(string companyName);
        Task<IGeneralModel> CheckIfExistsByCompanyName(string companyName);
        Task<IGeneralModel> CheckIfCompanyExistsByCompanyName(string companyName);
        Task<IGeneralModel> BulkCheckIfExistsByCompanyName(string[] companyNames);

        Task<IGeneralModel> BulkCompanyRegistration(InitialRegistrationListModel registrationModels, string username);
        Task<IGeneralModel> SetCUCC(UpdateCUCCCodeDTO obj);
        Task<IGeneralModel> GetAccreditor(int companyId);
        Task<IGeneralModel> GetCompanyCode(string companyGuid);
        Task<IGeneralModel> GetCompanyCode();
        Task<IGeneralModel> GetInvoiceActors(string billerId, string customerId);
        Task<byte[]> DownloadCompanyProfileLogs(int companyId);
        Task<IGeneralModel> GetAccreditorShippingLine(string companyId);
        Task<IGeneralModel> GetByCompanyCode(string code);

    }
    public class Company : ICompany
    {
        private readonly ICompanyData _companyData;
        private readonly xgca.core.Address.IAddress _coreAddress;
        private readonly xgca.core.ContactDetail.IContactDetail _coreContactDetail;
        private readonly IUser _coreUser;
        private readonly IAuditLogData _auditLog;
        private readonly IAuditLogCore _coreAuditLog;
        private readonly xgca.core.CompanyService.ICompanyService _coreCompanyService;
        private readonly ICompanyServiceRole _coreCompanyServiceRole;
        private readonly ICompanyServiceUser _coreCompanyServiceUser;
        private readonly ICompanyUser _coreCompanyUser;

        private readonly IUserData _userData;
        private readonly IGuestData _guestData;

        private readonly ICompanySectionService _companySectionService;
        private readonly IOptions<GlobalCmsService> _options;
        private readonly IHttpHelper _httpHelper;
        private readonly ITokenHelper _tokenHelper;
        private readonly IGeneral _general;

        private readonly IKYCStatusRepository _kycRepository;

        public Company(ICompanyData companyData,
            xgca.core.Address.IAddress coreAddress,
            xgca.core.ContactDetail.IContactDetail coreContactDetail,
            xgca.core.User.IUser coreUser,
            xgca.data.AuditLog.IAuditLogData auditLog,
            xgca.core.CompanyService.ICompanyService coreCompanyService,
            IAuditLogCore coreAuditLog,
            ICompanyServiceRole coreCompanyServiceRole,
            ICompanyServiceUser coreCompanyServiceUser,
            ICompanyUser coreCompanyUser,
            IUserData userData,
            IGuestData guestData,
            ICompanySectionService companySectionService,
            IOptions<GlobalCmsService> options,
            IHttpHelper httpHelper,
            ITokenHelper tokenHelper,
            IGeneral general,
            IKYCStatusRepository kycRepository)
        {
            _companyData = companyData;
            _coreAddress = coreAddress;
            _coreContactDetail = coreContactDetail;
            _coreUser = coreUser;
            _auditLog = auditLog;
            _coreAuditLog = coreAuditLog;
            _coreCompanyService = coreCompanyService;
            _coreCompanyServiceRole = coreCompanyServiceRole;
            _coreCompanyServiceUser = coreCompanyServiceUser;
            _coreCompanyUser = coreCompanyUser;
            _userData = userData;
            _guestData = guestData;
            _companySectionService = companySectionService;
            _httpHelper = httpHelper;
            _tokenHelper = tokenHelper;
            _options = options;
            _general = general;
            _kycRepository = kycRepository;
        }

        public async Task<IGeneralModel> List()
        {
            var company = await _companyData.List();
            var data = company.Select(c => new { CompanyId = c.Guid, c.CompanyName, c.Status });
            return _general.Response(new { companty = data }, 200, "Configurable companies has been listed", true);
        }

        public async Task<IGeneralModel> ListByService(int serviceId, string companyName, int page, int rowCount)
        {
            dynamic data = null;

            if (companyName is null)
            {
                data = await _companyData.ListByService(serviceId, page, rowCount, "");
            }
            else
            {
                data = await _companyData.ListByService(serviceId, companyName, page, rowCount);
            }

            return _general.Response(data, 200, "Company Has been listed!", true);
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

            string companyCode = "";
            bool isExists = true;
            int tries = 0;
            while (isExists)
            {
                companyCode = CompanyHelper.GenerateCompanyCode(obj.CompanyName, tries, 5);
                isExists = await _companyData.CheckCompanyCode(companyCode);
                tries++;
            }

            var company = new xgca.entity.Models.Company
            {
                ClientId = 1,
                CompanyName = obj.CompanyName,
                CompanyCode = companyCode,
                AddressId = addressId,
                ContactDetailId = contactDetailId,
                ImageURL = obj.ImageURL,
                WebsiteURL = obj.WebsiteURL,
                EmailAddress = obj.EmailAddress,
                TaxExemption = obj.TaxExemption,
                TaxExemptionStatus = obj.TaxExemptionStatus,
                CreatedBy = createdById,
                CreatedOn = DateTime.UtcNow,
                ModifiedBy = createdById,
                ModifiedOn = DateTime.UtcNow,
                Guid = Guid.NewGuid(),
                Status = 0,
                StatusName = "Inactive",
                KycStatusCode = Enum.GetName(typeof(Enums.KYCStatus), Enums.KYCStatus.NEW)
            };

            var companyId = await _companyData.CreateAndReturnId(company);
            if (companyId <= 0)
            { return _general.Response(null, 400, "Error on creating company", true); }
            await _coreCompanyService.CreateBatch(obj.Services, companyId, createdById);

            // Create audit log
            await _coreAuditLog.CreateAuditLog("Create", company.GetType().Name, companyId, createdById, obj, null);

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

            string companyCode = "";
            bool isExists = true;
            int tries = 0;
            while(isExists)
            {
                companyCode = CompanyHelper.GenerateCompanyCode(obj.CompanyName, tries, 5);
                isExists = await _companyData.CheckCompanyCode(companyCode);
                tries++;
            }

            var company = new entity.Models.Company
            {
                ClientId = 1,
                CompanyName = obj.CompanyName,
                CompanyCode = companyCode,
                AddressId = addressId,
                ContactDetailId = contactDetailId,
                ImageURL = obj.ImageURL,
                WebsiteURL = obj.WebsiteURL,
                EmailAddress = obj.EmailAddress,
                CreatedBy = GlobalVariables.SystemUserId,
                CreatedOn = DateTime.UtcNow,
                ModifiedBy = GlobalVariables.SystemUserId,
                ModifiedOn = DateTime.UtcNow,
                Guid = Guid.NewGuid(),
                Status = 0, // Default Inactive,
                StatusName = "Inactive",
                KycStatusCode = Enum.GetName(typeof(Enums.KYCStatus), Enums.KYCStatus.NEW)
            };

            var companyId = await _companyData.CreateAndReturnId(company);
            if (companyId <= 0)
            { return _general.Response(null, 400, "Error on creating company", true); }

            GlobalVariables.LoggedInCompanyId = companyId;
            await _companySectionService.CreateInitialSections();

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

            var kycReturn = await _kycRepository.GetByKycStatusCode(newCompany.KycStatusCode);

            // Create audit log
            await _coreAuditLog.CreateAuditLog("Create", company.GetType().Name, companyId, GlobalVariables.SystemUserId, companyLog, null);

            return companyId > 0
                ? _general.Response(new { CompanyId = companyId, MasterUserId = masterUser.MasterUserId, CompanyGuid = company.Guid }, 200, "Company registration successful", true)
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

            int contactDetailId = await _coreContactDetail.UpdateAndReturnId(obj, modifiedById);
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
                ModifiedOn = DateTime.UtcNow,
                Guid = Guid.Parse(obj.CompanyId),
                CUCC = obj.CUCC
            };

            var companyResult = await _companyData.Update(company);

            await _coreCompanyService.UpdateBatch(obj.CompanyServices, companyId, modifiedById);

            // Return updated company detail
            var companyServicesResponse = await _coreCompanyService.ListByCompanyId(obj.CompanyId);
            var companyServices = companyServicesResponse.data.companyService;
            var newCompany = await _companyData.Retrieve(companyId);

            var cityResponse = await _httpHelper.GetGuidById(_options.Value.BaseUrl, $"{_options.Value.GetCity}/", newCompany.Addresses.CityId, AuthToken.Contra);
            var cityJson = (JObject)cityResponse;
            var stateResponse = await _httpHelper.GetGuidById(_options.Value.BaseUrl, $"{_options.Value.GetState}/", newCompany.Addresses.StateId, AuthToken.Contra);
            var stateJson = (JObject)stateResponse;

            var kycReturn = await _kycRepository.GetByKycStatusCode(newCompany.KycStatusCode);
            var updatedCompany = CompanyHelper.ReturnUpdatedValue(newCompany, (cityJson)["data"]["cityId"].ToString(), (stateJson)["data"]["stateId"].ToString(), companyServices, kycReturn.Item1.Description);

            var newValue = CompanyHelper.BuildCompanyValue(newCompany, companyServices);

            // Create audit log
            await _coreAuditLog.CreateAuditLog("Update", company.GetType().Name, companyId, modifiedById, oldValue, newValue);

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

            var cityResponse = await _httpHelper.GetGuidById(_options.Value.BaseUrl, $"{_options.Value.GetCity}/", result.Addresses.CityId, AuthToken.Contra);
            var cityJson = (JObject)cityResponse;
            var stateResponse = await _httpHelper.GetGuidById(_options.Value.BaseUrl, $"{_options.Value.GetState}/", result.Addresses.StateId, AuthToken.Contra);
            var stateJson = (JObject)stateResponse;

            var companyServices = await _coreCompanyService.ListByCompanyId(companyKey);

            var kycReturn = await _kycRepository.GetByKycStatusCode(result.KycStatusCode);

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
                result.Addresses.AddressAdditionalInformation,
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
                result.CUCC,
                result.TaxExemption,
                result.TaxExemptionStatus,
                CompanyServices = companyServices.data.companyService,
                Status = (result.Status == 1) ? "Active" : "Inactive",
                KYCStatusCode = kycReturn.Item1?.KycStatusCode,
                KYCStatus = (kycReturn.Item1 is null) ? "NEW" : kycReturn.Item1.Description
            };

            return _general.Response(new { company = data }, 200, "Configurable information for selected company has been displayed", true);
        }

        public async Task<IGeneralModel> Retrieve(string companyId)
        {
            int companyKey = await _companyData.GetIdByGuid(Guid.Parse(companyId));
            if (!(companyKey > 0))
            { return _general.Response(null, 400, "Selected company might have been deleted or does not exists", false); }
            var result = await _companyData.Retrieve(companyKey);
            if (result == null)
            { return _general.Response(null, 400, "Selected company might have been deleted or does not exists", false); }

            var cityResponse = await _httpHelper.GetGuidById(_options.Value.BaseUrl, $"{_options.Value.GetCity}/", result.Addresses.CityId, AuthToken.Contra);
            var cityJson = (JObject)cityResponse;
            var stateResponse = await _httpHelper.GetGuidById(_options.Value.BaseUrl, $"{_options.Value.GetState}/", result.Addresses.StateId, AuthToken.Contra);
            var stateJson = (JObject)stateResponse;

            var companyServices = await _coreCompanyService.ListByCompanyId(companyId);

            var kycReturn = await _kycRepository.GetByKycStatusCode(result.KycStatusCode);

            var data = new
            {
                CompanyId = result.Guid,
                result.CompanyName,
                result.CompanyCode,
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
                result.Addresses.AddressAdditionalInformation,
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
                result.CUCC,
                result.TaxExemption,
                result.TaxExemptionStatus,
                CompanyServices = companyServices.data.companyService,
                Status = (result.Status == 1) ? "Active" : "Inactive",
                KYCStatusCode = kycReturn.Item1?.KycStatusCode,
                KYCStatus = (kycReturn.Item1 is null) ? "NEW" : kycReturn.Item1.Description
            };

            return _general.Response(new { company = data }, 200, "Configurable information for selected company has been displayed", true);
        }
        public async Task<IGeneralModel> Delete(string key, string username)
        {
            int companyId = await _companyData.GetIdByGuid(Guid.Parse(key));
            int deletedById = await _userData.GetIdByUsername(username);
            if (companyId == 0)
            { return _general.Response(false, 400, "Error on deleting company", true); }

            var result = await _companyData.Delete(companyId);

            // Create audit log
            await _coreAuditLog.CreateAuditLog("Delete", "Company", companyId, deletedById, null, null);

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

        public async Task<IGeneralModel> GetGuidById(int key)
        {
            string companyId = await _companyData.GetGuidById(key);
            if (companyId == "")
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
                CUCC = obj.CUCC,
                AddressId = addressId,
                ContactDetailId = contactDetailId,
                ImageURL = obj.ImageURL,
                WebsiteURL = obj.WebsiteURL,
                EmailAddress = obj.EmailAddress,
                TaxExemption = obj.TaxExemption,
                TaxExemptionStatus = obj.TaxExemptionStatus,
                ModifiedBy = GlobalVariables.SystemUserId,
                ModifiedOn = DateTime.UtcNow,
                Guid = Guid.Parse(obj.CompanyId),
            };

            var companyResult = await _companyData.Update(company);

            await _coreCompanyService.UpdateBatch(obj.CompanyServices, companyId, 0);

            // Return updated company detail
            var companyServicesResponse = await _coreCompanyService.ListByCompanyId(obj.CompanyId);
            var companyServices = companyServicesResponse.data.companyService;
            var newCompany = await _companyData.Retrieve(companyId);

            var cityResponse = await _httpHelper.GetIdByGuid(_options.Value.BaseUrl, $"{_options.Value.GetCity}/", newCompany.Addresses.CityId.ToString(), AuthToken.Contra);
            var cityJson = (JObject)cityResponse;
            var stateResponse = await _httpHelper.GetIdByGuid(_options.Value.BaseUrl, $"{_options.Value.GetState}/", newCompany.Addresses.StateId.ToString(), AuthToken.Contra);
            var stateJson = (JObject)stateResponse;

            var updatedCompany = CompanyHelper.ReturnUpdatedValue(newCompany, (cityJson)["data"]["cityId"].ToString(), (stateJson)["data"]["stateId"].ToString(), companyServices);

            var newValue = CompanyHelper.BuildCompanyValue(newCompany, companyServices);

            // Create audit log
            await _coreAuditLog.CreateAuditLog("Update", company.GetType().Name, companyId, GlobalVariables.SystemUserId, oldValue, newValue);

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
                    CreatedOn = auditLog.CreatedOn.ToString(GlobalVariables.AuditLogTimeFormat)
                });
            }

            return _general.Response(new { Logs = logs }, 200, "Company audit logs has been listed", true);
        }

        public async Task<IGeneralModel> ListCompanyLogs(string companyId)
        {
            int companyKey = await _companyData.GetIdByGuid(Guid.Parse(companyId));
            var data = await _auditLog.ListByTableNameAndKeyFieldId("Company", companyKey);

            List<ListAuditLogModel> logs = new List<ListAuditLogModel>();

            foreach (var d in data)
            {
                var user = await _userData.Retrieve(d.CreatedBy);

                logs.Add(new ListAuditLogModel
                {
                    AuditLogId = d.AuditLogId.ToString(),
                    AuditLogAction = d.AuditLogAction,
                    CreatedBy = (d.CreatedBy == 0) ? "System" : d.CreatedByName,
                    Username = d.CreatedBy != 0 ? (!(user.Username is null) ? user.Username : "Not Set") : "system",
                    //Username = !(user.Username is null) ? (auditLog.CreatedBy == 0 ? "system" : user.Username) : "Not Set",
                    CreatedOn = d.CreatedOn.ToString(GlobalVariables.AuditLogTimeFormat)
                });
            }

            return _general.Response(new { Logs = logs }, 200, "Company audit logs has been listed", true);
        }

        public async Task<IGeneralModel> GetReservationActors(List<GetReservationActorsModel> getReservationActorsModel)
        {
            List<dynamic> response = new List<dynamic>();

            var companies = await _companyData.GetAll();

            foreach (var item in getReservationActorsModel)
            {
                List<dynamic> notifyParties = new List<dynamic>();

                if (item.NotifyPartyIds != null)
                {
                    if (item.NotifyPartyIds.Contains(","))
                    {
                        var ids = Constant.SplitByComma(item.NotifyPartyIds);
                        foreach (string id in ids)
                        {
                            notifyParties.Add(companies.Where(c => c.Guid == Constant.CheckIfGuid(id)).FirstOrDefault());
                        }
                    }
                    else
                    {
                        notifyParties.Add(companies.Where(c => c.Guid == Constant.CheckIfGuid(item.NotifyPartyIds)).FirstOrDefault());
                    }

                }

                response.Add(new
                {
                    BookingParty = companies.Where(c => c.Guid == Constant.CheckIfGuid(item.BookingPartyId)).FirstOrDefault(),
                    Shipper = companies.Where(c => c.Guid == Constant.CheckIfGuid(item.ShipperId)).FirstOrDefault(),
                    Consignee = companies.Where(c => c.Guid == Constant.CheckIfGuid(item.ConsigneeId)).FirstOrDefault(),
                    ShippingLine = companies.Where(c => c.Guid == Constant.CheckIfGuid(item.ShippingLineId)).FirstOrDefault(),
                    NotifyParties = notifyParties
                });
            }
            return _general.Response(new { actors = response }, 200, "Retreive success", true);
        }

        public async Task<IGeneralModel> ListCompaniesByIDs(GetCompanyIDs obj)
        {
            var data = await _companyData.ListCompaniesByGuids(obj.CompanyIDs);

            var companies = data.Select(c => new
            {
                CompanyId = c.Guid,
                c.CompanyName,
                c.CompanyCode,
                c.ImageURL,
                c.Status
            });

            return _general.Response(new { Companies = companies }, 200, "Get Successful", true);
        }

        public async Task<IGeneralModel> ListCompanyDetailsByIds(GetCompanyIDs obj)
        {
            var data = await _companyData.ListCompaniesByGuids(obj.CompanyIDs);

            var companies = new List<GetCompanyInformation>();

            foreach (var d in data)
            {
                var cityResponse = await _httpHelper.GetGuidById(_options.Value.BaseUrl, $"{_options.Value.GetCity}/", d.Addresses.CityId, AuthToken.Contra);
                var cityJson = (JObject)cityResponse;
                var stateResponse = await _httpHelper.GetGuidById(_options.Value.BaseUrl, $"{_options.Value.GetState}/", d.Addresses.StateId, AuthToken.Contra);
                var stateJson = (JObject)stateResponse;

                companies.Add(new GetCompanyInformation
                {
                    CompanyId = d.Guid.ToString(),
                    CompanyName = d.CompanyName,
                    CompanyCode = d.CompanyCode,
                    ImageURL = d.ImageURL,
                    AddressId = d.Addresses.Guid.ToString(),
                    AddressLine = d.Addresses.AddressLine,
                    City = new City
                    {
                        CityId = d.Addresses.CityId,
                        CityGuid = (cityJson)["data"]["cityId"].ToString(),
                        CityName = d.Addresses.CityName,
                    },
                    State = new State
                    {
                        StateId = d.Addresses.StateId,
                        StateGuid = (stateJson)["data"]["stateId"].ToString(),
                        StateName = d.Addresses.StateName,
                    },
                    Country = new Country
                    {
                        CountryId = d.Addresses.CountryId,
                        CountryName = d.Addresses.CountryName,
                    },
                    ZipCode = d.Addresses.ZipCode,
                    FullAddress = d.Addresses.FullAddress,
                    Longitude = d.Addresses.Longitude,
                    Latitude = d.Addresses.Latitude,
                    AddressAdditionalInformation = d.Addresses.AddressAdditionalInformation,
                    WebsiteURL = d.WebsiteURL,
                    EmailAddress = d.EmailAddress,
                    ContactDetailId = d.ContactDetails.Guid.ToString(),
                    Phone = new PhoneNumber
                    {
                        PhonePrefixId = d.ContactDetails.PhonePrefixId,
                        PhonePrefix = d.ContactDetails.PhonePrefix,
                        Phone = d.ContactDetails.Phone,
                    },
                    Mobile = new MobileNumber
                    {
                        MobilePrefixId = d.ContactDetails.MobilePrefixId,
                        MobilePrefix = d.ContactDetails.MobilePrefix,
                        Mobile = d.ContactDetails.Mobile,
                    },
                    Fax = new FaxNumber
                    {
                        FaxPrefixId = d.ContactDetails.FaxPrefixId,
                        FaxPrefix = d.ContactDetails.FaxPrefix,
                        Fax = d.ContactDetails.Fax,
                    },
                    CUCC = d.CUCC,
                    Status = d.Status
                });
            }

            return _general.Response(new { Companies = companies }, 200, "Get Successful", true);
        }

        public async Task<IGeneralModel> BatchUpdateCompanyCode(string username)
        {
            int modifiedById = GlobalVariables.SystemUserId; //await _userData.GetIdByUsername(username);
            List<entity.Models.Company> woCompanyCode = await _companyData.GetCompanyWithNullCompanyCodes();

            foreach (var company in woCompanyCode)
            {
                string companyCode = "";
                bool isExists = true;
                int tries = 0;
                while (isExists)
                {
                    companyCode = CompanyHelper.GenerateCompanyCode(company.CompanyName, tries, 5);
                    isExists = await _companyData.CheckCompanyCode(companyCode);
                    tries++;
                }
                company.CompanyCode = companyCode;
                company.ModifiedBy = modifiedById;
                company.ModifiedOn = DateTime.UtcNow;

                var result = await _companyData.Update(company);
            }

            return _general.Response(null, 200, "Company code for existing companies has been generated and updated", true);
        }

        public async Task<IGeneralModel> UpdateAccreditedBy(UpdateAccreditedByModel obj, string username)
        {
            int modifiedBy = await _coreUser.GetIdByUsername(username);
            var result = await _companyData.SetAccreditedBy(obj.CompanyId, obj.AccreditedBy, modifiedBy);

            return result
                ? _general.Response(null, 200, "Accreditation Successful", true)
                : _general.Response(null, 400, "Error on accrediting process", true);
        }

        public async Task<IGeneralModel> ListByCompanyName(string companyName)
        {
            companyName = companyName.Replace("%20", " ");
            var companies = await _companyData.ListByCompanyName(companyName);
            return _general.Response(new { Companies = companies.Select(x => new { x.Guid, x.CompanyName }) }, 200, "Configurable companies has been listed", true);
        }

        public async Task<IGeneralModel> CheckIfExistsByCompanyName(string companyName)
        {
            companyName = companyName.Replace("%20", " ");
            bool isExist = await _companyData.CheckIfExistsByCompanyName(companyName);
            string message = (isExist) ? "Company exists" : "Company does not exists";
            return _general.Response(isExist, 200, message, true);
        }

        public async Task<IGeneralModel> CheckIfCompanyExistsByCompanyName(string companyName)
        {
            companyName = companyName.Replace("%20", " ");
            var companyInfo = await _companyData.CheckIfExistsCompanyByCompanyName(companyName);
            if(companyInfo == null) return _general.Response(null, StatusCodes.Status400BadRequest, "Company not found.", true);

            return _general.Response(companyInfo, StatusCodes.Status200OK, "Company found", true);
        }

        public async Task<IGeneralModel> BulkCheckIfExistsByCompanyName(string[] companyNames)
        {
            var result = await _companyData.BulkCheckIfExistsByCompanyName(companyNames);

            return (result.Count() > 0)
                ? _general.Response(new { companyNames = result }, 200, "Companies already Exists", false)
                : _general.Response(null, 200, "Companies doesn't Exists", true);
        }

        public async Task<IGeneralModel> BulkCompanyRegistration(InitialRegistrationListModel obj, string username)
        {
            if (obj == null || obj.companies.Count == 0)
            { return _general.Response(null, 400, "Data cannot be null", true); }

            List<RegistrationReturnModel> returnModel = new List<RegistrationReturnModel>();

            foreach (var o in obj.companies)
            {
                int createdById = await _userData.GetIdByUsername(username);

                int addressId = await _coreAddress.CreateAndReturnId(o, createdById);
                if (addressId <= 0)
                { return _general.Response(false, 400, "Error on creating company", true); }

                int contactDetailId = await _coreContactDetail.CreateAndReturnId(o, createdById);
                if (contactDetailId <= 0)
                { return _general.Response(false, 400, "Error on creating company", true); }

                string companyCode = "";
                bool isExists = true;
                int tries = 0;
                while (isExists)
                {
                    companyCode = CompanyHelper.GenerateCompanyCode(o.CompanyName, tries, 5);
                    isExists = await _companyData.CheckCompanyCode(companyCode);
                    tries++;
                }

                var company = new entity.Models.Company
                {
                    ClientId = 1,
                    CompanyName = o.CompanyName,
                    CompanyCode = companyCode,
                    AddressId = addressId,
                    ContactDetailId = contactDetailId,
                    ImageURL = o.ImageURL,
                    WebsiteURL = o.WebsiteURL,
                    EmailAddress = o.EmailAddress,
                    CreatedBy = createdById,
                    CreatedOn = DateTime.UtcNow,
                    ModifiedBy = createdById,
                    ModifiedOn = DateTime.UtcNow,
                    Guid = Guid.NewGuid(),
                    Status = 1,
                    StatusName = "Active",
                    AccreditedBy = o.AccreditedBy,
                    KycStatusCode = Enum.GetName(typeof(Enums.KYCStatus), Enums.KYCStatus.NEW)
                };

                var companyId = await _companyData.CreateAndReturnId(company);
                if (companyId <= 0)
                { return _general.Response(null, 400, "Error on creating company", true); }

                await _coreCompanyService.CreateBatch(o.Services, companyId, createdById);
                await _coreCompanyServiceRole.CreateDefault(companyId, createdById);

                var masterUserObj = o.MasterUser;
                dynamic masterUser = await _coreUser.CreateMasterUser(masterUserObj, createdById);
                int companyUserId = await _coreCompanyUser.CreateDefaultCompanyUser(companyId, masterUser.MasterUserId, createdById);
                await _coreCompanyServiceUser.CreateDefault(companyId, companyUserId, createdById);

                var newCompany = await _companyData.Retrieve(companyId);
                var newCompanyServicesResponse = await _coreCompanyService.ListByCompanyId(newCompany.Guid.ToString());
                var newCompanyServices = newCompanyServicesResponse.data.companyService;
                var companyLog = CompanyHelper.BuildCompanyValue(newCompany, newCompanyServices);

                // Create audit log
                await _coreAuditLog.CreateAuditLog("Create", company.GetType().Name, companyId, createdById, companyLog, null);

                returnModel.Add(new RegistrationReturnModel
                {
                    CompanyId = companyId,
                    CompanyGuid = company.Guid.ToString(),
                    MasterUserId = masterUser.MasterUserId,
                    MasterUserGuid = masterUser.MasterUserGuid.ToString(),
                    MasterUserEmail = masterUser.MasterUserEmail
                });
            }

            return returnModel.Count != 0
                ? _general.Response(new { Companies = returnModel }, 200, "Company bulk registration successful", true)
                : _general.Response(null, 400, "Error on company bulk registration", true);
        }

        public async Task<IGeneralModel> SetCUCC(UpdateCUCCCodeDTO obj)
        {
            var result = await _companyData.SetCUCCByCompanyGuid(obj.CompanyId, obj.CUCC);
            return result
                ? _general.Response(null, 200, "CUCC code updated", true)
                : _general.Response(null, 400, "Error in updating CUCC code", false);
        }

        public async Task<IGeneralModel> RetrieveCompanyName(GetCompanyNamesModel obj)
        {
            string payerName = null;
            string receiverName = null;

            int payerId = await _companyData.GetIdByGuid(Guid.Parse(obj.PayerId));
            if (payerId == 0)
            {
                var guestPayer = await _guestData.Retrieve(Guid.Parse(obj.PayerId));
                payerName = (guestPayer is null) ? "NotApplicable" : guestPayer.GuestName;
            }
            else
            {
                var payerResult = await _companyData.Retrieve(payerId);
                payerName = payerResult.CompanyName;
            }

            int receiverId = await _companyData.GetIdByGuid(Guid.Parse(obj.ReceiverId));
            if (receiverId == 0)
            {
                var guestReceiver = await _guestData.Retrieve(Guid.Parse(obj.ReceiverId));
                receiverName = (guestReceiver is null) ? "NotApplicable" : guestReceiver.GuestName;
            }
            else
            {
                var receiverResult = await _companyData.Retrieve(receiverId);
                receiverName = receiverResult.CompanyName;
            }

            var data = new
            {
                PayerName = (payerName is null) ? "-" : payerName,
                ReceiverName = (receiverName is null) ? "-" : receiverName
            };

            return _general.Response(new { company = data }, 200, "Company Name for selected companies has been displayed", true);
        }

        public async Task<IGeneralModel> GetAccreditor(int companyId)
        {
            var result = await _companyData.GetAccreditor(companyId);

            if (result is null)
            {
                return _general.Response(null, 400, "No service provider found", true);
            }

            var companyServices = await _coreCompanyService.ListByCompanyId(result.Guid.ToString());
            var services = companyServices.data.companyService;

            var accreditor = new 
            { 
                CompanyId = result.Guid.ToString(), 
                result.CompanyName,
                result.ImageURL,
                Services = services
            };

            return _general.Response(new { Accreditor = accreditor }, 200, "Service provider returned", true);
        }

        public async Task<IGeneralModel> GetCompanyCode(string companyGuid)
        {
            var code = await _companyData.GetCompanyCode(companyGuid);

            if (code == null)
            {
                code = "XLOG1";
            }

            return _general.Response(new { CompanyCode = code }, 200, "Company code retrieved", true);
        }

        public async Task<IGeneralModel> GetInvoiceActors(string billerId, string customerId)
        {
            var (biller, customer) = await _companyData.GetInvoiceActors(billerId, customerId);

            if (biller is null)
            {
                biller = await _guestData.GetGuestBiller(billerId);
            }

            if (customer is null)
            {
                customer = await _guestData.GetGuestCustomer(customerId);
            }

            return _general.Response(new { 
                Biller = (biller is null) ? new Biller { BillerName = "NotApplicable"} : biller,
                Customer = (customer is null) ? new Customer { CustomerName = "NotApplicable" } : customer  }, 
                200, "Invoice actors retrieved", true);
        }

        public async Task<IGeneralModel> GetCompanyAndGuestByIds(List<string> guids)
        {

            var companies = await _companyData.GetCompanyAndGuestByIds(guids);

            return _general.Response(new { actors = companies }, 200, "Actors retrieved", true);

        }

        public async Task<byte[]> DownloadCompanyProfileLogs(int companyId)
        {
            var logs = await _auditLog.ListByTableNameAndKeyFieldId("Company", companyId);

            var createdByIds = await _auditLog.GetCreatedByIds("Company", companyId);
            var users = await _userData.GetUsernamesByIds(createdByIds);
            


            var table = new DataTable { TableName = "AuditLogs" };
            table.Columns.Add("Date/Time", typeof(string));
            table.Columns.Add("Actions", typeof(string));
            table.Columns.Add("Updated By", typeof(string));
            table.Columns.Add("Username", typeof(string));

            for (int i = 0; i < logs.Count; i++)
            {
                var user = users.SingleOrDefault(x => x.UserId == logs[i]?.CreatedBy);

                string username = (user is null) ? "system" : user.Username;

                table.Rows.Add(
                    logs[i]?.CreatedOn,
                    logs[i]?.AuditLogAction,
                    logs[i]?.CreatedByName,
                    username
                );
            }

            var wb = new XLWorkbook();
            wb.Worksheets.Add(table);
            await using var memoryStream = new MemoryStream();
            wb.SaveAs(memoryStream);
            return memoryStream.ToArray();
        }

        public async Task<IGeneralModel> GetAccreditorShippingLine(string companyId)
        {
            if (companyId is null)
            {
                return _general.Response(null, 400, "Invalid company id", false);
            }

            if (Guid.Parse(companyId) == Guid.Empty)
            {
                return _general.Response(null, 400, "Invalid company id", false);
            }

            var (company, message) = await _companyData.GetAccreditorByCompnyGuid(companyId);

            if (company is null)
            {
                return _general.Response(null, 400, message, false);
            }

            var accreditor = new
            {
                CompanyId = company.Guid.ToString(),
                company.CompanyName,
                company.ImageURL
            };

            return _general.Response(new { Accreditor = accreditor }, 200, "Service provider returned", true);
        }

        public async Task<IGeneralModel> GetCompanyCode()
        {
            string companyGuid = await _companyData.GetGuidById(GlobalVariables.LoggedInCompanyId);
            var code = await _companyData.GetCompanyCode(companyGuid);

            if (code == null)
            {
                code = "XLOG1";
            }

            return _general.Response(new { CompanyCode = code }, 200, "Company code retrieved", true);
        }

        public async Task<IGeneralModel> GetByCompanyCode(string code)
        {
            var (result, message) = await _companyData.GetByCompanyCode(code);

            if (result is null)
            { return _general.Response(null, 400, "Selected company might have been deleted or does not exists", false); }

            var cityResponse = await _httpHelper.GetGuidById(_options.Value.BaseUrl, $"{_options.Value.GetCity}/", result.Addresses.CityId, AuthToken.Contra);
            var cityJson = (JObject)cityResponse;
            var stateResponse = await _httpHelper.GetGuidById(_options.Value.BaseUrl, $"{_options.Value.GetState}/", result.Addresses.StateId, AuthToken.Contra);
            var stateJson = (JObject)stateResponse;

            var companyServices = await _coreCompanyService.ListByCompanyId(result.Guid.ToString());

            var kycReturn = await _kycRepository.GetByKycStatusCode(result.KycStatusCode);

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
                result.Addresses.AddressAdditionalInformation,
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
                result.CUCC,
                result.TaxExemption,
                result.TaxExemptionStatus,
                CompanyServices = companyServices.data.companyService,
                Status = (result.Status == 1) ? "Active" : "Inactive",
                KYCStatusCode = kycReturn.Item1.KycStatusCode,
                KYCStatus = (kycReturn.Item1 is null) ? "NEW" : kycReturn.Item1.Description
            };

            return _general.Response(new { company = data }, 200, "Configurable information for selected company has been displayed", true);
        }

        //public async Task<IGeneralModel> GetCompanyAndGuestByIds(List<string> guids)
        //{

        //    var companies = await _companyData.GetCompanyAndGuestByIds(guids);

        //    return _general.Response(new { actors = companies }, 200, "Actors retrieved", true);

        //}
    }
}
    