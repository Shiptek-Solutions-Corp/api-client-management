using System;
using System.Collections.Generic;
using System.Text;

namespace xas.core.Request.DTO
{

    public class CompanyDTO
    {
        public string companyId { get; set; }
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

    public class City
    {
        public int cityId { get; set; }
        public string cityName { get; set; }
    }

    public class State
    {
        public int stateId { get; set; }
        public string stateName { get; set; }
    }

    public class Country
    {
        public int countryId { get; set; }
        public string countryName { get; set; }
    }

    public class Phone
    {
        public int phonePrefixId { get; set; }
        public string phonePrefix { get; set; }
        public object phone { get; set; }
    }

    public class Mobile
    {
        public int mobilePrefixId { get; set; }
        public string mobilePrefix { get; set; }
        public string mobile { get; set; }
    }

    public class Fax
    {
        public int faxPrefixId { get; set; }
        public string faxPrefix { get; set; }
        public string fax { get; set; }
    }
}
