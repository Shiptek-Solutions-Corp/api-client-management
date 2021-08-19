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
        /// Create accreditation request
        /// </summary>
        /// <response code="200">Success</response>  
        /// <response code="400">Error Found!</response>  
        /// <response code="401">Unauthorized!</response>
        /// <response code="500">Internal Server Error!</response>
        [HttpPost] //DONE
        [Route("request")] 
        [TokenAuthorize("scope", "accreditationShippingAgency.post|accreditationShippingLine.post")]
        public async Task<IActionResult> Post([FromBody] List<RequestModel> requestInfo)
        {
            var response = await _requestCore.CreateRequest(requestInfo);
            if (response.statusCode == StatusCodes.Status400BadRequest) return BadRequest(response);
            if (response.statusCode == StatusCodes.Status401Unauthorized) return Unauthorized(response);
            return Ok(response);
        }

        //[HttpGet]
        //[Route("request/{bound}")]
        //[TokenAuthorize("scope", "accreditationShippingAgency.get|accreditationShippingLine.get")]
        //public async Task<IActionResult> GetAllOutGoingRequest([FromRoute]string bound, [FromQuery] GetAccreditationRequestDTO data )
        //{
        //    _optionsToken.Value.GetToken = Request.Headers["Authorization"];
        //    var companyId = Request.HttpContext.User.Claims.First(x => x.Type == "custom:companyId").Value;
        //    data.companyId = Convert.ToInt32(companyId);
        //    // List<ResponseDTO> theList = JsonConvert.DeserializeObject<List<JsonObject>>(d.data["pageResponse"].ToString());

        //    if (data.ExportTo == "EXCEL")
        //    {
        //        var d = await _requestCore.GetAccreditationRequest(bound, data);
        //        byte[] fInfo = await _requestCore.GenerateExcelFile(d.data.pageResponse);
        //        Stream fStream = new MemoryStream(fInfo);
        //        string mimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        //        return new FileStreamResult(fStream, mimeType)
        //        {
        //            FileDownloadName = "companyListExcel.xlsx"
        //        };
        //    }
        //    if (data.ExportTo == "CSV")
        //    {
        //        var d = await _requestCore.GetAccreditationRequestCSVFormat(bound, data);
        //        byte[] fInfo = await _requestCore.GenerateCSVFile(d.data.pageResponse , "CSV");
        //        Stream fStream = new MemoryStream(fInfo);
        //        string mimeType = "application/octet-stream";

        //        return new FileStreamResult(fStream, mimeType)
        //        {
        //            FileDownloadName = "companyListCSV.csv"
        //        };
        //    }
        //    if (data.ExportTo == "TEMPLATE")
        //    {
        //        var d = await _requestCore.GetAccreditationRequestCSVFormat(bound, data);
        //        byte[] fInfo = await _requestCore.GenerateCSVFile(d.data.pageResponse);
        //        Stream fStream = new MemoryStream(fInfo);
        //        string mimeType = "application/octet-stream";

        //        return new FileStreamResult(fStream, mimeType)
        //        {
        //            FileDownloadName = "companyListTemplate.csv"
        //        };
        //    }
        //    else
        //    {
        //        var d = await _requestCore.GetAccreditationRequest(bound, data);
        //        if (d.statusCode == StatusCodes.Status400BadRequest)
        //        {
        //            return BadRequest(d);
        //        }
        //        else
        //        {
        //            return Ok(d);
        //        }
        //    }
        //}


        /// <summary>
        /// Updating of request.  Approved or Rejected
        /// </summary>
        /// <response code="200">Success</response>  
        /// <response code="400">Error Found!</response>  
        /// <response code="401">Unauthorized!</response>
        /// <response code="500">Internal Server Error!</response>
        [HttpPut]
        [Route("request")]
        [TokenAuthorize("scope", "accreditationShippingAgency.put|accreditationShippingLine.put")]
        public async Task<IActionResult> UpdateAccreditationStatus([FromBody] StatusUpdateRequest data)
        {
            _optionsToken.Value.GetToken = Request.Headers["Authorization"];
            var companyId = Request.HttpContext.User.Claims.First(x => x.Type == "custom:companyId").Value;
            var response = await _requestCore.UpdateRequestStatus(Convert.ToInt32(companyId), data.requestId, 
                data.status.ToLower() == "approve" ? 2 : 
                data.status.ToLower() == "reject" ? 3 : 0);

            if (response.statusCode == StatusCodes.Status400BadRequest) return BadRequest(response);
            if (response.statusCode == StatusCodes.Status401Unauthorized) return Unauthorized(response);
            return Ok(response);
        }

        /// <summary>
        /// Bulk updating of request.  Approved or Rejected
        /// </summary>
        /// <response code="200">Success</response>  
        /// <response code="400">Error Found!</response>  
        /// <response code="401">Unauthorized!</response>
        /// <response code="500">Internal Server Error!</response>
        [HttpPut]
        [Route("request/bulk-update")]
        [TokenAuthorize("scope", "accreditationShippingAgency.put|accreditationShippingLine.put")]
        public async Task<IActionResult> UpdateAccreditationStatusBulk([FromBody] Cus_UpdateBulk data)
        {
            _optionsToken.Value.GetToken = Request.Headers["Authorization"];
            var companyId = Request.HttpContext.User.Claims.First(x => x.Type == "custom:companyId").Value;
            var response = await _requestCore.UpdateRequestStatusBulk(Convert.ToInt32(companyId), data.requestId,
                data.status.ToLower() == "approve" ? 2 :
                data.status.ToLower() == "reject" ? 3 : 0);

            if (response.statusCode == StatusCodes.Status400BadRequest) return BadRequest(response);
            if (response.statusCode == StatusCodes.Status401Unauthorized) return Unauthorized(response);
            return Ok(response);
        }


        /// <summary>
        /// Get Status Statitics
        /// </summary>
        /// <response code="200">Success</response>  
        /// <response code="400">Error Found!</response>  
        /// <response code="401">Unauthorized!</response>
        /// <response code="500">Internal Server Error!</response>
        [HttpGet] //DONE
        [Route("request/statistics/{bound}")]
        [TokenAuthorize("scope", "accreditationShippingAgency.get|accreditationShippingLine.get")]
        public async Task<IActionResult> GetStatusStatistics([FromRoute]string bound)
        {
            _optionsToken.Value.GetToken = Request.Headers["Authorization"];
            var companyId = Request.HttpContext.User.Claims.First(x => x.Type == "custom:companyId").Value;
            var response = await _requestCore.GetAccreditationStats(Convert.ToInt32(companyId), bound);

            if (response.statusCode == StatusCodes.Status400BadRequest) return BadRequest(response);
            if (response.statusCode == StatusCodes.Status401Unauthorized) return Unauthorized(response);
            return Ok(response);
        }


        /// <summary>
        /// Single delete of request
        /// </summary>
        /// <response code="200">Success</response>  
        /// <response code="400">Error Found!</response>  
        /// <response code="401">Unauthorized!</response>
        /// <response code="500">Internal Server Error!</response>
        [HttpDelete]
        [Route("request/delete")]
        [TokenAuthorize("scope", "accreditationShippingAgency.delete|accreditationShippingLine.delete")]
        public async Task<IActionResult> DeleteAcccreditationRequest([FromBody]AccreditationDeleteDTO data)
        {
            var response = await _requestCore.DeleteAccreditaitonRequest(data.requestId);

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
        [Route("request/bulk-delete")]
        [TokenAuthorize("scope", "accreditationShippingAgency.delete|accreditationShippingLine.delete")]
        public async Task<IActionResult> DeleteAcccreditationRequestBulk([FromBody] Cus_DeleteBulk data)
        {        
            var response = await _requestCore.DeleteAccreditaitonRequestBulk(data.RequestId);

            if (response.statusCode == StatusCodes.Status400BadRequest) return BadRequest(response);
            if (response.statusCode == StatusCodes.Status401Unauthorized) return Unauthorized(response);
            return Ok(response);
        }


        /// <summary>
        /// Get All Status Statistics
        /// </summary>
        /// <response code="200">Success</response>  
        /// <response code="400">Error Found!</response>  
        /// <response code="401">Unauthorized!</response>
        /// <response code="500">Internal Server Error!</response>
        [HttpGet]//DONE
        [Route("requests/trucking/{bound}")]
        [TokenAuthorize("scope", "accreditationShippingAgency.get|accreditationShippingLine.get")]
        public async Task<IActionResult> GetAllTruckingAccreditationRequest(
            [FromRoute] string bound,
            [FromQuery] string quicksearch = "", 
                        string company = "",  
                        string address = "", 
                        string truckArea = "", 
                        string orderBy = "company", 
                        bool isDescending = false, 
                        int status = 0, 
                        int pageNumber = 0, 
                        int pageSize = 5)
        {
            _optionsToken.Value.GetToken = Request.Headers["Authorization"];
            var serviceRole = Request.HttpContext.Request.Headers["x-service-role"]; 
            var companyId = int.Parse(Request.HttpContext.User.Claims.First(x => x.Type == "custom:companyId").Value);
            var response = await _requestCore.GetTruckingAccreditationRequest(companyId, bound, serviceRole, quicksearch, company, address, truckArea, orderBy, isDescending, status, pageNumber, pageSize);

            if (response.statusCode == StatusCodes.Status400BadRequest) return BadRequest(response);
            if (response.statusCode == StatusCodes.Status401Unauthorized) return Unauthorized(response);
            return Ok(response);
        }

        [HttpGet]
        [Route("requests/trucking/{bound}/csv")]
        [TokenAuthorize("scope", "accreditationShippingAgency.get|accreditationShippingLine.get")]
        public async Task<IActionResult> ExportAllTruckingAccreditationRequest(
            [FromRoute] string bound,
            [FromQuery] string quicksearch = "",
                        string company = "",
                        string address = "",
                        string truckArea = "",
                        string orderBy = "company",
                        bool isDescending = false,
                        int status = 0,
                        int pageNumber = 0,
                        int pageSize = 5)
        {
            _optionsToken.Value.GetToken = Request.Headers["Authorization"];
            var serviceRole = Request.HttpContext.Request.Headers["x-service-role"];
            int companyId = int.Parse(Request.HttpContext.User.Claims.First(x => x.Type == "custom:companyId").Value);

            var fileName = $"Accreditation_ExportListing{bound}.csv";
            var response = await _requestCore.ExportTruckingAccreditationRequest(companyId, bound, serviceRole, quicksearch, company, address, truckArea, orderBy, isDescending, status, pageNumber, pageSize);
            return File(response, "application/octet-stream", fileName);
        }

        [HttpGet]
        [Route("requests/trucking/csv/template")]
        [TokenAuthorize("scope", "accreditationShippingAgency.get|accreditationShippingLine.get")]
        public async Task<IActionResult> ExportAllTruckingAccreditationRequest()
        {
            var fileName = $"Accreditation_ExportListingTemplate.csv";
            var response = await _requestCore.ExportTruckingAccreditationRequestTemplate();
            return File(response, "application/octet-stream", fileName);
        }

        /// <summary>
        /// Create accreditation request
        /// </summary>
        /// <response code="200">Success</response>  
        /// <response code="400">Error Found!</response>  
        /// <response code="401">Unauthorized!</response>
        /// <response code="500">Internal Server Error!</response>
        [HttpGet]//DONE
        [Route("request/trucking/statistics/{bound}")]
        [TokenAuthorize("scope", "accreditationShippingAgency.get|accreditationShippingLine.get")]
        public async Task<IActionResult> GetTruckingStatistics([FromRoute] string bound)
        {
            _optionsToken.Value.GetToken = Request.Headers["Authorization"];
            var serviceRoleId = Request.HttpContext.Request.Headers["x-service-role"];
            var companyId = int.Parse(Request.HttpContext.User.Claims.First(x => x.Type == "custom:companyId").Value);
            var response = await _requestCore.GetAccreditationStats(companyId, bound, serviceRoleId);

            if (response.statusCode == StatusCodes.Status400BadRequest) return BadRequest(response);
            if (response.statusCode == StatusCodes.Status401Unauthorized) return Unauthorized(response);
            return Ok(response);
        }
    }
}
