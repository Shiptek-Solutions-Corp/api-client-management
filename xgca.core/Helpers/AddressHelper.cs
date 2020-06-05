using System;
using System.Collections.Generic;
using System.Text;
using xgca.entity.Models;
using Newtonsoft.Json;

namespace xgca.core.Helpers
{
    public class AddressHelper
    { 
        public static string GenerateFullAddress(dynamic obj)
        {
            string fullAddress = null;
            fullAddress = obj.AddressLine != null ? obj.AddressLine : null;

            fullAddress = fullAddress == null
                ? (obj.CityName != null ? ", " + obj.CityName : "")
                : (obj.CityName != null ? fullAddress + ", " + obj.CityName : null);

            fullAddress = fullAddress == null
                ? (obj.StateName != null ? ", " + obj.StateName : "")
                : (obj.StateName != null ? fullAddress + ", " + obj.StateName : null);

            fullAddress = fullAddress == null
                ? (obj.ZipCode != null ? ", " + obj.ZipCode : "")
                : (obj.ZipCode != null ? fullAddress + ", " + obj.ZipCode : null);

            fullAddress = fullAddress == null
                ? (obj.CountryName != null ? ", " + obj.CountryName : "")
                : (obj.CountryName != null ? fullAddress + ", " + obj.CountryName : null);

            return fullAddress;
        }
    }
}
