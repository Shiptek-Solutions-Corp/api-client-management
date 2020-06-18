using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace xlog_client_management_api.Controllers.Profile
{
    [Route("clients/api/v1")]
    public class ProfileController : Controller
    {
        public readonly xgca.core.Profile.IProfile _profile;

        public ProfileController(xgca.core.Profile.IProfile profile)
        {
            _profile = profile;
        }

        [Route("profile/service/{companyServiceKey}")]
        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ListUser([FromRoute]string companyServiceKey)
        {
            var username = Request.HttpContext.User.Claims.First(x => x.Type == "cognito:username").Value;
            var response = await _profile.LoadProfile(username, companyServiceKey);

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
