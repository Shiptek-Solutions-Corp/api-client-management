using System;
using System.Collections.Generic;
using System.Text;
using xgca.core.Models.AuditLog;
using xgca.core.Models.Company;
using xgca.core.Models.CompanyGroupResource;
using xgca.core.Models.CompanyService;
using xgca.core.Models.CompanyServiceRole;
using xgca.core.Models.CompanyServiceUser;
using xgca.core.Models.CompanyServiceUserRole;
using xgca.core.Models.CompanyUser;
using xgca.core.Models.GroupResource;
using xgca.core.Models.Guest;
using xgca.core.Models.MenuModule;
using xgca.core.Models.ModuleGroup;
using xgca.core.Models.User;

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

            CreateMap<entity.Models.CompanyServiceUser, GetCompanyServiceUser>();
            CreateMap<CreateNewUserPerGroupModuleModel, entity.Models.CompanyServiceUser>();


            CreateMap<entity.Models.CompanyServiceRole, GetCompanyServiceRoleModel>();
            CreateMap<CreateGroupPermissionUserModel, entity.Models.CompanyServiceRole>();

            CreateMap<CreateCompanyServiceRoleModel, entity.Models.CompanyServiceRole>();
            CreateMap<UpdateCompanyServiceRoleModel, entity.Models.CompanyServiceRole>();

            CreateMap<entity.Models.CompanyService, GetCompanyService>();

            CreateMap<entity.Models.Company, GetCompanyModel>();

            CreateMap<entity.Models.CompanyUser, GetCompanyUserModel>();

            CreateMap<entity.Models.User, GetUserModel>();


            CreateMap<CreateGuest, entity.Models.Guest>();
            CreateMap<UpdateGuestContact, entity.Models.Guest>();
        }
    }
}
