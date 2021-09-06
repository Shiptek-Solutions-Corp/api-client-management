using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using xas.core.ServiceProvider;
using xlog_client_management_api;

namespace xlog_accreditation_service.Controllers
{
    [Route("accreditation/api/v1")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController]
    public class ServiceProviderController : ControllerBase
    {
        private readonly ICServiceProvider _serviceProvider;

        public ServiceProviderController(ICServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// List of Service Provider
        /// </summary>
        /// <response code="200">Success</response>  
        /// <response code="400">Error Found!</response>  
        /// <response code="401">Unauthorized!</response>
        /// <response code="500">Internal Server Error!</response>
        [Route("provider/list/{serviceProviderId}")]
        [TokenAuthorize("scope", "accreditationShippingAgency.get|accreditationTrucking.get|accreditationShippingLine.get")]
        [HttpGet]
        public async Task<IActionResult> ServiceProviderListing([FromHeader(Name = "Authorization")]string auth,[FromRoute]Guid serviceProviderId, [FromQuery]int page, [FromQuery]int rows , [FromQuery] string search = "")
        {
            var response = await _serviceProvider.ServiceProviderByServiceId(serviceProviderId, page, rows, auth , search);
            if (response.statusCode == StatusCodes.Status400BadRequest) return BadRequest(response);
            if (response.statusCode == StatusCodes.Status401Unauthorized) return Unauthorized(response);
            return Ok(response);
        }
    }
}