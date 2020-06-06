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
//using xgca.data.Service;
using xgca.data.AuditLog;
using xgca.core.Helpers;
using xgca.core.Helpers.Http;
using xgca.core.Constants;

namespace xgca.core.Company
{
    public class Company : ICompany
    {
        private readonly ICompanyData _companyData;
        //private readonly IServiceData _serviceData;
        private readonly xgca.core.Address.IAddress _coreAddress;
        private readonly xgca.core.ContactDetail.IContactDetail _coreContactDetail;
        private readonly xgca.core.User.IUser _coreUser;
        private readonly xgca.data.AuditLog.IAuditLog _auditLog;        
        private readonly xgca.core.CompanyService.ICompanyService _coreCompanyService;
        private readonly xgca.core.CompanyServiceRole.ICompanyServiceRole _coreCompanyServiceRole;
        private readonly xgca.core.CompanyServiceUser.ICompanyServiceUser _coreCompanyServiceUser;
        private readonly xgca.core.CompanyUser.ICompanyUser _coreCompanyUser;

        private readonly IHttpHelpers _httpHelpers;
        private readonly IOptions<GlobalCmsApi> _options;
        private readonly IGeneral _general;

        public Company(ICompanyData companyData,
            //IServiceData serviceData,
            xgca.core.Address.IAddress coreAddress,
            xgca.core.ContactDetail.IContactDetail coreContactDetail,
            xgca.core.User.IUser coreUser,
            xgca.data.AuditLog.IAuditLog auditLog,
            xgca.core.CompanyService.ICompanyService coreCompanyService,
            xgca.core.CompanyServiceRole.ICompanyServiceRole coreCompanyServiceRole,
            xgca.core.CompanyServiceUser.ICompanyServiceUser coreCompanyServiceUser,
            xgca.core.CompanyUser.ICompanyUser coreCompanyUser,
            IHttpHelpers httpHelpers,
            IOptions<GlobalCmsApi> options,
            IGeneral general)
        {
            _companyData = companyData;
            //_serviceData = serviceData;
            _coreAddress = coreAddress;
            _coreContactDetail = coreContactDetail;
            _coreUser = coreUser;
            _auditLog = auditLog;
            _coreCompanyService = coreCompanyService;
            _coreCompanyServiceRole = coreCompanyServiceRole;
            _coreCompanyServiceUser = coreCompanyServiceUser;
            _coreCompanyUser = coreCompanyUser;
            _httpHelpers = httpHelpers;
            _options = options;
            _general = general;
        }
        
        public async Task<IGeneralModel> List()
        {
            var company = await _companyData.List();
            var data = company.Select(c => new { CompanyId = c.Guid, c.CompanyName, c.Status });
            return _general.Response(new { companty = data }, 200, "Configurable companies has been listed", true);
        }

        public async Task<IGeneralModel> Create(CreateCompanyModel obj)
        {
            if (obj == null)
            { return _general.Response(null, 400, "Data cannot be null", true); }

            int userId = await _coreUser.GetIdByGuid(Guid.Parse(obj.CreatedBy));

            int addressId = await _coreAddress.CreateAndReturnId(obj);
            if (addressId <= 0)
            { return _general.Response(false, 400, "Error on creating company", true); }

            int contactDetailId = await _coreContactDetail.CreateAndReturnId(obj);
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
                CreatedBy = userId,
                CreatedOn = DateTime.Now,
                ModifiedBy = userId,
                ModifiedOn = DateTime.Now,
                Guid = Guid.NewGuid()
            };

            var companyId = await _companyData.CreateAndReturnId(company);
            if (companyId <= 0)
            { return _general.Response(null, 400, "Error on creating company", true); }
            await _coreCompanyService.CreateBatch(obj.Services, companyId, userId);
            var auditLog = AuditLogHelper.BuilCreateLog(obj, "Create", company.GetType().Name, companyId);
            await _auditLog.Create(auditLog);
            return companyId > 0
                ? _general.Response(new { companyId = companyId }, 200, "Company created", true)
                : _general.Response(null, 400, "Error on creating company", true);
        }
        public async Task<IGeneralModel> InitialRegistration(InitialRegistrationModel obj)
        {
            if (obj == null)
            { return _general.Response(null, 400, "Data cannot be null", true); }

            int addressId = await _coreAddress.CreateAndReturnId(obj);
            if (addressId <= 0)
            { return _general.Response(false, 400, "Error on creating company", true); }

            int contactDetailId = await _coreContactDetail.CreateAndReturnId(obj);
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
                CreatedBy = 0,
                CreatedOn = DateTime.Now,
                ModifiedBy = 0,
                ModifiedOn = DateTime.Now,
                Guid = Guid.NewGuid()
            };

