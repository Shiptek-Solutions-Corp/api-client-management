using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using xgca.core.Models.ModuleGroup;
using xgca.core.ModuleGroup;

namespace xlog_client_management_api.Controllers.ModuleGroup
{
    [Route("clients/api/v1/[controller]")]
    public class ModuleGroupController : Controller
    {
        private readonly IModuleGroup _moduleGroup;
        public ModuleGroupController(IModuleGroup moduleGroup)
        {
            _moduleGroup = moduleGroup;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateModuleGroup createModuleGroup)
        {
            var result = await _moduleGroup.Create(createModuleGroup);
            if (result.statusCode == 200)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}