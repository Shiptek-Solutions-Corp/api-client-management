using System;
using System.Collections.Generic;

namespace xgca.entity.Models
{
    public partial class AccreditationStatusConfig
    {
        public AccreditationStatusConfig()
        {
            Request = new HashSet<Request>();
        }

        public int AccreditationStatusConfigId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool IsDeleted { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? DeletedOn { get; set; }
        public Guid Guid { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Request> Request { get; set; }
    }
}
