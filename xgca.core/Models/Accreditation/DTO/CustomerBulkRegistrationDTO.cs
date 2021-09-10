using System;
using System.Collections.Generic;
using System.Text;

namespace xas.core.CustomerAccreditation.DTO
{
    public class CustomerBulkRegistrationDTO
    {
        public string EmailAddress { get; set; }
        public string CompanyName { get; set; }
        public string AddressLine { get; set; }
        public string CityName { get; set; }
        public string CityId { get; set; }
        public string StateName { get; set; }
        public string StateId { get; set; }
        public string ZipCode { get; set; }
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public string ImageUrl { get; set; }
        public string WebsiteUrl { get; set; }
        public string Phone { get; set; }
        public int PhonePrefixId { get; set; }
        public string PhonePrefix { get; set; }
        public int MobilePrefixId { get; set; }
        public string MobilePrefix { get; set; }
        public string Mobile { get; set; }
        public int FaxPrefixId { get; set; }
        public string FaxPrefix { get; set; }
        public string Fax { get; set; }
        public string[] Services { get; set; }
        public MasterUserDTO MasterUser { get; set; }
        public string AccreditedBy { get; set; }
    }
    public class MasterUserDTO {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public string Phone { get; set; }
        public int PhonePrefixId { get; set; }
        public string PhonePrefix { get; set; }
        public string Mobile { get; set; }
        public int MobilePrefixId { get; set; }
        public string MobilePrefix { get; set; }
        public string EmailAddress { get; set; }
    }
}
