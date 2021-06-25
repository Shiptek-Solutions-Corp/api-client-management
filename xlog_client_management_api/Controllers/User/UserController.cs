using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using xgca.core.Models.User;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace xlog_client_management_api.Controllers.User
{
    [Route("clients/api/v1")]
    public class UserController : Controller
    {
        public readonly xgca.core.User.IUser _user;

        public UserController(xgca.core.User.IUser user)
        {
            _user = user;
        }

        [Route("user")]
        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ListUser()
        {
            var response = await _user.List();

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

        [Route("user/filter")]
        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ListUser([FromQuery(Name = "query")] string query)
        {

            var response = await _user.List(query, "1", "0");

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

        [Route("user/{userId}")]
        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ViewUser(string userId)
        {
            var response = await _user.Retrieve(userId);

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

        [Route("user/profile")]
        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer")]
        //[TokenAuthorize("scope", "users.get")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> RetrieveProfile()
        {
            var username = Request.HttpContext.User.Claims.First(x => x.Type == "cognito:username").Value;
            var response = await _user.RetrieveByUsername(username);

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

        [Route("user/{username}/profile")]
        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer")]
        //[TokenAuthorize("scope", "users.get")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> RetrieveProfileByUsernameOld([FromRoute(Name = "username")] string username)
        {
            var response = await _user.RetrieveByUsername(username);

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

        [Route("user/activate-company-user")]
        [HttpPost]
        //[TokenAuthorize("scope", "users.get")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ActivateCompanyUser([FromQuery] string emailAddress, [FromQuery] bool isSendEmail = false)
        {
            string token = Request.Headers["Authorization"].ToString().Remove(0, 7);
            var response = await _user.ActivateCompanyUser(emailAddress, isSendEmail, token);

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

        [Route("user/{emailAddress}/details-by-email")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RetrieveProfileByEmail([FromRoute(Name = "emailAddress")] string emailAddress)
        {
            var response = await _user.RetrieveProfileByEmail(emailAddress);

            if (response.statusCode == 400)
                return BadRequest(response);

            return Ok(response);
        }

        [Route("user/{username}/details")]
        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer")]
        //[TokenAuthorize("scope", "users.get")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> RetrieveProfileByUsername([FromRoute(Name = "username")] string username)
        {
            var response = await _user.RetrieveByUsername(username);

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

        [Route("user")]
        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserModel request)
        {
            var companyId = Request.HttpContext.User.Claims.First(x => x.Type == "custom:companyId").Value;
            var CreatedBy = Request.HttpContext.User.Claims.First(x => x.Type == "cognito:username").Value;
            var authHeader = Request.Headers["Authorization"].ToString();
            var response = await _user.Create(request, companyId, authHeader, CreatedBy);

            if (response.statusCode == 200)
            {
                return Ok(response);
            }
            else if (response.statusCode == 401)
            {
                return Unauthorized(response);
            }

            return BadRequest(response);
        }

        [Route("user")]
        [HttpPut]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [TokenAuthorize("scope", "users.put")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserModel request)
        {
            var modifiedBy = Request.HttpContext.User.Claims.First(x => x.Type == "cognito:username").Value;
            var response = await _user.Update(request, modifiedBy);

            if (response.statusCode == 200)
            {
                return Ok(response);
            }
            else if (response.statusCode == 401)
            {
                return Unauthorized(response);
            }

            return BadRequest(response);
        }

        [Route("user/username")]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> SetUsername([FromBody] SetUsernameModel request)
        {
            var response = await _user.SetUsername(request);

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

        [Route("user/{userId}")]
        [HttpDelete]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [TokenAuthorize("scope", "accessManagementUser.delete")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var modifiedBy = Request.HttpContext.User.Claims.First(x => x.Type == "cognito:username").Value;
            var authHeader = Request.Headers["Authorization"].ToString();
            var response = await _user.Delete(userId, modifiedBy, authHeader);

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

        [Route("user/{userId}/id")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetUserIdByGuid(string userId)
        {
            var response = await _user.GetIdByGuid(userId);

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

        [Route("user/{id}/name")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetUserByReferenceId(int id)
        {
            var response = await _user.GetUserByReferenceId(id);

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


        [Route("user/status")]
        [HttpPut]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [TokenAuthorize("scope", "accessManagementUser.put")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UpdateStatus([FromBody] UpdateUserStatusModel request)
        {
            var modifiedBy = Request.HttpContext.User.Claims.First(x => x.Type == "cognito:username").Value;
            var authHeader = Request.Headers["Authorization"].ToString();
            var response = await _user.UpdateStatus(request, modifiedBy, authHeader);

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

        [Route("user/lock")]
        [HttpPut]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [TokenAuthorize("scope", "accessManagementUser.put")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UpdateLock([FromBody] UpdateUserLockModel request)
        {
            var modifiedBy = Request.HttpContext.User.Claims.First(x => x.Type == "cognito:username").Value;
            var authHeader = Request.Headers["Authorization"].ToString();
            var response = await _user.UpdateLock(request, modifiedBy, authHeader);

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

        [Route("user/delete/multiple")]
        [HttpPut]
        [TokenAuthorize("scope", "accessManagementUser.delete")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UpdateMultipleDelete([FromBody] DeleteMultipleUserModel request)
        {
            var modifiedBy = Request.HttpContext.User.Claims.First(x => x.Type == "cognito:username").Value;
            var authHeader = Request.Headers["Authorization"].ToString();
            var response = await _user.DeleteMultipleUser(request, modifiedBy, authHeader);

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

        [Route("user/lock/multiple")]
        [HttpPut]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UpdateMultipleLock([FromBody] UpdateMultipleLockModel request)
        {
            var modifiedBy = Request.HttpContext.User.Claims.First(x => x.Type == "cognito:username").Value;
            var authHeader = Request.Headers["Authorization"].ToString();
            var response = await _user.UpdateMultipleLock(request, modifiedBy, authHeader);

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

        [Route("user/status/multiple")]
        [HttpPut]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [TokenAuthorize("scope", "users.put")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UpdateMultipleStatus([FromBody] UpdateMultipleStatusModel request)
        {
            var modifiedBy = Request.HttpContext.User.Claims.First(x => x.Type == "cognito:username").Value;
            var authHeader = Request.Headers["Authorization"].ToString();
            var response = await _user.UpdateMultipleStatus(request, modifiedBy, authHeader);

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

        [Route("user/profile/logs")]
        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ListUserLogsFromProfile()
        {
            var username = Request.HttpContext.User.Claims.First(x => x.Type == "cognito:username").Value;
            var response = await _user.ListUserLogs(null, username);

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

        [Route("user/profile/logs/download")]
        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DownloadUserProfileLogs()
        {
            var username = Request.HttpContext.User.Claims.First(x => x.Type == "cognito:username").Value;
            var response = await _user.DownloadUserProfileLogs(username);

            var fileName = $"UserProfileLogs_{DateTime.Now:yyyyMMddhhmmss}.xlsx";

            return File(response, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        [Route("user/{userId}/logs")]
        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ListUserLogs([FromRoute] string userId)
        {
            var response = await _user.ListUserLogs(userId, null);

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

        [Route("user/{userId}/logs/download")]
        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DownloadUserLogs([FromRoute] string userId)
        {
            var response = await _user.DownloadUserLogs(userId, null);

            var fileName = $"UserAuditLog_{DateTime.Now:yyyyMMddhhmmss}.xlsx";

            return File(response, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
    }
}
