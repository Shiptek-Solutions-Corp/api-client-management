using System;
using System.Collections.Generic;

namespace xgca.entity.Models
{
    public partial class KycStatus
    {
        public KycStatus()
        {
            Company = new HashSet<Company>();
        }

        public string KycStatusCode { get; set; }
        public Guid Guid { get; set; }
        public string Description { get; set; }
        public int? EnumIndex { get; set; }
        public bool? IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool IsDeleted { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? DeletedOn { get; set; }

        public virtual ICollection<Company> Company { get; set; }
    }
}
