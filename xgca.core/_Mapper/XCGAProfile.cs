using System;
using System.Collections.Generic;
using System.Text;
using xgca.core.Models.AuditLog;
using xgca.core.Models.CompanyGroupResource;
using xgca.core.Models.CompanyServiceUserRole;

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

        }
    }
}
