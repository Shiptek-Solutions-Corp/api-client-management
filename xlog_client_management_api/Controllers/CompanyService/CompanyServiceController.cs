using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
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
    }
}