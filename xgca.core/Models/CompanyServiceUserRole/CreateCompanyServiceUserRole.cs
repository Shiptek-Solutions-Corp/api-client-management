using System;
using System.Collections.Generic;
using System.Text;

namespace xgca.core.Models.CompanyServiceUserRole
{
    public class CreateCompanyServiceUserRole
    {
        public int CompanyServiceId { get; set; }
        public int CompanyServiceUserId { get; set; }
        public int CompanyServiceRoleId { get; set; }
    }
}
