using System;
using System.Collections.Generic;
using System.Text;

namespace xgca.core.Helpers
{
    public class CompanyUserHelper
    {
        public static dynamic BuildNewCompanyUser(dynamic obj)
        {
            var companyUser = new entity.Models.CompanyUser
            {
                CompanyId  = obj.CompanyId,
                UserId = obj.User,
                UserTypeId = obj.UserTypeId,
                CreatedBy = obj.Createdby
            };

            return companyUser;
        }
    }
}
