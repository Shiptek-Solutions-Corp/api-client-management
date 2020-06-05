using System.Collections.Generic;

namespace xgca.core.Models.CompanyServiceRole
{
    public class CreateCompanyServiceRoleModel
    {
        public List<string> CompanyServiceId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
    }
}
