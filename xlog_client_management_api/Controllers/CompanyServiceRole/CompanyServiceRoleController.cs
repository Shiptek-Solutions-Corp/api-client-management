using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using xgca.core.CompanyServiceRole;
using xgca.core.Models.CompanyServiceRole;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;

namespace xlog_client_management_api.Controllers.CompanyServiceRole
{
    [Route("clients/api/v1")]
    [ApiController]
    public class CompanyServiceRoleController : Controller
    {
        public readonly ICompanyServiceRole _companyServiceRole;
        public CompanyServiceRoleController(ICompanyServiceRole companyServiceRole)
        {
            _companyServiceRole = companyServiceRole;
        }

        [Route("company/services/role")]
        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer")]
        //[TokenAuthorize("scope", "groupUser.post")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Create([FromBody] CreateCompanyServiceRoleModel createCompanyServiceRoleModel)
        {
            var result = await _companyServiceRole.Create(createCompanyServiceRoleModel);
            if (result.statusCode == 200)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [Route("company/services/role/{companyServiceId}")]
        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer")]
        //[TokenAuthorize("scope", "groupUser.get")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ListCompanyServiceByCompanyId(string companyServiceId)
        {
            var response = await _companyServiceRole.ListByCompanyServiceId(companyServiceId);

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

        [Route("company/services/role/company/{companyId}")]
        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer")]
        //[TokenAuthorize("scope", "groupUser.get")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ListByCompanyID(string companyId, [FromQuery] int status = -1)
        {
            var response = await _companyServiceRole.ListByCompany(companyId, status);

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

        [Route("company-service-role/{companyServiceRoleId}")]
        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer")]
        //[TokenAuthorize("scope", "groupUser.get")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Show(string companyServiceRoleId)
        {
            var isValidGuid = Guid.TryParse(companyServiceRoleId, out var guid);
            if (!isValidGuid) return BadRequest("Invalid id");

            var result = await _companyServiceRole.Show(guid);
            if (result.statusCode == 200)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [Route("company-service-role/{companyServiceRoleId}")]
        [HttpPut]
        [Authorize(AuthenticationSchemes = "Bearer")]
        //[TokenAuthorize("scope", "groupUser.get")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Update([FromBody] UpdateCompanyServiceRoleModel updateCompanyServiceRoleModel, string companyServiceRoleId)
        {
            var isValidGuid = Guid.TryParse(companyServiceRoleId, out var guid);
            if (!isValidGuid) return BadRequest("Invalid id");

            var response = await _companyServiceRole.Update(updateCompanyServiceRoleModel, guid);

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

        [Route("company-service-group-user")]
        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer")]
        //[TokenAuthorize("scope", "groupUser.post")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreateGroupPermissionUser([FromBody] CreateGroupPermissionUserModel createGroupPermissionUserModel)
        {

            var response = await _companyServiceRole.CreateGroupPermissionUser(createGroupPermissionUserModel);

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

        [Route("company-service-role")]
        [HttpPut]
        [Authorize(AuthenticationSchemes = "Bearer")]
        //[TokenAuthorize("scope", "groupUser.put")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> BatchUpdate([FromBody] BatchUpdateCompanyServiceRoleModel batchUpdateCompanyServiceRoleModel)
        {
            var response = await _companyServiceRole.BatchUpdate(batchUpdateCompanyServiceRoleModel);

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