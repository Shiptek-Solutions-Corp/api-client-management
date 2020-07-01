using System;
using System.Collections.Generic;
using System.Text;
using xgca.core.Models.CompanyServiceRole;
using xgca.core.Models.CompanyServiceUser;

namespace xgca.core.Models.CompanyServiceUserRole
{
    public class GetCompanyServiceUserRole
    {
        public int CompanyServiceUserRoleID { get; set; }
        public int CompanyServiceId { get; set; }
        public int CompanyServiceUserId { get; set; }
        public int CompanyServiceRoleId { get; set; }
        public GetCompanyServiceUser CompanyServiceUser { get; set; }
        public GetCompanyServiceRoleModel CompanyServiceRole { get; set; }

    }
}
