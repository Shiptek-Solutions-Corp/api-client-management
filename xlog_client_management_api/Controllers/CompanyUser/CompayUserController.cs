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


namespace xlog_client_management_api.Controllers.CompanyUser
{
    [Route("clients/api/v1")]
    [ApiController]
    public class CompayUserController : Controller
    {

        public readonly xgca.core.CompanyUser.ICompanyUser _companyUser;
        public CompayUserController(xgca.core.CompanyUser.ICompanyUser companyUser)
        {
            _companyUser = companyUser;
        }

        [Route("company/{companyId}/users")]
        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ListCompanyUserByCompanyid(string companyId)
        {
            var response = await _companyUser.ListByCompanyId(companyId);

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

        [Route("company/users")]
        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer")]
        //[TokenAuthorize("scope", "companyInformation.get")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ListCompanyUsersByCompanyId()
        {
            var companyId = Request.HttpContext.User.Claims.First(x => x.Type == "custom:companyId").Value;
            var response = await _companyUser.ListByCompanyId(Convert.ToInt32(companyId));

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

        [Route("company/users/filter")]
        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer")]
        //[TokenAuthorize("scope", "companyInformation.get")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ListCompanyUsersByCompanyIdFilter([FromQuery(Name = "query")] string query)
        {
            var companyId = Request.HttpContext.User.Claims.First(x => x.Type == "custom:companyId").Value;
            var response = await _companyUser.ListByCompanyIdAndFilter(query, Convert.ToInt32(companyId));

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

        [Route("company/users/download")]
        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer")]
        //[TokenAuthorize("scope", "companyInformation.get")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> downloadUsersExcel([FromQuery(Name = "query")] string query)
        {
            var companyId = Request.HttpContext.User.Claims.First(x => x.Type == "custom:companyId").Value;
            var response = await _companyUser.DownloadUsersExcel(query, Convert.ToInt32(companyId));
            var fileName = $"Users_{DateTime.Now:yyyyMMddhhmmss}.xlsx";

            return File(response, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
    }
}