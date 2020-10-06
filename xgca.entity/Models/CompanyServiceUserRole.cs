using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using static xgca.entity.Models._Model;

namespace xgca.entity.Models
{
    [Table("CompanyServiceUserRole")]
    public class CompanyServiceUserRole : SoftDeletableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CompanyServiceUserRoleID { get; set; }
        public int? CompanyServiceId { get; set; }
        public int? CompanyServiceUserId { get; set; }
        public int? CompanyServiceRoleId { get; set; }
        public virtual CompanyService CompanyService { get; set; }
        public virtual CompanyServiceUser CompanyServiceUser { get; set; }
        public virtual CompanyServiceRole CompanyServiceRole { get; set; }
    }
}
