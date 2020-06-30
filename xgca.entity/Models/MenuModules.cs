using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using static xgca.entity.Models._Model;

namespace xgca.entity.Models
{

    [Table("MenuModule", Schema = "Module")]
    public class MenuModule : AuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MenuModulesId { get; set; }
        public int MenuId { get; set; }
        public int SubMenuId { get; set; }
        public int ModuleId { get; set; }

    }
}
