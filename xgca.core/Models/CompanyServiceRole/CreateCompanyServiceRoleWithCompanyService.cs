using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace xgca.core.Models.CompanyServiceRole
{
    public class CreateCompanyServiceRoleWithCompanyService
    {
        [Required]
        public string CompanyId { get; set; }
        [Required]
        public string ServiceId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
    }
}
