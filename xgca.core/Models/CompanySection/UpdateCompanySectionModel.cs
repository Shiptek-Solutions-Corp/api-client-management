using System;
using System.Collections.Generic;
using System.Text;

namespace xgca.core.Models.CompanySection
{
    public class UpdateCompanySectionModel
    {
        public string Id { get; set; }
        public string SectionStatusCode { get; set; }
        public bool IsDraft { get; set; }
        public bool? IsActive { get; set; }
    }
}
