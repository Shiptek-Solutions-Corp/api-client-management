using System;
using System.Collections.Generic;
using System.Text;

namespace xgca.core.Models.CompanyService
{
    public class ListProvidersModel
    {
        public string CompanyServiceId { get; set; }
        public string CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string CompanyImageURL { get; set; }
        public string CompanyAddress { get; set; }
        public string ServiceId { get; set; }
        public string ServiceName { get; set; }
        public string ServiceImageURL { get; set; }
        public string PhoneNumber { get; set; }
        public string MobileNumber { get; set; }
        public string FaxNumber { get; set; }
        public string Email { get; set; }
        public string Country { get; set; }
        public int CountryId { get; set; }

    }
}
