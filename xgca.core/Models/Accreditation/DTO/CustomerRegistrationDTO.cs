using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace xas.core.CustomerAccreditation.DTO
{
    public class CustomerRegistrationDTO
    {
        [Required]
        [EmailAddress]
        public string emailAddress { get; set; }
        [Required]
        public string companyName { get; set; }
        [Required]
        public string addressLine { get; set; }
        [Required]
        public string cityName { get; set; }
        [Required]
        public string cityId { get; set; }
        [Required]
        public string stateName { get; set; }
        [Required]
        public string stateId { get; set; }
        [Required]
        public string zipCode { get; set; }
        [Required]
        public int countryId { get; set; }
        [Required]
        public string countryName { get; set; }
        public string imageUrl { get; set; }
        public string websiteUrl { get; set; }
        public string phone { get; set; }
        public int phonePrefixId { get; set; }
        public string phonePrefix { get; set; }
        public int mobilePrefixId { get; set; }
        public string mobilePrefix { get; set; }
        public string mobile { get; set; }
        public int faxPrefixId { get; set; }
        public string faxPrefix { get; set; }
        public string fax { get; set; }
        public dynamic services { get; set; }
        [Required]
        public dynamic masterUser { get; set; }
        public string serviceRoleId { get; set; }
        public string longitude { get; set; }
        public string latitude { get; set; }
        public string addressAdditionalInformation { get; set; }
    }
}
