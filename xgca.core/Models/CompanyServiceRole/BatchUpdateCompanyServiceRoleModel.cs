using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace xgca.core.Models.CompanyServiceRole
{
    public class BatchUpdateCompanyServiceRoleModel
    {
        [Required]
        public ICollection<string> Guids { get; set; }
        [Required]
        public string Type { get; set; }
    }
}
