using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace xgca.core.Models.CompanyServiceRole
{
    public class CreateCompanyServiceRoleModel
    {
        [Required]
        public int CompanyServiceId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        public string CreatedBy { get; set; }
    }
}
