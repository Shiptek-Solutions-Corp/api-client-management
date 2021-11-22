using System;
using System.Collections.Generic;
using System.Text;

namespace xas.data.CustomModel.TruckArea
{
    public class CustomGetTruckArea
    {
        public string Guid { get; set; }
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public string StateId { get; set; }
        public string StateName { get; set; }
        public string CityId { get; set; }
        public string CityName { get; set; }
        public string PostalId { get; set; }
        public string PostalCode { get; set; }
    }
}
