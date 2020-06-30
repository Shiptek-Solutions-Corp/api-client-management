using System;
using System.Collections.Generic;
using System.Text;

namespace xgca.core.Models.CompanyServiceUserRole
{
    public class GetCompanyServiceUserRole
    {
        public int CompanyServiceId { get; set; }
        public int CompanyServiceUserId { get; set; }
        public int CompanyServiceRoleId { get; set; }
        public entity.Models.CompanyService CompanyService { get; set; }
        public entity.Models.CompanyServiceRole CompanyServiceRole { get; set; }

    }
}
