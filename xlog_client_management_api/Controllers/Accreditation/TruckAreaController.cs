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

        /// <summary>
        /// Creation of Truck Area
        /// </summary>
        /// <response code="200">Success</response>  
        /// <response code="400">Error Found!</response>  
        /// <response code="401">Unauthorized!</response>
        /// <response code="500">Internal Server Error!</response>
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

        /// <summary>
        /// Updating of Truck Area
        /// </summary>
        /// <response code="200">Success</response>  
        /// <response code="400">Error Found!</response>  
        /// <response code="401">Unauthorized!</response>
        /// <response code="500">Internal Server Error!</response>
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

        /// <summary>
        /// Listing of Truck Area Per Request
        /// </summary>
        /// <response code="200">Success</response>  
        /// <response code="400">Error Found!</response>  
        /// <response code="401">Unauthorized!</response>
        /// <response code="500">Internal Server Error!</response>
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

        /// <summary>
        /// Deletion for Truck Area
        /// </summary>
        /// <response code="200">Success</response>  
        /// <response code="400">Error Found!</response>  
        /// <response code="401">Unauthorized!</response>
        /// <response code="500">Internal Server Error!</response>
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

        /// <summary>
        /// Multiple Deletion of Truck Area
        /// </summary>
        /// <response code="200">Success</response>  
        /// <response code="400">Error Found!</response>  
        /// <response code="401">Unauthorized!</response>
        /// <response code="500">Internal Server Error!</response>
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
