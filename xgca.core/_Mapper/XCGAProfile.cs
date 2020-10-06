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
using xgca.core.Models.PreferredProvider;
using xgca.core.Models.User;

namespace xgca.core._Mapper
{
    public class XCGAProfile : AutoMapper.Profile
    {
        public XCGAProfile()
        {
            CreateMap<CreatedAuditLog, entity.Models.AuditLog>();

            CreateMap<CreateCompanyGroupResource, entity.Models.CompanyGroupResource>()
                .ForMember(x => x.Guid, opt => opt.MapFrom(o => Guid.NewGuid()))
                .ForMember(x => x.CreatedOn, opt => opt.MapFrom(o => DateTime.UtcNow))
                .ForMember(x => x.CreatedBy, opt => opt.MapFrom(o => 1))
                .ForMember(x => x.ModifiedOn, opt => opt.MapFrom(o => DateTime.UtcNow))
                .ForMember(x => x.ModifiedOn, opt => opt.MapFrom(o => 1));


            CreateMap<entity.Models.CompanyGroupResource, GetCompanyGroupResource>();


            CreateMap<CreateCompanyServiceUserRole, entity.Models.CompanyServiceUserRole>();
            CreateMap<entity.Models.CompanyServiceUserRole, GetCompanyServiceUserRole>();

            CreateMap<entity.Models.CompanyServiceUser, GetCompanyServiceUser>();
            CreateMap<CreateNewUserPerGroupModuleModel, entity.Models.CompanyServiceUser>()
                .ForMember(x => x.IsMasterUser, opt => opt.MapFrom(o => 0));


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

            CreateMap<CreatePreferredProvider, entity.Models.PreferredProvider>();
        }
    }
}
