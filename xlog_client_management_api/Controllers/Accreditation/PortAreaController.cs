using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using xas.core._Helpers.IOptionModels;
using xas.core.PortArea;
using xas.core.PortArea.DTO;
using xas.data._IOptionsModel;
using xgca.core.Models.Accreditation.PortArea;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace xlog_accreditation_service.Controllers.PortArea
{
    [Route("accreditation/api/v1")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class PortAreaController : Controller
    {
        private readonly IPortAreaCore _portAreaCore;
        private readonly IOptions<ClientToken> _optionsToken;
        private readonly IOptions<ClientTokenData> _optionsTokenData;

        public PortAreaController(IPortAreaCore portAreaCore, IOptions<ClientToken> optionsToken, IOptions<ClientTokenData> optionsTokenData)
        {
            _portAreaCore = portAreaCore;
            _optionsToken = optionsToken;
            _optionsTokenData = optionsTokenData;
        }


        /// <summary>
        /// Multiple Add Port Area
        /// </summary>
        /// <response code="200">Success</response>  
        /// <response code="400">Error Found!</response>  
        /// <response code="401">Unauthorized!</response>
        /// <response code="500">Internal Server Error!</response>
        [HttpPost]
        [Route("request/port")]
        public async Task<IActionResult> AddPort([FromBody] List<CreatePortAreaModel> portInfoList)
        {
            var response =  await _portAreaCore.AddPortOfResponsibility(portInfoList);
            if (response.statusCode == StatusCodes.Status400BadRequest) return BadRequest(response);
            if (response.statusCode == StatusCodes.Status401Unauthorized) return Unauthorized(response);
            return Ok(response);
        }

        /// <summary>
        /// Delete Port Area
        /// </summary>
        /// <response code="200">Success</response>  
        /// <response code="400">Error Found!</response>  
        /// <response code="401">Unauthorized!</response>
        /// <response code="500">Internal Server Error!</response>
        [HttpDelete]
        [Route("port/{portAreaId}")]
        public async Task<IActionResult> RemovePort(Guid portAreaId)
        {
            var response = await _portAreaCore.RemovePortResponsibility(portAreaId);
            if (response.statusCode == StatusCodes.Status400BadRequest) return BadRequest(response);
            if (response.statusCode == StatusCodes.Status401Unauthorized) return Unauthorized(response);
            return Ok(response);
        }


        /// <summary>
        /// List of Ports based on RequestId
        /// </summary>
        /// <response code="200">Success</response>  
        /// <response code="400">Error Found!</response>  
        /// <response code="401">Unauthorized!</response>
        /// <response code="500">Internal Server Error!</response>
        [HttpGet]
        [Route("port/{requestId}")]
        public async Task<IActionResult> ListPort([FromRoute]Guid requestId , [FromQuery] string ExportTo= "")
        {
            if (ExportTo.ToUpper() == "EXCEL")
            {
                var Altresponse = await _portAreaCore.GenerateExcelFile(requestId);
                Stream fStream = new MemoryStream(Altresponse);
                string mimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

                return new FileStreamResult(fStream, mimeType)
                {
                    FileDownloadName = "PortListingExcel.xlsx"
                };
            }

            if (ExportTo.ToUpper() == "CSV")
            {
                ////var response = await _portAreaCore.GetListofPorts(requestId);
                byte[] fInfo = await _portAreaCore.GenerateCSVFile(requestId, "CSV");
                Stream fStream = new MemoryStream(fInfo);
                string mimeType = "application/octet-stream";

                return new FileStreamResult(fStream, mimeType)
                {
                    FileDownloadName = "PortListingCSV.csv"
                };
            }

            if (ExportTo.ToUpper() == "TEMPLATE")
            {
                ////var response = await _portAreaCore.GetListofPortsCSVFormat(requestId);
                byte[] fInfo = await _portAreaCore.GenerateCSVFile(requestId,"TEMPLATE");
                Stream fStream = new MemoryStream(fInfo);
                string mimeType = "application/octet-stream";

                return new FileStreamResult(fStream, mimeType)
                {
                    FileDownloadName = "PortListingTemplate.csv"
                };
            }
            else
            {
                var response = await _portAreaCore.GetListofPorts(requestId);
                if (response.statusCode == StatusCodes.Status400BadRequest) return BadRequest(response);
                if (response.statusCode == StatusCodes.Status401Unauthorized) return Unauthorized(response);
                return Ok(response);
            }
        }
    }
}
