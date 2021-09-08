using System;
using System.Collections.Generic;
using System.Text;

namespace xgca.core.Validators.Request
{
    public class ExportPortAreaModel
    {
        public string CountryCode { get; set; }
        public string LoCode { get; set; }
        public string PortName { get; set; }
        public string Location { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string PortOfLoading { get; set; }
        public string PortOfDischarge { get; set; }
        public string CountryName { get; set; }
        public string CityName { get; set; }
        
    }
}
