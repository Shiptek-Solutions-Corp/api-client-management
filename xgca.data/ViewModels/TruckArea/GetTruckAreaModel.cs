using System;
using System.Collections.Generic;
using System.Text;

namespace xgca.data.ViewModels.TruckArea
{
    public class GetTruckAreaModel
    {
        public int TruckAreaId { get; set; }
        public Guid TruckAreaGuid { get; set; }
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
