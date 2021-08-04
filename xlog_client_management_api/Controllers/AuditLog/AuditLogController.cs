using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace xlog_client_management_api.Controllers.AuditLog
{
    [Route("clients/api/v1")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class AuditLogController : Controller
    {
        public readonly xgca.core.AuditLog.IAuditLogCore _auditLog;
        public AuditLogController(xgca.core.AuditLog.IAuditLogCore auditLog)
        {
            _auditLog = auditLog;
        }

        [Route("logs/{tableName}/{keyFieldId}")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ListAuditLogs([FromRoute] string tableName, string keyFieldId)
        {
            var response = await _auditLog.ListByTableNameAndKeyFieldId(tableName, keyFieldId);

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

        [Route("logs/{tableName}/{keyFieldId}/filter")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetFilteredList(
            [FromQuery] DateTime createdDateFrom,
            [FromQuery] DateTime createdDateTo,
            [FromRoute] string tableName,
            [FromRoute] int keyFieldId,
            [FromQuery] string action = "",
            [FromQuery] string username = "",
            [FromQuery] string orderBy = "CreatedOn",
            [FromQuery] string search = "",
            [FromQuery] int pageNumber = 0,
            [FromQuery] int pageSize = 10)
        {
            var response = await _auditLog.ListPaginate(tableName, keyFieldId, createdDateFrom, createdDateTo, action, username, orderBy, search, pageNumber, pageSize);
            if (response.statusCode == 400) return BadRequest(response);
            if (response.statusCode == 401)return Unauthorized(response);

            return Ok(response);
        }

        [Route("logs/{tableName}/{keyFieldId}/filter/download")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DownloadFiltered(
            [FromQuery] DateTime createdDateFrom,
            [FromQuery] DateTime createdDateTo,
            [FromRoute] string tableName,
            [FromRoute] int keyFieldId,
            [FromQuery] string action = "",
            [FromQuery] string username = "",
            [FromQuery] string orderBy = "CreatedOn",
            [FromQuery] string search = "",
            [FromQuery] int pageNumber = 0,
            [FromQuery] int pageSize = 10,
            [FromQuery] string fileType = "xlsx")
        {
            var response = await _auditLog.DownloadFiltered(tableName, keyFieldId, createdDateFrom, createdDateTo, action, username, orderBy, search, pageNumber, pageSize, fileType);
            return File(response.Bytes, MimeTypes.GetMimeType(response.FileName), response.FileName);
        }

        [Route("logs/details/{auditLogId}")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> AuditLogDetails([FromRoute] string auditLogId)
        {
            var response = await _auditLog.RetrieveDetails(auditLogId);

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

        [Route("logs/{tableName}/{keyFieldId}/download")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<FileResult> DownloadAuditLogs([FromRoute] string tableName, [FromRoute] string keyFieldId, [FromQuery] string fileType = "xlsx")
        {
            var response = await _auditLog.DownloadByTableNameAndKeyFieldId(tableName, keyFieldId, fileType);
            return File(response.Bytes, MimeTypes.GetMimeType(response.FileName), response.FileName);
        }

        [Route("audit-logs")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetCompanyServiceRoleLogs(
            [FromQuery] string type, 
            [FromQuery] string companyServiceGuid = "", 
            [FromQuery] string keyGuid = "")
        {
            var response = await _auditLog.GetCompanyServiceRoleLogs(type, companyServiceGuid, keyGuid);

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

        [Route("audit-logs/acm-group/download")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DownloadCompanyServiceRoleLogs(
            [FromQuery] string type,
            [FromQuery] string companyServiceGuid = "",
            [FromQuery] string keyGuid = "")
        {
            var response = await _auditLog.DownloadCompanyServiceRoleLogs(type, companyServiceGuid, keyGuid);

            var fileName = $"ACMGroupsAuditLog_{DateTime.Now:yyyyMMddhhmmss}.xlsx";

            return File(response, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
    }
}
