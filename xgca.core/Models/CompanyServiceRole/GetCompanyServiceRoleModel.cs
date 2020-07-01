using System;
using System.Collections.Generic;
using System.Text;

namespace xgca.core.Models.CompanyServiceRole
{
    public class GetCompanyServiceRoleModel
    {
        public int CompanyServiceRoleId { get; set; }
        public int? CompanyServiceId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid Guid { get; set; }
    }
}
