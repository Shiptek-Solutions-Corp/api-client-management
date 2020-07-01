using System;
using System.Collections.Generic;
using System.Text;
using xgca.core.Models.CompanyServiceRole;
using xgca.core.Models.GroupResource;
using xgca.entity.Models;

namespace xgca.core.Models.CompanyGroupResource
{
    public class GetCompanyGroupResource
    {
        public int CompanyGroupResourceId { get; set; }
        public int CompanyServiceRoleId { get; set; }
        public int GroupResourceId { get; set; }
        public Guid Guid { get; set; }
        public GetGroupResource GroupResource { get; set; }
        public GetCompanyServiceRoleModel CompanyServiceRole { get; set; }
    }
}
