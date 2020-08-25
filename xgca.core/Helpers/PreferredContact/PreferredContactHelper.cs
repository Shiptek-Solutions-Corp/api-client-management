using DocumentFormat.OpenXml.Bibliography;
using System;
using System.Collections.Generic;
using System.Text;

namespace xgca.core.Helpers.Utility
{
    public class PreferredContactHelper : IPreferredContactHelper
    {
        public string GuestAddress(dynamic obj)
        {
            string contactAddress = "";

            contactAddress = (obj.AddressLine is null) ? "" : obj.AddressLine;
            contactAddress += contactAddress.Length != 0 ? ", " : "";
            contactAddress += (obj.CityName is null) ?  "" : obj.CityName;
            contactAddress += contactAddress.Length != 0 ? ", " : "";
            contactAddress += (obj.StateName is null) ? "" : obj.StateName;
            contactAddress += contactAddress.Length != 0 ? ", " : "";
            contactAddress += obj.Country;

            return contactAddress;
        }

        public string GuestCityState(dynamic obj)
        {
            string cityState = "";

            cityState = (obj.CityName is null) ? "" : obj.CityName;
            cityState += (obj.StateName is null) ? "" : $", {obj.StateName}";

            return cityState;
        }

        public entity.Models.PreferredContact ParseObject(string guestId, string companyId, int profileId, int contactType, int createdBy)
        {
            var preferredContact = new entity.Models.PreferredContact
            {
                GuestId = guestId,
                CompanyId = companyId,
                ProfileId = profileId,
                ContactType = contactType,
                CreatedBy = createdBy,
                ModifiedBy = createdBy
            };

            return preferredContact;
        }

        public string RegisteredAddress(dynamic obj)
        {
            string contactAddress = "";

            contactAddress = (!(obj.Addresses.AddressLine is null) ? obj.Addresses.AddressLine : "");
            contactAddress += contactAddress.Length != 0 ? ", " : "";
            contactAddress += (!(obj.Addresses.CityName is null) ? obj.Addresses.CityName : "");
            contactAddress += contactAddress.Length != 0 ? ", " : "";
            contactAddress += (!(obj.Addresses.StateName is null) ? obj.Addresses.StateName : "");
            contactAddress += contactAddress.Length != 0 ? ", " : "";
            contactAddress += obj.Addresses.CountryName;

            return contactAddress;
        }

        public string RegisteredCityState(dynamic obj)
        {
            string cityState = "";

            cityState = (obj.Addresses.CityName is null) ? "" : obj.Addresses.CityName;
            cityState += (obj.Addresses.StateName is null) ? "" : $", {obj.Addresses.StateName}";

            return cityState;
        }
    }
}
