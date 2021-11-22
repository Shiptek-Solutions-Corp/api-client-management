using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using xas.core.ServiceRoleConfig;

namespace xlog_accreditation_service.Controllers
{
    [Route("accreditation/api/v1")]
    [ApiController]
    public class ServiceRoleConfigController : Controller
    {
        IServiceRoleConfigCore _serviceRoleConfigCore;
        public ServiceRoleConfigController(IServiceRoleConfigCore serviceRoleConfigCore)
        {
            _serviceRoleConfigCore = serviceRoleConfigCore;
        }

        /// <summary>
        /// List of allowed Services
        /// </summary>
        /// <response code="200">Success</response>  
        /// <response code="400">Error Found!</response>  
        /// <response code="401">Unauthorized!</response>
        /// <response code="500">Internal Server Error!</response>
        [HttpGet]
        [Route("service/Role/allowed-list/{serviceId}")]

        public async Task<IActionResult> Get([FromRoute]Guid serviceId)
        {
            var response = await _serviceRoleConfigCore.ServiceRoleConfig(serviceId);
            if (response.statusCode == StatusCodes.Status400BadRequest) return BadRequest(response);
            if (response.statusCode == StatusCodes.Status401Unauthorized) return Unauthorized(response);
            return Ok(response);
        } 
    }
}
