using System;
using System.Collections.Generic;

namespace xgca.entity.Models
{
    public partial class PortArea
    {
        public int RequestId { get; set; }
        public int PortAreaId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsDeleted { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? DeletedOn { get; set; }
        public Guid Guid { get; set; }
        public int CountryAreaId { get; set; }
        public Guid PortId { get; set; }
        public int PortOfLoading { get; set; }
        public int PortOfDischarge { get; set; }

        public virtual Request Request { get; set; }
    }
}
