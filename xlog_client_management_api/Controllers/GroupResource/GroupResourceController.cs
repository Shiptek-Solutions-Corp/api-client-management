using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using xgca.core.GroupResource;
using xgca.core.Models.GroupResource;

namespace xlog_client_management_api.Controllers
{
    [Route("clients/api/v1/[controller]")]
    [ApiController]
    public class GroupResourceController : Controller
    {
        private readonly IGroupResource _groupResource;
        public GroupResourceController(IGroupResource groupResource)
        {
            _groupResource = groupResource;
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Create([FromBody] CreateGroupResource createGroupResource)
        {
            var result = await _groupResource.Create(createGroupResource);

            if (result.statusCode == 200)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}