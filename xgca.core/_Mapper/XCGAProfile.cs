using System;
using System.Collections.Generic;
using System.Text;
using xgca.core.Models.AuditLog;
using xgca.core.Models.CompanyGroupResource;
using xgca.core.Models.CompanyServiceRole;
using xgca.core.Models.CompanyServiceUserRole;
using xgca.core.Models.GroupResource;
using xgca.core.Models.MenuModule;
using xgca.core.Models.ModuleGroup;

namespace xgca.core._Mapper
{
    public class XCGAProfile : AutoMapper.Profile
    {
        public XCGAProfile()
        {
            CreateMap<CreatedAuditLog, entity.Models.AuditLog>();

            CreateMap<CreateCompanyGroupResource, entity.Models.CompanyGroupResource>();
            CreateMap<entity.Models.CompanyGroupResource, GetCompanyGroupResource>();


            CreateMap<CreateCompanyServiceUserRole, entity.Models.CompanyServiceUserRole>();
            CreateMap<entity.Models.CompanyServiceUserRole, GetCompanyServiceUserRole>();

            CreateMap<CreateGroupResource, entity.Models.GroupResource>();
            CreateMap<entity.Models.GroupResource, GetGroupResource>();

            CreateMap<CreateMenuModule, entity.Models.MenuModule>();
            CreateMap<entity.Models.MenuModule, GetMenuModule>();

            CreateMap<CreateModuleGroup, entity.Models.ModuleGroup>();
            CreateMap<entity.Models.ModuleGroup, GetModuleGroup>();

            CreateMap<entity.Models.CompanyServiceRole, GetCompanyServiceRoleModel>();

        }
    }
}
