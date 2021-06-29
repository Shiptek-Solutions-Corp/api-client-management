using System;

namespace xgca.core.Models.Address
{
    public class UpdateAddressModel
    {
        public Guid Guid{ get; set; }
        public string AddressId { get; set; }
        public string AddressLine { get; set; }
        public string CityId { get; set; }
        public string CityName { get; set; }
        public string StateId { get; set; }
        public string StateName { get; set; }
        public string ZipCode { get; set; }
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public string FullAddress { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string AddressAdditionalInformation { get; set; }
    }
}
