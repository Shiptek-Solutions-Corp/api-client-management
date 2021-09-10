using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using xgca.core.Models.Guest;
using xgca.core.Response;

namespace xlog_client_management_api.Controllers.Guest
{
    [ApiExplorerSettings(GroupName = "v1")]
    [Route("clients/api/v1")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class GuestController : ControllerBase
    {
        public readonly xgca.core.Guest.IGuestCore _guest;
        public GuestController(xgca.core.Guest.IGuestCore guest)
        {
            _guest = guest;
        }


        /// <summary>
        /// List all Guest
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="400">An Error Occured</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(typeof(IGeneralModel), 200)]
        [Route("guest")]
        [HttpGet]
        public async Task<IActionResult> ListGuests()
        {
            var response = await _guest.List();

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
        /// Register Guest
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="400">An Error Occured</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(typeof(IGeneralModel), 200)]
        [Route("guest/register")]
        [TokenAuthorize("scope", "contacts.post")]
        [HttpPost]
        public async Task<IActionResult> RegisterGuest([FromBody] CreateGuest obj)
        {
            var username = Request.HttpContext.User.Claims.First(x => x.Type == "cognito:username").Value;
            var companyId = Request.HttpContext.User.Claims.First(x => x.Type == "custom:companyId").Value;
            var response = await _guest.Create(obj, username, Convert.ToInt32(companyId));

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
        /// Get Guest by Id
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="400">An Error Occured</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(typeof(IGeneralModel), 200)]
        [Route("guest/{guestId}")]
        [HttpGet]
        public async Task<IActionResult> GuestDetails([FromRoute] int guestId)
        {
            var response = await _guest.ShowDetails(guestId);

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
        /// Get Guest's details by id
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="400">An Error Occured</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(typeof(IGeneralModel), 200)]
        [Route("guest/{guestId}/details")]
        [HttpGet]
        public async Task<IActionResult> GuestDetails([FromRoute] string guestId)
        {
            var response = await _guest.ShowDetails(guestId);

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
        /// Update Guest
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="400">An Error Occured</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(typeof(IGeneralModel), 200)]
        [Route("guest")]
        [HttpPut]
        public async Task<IActionResult> UpdateGuest([FromBody] UpdateGuest obj)
        {
            var username = Request.HttpContext.User.Claims.First(x => x.Type == "cognito:username").Value;
            var companyId = Request.HttpContext.User.Claims.First(x => x.Type == "custom:companyId").Value;
            var response = await _guest.Update(obj, username, Convert.ToInt32(companyId));

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
        /// Update guest contact details
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="400">An Error Occured</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(typeof(IGeneralModel), 200)]
        [Route("preferred-contacts/guest")]
        [TokenAuthorize("scope", "contacts.put")]
        [HttpPut]
        public async Task<IActionResult> UpdateGuestContact([FromBody] UpdateGuestContact obj)
        {
            var username = Request.HttpContext.User.Claims.First(x => x.Type == "cognito:username").Value;
            var companyId = Request.HttpContext.User.Claims.First(x => x.Type == "custom:companyId").Value;
            var response = await _guest.Update(obj, username, Convert.ToInt32(companyId));

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
        /// Delete Guest
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="400">An Error Occured</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(typeof(IGeneralModel), 200)]
        [Route("guest/{guestId}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteGuest([FromRoute] string guestId)
        {
            var username = Request.HttpContext.User.Claims.First(x => x.Type == "cognito:username").Value;
            var response = await _guest.Delete(guestId, username);

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
