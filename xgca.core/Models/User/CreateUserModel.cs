using System;
using System.Collections.Generic;

namespace xgca.core.Models.User
{
    public class CreateUserModel
    {
        public string CompanyId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string Title { get; set; }
        public string ImageURL { get; set; }
        public int PhonePrefixId { get; set; }
        public string PhonePrefix { get; set; }
        public string Phone { get; set; }
        public int MobilePrefixId { get; set; }
        public string MobilePrefix { get; set; }
        public string Mobile { get; set; }
        public string EmailAddress { get; set; }
        public string CreatedBy { get; set; }
        public string ServiceId { get; set; }
        public List<RoleProperty> Roles { get; set; }

    }

    public class RoleProperty
    {
        public string companyServiceId { get; set; }
        public string companyServiceRoleId { get; set; }
    }
}
