using System;
using System.Collections.Generic;
using System.Text;

namespace xas.core.CompanyDetail.DTO
{
    public class CreateCompanyDetail
    {
        public string CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string FullAddress { get; set; }
        public string EmailAddress { get; set; }
        public string Fax { get; set; }
        public string FaxPrefix { get; set; }
        public int FaxPrefixId { get; set; }
        public string Mobile { get; set; }
        public string MobilePrefix { get; set; }
        public int? MobilePrefixId { get; set; }
        public string Phone { get; set; }
        public string PhonePrefix { get; set; }
        public int? PhonePrefixId { get; set; }
        public string WebsiteUrl { get; set; }
        public string ImageUrl { get; set; }
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public Guid? CityId { get; set; }
        public string CityName { get; set; }
        public Guid? StateId { get; set; }
        public string ZipCode { get; set; }
        public string StateName { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }
}
