using System;
using System.Collections.Generic;
using System.Text;

namespace xas.core.CustomerAccreditation.DTO
{
    public class CusAccTemplateDTO
    {
        public string CompanyName { get; set; }
        public string AddressLine { get; set; }
        public string CityID { get; set; }
        public string CityName { get; set; }
        public string StateID { get; set; }
        public string StateName { get; set; }
        public string ZipCode { get; set; }
        public string CountryID { get; set; }
        public string CountryName { get; set; }
        public string ImageURL { get; set; }
        public string WebsiteURL { get; set; }
        public string EmailAddress { get; set; }
        public string Phone { get; set; }
        public string PhonePrefixID { get; set; }
        public string PhonePrefix { get; set; }
        public string Mobile { get; set; }
        public string MobilePrefixID { get; set; }
        public string MobilePrefix { get; set; }
        public string Fax { get; set; }
        public string FaxPrefixID { get; set; }
        public string FaxPrefix { get; set; }
        public MasterUserDTO masteruser { get; set; }
    }
}
