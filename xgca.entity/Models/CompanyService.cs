using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xgca.entity.Models
{
    [Table("CompanyService")]
    public class CompanyService
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CompanyServiceId { get; set; }
        public int CompanyId { get; set; }
        public int ServiceId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Guid { get; set; }
        [System.ComponentModel.DefaultValue(0)]
        public byte IsDeleted { get; set; }
        public byte Status { get; set; }

        public virtual Company Companies { get; set; }
        public virtual ICollection<CompanyServiceRole> CompanyServiceRoles { get; set; }
        public virtual ICollection<CompanyServiceUser> CompanyServiceUsers { get; set; }
        public virtual ICollection<CompanyServiceUserRole> CompanyServiceUserRoles { get; set; }


    }
}
