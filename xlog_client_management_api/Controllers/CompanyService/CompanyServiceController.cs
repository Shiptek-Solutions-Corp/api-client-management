using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using xgca.core.CompanyService;

namespace xlog_client_management_api.Controllers.CompanyService
{
    [Route("clients/api/v1")]
    [ApiController]
    public class CompanyServiceController : Controller
    {
        public readonly ICompanyService _companyService;
        public CompanyServiceController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        [Route("company/services/user/{referenceId}")]
        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ListCompanyServiceByUserId(int referenceId)
        {
            var response = await _companyService.ListByCompanyUserId(referenceId);

            if (response.statusCode == 400)
            {
                return BadRequest(response);
            }
            else if (response.statusCode == 401)
            {
                return Unauthorized(response);
            }

            return Ok(response);
        }

        [Route("company/services/{companyId}")]
        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ListCompanyServiceByCompanyId(string companyId)
        {
            var response = await _companyService.ListByCompanyId(companyId);

            if (response.statusCode == 400)
            {
                return BadRequest(response);
            }
            else if (response.statusCode == 401)
            {
                return Unauthorized(response);
            }

            return Ok(response);
        }


        //[Route("providers")]
        //[HttpGet]
        //[Authorize(AuthenticationSchemes = "Bearer")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
        //public async Task<IActionResult> List([FromQuery] string serviceId = null, [FromQuery] int pageNumber = 0, [FromQuery] int pageSize = 50, [FromQuery] int recordCount = 0)
        //{
        //    var companyId = Request.HttpContext.User.Claims.First(x => x.Type == "custom:companyId").Value;
        //    var response = await _companyService.ListProviders(Convert.ToInt32(companyId), serviceId, pageNumber, pageSize, recordCount);

        //    if (response.statusCode == 400)
        //    {
        //        return BadRequest(response);
        //    }
        //    else if (response.statusCode == 401)
        //    {
        //        return Unauthorized(response);
        //    }

        //    return Ok(response);
        //}

        [Route("providers/quick-search")]
        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> List([FromQuery] string search, [FromQuery] string serviceId = null, [FromQuery] int preferredProviderPageNumber = 0, [FromQuery] int preferredProviderPageSize = 50, [FromQuery] int preferredProviderRecordCount = 0, [FromQuery] int otherProviderPageNumber = 0, [FromQuery] int otherProviderPageSize = 50, [FromQuery] int otherProviderRecordCount = 0)
        {
            var companyId = Request.HttpContext.User.Claims.First(x => x.Type == "custom:companyId").Value;
            var response = await _companyService.ListProviders(Convert.ToInt32(companyId), search, serviceId, otherProviderPageNumber, otherProviderPageSize, otherProviderRecordCount, preferredProviderPageNumber, preferredProviderPageSize, preferredProviderRecordCount);

            if (response.statusCode == 400)
            {
                return BadRequest(response);
            }
            else if (response.statusCode == 401)
            {
                return Unauthorized(response);
            }

            return Ok(response);
        }
    }
}