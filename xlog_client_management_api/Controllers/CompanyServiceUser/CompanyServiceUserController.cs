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

namespace xlog_client_management_api.Controllers.CompanyServiceUser
{
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
    }
}
