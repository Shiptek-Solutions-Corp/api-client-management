using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Create([FromBody] CreateModuleGroup createModuleGroup)
        {
            var result = await _moduleGroup.Create(createModuleGroup);
            if (result.statusCode == 200)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _moduleGroup.GetAll();

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
            var result = await _moduleGroup.Get(id);

            if (result.statusCode == 200)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}