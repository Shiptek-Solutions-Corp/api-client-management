using System;
using System.Collections.Generic;
using System.Text;

namespace xgca.core.Models.Accreditation
{
    //SC Shipper/Consignee
    //SL  Shipping Line
    //SA Shipping Agent
    //TR  Trucker
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

    //SC Shipper/Consignee
    public class ExportRequestCSVModel_ShipperConsignee
    {
        public string Company { get; set; }
        public string Country { get; set; }
        public string StateCity { get; set; }
        public string CompanyStatus { get; set; }
        public string RequestStatus { get; set; }

    }

    //SL  Shipping Line
    public class ExportRequestCSVModel_ShippingLine
    {
        public string Company { get; set; }
        public string Address { get; set; }
        public string OperatingCountry { get; set; }
        public string PortResponsibility { get; set; }
        public string CompanyStatus { get; set; }
        public string RequestStatus { get; set; }

    }

    //SA Shipping Agent
    public class ExportRequestCSVModel_ShippingAgency
    {
        public string Company { get; set; }
        public string Address { get; set; }
        public string OperatingCountry { get; set; }
        public string PortResponsibility { get; set; }
        public string CompanyStatus { get; set; }
        public string RequestStatus { get; set; }

    }

    //TR  Trucker
    public class ExportRequestCSVModel_Trucking
    {
        public string Company { get; set; }
        public string Address { get; set; }
        public string AreaOfResponsibility { get; set; }
        public string CompanyStatus { get; set; }
        public string RequestStatus { get; set; }

    }
}
