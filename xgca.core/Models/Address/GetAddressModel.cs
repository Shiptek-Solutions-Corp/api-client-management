using System;
using System.Collections.Generic;
using System.Text;
using xgca.core.Models.AddressType;

namespace xgca.core.Models.Address
{
    public class GetAddressModel
    {
        public Guid Guid { get; set; }
        public string AddressLine { get; set; }
        public string CityName { get; set; }
        public string StateName { get; set; }
        public string ZipCode { get; set; }
        public string CountryName { get; set; }
        public string FullAddress { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string AddressAdditionalInformation { get; set; }
        public GetAddressTypeModel AddressTypes { get; set; }
    }
}
