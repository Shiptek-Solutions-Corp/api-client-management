using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using xgca.core.Models.AuditLog;

namespace xgca.core._Mapper
{
    public class AuditLogProfile : AutoMapper.Profile
    {
        public AuditLogProfile()
        {
            CreateMap<CreatedAuditLog, entity.Models.AuditLog>();
        }
    }
}
