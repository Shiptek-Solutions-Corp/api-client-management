using System;
using System.Collections.Generic;
using System.Text;

namespace xgca.core.Models.Accreditation.PortArea
{
    public class CreatePortAreaModel
    {
        public int RequestId { get; set; }
        public int CountryAreaId { get; set; }
        public Guid PortId { get; set; }
        public int PortOfLoading { get; set; }
        public int PortOfDischarge { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public Guid? StateCode { get; set; }
        public string StateName { get; set; }
        public Guid? CityCode { get; set; }
        public string CityName { get; set; }
        public string Locode { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string Location { get; set; }
    }
}
