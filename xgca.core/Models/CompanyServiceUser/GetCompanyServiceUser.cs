using System;
using System.Collections.Generic;
using System.Text;

namespace xgca.core.Models.CompanyServiceUser
{
    public class GetCompanyServiceUser
    {
        public int CompanyServiceUserId { get; set; }
        public int? CompanyServiceId { get; set; }
        public int CompanyUserId { get; set; }
        public int? CompanyServiceRoleId { get; set; }
        public Guid Guid { get; set; }
        public  entity.Models.CompanyService CompanyServices { get; set; }
        public entity.Models.CompanyUser CompanyUsers { get; set; }
    }
}
