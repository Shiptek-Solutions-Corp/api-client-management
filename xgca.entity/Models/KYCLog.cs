using System;
using System.Collections.Generic;

namespace xgca.entity.Models
{
    public partial class KYCLog
    {
        public int KyclogId { get; set; }
        public int CompanyId { get; set; }
        public int CompanySectionsId { get; set; }
        public string Remarks { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid Guid { get; set; }
        public string SectionStatusCode { get; set; }

        public virtual Company Company { get; set; }
        public virtual CompanySections CompanySections { get; set; }
    }
}
