using System;
using System.Collections.Generic;

namespace xgca.entity.Models
{
    public partial class ServiceRoleConfig
    {
        public int ServiceRoleConfigId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool IsDeleted { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? DeletedOn { get; set; }
        public Guid Guid { get; set; }
        public Guid ServiceRoleId { get; set; }
        public Guid ServiceRoleIdAllowed { get; set; }
    }
}
