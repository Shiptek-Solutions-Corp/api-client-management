using System;
using System.Collections.Generic;
using System.Text;
using xgca.core.Models.Company;
using xgca.core.Models.User;

namespace xgca.core.Models.CompanyUser
{
    public class GetCompanyUserModel
    {
        public int CompanyUserId { get; set; }
        public int CompanyId { get; set; }
        public int UserId { get; set; }
        public int UserTypeId { get; set; }
        public Guid Guid { get; set; }
        public virtual GetUserModel Users { get; set; }
        public virtual GetCompanyModel Companies { get; set; }
    }
}
