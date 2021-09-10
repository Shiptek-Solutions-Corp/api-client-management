using System;
using System.Collections.Generic;

namespace xgca.entity.Models
{
    public partial class Request
    {
        public Request()
        {
            PortArea = new HashSet<PortArea>();
            TruckArea = new HashSet<TruckArea>();
        }

        public int AccreditationStatusConfigId { get; set; }
        public int? RequestStatusId { get; set; }
        public int RequestId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool IsDeleted { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? DeletedOn { get; set; }
        public Guid Guid { get; set; }
        public Guid ServiceRoleIdFrom { get; set; }
        public Guid CompanyIdFrom { get; set; }
        public Guid ServiceRoleIdTo { get; set; }
        public Guid CompanyIdTo { get; set; }
        public bool? IsActive { get; set; }

        public virtual AccreditationStatusConfig AccreditationStatusConfig { get; set; }
        public virtual ICollection<PortArea> PortArea { get; set; }
        public virtual ICollection<TruckArea> TruckArea { get; set; }
    }
}
