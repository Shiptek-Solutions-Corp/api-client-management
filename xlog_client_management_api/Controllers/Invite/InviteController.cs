using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using xgca.core.Invite;
using xgca.core.Models.Invite;

namespace xlog_client_management_api.Controllers.Invite
{
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

        [Route("preferred-providers/send-invite")]
        [TokenAuthorize("scope", "preferredProviders.post")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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

        [Route("preferred-providers/check-invite/{code}")]
        [TokenAuthorize("scope", "preferredProviders.get")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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

        [Route("preferred-providers/accept-invite")]
        [TokenAuthorize("scope", "preferredProviders.post")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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

        [Route("preferred-contacts/send-invite")]
        [TokenAuthorize("scope", "contacts.post")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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

        [Route("preferred-contacts/check-invite/{code}")]
        [TokenAuthorize("scope", "contacts.get")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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

        [Route("preferred-contacts/accept-invite")]
        [TokenAuthorize("scope", "contacts.post")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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
