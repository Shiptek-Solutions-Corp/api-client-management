using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xgca.entity.Models
{
    [Table("CompanyServiceUser", Schema = "Company")]
    public class CompanyServiceUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CompanyServiceUserId { get; set; }
        public int CompanyServiceId { get; set; }
        public int CompanyUserId { get; set; }
        public int CompanyServiceRoleId { get; set; }
        public int IsMasterUser { get; set; }
        public int CompanyId { get; set; }
        [System.ComponentModel.DefaultValue(1)]
        public byte IsActive { get; set; }
        [System.ComponentModel.DefaultValue(0)]
        public byte IsLocked { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Guid { get; set; }
        [System.ComponentModel.DefaultValue(0)]
        public byte IsDeleted { get; set; }
        public virtual CompanyService CompanyServices { get; set; }
        public virtual CompanyUser CompanyUsers { get; set; }
        public virtual CompanyServiceRole CompanyServiceRoles { get; set; }
        public virtual ICollection<CompanyServiceUserRole> CompanyServiceUserRoles { get; set; }
    }
}
