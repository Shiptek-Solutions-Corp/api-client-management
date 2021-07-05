using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using xgca.core.Models.CompanyTaxSettings;
using xgca.core.Services;

namespace xlog_client_management_api.Controllers.CompanyTaxSettings
{
    [Route("clients/api/v1/company-tax-settings")]
    public class CompanyTaxSettingsController : ControllerBase
    {
        private readonly ICompanyTaxSettingsService companyTaxSettingsService;
        public CompanyTaxSettingsController(ICompanyTaxSettingsService companyTaxSettingsService)
        {
            this.companyTaxSettingsService = companyTaxSettingsService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Create(CreateCompanyTaxSettingsModel payload)
        {
            var result = await companyTaxSettingsService.Create(payload);

            if (result.StatusCode == 400) return BadRequest(result);
            if (result.StatusCode == 500) return BadRequest(result);
            if (result.StatusCode == 401) return Unauthorized();

            return Ok(result);
        }

        [Route("{guid}")]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Put(string guid, UpdateCompanyTaxSettingsModel payload)
        {
            var isValidGuid = Guid.TryParse(guid, out var id);
            if (!isValidGuid) return BadRequest("Invalid guid");

            var result = await companyTaxSettingsService.Put(payload);

            if (result.StatusCode == 400) return BadRequest(result);
            if (result.StatusCode == 500) return BadRequest(result);
            if (result.StatusCode == 401) return Unauthorized();

            return Ok(result);
        }

    }
}
