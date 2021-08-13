using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace xlog_client_management_api.Controllers.Accreditation
{
    [ApiExplorerSettings(GroupName = "v2")]
    [Route("clients/api/v1")]

    [ApiController]
    public class AccreditationController : ControllerBase
    {
        
        public AccreditationController()
        {

        }
    }
}
