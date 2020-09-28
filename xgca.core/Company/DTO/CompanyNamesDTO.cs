using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace xgca.core.Company.DTO
{
    public class CompanyNamesDTO
    {
        [Required]
        public string[] CompanyNames { get; set; }
    }
}