            var companyId = await _companyData.CreateAndReturnId(company);
            if (companyId <= 0)
            { return _general.Response(null, 400, "Error on creating company", true); }
            await _coreCompanyService.CreateBatch(obj.Services, companyId, 0);
            await _coreCompanyServiceRole.CreateDefault(companyId, 0);

            var masterUserObj = obj.MasterUser;
            dynamic masterUser = await _coreUser.CreateMasterUser(masterUserObj, 0);
            int companyUserId = await _coreCompanyUser.CreateDefaultCompanyUser(companyId, masterUser.MasterUserId, 0);
            await _coreCompanyServiceUser.CreateDefault(companyId, companyUserId, 0);
            var auditLog = AuditLogHelper.BuilCreateLog(obj, "Create", company.GetType().Name, companyId);
            await _auditLog.Create(auditLog);
            return companyId > 0
                ? _general.Response(new { masterUserId = masterUser.MasterUserGuid }, 200, "Company registration successful", true)
                : _general.Response(false, 400, "Error on registration", true);
        }

        public async Task<IGeneralModel> Update(UpdateCompanyModel obj)
        {
            if (obj == null)
            { return _general.Response(null, 400, "Data cannot be null", true); }

            int userId = await _coreUser.GetIdByGuid(Guid.Parse(obj.ModifiedBy));

            int oldCompanyId = await _companyData.GetIdByGuid(Guid.Parse(obj.CompanyId));
            var oldCompany = await _companyData.Retrieve(oldCompanyId);
            var oldValue = CompanyHelper.BuildCompanyValue(oldCompany);

            
            int addressId = await _coreAddress.UpdateAndReturnId(obj);
            if (addressId <= 0)
            { return _general.Response(false, 400, "Error on updating company", true); }
           
            int contactDetailId  = await _coreContactDetail.UpdateAndReturnId(obj);
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
                ModifiedBy = userId,
                ModifiedOn = DateTime.Now,
                Guid = Guid.Parse(obj.CompanyId),
            };

            var companyResult = await _companyData.Update(company);
            var newValue = CompanyHelper.BuildCompanyValue(company);
            await _coreCompanyService.UpdateBatch(obj.Services, companyId, userId);
            var auditLog = AuditLogHelper.BuildUpdateLog(oldValue, newValue, "Update", company.GetType().Name, company.CompanyId);
            await _auditLog.Create(auditLog);
            return companyResult
                ? _general.Response(true, 200, "Company updated", true)
                : _general.Response(false, 400, "Error on updating company", true);
        }
        public async Task<IGeneralModel> Retrieve(string key)
        {
            int companyId = await _companyData.GetIdByGuid(Guid.Parse(key));
            if (companyId == 0)
            { return _general.Response(null, 400, "Selected company might have been deleted or does not exists", false); }
            var result = await _companyData.Retrieve(companyId);
            if (result == null)
            { return _general.Response(null, 400, "Selected company might have been deleted or does not exists", false); }

            var data = new
            {
                CompanyId = result.Guid,
                result.ClientId,
                result.CompanyName,
                result.ImageURL,
                AddressId = result.Addresses.Guid,
                result.Addresses.AddressLine,
                result.Addresses.CityName,
                result.Addresses.StateName,
                result.Addresses.ZipCode,
                result.Addresses.CountryId,
                result.Addresses.CountryName,
                result.Addresses.FullAddress,
                result.Addresses.Longitude,
                result.Addresses.Latitude,
                result.WebsiteURL,
                result.EmailAddress,
                ContactDetailId = result.ContactDetails.Guid,
                result.ContactDetails.PhonePrefixId,
                result.ContactDetails.PhonePrefix,
                result.ContactDetails.Phone,
                result.ContactDetails.MobilePrefixId,
                result.ContactDetails.MobilePrefix,
                result.ContactDetails.Mobile,
                result.ContactDetails.FaxPrefixId,
                result.ContactDetails.FaxPrefix,
                result.ContactDetails.Fax,
                //Services = result.CompanyServices.Select(s => new { ServiceId = s.Services.Guid, s.Services.ServiceName })
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
    }
}
