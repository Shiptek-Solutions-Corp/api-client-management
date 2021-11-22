using System;
using System.Collections.Generic;

namespace xgca.entity.Models
{
    public partial class CompanySections
    {
        public CompanySections()
        {
            Kyclog = new HashSet<KYCLog>();
        }

        public int CompanyId { get; set; }
        public string SectionStatusCode { get; set; }
        public string SectionCode { get; set; }
        public int CompanySectionsId { get; set; }
        public bool IsDraft { get; set; }
        public bool? IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool IsDeleted { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? DeletedOn { get; set; }
        public Guid Guid { get; set; }

        public virtual Company Company { get; set; }
        public virtual Section SectionCodeNavigation { get; set; }
        public virtual SectionStatus SectionStatusCodeNavigation { get; set; }
        public virtual ICollection<KYCLog> Kyclog { get; set; }
    }
}
