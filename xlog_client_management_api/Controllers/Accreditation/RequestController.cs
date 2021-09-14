using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using xas.core._Helpers.IOptionModels;
using xas.core.accreditation.Request;
using xas.core.Request;
using xlog_accreditation_service.Controllers.AccreditationRequest.DTO;
using xas.core.Request.DTO;
using System.IO;
using Newtonsoft.Json;
using xlog_client_management_api;

namespace xlog_accreditation_service.Controllers
{
    [ApiExplorerSettings(GroupName = "v2")]
    [Route("accreditation/api/v1")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class RequestController : Controller
    {
        private readonly IRequestCore _requestCore;
        private readonly IOptions<ClientToken> _optionsToken;

        public RequestController(IRequestCore requestCore, IOptions<ClientToken> optionsToken)
        {
            _requestCore = requestCore;
            _optionsToken = optionsToken;
        }


        /// <summary>
        /// Get List of Request
        /// </summary>
        /// <response code="200">Success</response>  
        /// <response code="400">Error Found!</response>  
        /// <response code="401">Unauthorized!</response>
        /// <response code="500">Internal Server Error!</response>
        [HttpGet]
        [Route("request")]
        [TokenAuthorize("scope", "accreditationShippingAgency.get|accreditationShippingLine.get")]
        public async Task<IActionResult> GetRequestList(DateTime requestedDate, Guid loginCompanyGuid, Guid loginServiceRoleGuid, Guid serviceRoleGuid, string bound = "INCOMING", int pageSize = 10, int pageNumber = 0
                                                        , string companyName = "", string companyAddress = "", string companyCountryName = "", string companyStateCityName = "", string portAreaResponsibility = ""
                                                        , string portAreaOperatingCountryName = "", string truckAreaResponsibility = "", int accreditationStatusConfigId = 0, byte? companyStatus = null
                                                        , string sortOrder = "asc", string sortBy = "RequestId", string quickSearch = "")
        {
            var response = await _requestCore.GetRequestList(bound, pageSize, pageNumber, loginCompanyGuid, loginServiceRoleGuid, serviceRoleGuid, companyName, companyAddress, companyCountryName, companyStateCityName, portAreaResponsibility, portAreaOperatingCountryName, truckAreaResponsibility, accreditationStatusConfigId ,companyStatus, sortOrder, sortBy, quickSearch, requestedDate);

            if (response.statusCode == StatusCodes.Status400BadRequest) return BadRequest(response);
            if (response.statusCode == StatusCodes.Status401Unauthorized) return Unauthorized(response);
            return Ok(response);
        }

        /// <summary>
        /// Create accreditation request
        /// </summary>
        /// <response code="200">Success</response>  
        /// <response code="400">Error Found!</response>  
        /// <response code="401">Unauthorized!</response>
        /// <response code="500">Internal Server Error!</response>
        [HttpPost]
        [Route("request")] 
        [TokenAuthorize("scope", "accreditationShippingAgency.post|accreditationShippingLine.post")]
        public async Task<IActionResult> Post([FromBody] List<RequestModel> requestInfo)
        {
            var response = await _requestCore.CreateRequest(requestInfo);
            if (response.statusCode == StatusCodes.Status400BadRequest) return BadRequest(response);
            if (response.statusCode == StatusCodes.Status401Unauthorized) return Unauthorized(response);
            return Ok(response);
        }

        /// <summary>
        /// Multiple delete of request
        /// </summary>
        /// <response code="200">Success</response>  
        /// <response code="400">Error Found!</response>  
        /// <response code="401">Unauthorized!</response>
        /// <response code="500">Internal Server Error!</response>
        [HttpDelete]
        [Route("request")]
        [TokenAuthorize("scope", "accreditationShippingAgency.delete|accreditationShippingLine.delete")]
        public async Task<IActionResult> DeleteAcccreditationRequest([FromBody] List<Guid> requestIds)
        {
            var response = await _requestCore.DeleteAccreditaitonRequest(requestIds);

            if (response.statusCode == StatusCodes.Status400BadRequest) return BadRequest(response);
            if (response.statusCode == StatusCodes.Status401Unauthorized) return Unauthorized(response);
            return Ok(response);
        }


        /// <summary>
        /// Bulk updating of request status.  approved or rejected
        /// </summary>
        /// <response code="200">Success</response>  
        /// <response code="400">Error Found!</response>  
        /// <response code="401">Unauthorized!</response>
        /// <response code="500">Internal Server Error!</response>
        [HttpPut]
        [Route("request")]
        [TokenAuthorize("scope", "accreditationShippingAgency.put|accreditationShippingLine.put")]
        public async Task<IActionResult> UpdateAccreditationStatusBulk([FromBody] Cus_UpdateBulk data)
        {
            _optionsToken.Value.GetToken = Request.Headers["Authorization"];
            var companyId = Request.HttpContext.User.Claims.First(x => x.Type == "custom:companyId").Value;
            var response = await _requestCore.UpdateRequestStatusBulk(Convert.ToInt32(companyId), data.requestId,
                data.status.ToLower() == "approved" ? 2 :
                data.status.ToLower() == "rejected" ? 3 : 0);

            if (response.statusCode == StatusCodes.Status400BadRequest) return BadRequest(response);
            if (response.statusCode == StatusCodes.Status401Unauthorized) return Unauthorized(response);
            return Ok(response);
        }

        /// <summary>
        /// Activate Deactivate list of Request
        /// </summary>
        /// <response code="200">Success</response>  
        /// <response code="400">Error Found!</response>  
        /// <response code="401">Unauthorized!</response>
        /// <response code="500">Internal Server Error!</response>
        [HttpPut]
        [Route("request/activate-deactivate")]
        public async Task<IActionResult> ActivateDeactivateRequest([FromBody] List<Guid> requestIds, [FromQuery] bool status)
        {
            var response = await _requestCore.ActivateDeactivateRequest(requestIds, status);

            if (response.statusCode == StatusCodes.Status400BadRequest) return BadRequest(response);
            if (response.statusCode == StatusCodes.Status401Unauthorized) return Unauthorized(response);
            return Ok(response);
        }

        /// <summary>
        /// Get Status Statitics Dashboard
        /// </summary>
        /// <response code="200">Success</response>  
        /// <response code="400">Error Found!</response>  
        /// <response code="401">Unauthorized!</response>
        /// <response code="500">Internal Server Error!</response>
        [HttpGet] 
        [Route("request/statistics")]
        [TokenAuthorize("scope", "accreditationShippingAgency.get|accreditationShippingLine.get")]
        public async Task<IActionResult> GetStatusStatistics([FromQuery] string bound, [FromQuery] Guid loginCompanyGuid, [FromQuery] Guid loginServiceRoleGuid, [FromQuery] Guid serviceRoleGuid)
        {         
            var response = await _requestCore.GetAccreditationStats(bound, loginCompanyGuid, loginServiceRoleGuid, serviceRoleGuid);

            if (response.statusCode == StatusCodes.Status400BadRequest) return BadRequest(response);
            if (response.statusCode == StatusCodes.Status401Unauthorized) return Unauthorized(response);
            return Ok(response);
        }

 
        /// <summary>
        /// Export list of Request to CSV
        /// </summary>
        /// <response code="200">Success</response>  
        /// <response code="400">Error Found!</response>  
        /// <response code="401">Unauthorized!</response>
        /// <response code="500">Internal Server Error!</response>
        [HttpGet]
        [Route("requests/export/csv")]
        [TokenAuthorize("scope", "accreditationShippingAgency.get|accreditationShippingLine.get")]
        public async Task<IActionResult> ExportToCSV(DateTime requestedDate, Guid loginCompanyGuid, Guid loginServiceRoleGuid, Guid serviceRoleGuid, string bound = "INCOMING", int pageSize = 10, int pageNumber = 0
                                                    , string companyName = "", string companyAddress = "", string companyCountryName = "", string companyStateCityName = "", string portAreaResponsibility = ""
                                                    , string portAreaOperatingCountryName = "", string truckAreaResponsibility = "", int accreditationStatusConfigId = 0, byte? companyStatus = null
                                                    , string sortOrder = "asc", string sortBy = "RequestId", string quickSearch = "", string viewerServiceRoleId = "")
        {
            var response = await _requestCore.ExportRequestListToCSV(bound, pageSize, pageNumber, loginCompanyGuid, loginServiceRoleGuid, serviceRoleGuid, companyName, companyAddress, companyCountryName, companyStateCityName, portAreaResponsibility, portAreaOperatingCountryName, truckAreaResponsibility, accreditationStatusConfigId, companyStatus, sortOrder, sortBy, quickSearch, viewerServiceRoleId, requestedDate);
            var fileName = $"request_exportCSV.csv";
            return File(response, "application/octet-stream", fileName);
        }


        /// <summary>
        /// Get Accredited Trucking Companies
        /// </summary>
        /// <response code="200">Success</response>  
        /// <response code="400">Error Found!</response>  
        /// <response code="401">Unauthorized!</response>
        /// <response code="500">Internal Server Error!</response>
        [HttpGet]
        [Route("request-accredited-trucking-companies")]
        [TokenAuthorize("scope", "accreditationShippingAgency.put|accreditationShippingLine.put")]
        public async Task<IActionResult> GetAccreditedTruckingCompanies([FromQuery] Guid companyGuid)
        {
            var response = await _requestCore.GetAccreditedTruckingCompanies(companyGuid);
            if (response.statusCode == StatusCodes.Status400BadRequest) return BadRequest(response);
            if (response.statusCode == StatusCodes.Status401Unauthorized) return Unauthorized(response);
            return Ok(response);
        }
    }
}
