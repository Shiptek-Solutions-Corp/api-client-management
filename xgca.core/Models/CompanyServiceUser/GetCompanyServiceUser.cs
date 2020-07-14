using System;
using System.Collections.Generic;
using System.Text;
using xgca.core.Models.CompanyService;
using xgca.core.Models.CompanyUser;

namespace xgca.core.Models.CompanyServiceUser
{
    public class GetCompanyServiceUser
    {
        public int CompanyServiceUserId { get; set; }
        public int? CompanyServiceId { get; set; }
        public int CompanyUserId { get; set; }
        public int? CompanyServiceRoleId { get; set; }
        public Guid Guid { get; set; }
        public  GetCompanyService CompanyServices { get; set; }
        public GetCompanyUserModel CompanyUsers { get; set; }
    }
}
