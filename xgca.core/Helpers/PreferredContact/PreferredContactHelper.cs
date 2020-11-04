using DocumentFormat.OpenXml.Bibliography;
using System;
using System.Collections.Generic;
using System.Text;

namespace xgca.core.Helpers.Utility
{
    public class PreferredContactHelper : IPreferredContactHelper
    {
        public dynamic BuildCompanyDetails(string preferredContactGuid, dynamic company, dynamic state, dynamic city, dynamic masterUser)
        {
            string completeAddress = this.RegisteredAddress(company.Addresses);

            var contact = new
            {
                PreferredContactId = preferredContactGuid,
                ContactId = company.Guid.ToString(),
                ContactName = company.CompanyName,
                company.ImageURL,
                CompleteAddress = completeAddress,
                City = new
                {
                    city.CityId,
                    city.CityName
                },
                State = new
                {
                    state.StateId,
                    state.StateName
                },
                Country = new
                {
                    company.Addresses.CountryId,
                    company.Addresses.CountryName,
                },
                company.Addresses.ZipCode,
                Phone = new
                {
                    company.ContactDetails.PhonePrefixId,
                    company.ContactDetails.PhonePrefix,
                    company.ContactDetails.Phone
                },
                Mobile = new
                {
                    company.ContactDetails.MobilePrefixId,
                    company.ContactDetails.MobilePrefix,
                    company.ContactDetails.Mobile
                },
                Fax = new
                {
                    company.ContactDetails.FaxPrefixId,
                    company.ContactDetails.FaxPrefix,
                    company.ContactDetails.Fax
                },
                masterUser.FirstName,
                masterUser.LastName,
                masterUser.EmailAddress
            };

            return contact;
        }

        public dynamic BuildGuestDetails(string preferredContactGuid, dynamic guest)
        {
            var contact = new
            {
                PreferredContactId = preferredContactGuid,
                ContactId = guest.Id.ToString(),
                ContactName = guest.GuestName,
                ImageURL = guest.Image,
                CompleteAddress = guest.AddressLine,
                City = new
                {
                    guest.CityId,
                    guest.CityName
                },
                State = new
                {
                    guest.StateId,
                    guest.StateName
                },
                Country = new
                {
                    guest.CountryId,
                    guest.CountryName,
                },
                guest.ZipCode,
                Phone = new
                {
                    guest.PhoneNumberPrefixId,
                    guest.PhoneNumberPrefix,
                    guest.PhoneNumber
                },
                Mobile = new
                {
                    guest.MobileNumberPrefixId,
                    guest.MobileNumberPrefix,
                    guest.MobileNumber
                },
                Fax = new
                {
                    guest.FaxNumberPrefixId,
                    guest.FaxNumberPrefix,
                    guest.FaxNumber
                },
                guest.FirstName,
                guest.LastName,
                guest.EmailAddress
            };

            return contact;
        }

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
