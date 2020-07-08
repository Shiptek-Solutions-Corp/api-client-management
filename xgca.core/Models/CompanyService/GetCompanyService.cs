using System;
using System.Collections.Generic;
using System.Text;
using xgca.core.Models.Company;

namespace xgca.core.Models.CompanyService
{
    public class GetCompanyService
    {
        public Guid Guid { get; set; }
        public int ServiceId { get; set; }
        public string ServiceName { get; set; }
        public GetCompanyModel Companies { get; set; }
    }
}
