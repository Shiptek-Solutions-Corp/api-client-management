using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Bibliography;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using xgca.core.PreferredContact;
using xgca.core.Response;

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
        /// <summary>
        /// Get all preferred contacts by company with filtering
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="400">An Error Occured</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(typeof(IGeneralModel), 200)]
        [Route("company/details/preferred-contacts")]
        [HttpGet]
        [TokenAuthorize("scope", "contacts.get")]
        public async Task<IActionResult> CompanyPreferredContacts([FromQuery(Name = "search")] string search, [FromQuery(Name = "name")] string name, [FromQuery(Name = "country")] string country, [FromQuery(Name = "stateCity")] string stateCity, [FromQuery(Name = "type")] int type, [FromQuery(Name = "contact")] string contact, [FromQuery(Name = "sortBy")] string sortBy, [FromQuery(Name = "sortOrder")] string sortOrder, [FromQuery(Name = "pageNumber")] int pageNumber = 0, [FromQuery(Name = "pageSize")] int pageSize = 50)
        {
            string companyId = Request.HttpContext.User.Claims.First(x => x.Type == "custom:companyId").Value;
            var response = await _preferredContact.List(Convert.ToInt32(companyId), search, name, country, stateCity, type, contact, sortBy, sortOrder, pageNumber, pageSize);

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
        /// <summary>
        /// Get preferred contacts by company
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="400">An Error Occured</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(typeof(IGeneralModel), 200)]
        [Route("company/{companyId}/details/preferred-contacts")]
        [HttpGet]
        [TokenAuthorize("scope", "contacts.get")]
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
        /// <summary>
        /// Get preferred contacts details
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="400">An Error Occured</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(typeof(IGeneralModel), 200)]
        [Route("preferred-contacts/{preferredContactId}/details")]
        [HttpGet]
        [TokenAuthorize("scope", "contacts.get")]
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
        /// <summary>
        /// Delete preferred contacts
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="400">An Error Occured</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(typeof(IGeneralModel), 200)]
        [Route("preferred-contacts/{preferredContactId}")]
        [HttpDelete]
        [TokenAuthorize("scope", "contacts.delete")]
        public async Task<IActionResult> DeletePreferredContact([FromRoute] string preferredContactId)
        {
            var username = Request.HttpContext.User.Claims.First(x => x.Type == "cognito:username").Value;
            var response = await _preferredContact.DeleteContact(preferredContactId, username);

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
        /// <summary>
        /// Get all preferred contacts with filtering
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="400">An Error Occured</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(typeof(IGeneralModel), 200)]
        [Route("preferred-contacts/quick-search")]
        [HttpGet]
        [TokenAuthorize("scope", "contacts.get")]
        public async Task<IActionResult> QuickSearch([FromQuery] string search, [FromQuery] int pageNumber = 0, [FromQuery] int pageSize = 10, [FromQuery] int totalRecords = 0, [FromQuery] int serviceRoleGroup = (int)xgca.core.Enums.ServiceRoleGroup.All)
        {
            string companyId = Request.HttpContext.User.Claims.First(x => x.Type == "custom:companyId").Value;
            var response = await _preferredContact.QuickSearch(search, Convert.ToInt32(companyId), pageNumber, pageSize, totalRecords, serviceRoleGroup);

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
