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
using xas.core._ResponseModel;
using xas.core.accreditation.Request;
using xas.core.CustomerAccreditation.DTO;
using xlog_client_management_api;

namespace xlog_accreditation_service.Controllers.CustomerAccreditation
{
    [Route("accreditation/api/v1")]
    [ApiController]
    public class CustomerAccreditationController : Controller
    {
        private readonly IOptions<ClientToken> _optionsToken;
        private readonly IGeneralResponse _general;
        private readonly IRequestCore _requestCore;

        public CustomerAccreditationController(IOptions<ClientToken> optionsToken, IGeneralResponse general, IRequestCore requestCore)
        {
            _optionsToken = optionsToken;
            _general = general;
            _requestCore = requestCore;
        }

        [HttpPost]
        [Route("company/{serviceRole}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [TokenAuthorize("scope", "accreditationCustomer.post")]
        public async Task<IActionResult> CreateCustomerAccreditation([FromBody]CustomerRegistrationDTO customerRegistrationDTO,[FromRoute]string serviceRole)
        {       
            _optionsToken.Value.GetToken = Request.Headers["Authorization"];
            var companyId = Request.HttpContext.User.Claims.First(x => x.Type == "custom:companyId").Value;
            var username = Request.HttpContext.User.Claims.First(x => x.Type == "cognito:username").Value;
            var serviceRoleId = Request.HttpContext.Request.Headers["x-service-role"];

            var authModel = await _requestCore.CreateCustomerAccreditation(customerRegistrationDTO, Convert.ToInt32(companyId), username, serviceRole, serviceRoleId);
            if (!authModel.IsSuccessful)
            {
                return BadRequest(authModel);
            }

            return Ok(authModel);
        }


        //[HttpPost]
        //[Route("customer/bulk")]
        //[Authorize(AuthenticationSchemes = "Bearer")]
        //[TokenAuthorize("scope", "accreditationCustomer.post")]
        //public async Task<IActionResult> BulkCreateCompanies(IFormFile file)
        //{
        //    var serviceRole = Request.Headers["x-service-role"];
        //    _optionsToken.Value.GetToken = Request.Headers["Authorization"];

        //    var companyId = Request.HttpContext.User.Claims.First(x => x.Type == "custom:companyId").Value;
        //    var username = Request.HttpContext.User.Claims.First(x => x.Type == "cognito:username").Value;
        //    var response = await _auth.BulkCreateCompanies(file, Convert.ToInt32(companyId), username, serviceRole);

        //    if (response.StatusCode == 400) 
        //    {
        //        return BadRequest(response);
        //    }
        //    else if (response.StatusCode == 401)
        //    {
        //        return Unauthorized(response);
        //    }

        //    return Ok(response);
        //}

        //[HttpPost]
        //[Route("customer/{guid}/status")]
        //public async Task<IActionResult> UpdateCustomerAccreditationStatus([FromRoute]string guid)
        //{
        //    var response = await _auth.UpdateCustomerAccreditationStatus(guid, "system");

        //    if (response.StatusCode == 400)
        //    {
        //        return BadRequest(response);
        //    }
        //    else if (response.StatusCode == 401)
        //    {
        //        return Unauthorized(response);
        //    }

        //    return Ok(response);
        //}

        //[HttpDelete]
        //[Route("customer/{guid}")]
        //[Authorize(AuthenticationSchemes = "Bearer")]
        //[TokenAuthorize("scope", "accreditationCustomer.delete")]
        //public async Task<IActionResult> DeleteCustomerAccreditation([FromRoute] string guid)
        //{
        //    var username = Request.HttpContext.User.Claims.First(x => x.Type == "cognito:username").Value;
        //    var response = await _auth.DeleteCustomerAccreditation(guid, username);

        //    if (response.StatusCode == 400)
        //    {
        //        return BadRequest(response);
        //    }
        //    else if (response.StatusCode == 401)
        //    {
        //        return Unauthorized(response);
        //    }

        //    return Ok(response);
        //}

        //[HttpGet]
        //[Route("customer/companies")]
        //[Authorize(AuthenticationSchemes = "Bearer")]
        //[TokenAuthorize("scope", "accreditationCustomer.get")]
        //public async Task<IActionResult> GetAllCompanies([FromQuery] GetCompanyListingDTO data)
        //{
        //    _optionsToken.Value.GetToken = Request.Headers["Authorization"];
        //    var serviceRoleId = Request.HttpContext.Request.Headers["x-service-role"];
        //    var companyId = Request.HttpContext.User.Claims.First(x => x.Type == "custom:companyId").Value;
        //    data.companyId = Convert.ToInt32(companyId);

        //    if (data.ExportTo == "EXCEL")
        //    {
        //        var d = await _customerCore.GetCompanyListing(data, serviceRoleId);
        //        byte[] fInfo = await _customerCore.GenerateExcelFile(d.data.pageResponse);
        //        Stream fStream = new MemoryStream(fInfo);
        //        string mimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        //        return new FileStreamResult(fStream, mimeType)
        //        {
        //            FileDownloadName = "customerAccreditationList.xlsx"
        //        };
        //    }
        //    if (data.ExportTo == "TEMPLATE")
        //    {
        //        byte[] fInfo = await _customerCore.GenerateTemplate();
        //        Stream fStream = new MemoryStream(fInfo);
        //        string mimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        //        return new FileStreamResult(fStream, mimeType)
        //        {
        //            FileDownloadName = "customerAccreditationTEMPLATE.xlsx"
        //        };
        //    }
        //    if (data.ExportTo == "CSV")
        //    {
        //        var d = await _customerCore.GetCompanyListingCSVFormat(data, serviceRoleId);
        //        byte[] fInfo = await _customerCore.GenerateCSVFile(d.data.pageResponse);
        //        Stream fStream = new MemoryStream(fInfo);
        //        string mimeType = "application/octet-stream";

        //        return new FileStreamResult(fStream, mimeType)
        //        {
        //            FileDownloadName = "customerAccreditationList.csv"
        //        };
        //    }
        //    else
        //    {
        //        var d = await _customerCore.GetCompanyListing(data, serviceRoleId);
        //        return Ok(d);
        //    }

            
        //}

        //[HttpPut]
        //[Authorize(AuthenticationSchemes = "Bearer")]
        //[Route("customer/{customerGuid}/approve")]
        //[TokenAuthorize("scope", "accreditationCustomer.put")]
        //public async Task<IActionResult> ApprovedCustomer([FromRoute] string customerGuid)
        //{
        //    var approvedBy = Request.HttpContext.User.Claims.First(x => x.Type == "cognito:username").Value;
        //    var d = await _customerCore.ApproveCustomer(customerGuid, approvedBy);

        //    return Ok(d);
        //}

        //[HttpPut]
        //[Authorize(AuthenticationSchemes = "Bearer")]
        //[Route("customer/{customerGuid}/reject")]
        //[TokenAuthorize("scope", "accreditationCustomer.put")]
        //public async Task<IActionResult> RejectCustomer([FromRoute] string customerGuid)
        //{
        //    var rejectedBy = Request.HttpContext.User.Claims.First(x => x.Type == "cognito:username").Value;
        //    var d = await _customerCore.RejectCustomer(customerGuid, rejectedBy);

        //    return Ok(d);
        //}

        //[HttpPut]
        //[Authorize(AuthenticationSchemes = "Bearer")]
        //[Route("customer/activate")]
        //[TokenAuthorize("scope", "accreditationCustomer.put")]
        //public async Task<IActionResult> BulkActivateCustomer([FromBody] ListCustomerGuidDTO companyGuids)
        //{
        //    var updatedBy = Request.HttpContext.User.Claims.First(x => x.Type == "cognito:username").Value;
        //    var d = await _customerCore.BulkActivateCustomer(companyGuids, updatedBy);

        //    return Ok(d);
        //}

        //[HttpPut]
        //[Authorize(AuthenticationSchemes = "Bearer")]
        //[Route("customer/deactivate")]
        //[TokenAuthorize("scope", "accreditationCustomer.put")]
        //public async Task<IActionResult> BulkDeactivateCustomer([FromBody] ListCustomerGuidDTO companyGuids)
        //{
        //    var updatedBy = Request.HttpContext.User.Claims.First(x => x.Type == "cognito:username").Value;
        //    var d = await _customerCore.BulkDeactivateCustomer(companyGuids, updatedBy);

        //    return Ok(d);
        //}

        //[HttpDelete]
        //[Authorize(AuthenticationSchemes = "Bearer")]
        //[Route("customer/batch")]
        //[TokenAuthorize("scope", "accreditationCustomer.delete")]
        //public async Task<IActionResult> BulkDeleteCustomerAccreditation([FromBody] ListCustomerGuidDTO guids)
        //{
        //    var username = Request.HttpContext.User.Claims.First(x => x.Type == "cognito:username").Value;
        //    var response = await _auth.BulkDeleteCustomerAccreditation(guids, username);

        //    if (response.StatusCode == 400)
        //    {
        //        return BadRequest(response);
        //    }
        //    else if (response.StatusCode == 401)
        //    {
        //        return Unauthorized(response);
        //    }

        //    return Ok(response);
        //}

        //[HttpPut]
        //[Authorize(AuthenticationSchemes = "Bearer")]
        //[Route("customer/approve")]
        //[TokenAuthorize("scope", "accreditationCustomer.put")]
        //public async Task<IActionResult> BulkApproveCustomer([FromBody] ListCustomerGuidDTO companyGuids)
        //{
        //    var approvedBy = Request.HttpContext.User.Claims.First(x => x.Type == "cognito:username").Value;
        //    var d = await _customerCore.BulkApproveCustomer(companyGuids, approvedBy);

        //    return Ok(d);
        //}

        //[HttpPut]
        //[Authorize(AuthenticationSchemes = "Bearer")]
        //[Route("customer/reject")]
        //[TokenAuthorize("scope", "accreditationCustomer.put")]
        //public async Task<IActionResult> BulkRejectCustomer([FromBody] ListCustomerGuidDTO companyGuids)
        //{
        //    var rejectBy = Request.HttpContext.User.Claims.First(x => x.Type == "cognito:username").Value;
        //    var d = await _customerCore.BulkRejectCustomer(companyGuids, rejectBy);

        //    return Ok(d);
        //}

        //[HttpGet]
        //[Route("customer/statistics")]
        //[Authorize(AuthenticationSchemes = "Bearer")]
        //[TokenAuthorize("scope", "accreditationCustomer.get")]
        //public async Task<IActionResult> GetCustomerAccrediationStatistics()
        //{

        //    _optionsToken.Value.GetToken = Request.Headers["Authorization"];
        //    var serviceRoleId = Request.HttpContext.Request.Headers["x-service-role"];
        //    var companyId = Request.HttpContext.User.Claims.First(x => x.Type == "custom:companyId").Value;
        //    var data = await _customerCore.GenerateCustomerAccreditationStatistics(companyId, serviceRoleId);

        //    if(data.statusCode == 400)
        //    {
        //        return BadRequest(data);
        //    }

        //    return Ok(data);
        //}

        //[HttpPost]
        //[Route("customer/accredited/{companyId}")]
        //[Authorize(AuthenticationSchemes = "Bearer")]
        //[TokenAuthorize("scope", "accreditationCustomer.post")]
        //public async Task<IActionResult> GetAccreditedCustomers([FromRoute] string companyId)
        //{
        //    _optionsToken.Value.GetToken = Request.Headers["Authorization"];
        //    var data = await _customerCore.GenerateAccreditedCustomer(companyId);

        //    if (data.statusCode == 400)
        //    {
        //        return BadRequest(data);
        //    }

        //    return Ok(data);
        //}

        //[HttpPost]
        //[Route("customer/accredited/portOfResponsibility")]
        //[Authorize(AuthenticationSchemes = "Bearer")]
        //[TokenAuthorize("scope", "accreditationCustomer.get|bookingReservation.post|serviceRequest.post")]
        //public async Task<IActionResult> GetPortOfResponsibility([FromBody] ListPortOfResponsibility obj)
        //{
        //    _optionsToken.Value.GetToken = Request.Headers["Authorization"];
        //    var data = await _customerCore.PortOfResponsibilityAccreditedCustomer(obj);

        //    if (data.statusCode == 400)
        //    {
        //        return BadRequest(data);
        //    }

        //    return Ok(data);
        //}

        //[HttpGet]
        //[Route("customer/accredited/individual/portOfResponsibility/{companyId}/{portId}")]
        //[Authorize(AuthenticationSchemes = "Bearer")]
        //[TokenAuthorize("scope", "accreditationCustomer.get|bookingReservation.post|serviceRequest.post")]
        //public async Task<IActionResult> GetIndividualPortOfResponsibility([FromRoute] string companyId, string portId)
        //{
        //    _optionsToken.Value.GetToken = Request.Headers["Authorization"];
        //    var data = await _customerCore.GetIndividualPortOfResponsibility(companyId, portId);

        //    if (data.statusCode == 400)
        //    {
        //        return BadRequest(data);
        //    }

        //    return Ok(data);
        //}
    }
}
