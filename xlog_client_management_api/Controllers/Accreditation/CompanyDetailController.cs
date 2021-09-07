using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using xas.core.accreditation.Request;

namespace xlog_client_management_api.Controllers.Accreditation
{
    [ApiExplorerSettings(GroupName = "v2")]
    [Route("accreditation/api/v1")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class CompanyDetailController : ControllerBase
    {
        private readonly IRequestCore _requestCore;
        public CompanyDetailController(IRequestCore requestCore)
        {
            _requestCore = requestCore;
        }

        // <summary>
        /// Check company details via CompanyCode
        /// </summary>
        /// <response code="200">Success</response>  
        /// <response code="400">Error Found!</response>  
        /// <response code="401">Unauthorized!</response>
        /// <response code="500">Internal Server Error!</response>
        [HttpPost]
        [Route("company-detail/check/{companyCode}")]
        public async Task<IActionResult> CheckCompanyDetailCode([FromRoute] string companyCode)
        {
            var response = await _requestCore.CheckCompanyDetail(companyCode);
            if (!response.isSuccessful)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}
