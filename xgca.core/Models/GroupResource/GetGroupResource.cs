using System;
using System.Collections.Generic;
using System.Text;
using xgca.core.Models.CompanyGroupResource;
using xgca.core.Models.ModuleGroup;
using xgca.entity.Models;

namespace xgca.core.Models.GroupResource
{
    public class GetGroupResource
    {
        public int GroupResourceId { get; set; }
        public int ModuleGroupId { get; set; }
        public int ResourceId { get; set; }
        public GetModuleGroup ModuleGroup { get; set; }
        public ICollection<GetCompanyGroupResource> CompanyGroupResources { get; set; }

    }
}
