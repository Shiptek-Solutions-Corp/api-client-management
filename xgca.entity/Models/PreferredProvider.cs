using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace xgca.entity.Models
{

    [Table("PreferredProvider", Schema = "Company")]
    public class PreferredProvider
    {
        public int PreferredProviderId { get; set; }
        public string CompanyServiceId { get; set; }
        public string CompanyId { get; set; }
        public string ServiceId { get; set; }
        public int ProfileId { get; set; }
        public Guid Guid { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public int IsDeleted { get; set; }
        public int DeletedBy { get; set; }
        public DateTime DeletedOn { get; set; }
    }
}
