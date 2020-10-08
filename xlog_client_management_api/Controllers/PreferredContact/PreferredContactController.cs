using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Bibliography;
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
        [TokenAuthorize("scope", "contacts.get")]
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
        [TokenAuthorize("scope", "contacts.get")]
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
        [TokenAuthorize("scope", "contacts.get")]
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

        [Route("preferred-contacts/{preferredContactId}")]
        [HttpDelete]
        [TokenAuthorize("scope", "contacts.delete")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DeletePreferredContact([FromRoute] string preferredContactId)
        {
            var response = await _preferredContact.DeleteContact(preferredContactId);

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

        [Route("preferred-contacts/quick-search")]
        [HttpGet]
        [TokenAuthorize("scope", "contacts.get")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> QuickSearch([FromQuery] string search, [FromQuery] int pageNumber = 0, [FromQuery] int pageSize = 10, [FromQuery] int totalRecords = 0)
        {
            string companyId = Request.HttpContext.User.Claims.First(x => x.Type == "custom:companyId").Value;
            var response = await _preferredContact.QuickSearch(search, Convert.ToInt32(companyId), pageNumber, pageSize, totalRecords);

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
