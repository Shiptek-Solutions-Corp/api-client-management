using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xgca.entity.Models
{
   [Table("CompanyGroupResource", Schema = "Company")]
    public class CompanyGroupResource
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CompanyGroupResourceId { get; set; }
        public int CompanyServiceRoleId { get; set; }
        public int GroupResourceId { get; set; }
        [System.ComponentModel.DefaultValue(0)]
        public byte IsAllowed { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Guid { get; set; }
        [System.ComponentModel.DefaultValue(0)]
        public byte IsDeleted { get; set; }
        public virtual CompanyServiceRole CompanyServiceRole { get; set; }
    }
}
