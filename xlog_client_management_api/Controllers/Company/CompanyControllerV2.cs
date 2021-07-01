using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using xgca.core.Models.Company;
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
        public async Task<IActionResult> Index([FromQuery] string orderBy, string query, int pageNumber = 1, int pageSize = 10, bool isFromSettings = false)
        {
            var result = await companyService.GetCompanyList(isFromSettings, pageNumber, pageSize, orderBy, query);
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
            var isValidGuid = Guid.TryParse(guid, out var id);
            if (!isValidGuid) return BadRequest("Invalid guid");

            var result = await companyService.GetCompany(id);

            if (result.StatusCode == 400) return BadRequest(result);
            if (result.StatusCode == 500) return BadRequest(result);
            if (result.StatusCode == 401) return Unauthorized();

            return Ok(result);
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
        public async Task<IActionResult> Patch(string guid, [FromBody] JsonPatchDocument<UpdateCompanyViewModel> payload)
        {
            var isValidGuid = Guid.TryParse(guid, out var id);
            if (!isValidGuid) return BadRequest("Invalid guid");

            var response = await companyService.Patch(id, payload);

            if (response.StatusCode == 400) return BadRequest(response);
            if (response.StatusCode == 500) return BadRequest(response);
            if (response.StatusCode == 401) return Unauthorized();

            return Ok(response);
        }

        [Route("{guid}")]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Put(string guid, [FromBody] UpdateCompanyViewModel payload)
        {
            var isValidGuid = Guid.TryParse(guid, out var id);
            if (!isValidGuid) return BadRequest("Invalid guid");

            var result = await companyService.Put(payload);

            if (result.StatusCode == 400) return BadRequest(result);
            if (result.StatusCode == 500) return BadRequest(result);
            if (result.StatusCode == 401) return Unauthorized();

            return Ok(result);
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
