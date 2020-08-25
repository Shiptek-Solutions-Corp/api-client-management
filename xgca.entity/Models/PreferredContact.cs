using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Policy;

namespace xgca.entity.Models
{
    [Table("PreferredContact", Schema = "Company")]
    public class PreferredContact
    {
        public int PreferredContactId { get; set; }
        public string GuestId { get; set; }
        public string CompanyId { get; set; }
        public int ProfileId { get; set; }
        public int ContactType { get; set; } // 1 - Registered, 2 - Guest
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
