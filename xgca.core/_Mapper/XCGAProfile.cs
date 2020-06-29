using System;
using System.Collections.Generic;
using System.Text;
using xgca.core.Models.AuditLog;
using xgca.core.Models.CompanyGroupResource;

namespace xgca.core._Mapper
{
    public class XCGAProfile : AutoMapper.Profile
    {
        public XCGAProfile()
        {
            CreateMap<CreatedAuditLog, entity.Models.AuditLog>();

            CreateMap<CreateCompanyGroupResource, entity.Models.CompanyGroupResource>();
        }
    }
}
