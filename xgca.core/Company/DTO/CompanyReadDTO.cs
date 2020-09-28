using DocumentFormat.OpenXml.Office2013.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace xgca.core.Company.DTO
{
    public class CompanyReadDTO
    {
        public string CompanyName { get; set; }
        public string CompanyAddressLine { get; set; }
        public string CompanyCityId { get; set; }
        public string CompanyCityName { get; set; }
        public string CompanyStateId { get; set; }
        public string CompanyStateName { get; set; }
        public string CompanyZipCode { get; set; }
        public string CompanyCountryId { get; set; }
        public string CompanyCountryName { get; set; }
        public string CompanyImageUrl { get; set; }
        public string CompanyWebsiteUrl { get; set; }
        public string CompanyEmailAddress { get; set; }
        public string CompanyPhoneNumber { get; set; }
        public string CompanyPhonePrefixId { get; set; }
        public string CompanyPhonePrefix { get; set; }
        public string CompanyMobileNumber { get; set; }
        public string CompanyMobilePrefixId { get; set; }
        public string CompanyMobilePrefix { get; set; }
        public string CompanyFaxNumber { get; set; }
        public string CompanyFaxPrefixId { get; set; }
        public string CompanyFaxPrefix { get; set; }
        public string CompanyServices { get; set; }
        public string MasterUserFirstName { get; set; }
        public string MasterUserLastName { get; set; }
        public string MasterUserTitle { get; set; }
        public string MasterUserImageUrl { get; set; }
        public string MasterUserPhoneNumber { get; set; }
        public string MasterUserPhonePrefixId { get; set; }
        public string MasterUserPhonePrefix { get; set; }
        public string MasterUserMobileNumber { get; set; }
        public string MasterUserMobilePrefixId { get; set; }
        public string MasterUserMobilePrefix { get; set; }
        public string MasterUserEmailAddress { get; set; }
        public string CompanyServiceRoleId { get; set; }
    }
}
