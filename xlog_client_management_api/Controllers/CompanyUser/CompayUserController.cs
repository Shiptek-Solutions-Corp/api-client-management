using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    }
}