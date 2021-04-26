using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using xgca.core.Models.PreferredProvider;
using xgca.core.PreferredProvider;

namespace xlog_client_management_api.Controllers.PreferredProvider
{
    [Route("clients/api/v1")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class PreferredProviderController : Controller
    {
        public readonly IPreferredProviderCore _preferredProvider;

        public PreferredProviderController(IPreferredProviderCore preferredProvider)
        {
            _preferredProvider = preferredProvider;
        }

        [Route("company/details/preferred-providers")]
        [HttpGet]
        [TokenAuthorize("scope", "preferredProviders.get")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CompanyPreferredProvider([FromQuery] string filters, [FromQuery] string sortBy = null, [FromQuery] string sortOrder = "asc", [FromQuery] int pageNumber = 0, [FromQuery] int pageSize = 50, [FromQuery] int totalRecords = 0)
        {
            string companyId = Request.HttpContext.User.Claims.First(x => x.Type == "custom:companyId").Value;
            var response = await _preferredProvider.List(Convert.ToInt32(companyId), filters, sortBy, sortOrder, pageNumber, pageSize, totalRecords);

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

        [Route("company/details/preferred-providers/{preferredProviderId}")]
        [HttpDelete]
        [TokenAuthorize("scope", "preferredProviders.delete")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DeletePreferredProvider([FromRoute] string preferredProviderId)
        {
            var username = Request.HttpContext.User.Claims.First(x => x.Type == "cognito:username").Value;
            var response = await _preferredProvider.DeleteProvider(preferredProviderId, username);

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

        [Route("company/details/preferred-providers/quick-search")]
        [HttpGet]
        [TokenAuthorize("scope", "preferredProviders.get")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CompanyPreferredProvider([FromQuery] string search, [FromQuery] string filters, [FromQuery] string sortBy = null, [FromQuery] string sortOrder = "asc", [FromQuery(Name = "pageNumber")] int pageNumber = 0, [FromQuery(Name = "pageSize")] int pageSize = 50, [FromQuery(Name = "totalRecords")] int totalRecords = 0)
        {
            string companyId = Request.HttpContext.User.Claims.First(x => x.Type == "custom:companyId").Value;
            var response = await _preferredProvider.QuickSearch(search, Convert.ToInt32(companyId), filters, sortBy, sortOrder, pageNumber, pageSize, totalRecords);

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

        [Route("company/details/preferred-providers")]
        [HttpPost]
        [TokenAuthorize("scope", "preferredProviders.post")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> AddPreferredProvider([FromBody] BatchCreatePreferredProvider providers)
        {
            string companyId = Request.HttpContext.User.Claims.First(x => x.Type == "custom:companyId").Value;
            var username = Request.HttpContext.User.Claims.First(x => x.Type == "cognito:username").Value;
            var response = await _preferredProvider.AddPreferredProviders(providers, Convert.ToInt32(companyId), username);

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
