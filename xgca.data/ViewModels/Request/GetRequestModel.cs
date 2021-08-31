using System;
using System.Collections.Generic;
using System.Text;

namespace xgca.data.ViewModels.Request
{
    public class GetRequestModel
    {
        public int AccreditationStatusConfigId { get; set; }
        public string AccreditationStatusConfigDescription { get; set; }
        public int RequestId { get; set; }
        public Guid RequestGuid { get; set; }
        public Guid ServiceRoleIdFrom { get; set; }
        public Guid CompanyIdFrom { get; set; }
        public Guid ServiceRoleIdTo { get; set; }
        public Guid CompanyIdTo { get; set; }
        public bool? IsActive { get; set; }
        public string PortAreaList { get; set; }
        public List<string> PortAreaOperatingCountries { get; set; }
        public string TruckAreaList { get; set; }
        public string CompanyLogo { get; set; }
        public Guid CompanyGuid { get; set; }
        public string CompanyName { get; set; }
        public string CompanyFullAddress { get; set; }
        public string CompanyCountryName { get; set; }
        public string CompanyStateCityName { get; set; }
        public string CompanyStateName { get; set; }
        public string CompanyCityName { get; set; }
        public string CompanyPhonePrefix { get; set; }
        public string CompanyPhoneNumber { get; set; }
        public string CompanyMobilePrefix { get; set; }
        public string CompanyMobileNumber { get; set; }
        public string CompanyFaxPrefix { get; set; }
        public string CompanyFaxNumber { get; set; }
        public string CompanyCUCC { get; set; }
        public string CompanyPostalCode { get; set; }
        public string CompanyWebsiteURL { get; set; }

        //public List<GetPortAreaModel> PortAreaDataList { get; set; }
        //public List<GetTruckAreaModel> TruckAreaDataList { get; set; }
        //public CompanyInfo CompanyInfo { get; set; }
    }


    public class GetPortAreaModel
    {
        public int RequestId { get; set; }
        public int PortAreaId { get; set; }
        public Guid PortAreaGuid { get; set; }
        public int CountryAreaId { get; set; }
        public Guid PortId { get; set; }
        public string PortName { get; set; }
        public int PortOfLoading { get; set; }
        public int PortOfDischarge { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public Guid? StateCode { get; set; }
        public string StateName { get; set; }
        public Guid? CityCode { get; set; }
        public string CityName { get; set; }
        public string Locode { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string Location { get; set; }
        public bool IsDeleted { get; set; }

    }

    public class GetTruckAreaModel
    {
        public int RequestId { get; set; }
        public int TruckAreaId { get; set; }
        public Guid TruckAreaGuid { get; set; }
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public string StateId { get; set; }
        public string StateName { get; set; }
        public string CityId { get; set; }
        public string CityName { get; set; }
        public string PostalId { get; set; }
        public string PostalCode { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public bool IsDeleted { get; set; }
    }


    public class CompanyInfo
    {
        public int CompanyId { get; set; }
        public int ClientId { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public string ImageURL { get; set; }
        public string EmailAddress { get; set; }
        public string WebsiteURL { get; set; }
        public byte Status { get; set; }
        public string StatusName { get; set; }
        public byte TaxExemption { get; set; }
        public byte TaxExemptionStatus { get; set; }
        public string CUCC { get; set; }
        public Guid CompanyGuid { get; set; }
        public string AccreditedBy { get; set; }
        public string KycStatusCode { get; set; }
        public string PricingSettingsDescription { get; set; }
        public CompanyAddress Addresses { get; set; }
        public CompanyContact ContactDetails { get; set; }
    }

    public class CompanyAddress
    {
        public int AddressId { get; set; }
        public int AddressTypeId { get; set; }
        public string AddressLine { get; set; }
        public int CityId { get; set; }
        public string CityName { get; set; }
        public int StateId { get; set; }
        public string StateName { get; set; }
        public string ZipCode { get; set; }
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public string FullAddress { get; set; }
        public byte IsDeleted { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public Guid Guid { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
    }

    public class CompanyContact
    {
        public int ContactDetailId { get; set; }
        public int PhonePrefixId { get; set; }
        public string PhonePrefix { get; set; }
        public string Phone { get; set; }
        public int MobilePrefixId { get; set; }
        public string MobilePrefix { get; set; }
        public string Mobile { get; set; }
        public int FaxPrefixId { get; set; }
        public string FaxPrefix { get; set; }
        public string Fax { get; set; }
        public byte IsDeleted { get; set; }
        public Guid ContactDetailGuid  { get; set; }
    }

}
