using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using xgca.core.Invite;
using xgca.core.Models.Invite;
using xgca.core.Response;

namespace xlog_client_management_api.Controllers.Invite
{
    [ApiExplorerSettings(GroupName = "v1")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("clients/api/v1")]
    public class InviteController : Controller
    {
        private readonly IInviteCore _invite;

        public InviteController(IInviteCore invite)
        {
            _invite = invite;
        }

        /****** Invite Preferred Providers ******/

        /// <summary>
        /// Providers Send invite
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="400">An Error Occured</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(typeof(IGeneralModel), 200)]
        [Route("preferred-providers/send-invite")]
        [TokenAuthorize("scope", "preferredProviders.post")]
        [HttpPost]
        public async Task<IActionResult> SendProviderInvite([FromBody] ListReceiverEmails emails)
        {
            var companyId = Request.HttpContext.User.Claims.First(x => x.Type == "custom:companyId").Value;
            var response = await _invite.SendProvidersInvites(emails, Convert.ToInt32(companyId));
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
        /// Providers Check invite by code
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="400">An Error Occured</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(typeof(IGeneralModel), 200)]
        [Route("preferred-providers/check-invite/{code}")]
        [TokenAuthorize("scope", "preferredProviders.get")]
        [HttpGet]
        public async Task<IActionResult> CheckProviderInvite([FromRoute] string code)
        {
            var response = await _invite.CheckInviteCode(code, 2);
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
        /// Providers Accept Invite
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="400">An Error Occured</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(typeof(IGeneralModel), 200)]
        [Route("preferred-providers/accept-invite")]
        [TokenAuthorize("scope", "preferredProviders.post")]
        [HttpPost]
        public async Task<IActionResult> AcceptProviderInvite([FromBody] AcceptInviteCode accept)
        {
            var companyId = Request.HttpContext.User.Claims.First(x => x.Type == "custom:companyId").Value;
            var username = Request.HttpContext.User.Claims.First(x => x.Type == "cognito:username").Value;
            var response = await _invite.AcceptProvidersInvite(accept, Convert.ToInt32(companyId), username);
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

        /****** Invite Contacts ******/

        /// <summary>
        /// Contacts Send invite
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="400">An Error Occured</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(typeof(IGeneralModel), 200)]
        [Route("preferred-contacts/send-invite")]
        [TokenAuthorize("scope", "contacts.post")]
        [HttpPost]
        public async Task<IActionResult> SendContactInvite([FromBody] ListReceiverEmails emails)
        {
            var companyId = Request.HttpContext.User.Claims.First(x => x.Type == "custom:companyId").Value;
            var response = await _invite.SendContactsInvites(emails, Convert.ToInt32(companyId));
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
        /// Contacts Check invite by code
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="400">An Error Occured</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(typeof(IGeneralModel), 200)]
        [Route("preferred-contacts/check-invite/{code}")]
        [TokenAuthorize("scope", "contacts.get")]
        [HttpGet]
        public async Task<IActionResult> CheckContactInvite([FromRoute] string code)
        {
            var response = await _invite.CheckInviteCode(code, 1);
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
        /// Contacts Accept Invite
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="400">An Error Occured</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(typeof(IGeneralModel), 200)]
        [Route("preferred-contacts/accept-invite")]
        [TokenAuthorize("scope", "contacts.post")]
        [HttpPost]
        public async Task<IActionResult> AcceptContactInvite([FromBody] AcceptInviteCode accept)
        {
            var companyId = Request.HttpContext.User.Claims.First(x => x.Type == "custom:companyId").Value;
            var username = Request.HttpContext.User.Claims.First(x => x.Type == "cognito:username").Value;
            var response = await _invite.AcceptContactsInvite(accept, Convert.ToInt32(companyId), username);
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
