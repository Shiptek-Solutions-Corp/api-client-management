using System;
using System.Collections.Generic;
using System.Text;

namespace xgca.data.ViewModels.Request
{
    public class TruckingResponseDataModel
    {
        public string RequestId { get; set; }
        public string CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string FullAddress { get; set; }
        public string EmailAddress { get; set; }
        public string Fax { get; set; }
        public string FaxPrefix { get; set; }
        public string FaxPrefixId { get; set; }
        public string Mobile { get; set; }
        public string MobilePrefix { get; set; }
        public string MobilePrefixId { get; set; }
        public string Phone { get; set; }
        public string PhonePrefix { get; set; }
        public string PhonePrefixId { get; set; }
        public string WebsiteUrl { get; set; }
        public string ImageUrl { get; set; }
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string ServiceRoleId { get; set; }
        public string ServiceRoleName { get; set; }
        public string TruckArea { get; set; }
        public string Status { get; set; }
    }
}
