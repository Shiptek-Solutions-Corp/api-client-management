using System;
using System.Collections.Generic;
using System.Text;
using xgca.entity.Models;

namespace xgca.core.Models.GroupResource
{
    public class GetGroupResource
    {
        public int ModuleGroupId { get; set; }
        public int ResourceId { get; set; }
        public entity.Models.ModuleGroup ModuleGroup { get; set; }
        public ICollection<entity.Models.CompanyGroupResource> CompanyGroupResources { get; set; }

    }
}
