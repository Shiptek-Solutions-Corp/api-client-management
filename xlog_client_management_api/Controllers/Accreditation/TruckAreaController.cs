using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using xas.core._Helpers.IOptionModels;
using xas.core.TruckArea;
using xas.core.TruckArea.Models;
using xgca.core.Constants;

namespace xlog_accreditation_service.Controllers.TruckArea
{
    [Route("accreditation/api/v1")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class TruckAreaController : Controller
    {
        private readonly ITruckAreaCore _service;

        public TruckAreaController(ITruckAreaCore _service)
        {
            this._service = _service;
        }

        [HttpPost]
        [Route("truck-area")]
        public async Task<IActionResult> CreateTruckArea([FromBody] CreateTruckArea obj)
        {
            GlobalVariables.LoggedInToken = Request.Headers["Authorization"];
            GlobalVariables.LoggedInUsername = Request.HttpContext.User.Claims.First(x => x.Type == "cognito:username").Value.ToString();
            var response = await _service.CreateTruckArea(obj);
            if (!response.isSuccessful)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPut]
        [Route("truck-area")]
        public async Task<IActionResult> UpdateTruckArea([FromBody] UpdateTruckArea obj)
        {
            GlobalVariables.LoggedInToken = Request.Headers["Authorization"];
            GlobalVariables.LoggedInUsername = Request.HttpContext.User.Claims.First(x => x.Type == "cognito:username").Value.ToString();
            var response = await _service.UpdateTruckArea(obj);
            if (!response.isSuccessful)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpGet]
        [Route("truck-area")]
        public async Task<IActionResult> ListTruckAreas([FromQuery] Guid requestGuid,
           [FromQuery] string search = "",
           [FromQuery] string city = "",
           [FromQuery] string state = "",
           [FromQuery] string country = "",
           [FromQuery] string postal = "",
           [FromQuery] string sortBy = "CountryName",
           [FromQuery] string sortOrder = "asc",
           [FromQuery] int pageNumber = 0,
           [FromQuery] int pageSize = 10)
        {
            var response = await _service.List(requestGuid, search, city, state, country, postal, sortBy, sortOrder, pageNumber, pageSize);
            if (response.statusCode == StatusCodes.Status400BadRequest) return BadRequest(response);
            if (response.statusCode == StatusCodes.Status401Unauthorized) return Unauthorized(response);
            return Ok(response);
        }

        [HttpDelete]
        [Route("truck-area/{id}")]
        public async Task<IActionResult> DeleteTruckArea([FromRoute] string id)
        {
            GlobalVariables.LoggedInToken = Request.Headers["Authorization"];
            GlobalVariables.LoggedInUsername = Request.HttpContext.User.Claims.First(x => x.Type == "cognito:username").Value.ToString();
            var response = await _service.Delete(id);
            if (!response.isSuccessful)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
        [HttpDelete]
        [Route("truck-area/multiple")]
        public async Task<IActionResult> DeleteTruckAreas([FromBody] DeleteMultipleTruckArea list)
        {
            GlobalVariables.LoggedInToken = Request.Headers["Authorization"];
            GlobalVariables.LoggedInUsername = Request.HttpContext.User.Claims.First(x => x.Type == "cognito:username").Value.ToString();
            var response = await _service.DeleteMultiple(list);
            if (!response.isSuccessful)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

    }
}
