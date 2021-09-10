using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using xgca.core.Services;
using xgca.core.Models.YourEDI;

namespace xlog_client_management_api.Controllers.YourEdi
{
    [ApiExplorerSettings(GroupName = "v1")]
    [Route("clients/api/v1/your-edi")]
    public class YourEdiController : ControllerBase
    {
        private readonly IYourEDIService _yourEdi;

        public YourEdiController(IYourEDIService yourEdi)
        {
            _yourEdi = yourEdi;
        }

        [Route("actors")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetYourEdiActors([FromBody] YourEdiRequest request)
        {
            var response = await _yourEdi.GetYourEIDActors(request);

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

        [Route("cucc")]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> SetCUCC([FromBody] YourEdiCUCC request)
        {
            var response = await _yourEdi.SetCUCC(request);

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
