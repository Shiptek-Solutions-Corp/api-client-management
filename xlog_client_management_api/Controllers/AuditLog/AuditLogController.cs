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
    public class AuditLogController : Controller
    {
        public readonly xgca.core.AuditLog.IAuditLogCore _auditLog;
        public AuditLogController(xgca.core.AuditLog.IAuditLogCore auditLog)
        {
            _auditLog = auditLog;
        }

        [Route("logs/{tableName}/{keyFieldId}")]
        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ListAuditLogs([FromRoute] string tableName, int keyFieldId)
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

        [Route("logs/details/{auditLogId}")]
        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer")]
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

        [Route("audit-logs")]
        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer")]
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
    }
}
