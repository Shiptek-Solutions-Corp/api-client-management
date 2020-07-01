using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using static xgca.entity.Models._Model;

namespace xgca.entity.Models
{
    [Table("ModuleGroup", Schema = "Module")]
    public class ModuleGroup : AuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ModuleGroupsId { get; set; }
        public int ModuleId { get; set; }
        public int ResourceGroupId { get; set; }
    }
}
