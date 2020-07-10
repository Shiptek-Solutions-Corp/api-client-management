using System;
using System.Collections.Generic;
using System.Text;
using xgca.core.Models.CompanyService;

namespace xgca.core.Models.CompanyServiceRole
{
    public class GetCompanyServiceRoleModel
    {
        public int CompanyServiceRoleId { get; set; }
        public int? CompanyServiceId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid Guid { get; set; }
        public byte IsActive { get; set; }
        public GetCompanyService CompanyServices { get; set; }
    }
}
