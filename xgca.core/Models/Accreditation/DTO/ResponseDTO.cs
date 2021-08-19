using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using System.Xml.Linq;



namespace xas.core.Request
{
    public class ResponseDTO
    {
        public Guid requestId { get; set; }
        public string companyName { get; set; }
        public string fullAddress { get; set; }
        public string imageURL { get; set; }
        public CompanyDetails companyDetails { get; set; }
        public List<PortAreaResponse> portAreas { get; set; }
        public string status { get; set; }
    }

    public class TruckingResponseDTO
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

    public class PortAreaResponse 
    {
        public Guid PortAreaId { get; set; }
        public Guid PortId { get; set; }
        public int CountryAreaId { get; set; }
        public string CountryCode { get; set; }
        public string LoCode { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string PortOfLoading { get; set; }
        public string PortOfDischarge { get; set; }
        public string CountryName { get; set; }
        public string CityName { get; set; }
        public int IsDeleted { get; set; }
    }

    public class TruckArea
    {
        public Guid TruckAreaId { get; set; }
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
        public int IsDeleted { get; set; }
    }

    public class CompanyDetails
    {
        public string companyName { get; set; }
        public string imageURL { get; set; }
        public string addressId { get; set; }
        public string addressLine { get; set; }
        public City city { get; set; }
        public State state { get; set; }
        public Country country { get; set; }
        public string zipCode { get; set; }
        public string fullAddress { get; set; }
        public object longitude { get; set; }
        public object latitude { get; set; }
        public string websiteURL { get; set; }
        public object emailAddress { get; set; }
        public string contactDetailId { get; set; }
        public Phone phone { get; set; }
        public Mobile mobile { get; set; }
        public Fax fax { get; set; }
    }

    public class CSVCompanyDetails
    {
        public string companyName { get; set; }
        public string addressLine { get; set; }
        public City city { get; set; }
        public State state { get; set; }
        public Country country { get; set; }
        public string zipCode { get; set; }
        public string fullAddress { get; set; }
        public object longitude { get; set; }
        public object latitude { get; set; }
        public string websiteURL { get; set; }
        public object emailAddress { get; set; }
        public Phone phone { get; set; }
        public Mobile mobile { get; set; }
        public Fax fax { get; set; }
    }

    public class City
    {
        public string cityName { get; set; }
    }

    public class State
    {
        public string stateName { get; set; }
    }

    public class Country
    {
        public string countryName { get; set; }
    }

    public class Phone
    {
        public string phonePrefix { get; set; }
        public object phone { get; set; }
    }

    public class Mobile
    {
        public string mobilePrefix { get; set; }
        public string mobile { get; set; }
    }

    public class Fax
    {
        public string faxPrefix { get; set; }
        public string fax { get; set; }
    }

}
