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
using xgca.core.Models.CompanyService;
using xgca.core.Models.CompanyServiceUser;

namespace xlog_client_management_api.Controllers.CompanyServiceUser
{
    [ApiExplorerSettings(GroupName = "v1")]
    [Route("clients/api/v1")]
    [ApiController]
    public class CompanyServiceUserController : Controller
    {
        public readonly xgca.core.CompanyServiceUser.ICompanyServiceUser _companyServiceUser;
        public CompanyServiceUserController(xgca.core.CompanyServiceUser.ICompanyServiceUser companyServiceUser)
        {
            _companyServiceUser = companyServiceUser;
        }

        [Route("company/user-roles")]
        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ListCompanyUserByCompanyid()
        {
            var companyId = Request.HttpContext.User.Claims.First(x => x.Type == "custom:companyId").Value;
            var response = await _companyServiceUser.ListUserServiceRolesByCompanyId(Convert.ToInt32(companyId));

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

        [Route("company/{companyId}/user-roles")]
        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ListCompanyUserByCompanyid(string companyId)
        {
            var response = await _companyServiceUser.ListUserServiceRolesByCompanyId(Convert.ToInt32(companyId));

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

        [Route("company-service-user/{companyId}")]
        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetUserWithNoExistingRole(
            string companyId,
            [FromQuery] string groupName = "", 
            [FromQuery] string companyServiceRoleGuid = "", 
            [FromQuery] string companyServiceGuid = "",
            [FromQuery] string fullName = "")
        {
            var response = await _companyServiceUser.ListUserWithNoDuplicateRole(
                companyId, 
                companyServiceRoleGuid, 
                groupName, 
                companyServiceGuid,
                fullName);

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

        [Route("company-service-user/{companyServiceUserId}")]
        [HttpPut]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UpdateCompanyServiceUserRole(string companyServiceUserId, [FromBody] UpdateCompanyServiceUser updateCompanyServiceUser)
        {
            var response = await _companyServiceUser.UpdateCompanyServiceUserRole(companyServiceUserId, updateCompanyServiceUser);

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

        [Route("company-service-user")]
        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreateCompanyServiceUser([FromBody] CreateCompanyServiceUserModel createCompanyServiceUserModel)
        {
            var response = await _companyServiceUser.CreateCompanyServiceUser(createCompanyServiceUserModel);

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
