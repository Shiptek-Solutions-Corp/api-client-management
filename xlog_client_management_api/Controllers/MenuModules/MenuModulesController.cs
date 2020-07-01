using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using xgca.core.MenuModule;
using xgca.core.Models.MenuModule;

namespace xlog_client_management_api.Controllers.MenuModules
{
    [Route("clients/api/v1/[controller]")]
    [ApiController]
    public class MenuModulesController : Controller
    {
        private readonly IMenuModule _menuModule;
        public MenuModulesController(IMenuModule menuModule)
        {
            _menuModule = menuModule;
        }
        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer")]

        public async Task<IActionResult> Create([FromBody] CreateMenuModule createMenuModules)
        {
            var result = await _menuModule.Create(createMenuModules);
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
            var result = await _menuModule.Get(id);

            if (result.statusCode == 200)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet()]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _menuModule.GetAll();

            if (result.statusCode == 200)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}