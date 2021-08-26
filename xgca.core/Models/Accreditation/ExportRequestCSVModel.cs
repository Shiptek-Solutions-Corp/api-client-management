using System;
using System.Collections.Generic;
using System.Text;

namespace xgca.core.Models.Accreditation
{
    public class ExportRequestCSVModel
    {

        public string CompanyName { get; set; }
        public string CompanyFullAddress { get; set; }    
        public string CompanyCountryName { get; set; }
        public string CompanyStateCityName { get; set; }            
        public string PortAreaList { get; set; }
        public string TruckAreaList { get; set; }
        public string RequestStatus { get; set; }

    }
}
