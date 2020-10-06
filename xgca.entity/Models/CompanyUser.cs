using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xgca.entity.Models
{
    [Table("CompanyUser")]
    public class CompanyUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CompanyUserId { get; set; }
        public int CompanyId { get; set; }
        public int UserId { get; set; }
        [System.ComponentModel.DefaultValue(1)]
        public int UserTypeId { get; set; }
        public byte Status { get; set; }
        [System.ComponentModel.DefaultValue(0)]
        public byte IsDeleted { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Guid { get; set; }

        public virtual User Users { get; set; }
        public virtual Company Companies { get; set; }
        public virtual List<CompanyServiceUser> CompanyServiceUsers { get; set; }
    }
}
