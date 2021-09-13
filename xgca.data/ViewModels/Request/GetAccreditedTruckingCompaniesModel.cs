using System;
using System.Collections.Generic;
using System.Text;

namespace xgca.data.ViewModels.Request
{
    public class GetAccreditedTruckingCompaniesModel
    {      
        public Guid CompanyGuid { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public string CompanyEmail { get; set; }  
        public string CompanyLogo { get; set; }
    }
}
