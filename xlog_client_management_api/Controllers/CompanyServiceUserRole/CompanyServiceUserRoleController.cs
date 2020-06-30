using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using xgca.core.CompanyServiceUserRole;
using xgca.core.Models.CompanyServiceUserRole;

namespace xlog_client_management_api.Controllers.CompanyServiceUserRole
{
    [Route("clients/api/v1/[controller]")]
    public class CompanyServiceUserRoleController : Controller
    {
        private readonly ICompanyServiceUserRole _companyServiceUserRole;
        public CompanyServiceUserRoleController(ICompanyServiceUserRole companyServiceUserRole)
        {
            _companyServiceUserRole = companyServiceUserRole;
        }
        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Create([FromBody] CreateCompanyServiceUserRole createCompanyServiceUser)
        {
            var result = await _companyServiceUserRole.Create(createCompanyServiceUser);
            if (result.statusCode == 200)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}