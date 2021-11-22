using System;
using System.Collections.Generic;
using System.Text;

namespace xgca.core.Models.CompanyService
{
    public class GetCompanyServiceModel
    {
        public string Guid { get; set; }
        public int ServiceId { get; set; }
        public string ServiceName { get; set; }
        public string ImageUrl { get; set; }
    }
}
