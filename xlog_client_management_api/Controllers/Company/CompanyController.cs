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
using xgca.core.Company.DTO;
using xgca.core.Models.Company;
using xgca.core.Response;
using xlog_client_management_api;

namespace xlog_company_service_api.Controllers.Company
{
    [Route("clients/api/v1")]
    public class CompanyController : Controller
    {
        public readonly xgca.core.Company.ICompany _company;
        private readonly IGeneral _general;

        public CompanyController(xgca.core.Company.ICompany company, IGeneral general)
        {
            _company = company;
            _general = general;
        }

        [Route("company")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ListCompany()
        {
            var response = await _company.List();

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

        [Route("company/service/{serviceId}")]
        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ListCompany([FromRoute]int serviceId,[FromQuery] string companyName, [FromQuery]int page = 0, [FromQuery]int rows = 10)
        {

            //if (page == 0 || rows == 0)
            //{
            //    return BadRequest(_general.Response(null, 400, "Queryparams 'page' and 'rows' must not be zero", false));
            //}


            var response = await _company.ListByService(serviceId, companyName, page, rows);

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

        [Route("company/details")]
        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer")]
        //[TokenAuthorize("scope", "companyInformation.get")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ViewCompany()
        {
            var companyId = Request.HttpContext.User.Claims.First(x => x.Type == "custom:companyId").Value;
            var response = await _company.Retrieve(Convert.ToInt32(companyId));

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

        [Route("company/{companyId}/details")]
        [HttpGet]
        //[Authorize(AuthenticationSchemes = "Bearer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ViewCompany(string companyId)
        {
            var response = await _company.Retrieve(companyId);

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

        [Route("company/id/{companyId}/details")]
        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ViewCompanyById(int companyId)
        {
            var response = await _company.GetGuidById(companyId);

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


        [Route("company/registration")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> InitialRegistration([FromBody]xgca.core.Models.Company.InitialRegistrationModel request)
        {
            var response = await _company.InitialRegistration(request);

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

        [Route("company/registration/bulk")]
        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> BulkRegistration([FromBody] InitialRegistrationListModel request)
        {
            var username = Request.HttpContext.User.Claims.First(x => x.Type == "cognito:username").Value;
            var response = await _company.BulkCompanyRegistration(request, username);

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

        [Route("company")]
        [HttpPut]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [TokenAuthorize("scope", "companyInformation.put")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UpdateCompany([FromBody] UpdateCompanyModel request)
        {
            var username = Request.HttpContext.User.Claims.First(x => x.Type == "cognito:username").Value;
            var response = await _company.Update(request, username);

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

        [Route("company/{companyId}")]
        [HttpDelete]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DeleteCompany(string companyId)
        {
            var username = Request.HttpContext.User.Claims.First(x => x.Type == "cognito:username").Value;
            var response = await _company.Delete(companyId, username);

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

        [Route("company/{companyId}/id")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetIdByGuid(string companyId)
        {
            var response = await _company.GetIdByGuid(companyId);

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

        [Route("company/profile/logs")]
        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CompanyProfileLogs()
        {
            var companyId = Request.HttpContext.User.Claims.First(x => x.Type == "custom:companyId").Value;
            var response = await _company.ListCompanyLogs(Convert.ToInt32(companyId));

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

        [Route("company/{companyId}/logs")]
        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CompanyLogs([FromRoute] string companyId)
        {
            var response = await _company.ListCompanyLogs(companyId);

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

        [Route("company/reservation-actors")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetReservationActors([FromBody] GetReservationActorsListModel getReservationActorsListModel)
        {
            var response = await _company.GetReservationActors(getReservationActorsListModel.Actors);

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

        [Route("company/list")]
        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ListCompaniesByIDs([FromBody] GetCompanyIDs obj)
        {
            var response = await _company.ListCompaniesByIDs(obj);

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

        [Route("company/list/details")]
        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ListCompanyDetailsByIds([FromBody] GetCompanyIDs obj)
        {
            var response = await _company.ListCompanyDetailsByIds(obj);

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

        [Route("company/accredit")]
        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UpdateAccreditedBy([FromBody]UpdateAccreditedByModel obj)
        {
            var username = Request.HttpContext.User.Claims.First(x => x.Type == "cognito:username").Value;
            var response = await _company.UpdateAccreditedBy(obj, username);

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

        [Route("company/{companyName}/exists")]
        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CheckIfExists([FromRoute] string companyName)
        {
            var response = await _company.CheckIfExistsByCompanyName(companyName);

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

        [Route("company/companyNames")]
        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> BulkCheckIfExistsByCompanyName([FromBody]CompanyNamesDTO companyNamesDTO)
        {
            var response = await _company.BulkCheckIfExistsByCompanyName(companyNamesDTO.CompanyNames);

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

        [Route("company/list")]
        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ListByCompanyName([FromQuery] string companyName)
        {
            var response = await _company.ListByCompanyName(companyName);

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

        [Route("company/set-cucc")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> BatchUpdateCompanyCode([FromBody] UpdateCUCCCodeDTO obj)
        {
            //var username = Request.HttpContext.User.Claims.First(x => x.Type == "cognito:username").Value;
            var response = await _company.SetCUCC(obj);

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

        [Route("company/retrieve-companyName")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> RetrieveCompanyName([FromBody] GetCompanyNamesModel serviceId)
        {
            var response = await _company.RetrieveCompanyName(serviceId);

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

        [Route("company/accreditor")]
        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAccreditor()
        {
            var companyId = Request.HttpContext.User.Claims.First(x => x.Type == "custom:companyId").Value;
            var response = await _company.GetAccreditor(Convert.ToInt32(companyId));

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

        //[Route("company/update-company-code")]
        //[HttpGet]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
        //public async Task<IActionResult> BatchUpdateCompanyCode([FromBody] GetCompanyIDs obj)
        //{
        //    //var username = Request.HttpContext.User.Claims.First(x => x.Type == "cognito:username").Value;
        //    var response = await _company.BatchUpdateCompanyCode("username");

        //    if (response.statusCode == 400)
        //    {
        //        return BadRequest(response);
        //    }
        //    else if (response.statusCode == 401)
        //    {
        //        return Unauthorized(response);
        //    }

        //    return Ok(response);
        //}
    }
}