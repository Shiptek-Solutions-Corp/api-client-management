using System;
using System.Collections.Generic;
using System.Text;

namespace xgca.core.Models.Guest
{
    public class UpdateGuest
    {
        public string GuestId { get; set; }
        public string GuestName { get; set; }
        public string Image { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumberPrefixId { get; set; }
        public string PhoneNumberPrefix { get; set; }
        public string PhoneNumber { get; set; }
        public string MobileNumberPrefixId { get; set; }
        public string MobileNumberPrefix { get; set; }
        public string MobileNumber { get; set; }
        public string FaxNumberPrefixId { get; set; }
        public string FaxNumberPrefix { get; set; }
        public string FaxNumber { get; set; }
        public string AddressLine { get; set; }
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public string StateId { get; set; }
        public string StateName { get; set; }
        public string CityId { get; set; }
        public string CityName { get; set; }
        public string ZipCode { get; set; }
    }
}
