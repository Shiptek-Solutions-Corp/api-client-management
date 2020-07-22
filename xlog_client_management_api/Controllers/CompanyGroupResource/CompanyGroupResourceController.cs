using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using xgca.core.CompanyGroupResource;
using xgca.core.Models.CompanyGroupResource;

namespace xlog_client_management_api.Controllers.CompanyGroupResource
{
    [Route("clients/api/v1/[controller]")]
    public class CompanyGroupResourceController : Controller
    {
        private readonly ICompanyGroupResource _companyGroupResource;
        public CompanyGroupResourceController(ICompanyGroupResource companyGroupResource)
        {
            _companyGroupResource = companyGroupResource;
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Create([FromBody] CreateCompanyGroupResource createCompanyGroupResource)
        {
            var result = await _companyGroupResource.Create(createCompanyGroupResource);
            if (result.statusCode == 200)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetAll([FromQuery] string companyServiceRoleGuid = "")
        {
            var result = await _companyGroupResource.GetAll(companyServiceRoleGuid);

            if (result.statusCode == 200)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _companyGroupResource.Get(id);

            if (result.statusCode == 200)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("authorization")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetAuthorizationDetails()
        {
            var token = Request.Headers["Authorization"].ToString();
            var username = Request.HttpContext.User.Claims.First(x => x.Type == "cognito:username").Value;
            var result = await _companyGroupResource.GetAuthorizationDetails(username, token);

            if (result.statusCode == 200)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }
    }
}