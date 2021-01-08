using System;
using System.Collections.Generic;
using System.Text;

namespace xgca.core.Models.PreferredContact
{
    public class ListPreferredContact
    {
        public string PreferredContactId { get; set; }
        public string ContactId { get; set; }
        public string ContactName { get; set; }
        public int ContactType { get; set; }
        public string ImageURL { get; set; }
        public string CityProvince { get; set; }
        public string Country { get; set; }
        public string PhoneNumber { get; set; }
        public string MobileNumber { get; set; }
        public string FaxNumber { get; set; }
        public string Email { get; set; }
    }
}
