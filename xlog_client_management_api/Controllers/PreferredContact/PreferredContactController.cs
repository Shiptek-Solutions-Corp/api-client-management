using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using xgca.core.PreferredContact;

namespace xlog_client_management_api.Controllers.PreferredContact
{
    [Route("clients/api/v1")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class PreferredContactController : Controller
    {
        public readonly IPreferredContactCore _preferredContact;

        public PreferredContactController(IPreferredContactCore preferredContact)
        {
            _preferredContact = preferredContact;
        }

        [Route("company/details/preferred-contacts")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CompanyPreferredContacts([FromQuery(Name = "pageNumber")] int pageNumber = 0, [FromQuery(Name = "pageSize")] int pageSize = 50)
        {
            string companyId = Request.HttpContext.User.Claims.First(x => x.Type == "custom:companyId").Value;
            var response = await _preferredContact.List(Convert.ToInt32(companyId), pageNumber, pageSize);

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

        [Route("company/{companyId}/details/preferred-contacts")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CompanyPreferredContacts(string companyId, [FromQuery(Name = "pageNumber")] int pageNumber = 0, [FromQuery(Name = "pageSize")] int pageSize = 50)
        {
            var response = await _preferredContact.List(companyId, pageNumber, pageSize);

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
        [Route("preferred-contacts/{preferredContactId}/details")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> PreferredConotactDetails([FromRoute] string preferredContactId)
        {
            var response = await _preferredContact.ShowDetails(preferredContactId);

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
