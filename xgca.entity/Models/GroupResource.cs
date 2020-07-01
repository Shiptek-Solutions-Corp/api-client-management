using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using static xgca.entity.Models._Model;

namespace xgca.entity.Models
{
    [Table("GroupResource", Schema = "Module")]
    public class GroupResource : AuditableEntity
    {
        public int GroupResourceId { get; set; }
        public int ModuleGroupId { get; set; }
        public int ResourceId { get; set; }

        public virtual ModuleGroup ModuleGroup { get; set; }
        public virtual List<CompanyGroupResource> CompanyGroupResources { get; set; }
    }
}
