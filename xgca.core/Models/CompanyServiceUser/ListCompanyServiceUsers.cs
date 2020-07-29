using System;
using System.Collections.Generic;
using System.Text;

namespace xgca.core.Models.CompanyServiceUser
{
    public class ListCompanyServiceUsers
    {
        public string CompanyServiceuserId { get; set; }
        public string CompanyServiceId { get; set; }
        public string CompanyServiceRoleId { get; set; }
        public string ServiceId { get; set; }
        public string Fullname { get; set; }
        public string Username { get; set; }
        public string EmailAddress { get; set; }
        public string ImageURL { get; set; }
        public string Role { get; set; }
        public string Service { get; set; }
        public byte IsActive { get; set; }
        public byte IsLocked { get; set; }
    }
}
