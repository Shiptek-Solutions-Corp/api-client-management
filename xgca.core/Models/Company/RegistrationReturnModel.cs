using System;
using System.Collections.Generic;
using System.Text;

namespace xgca.core.Models.Company
{
    public class RegistrationReturnModel
    {
        public int CompanyId { get; set; }
        public string CompanyGuid { get; set; }
        public int MasterUserId { get; set; }
        public string MasterUserGuid { get; set; }
        public string MasterUserEmail { get; set; }
    }
}
