using System;
using System.Collections.Generic;
using System.Text;

namespace xgca.core.Models.Company
{
    public class City
    {
        public string CityGuid { get; set; }
        public int CityId { get; set; }
        public string CityName { get; set; }
    }
    public class State
    {
        public string StateGuid { get; set; }
        public int StateId { get; set; }
        public string StateName { get; set; }
    }
    public class Country
    {
        public int CountryId { get; set; }
        public string CountryName { get; set; }
    }
    public class PhoneNumber
    {
        public int PhonePrefixId { get; set; }
        public string PhonePrefix { get; set; }
        public string Phone { get; set; }
    }
    public class MobileNumber
    {
        public int MobilePrefixId { get; set; }
        public string MobilePrefix { get; set; }
        public string Mobile { get; set; }
    }
    public class FaxNumber
    {
        public int FaxPrefixId { get; set; }
        public string FaxPrefix { get; set; }
        public string Fax { get; set; }
    }
    public class GetCompanyInformation
    {
        public string CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string CompanyCode { get; set; }
        public string ImageURL { get; set; }
        public string AddressId { get; set; }
        public string AddressLine { get; set; }
        public City City { get; set; }
        public State State { get; set; }
        public Country Country { get; set; }
        public string ZipCode { get; set; }
        public string FullAddress { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string AddressAdditionalInformation { get; set; }
        public string WebsiteURL { get; set; }
        public string EmailAddress { get; set; }
        public string ContactDetailId { get; set; }
        public PhoneNumber Phone { get; set; }
        public MobileNumber Mobile { get; set; }
        public FaxNumber Fax { get; set; }
        public string CUCC { get; set; }
        public int Status { get; set; }
    }
}
