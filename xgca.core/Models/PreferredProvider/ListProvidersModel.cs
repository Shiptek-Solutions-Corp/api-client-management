using System;
using System.Collections.Generic;
using System.Text;

namespace xgca.core.Models.CompanyService
{
    public class ListProvidersModel
    {
        public string CompanyServiceId { get; set; }
        public string CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string CompanyImageURL { get; set; }
        public string CompanyAddress { get; set; }
        public string ServiceId { get; set; }
        public string ServiceName { get; set; }
        public string ServiceImageURL { get; set; }
        
    }
}
