using System;
using System.Collections.Generic;

namespace xgca.entity.Models
{
    public partial class BeneficialOwnersType
    {
        public BeneficialOwnersType()
        {
            CompanyBeneficialOwners = new HashSet<CompanyBeneficialOwners>();
        }

        public string BeneficialOwnersTypeCode { get; set; }
        public string Description { get; set; }
        public bool? IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool IsDeleted { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? DeletedOn { get; set; }

        public virtual ICollection<CompanyBeneficialOwners> CompanyBeneficialOwners { get; set; }
    }
}
