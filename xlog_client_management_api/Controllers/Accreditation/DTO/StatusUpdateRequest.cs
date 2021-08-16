using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace xlog_accreditation_service.Controllers.AccreditationRequest.DTO
{
    public class StatusUpdateRequest
    {
        public Guid requestId { get; set; }
        public string status { get; set; }
    }
}
