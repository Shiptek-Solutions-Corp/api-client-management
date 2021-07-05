using System;
using System.Collections.Generic;
using System.Text;
using xgca.core.Models.Address;
using xgca.core.Models.CompanyService;
using xgca.core.Models.CompanyTaxSettings;
using xgca.core.Models.ContactDetail;

namespace xgca.core.Models.Company
{
    public class CreateCompanyViewModel
    {
        public string Guid { get; set; }
        //public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public string ImageURL { get; set; }
        public string EmailAddress { get; set; }
        public string WebsiteURL { get; set; }
        public string StatusName { get; set; }
        public string Status { get; set; }
        //public byte TaxExemption { get; set; }
        //public byte TaxExemptionStatus { get; set; }
        //public string CUCC { get; set; }
        //public string AccreditedBy { get; set; }
        //public string KycStatusCode { get; set; }
        public GetAddressModel Addresses { get; set; }
        public GetContactDetailsModel ContactDetails { get; set; }
        public ICollection<GetCompanyTaxSettingsModel> CompanyTaxSettings { get; set; }
    }
    public class GetCompanyListingViewModel
    {
        public string CompanyName { get; set; }
        public string CountryName { get; set; }
        public string ServiceName { get; set; }
        public string CreatedOn { get; set; }
        public string Status { get; set; }
        public string KycStatusCode { get; set; }
        public string Guid { get; set; }
        public string StateName { get; set; }
        public string PricingSettingsDescription { get; set; }
        public string ImageURL { get; set; }
    }
    public class GetCompanyViewModel : UpdateCompanyViewModel
    {
        public ICollection<GetCompanyServiceModel> CompanyServices { get; set; }
    }

    public class UpdateCompanyViewModel : CreateCompanyViewModel
    {
        public int CompanyId { get; set; }
    }
}
