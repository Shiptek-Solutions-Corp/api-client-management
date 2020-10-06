using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xgca.entity.Models
{
    [Table("CompanyServiceRole")]
    public class CompanyServiceRole
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CompanyServiceRoleId { get; set; }
        public int? CompanyServiceId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Guid { get; set; }
        [System.ComponentModel.DefaultValue(0)]
        public byte IsDeleted { get; set; }
        [System.ComponentModel.DefaultValue(1)]
        public byte IsActive { get; set; }
        public virtual CompanyService CompanyServices { get; set; } 
        public virtual ICollection<CompanyGroupResource> CompanyGroupResources { get; set; }
        public virtual ICollection<CompanyServiceUser> CompanyServiceUsers { get; set; }
        public virtual ICollection<CompanyServiceUserRole> CompanyServiceUserRoles { get; set; }

    }
}
