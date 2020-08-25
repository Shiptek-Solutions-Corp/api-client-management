using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Text;

namespace xgca.core.Helpers.Guest
{
    public class GuestHelper : IGuestHelper
    {
        public string ParseCompleteAddress(entity.Models.Guest guest)
        {
            string completeAddress = "";

            completeAddress = (guest.AddressLine is null) ? "" : guest.AddressLine;
            completeAddress += (guest.CityName is null) ? "" : $", {guest.CityName}";
            completeAddress += (guest.StateName is null) ? "" : $", {guest.StateName}";
            completeAddress += (guest.CountryName is null) ? "" : $", {guest.CountryName}";
            completeAddress += (guest.ZipCode is null) ? "" : $", {guest.ZipCode}";

            return completeAddress;
        }
    }
}
