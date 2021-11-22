using System;
using System.Collections.Generic;
using System.Text;

namespace xas.core.CustomerAccreditation.DTO
{
    public class CusAccCSVDTO
    {
        public string companyName { get; set; }
        public string countryName { get; set; }
        public string cityName { get; set; }
        public string stateName { get; set; }
        public string fullAddress { get; set; }
        public string addressLine { get; set; }
        public string zipCode { get; set; }
        public object longitude { get; set; }
        public object latitude { get; set; }
        public string websiteURL { get; set; }
        public object emailAddress { get; set; }
        public string phonePrefix { get; set; }
        public object phone { get; set; }
        public string mobilePrefix { get; set; }
        public string mobile { get; set; }
        public string faxPrefix { get; set; }
        public string fax { get; set; }
        public string status { get; set; }
    }
}
