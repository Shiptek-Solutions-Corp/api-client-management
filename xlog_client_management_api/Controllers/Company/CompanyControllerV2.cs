using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using xgca.core.Services;

namespace xlog_client_management_api.Controllers.Company
{
    [Route("clients/api/v2/companies")]
    //[Authorize(AuthenticationSchemes = "Bearer")]
    public class CompanyControllerV2 : Controller
    {
        private readonly ICompanyServiceV2 companyService;
        public CompanyControllerV2(ICompanyServiceV2 companyService)
        {
            this.companyService = companyService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Index([FromQuery] string orderBy, string query, int pageNumber = 1, int pageSize = 10)
        {
            var result = await companyService.GetCompanyList(pageNumber, pageSize, orderBy, query);
            if (result.StatusCode == 400) return BadRequest(result);
            if (result.StatusCode == 500) return BadRequest(result);
            if (result.StatusCode == 401) return Unauthorized();

            return Ok(result);
        }

        [Route("{guid}")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Show(string guid)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Create()
        {
            throw new NotImplementedException();

        }

        [Route("{guid}")]
        [HttpPatch]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Patch(string guid)
        {
            throw new NotImplementedException();

        }

        [Route("{guid}")]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Put(string guid)
        {
            throw new NotImplementedException();

        }

        [Route("{guid}")]
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Delete(string guid)
        {
            throw new NotImplementedException();

        }
    }
}
