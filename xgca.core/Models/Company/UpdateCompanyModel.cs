using System.Collections.Generic;
using xgca.core.Models.CompanyService;

namespace xgca.core.Models.Company
{
    public class UpdateCompanyModel
    {
        public string CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string AddressId { get; set; }
        public string AddressLine { get; set; }
        public string CityId { get; set; }
        public string CityName { get; set; }
        public string StateId { get; set; }
        public string StateName { get; set; }
        public string ZipCode { get; set; }
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string ImageURL { get; set; }
        public string ContactDetailId { get; set; }
        public int PhonePrefixId { get; set; }
        public string PhonePrefix { get; set; }
        public string Phone { get; set; }
        public int MobilePrefixId { get; set; }
        public string MobilePrefix { get; set; }
        public string Mobile { get; set; }
        public int FaxPrefixId { get; set; }
        public string FaxPrefix { get; set; }
        public string Fax { get; set; }
        public string WebsiteURL { get; set; }
        public string EmailAddress { get; set; }
        public byte TaxExemption { get; set; }
        public byte TaxExemptionStatus { get; set; }
        public List<dynamic> CompanyServices { get; set; }

        public string ModifiedBy { get; set; }
    }
}
