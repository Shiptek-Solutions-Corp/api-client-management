using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using xgca.core.Constants;
using xgca.core.Services;
using xgca.core.Models.CompanySection;
using Microsoft.AspNetCore.Authorization;

namespace xlog_client_management_api.Controllers
{
    [Route("clients/api/v1/kyc")]
    public class KYCController : Controller
    {
        private readonly ICompanySectionService _companySectionService;
        private readonly IDocumentTypeService _documentTypeService;

        public KYCController(ICompanySectionService _companySectionService, IDocumentTypeService _documentTypeService)
        {
            this._companySectionService = _companySectionService;
            this._documentTypeService = _documentTypeService;
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [Route("company-sections")]
        [HttpPost]
        public async Task<IActionResult> CreateInitialSections([FromBody] CreateInitialCompanySectionModel obj)
        {
            GlobalVariables.LoggedInUsername = Request.HttpContext.User.Claims.First(x => x.Type == "cognito:username").Value.ToString();
            var response = await _companySectionService.CreateInitialSections(obj);

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

        [Authorize(AuthenticationSchemes = "Bearer")]
        [Route("company-sections")]
        [HttpGet]
        public async Task<IActionResult> GetCompanySectionsByLoggedInCompany()
        {
            GlobalVariables.LoggedInCompanyId = Convert.ToInt32(Request.HttpContext.User.Claims.First(x => x.Type == "custom:companyId").Value.ToString());
            GlobalVariables.LoggedInUsername = Request.HttpContext.User.Claims.First(x => x.Type == "cognito:username").Value.ToString();
            var response = await _companySectionService.GetCompanySectionsByCompanyId(GlobalVariables.LoggedInCompanyId);

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

        //[Authorize(AuthenticationSchemes = "Bearer")]
        [Route("company-sections/{companyId}")]
        [HttpGet]
        public async Task<IActionResult> GetCompanySectionsByCompanyGuid([FromRoute] string companyId)
        {
            //GlobalVariables.LoggedInUsername = Request.HttpContext?.User?.Claims.First(x => x.Type == "cognito:username")?.Value.ToString();
            var response = await _companySectionService.GetCompanySectionsByCompanyGuid(companyId);

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

        [Authorize(AuthenticationSchemes = "Bearer")]
        [Route("company-structure/submit")]
        [HttpPost]
        public async Task<IActionResult> SubmitCompanyStructureSection([FromBody] UpdateCompanyStructureSectionModel obj)
        {
            GlobalVariables.LoggedInCompanyId = Convert.ToInt32(Request.HttpContext.User.Claims.First(x => x.Type == "custom:companyId").Value.ToString());
            GlobalVariables.LoggedInUsername = Request.HttpContext.User.Claims.First(x => x.Type == "cognito:username").Value.ToString();
            var response = await _companySectionService.SubmitCompanyStructureSection(obj, GlobalVariables.LoggedInCompanyId);

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

        [Authorize(AuthenticationSchemes = "Bearer")]
        [Route("company-structure/draft")]
        [HttpPost]
        public async Task<IActionResult> DraftCompanyStructureSection([FromBody] UpdateCompanyStructureSectionModel obj)
        {
            GlobalVariables.LoggedInCompanyId = Convert.ToInt32(Request.HttpContext.User.Claims.First(x => x.Type == "custom:companyId").Value.ToString());
            GlobalVariables.LoggedInUsername = Request.HttpContext.User.Claims.First(x => x.Type == "cognito:username").Value.ToString();
            var response = await _companySectionService.DraftCompanyStructureSection(obj, GlobalVariables.LoggedInCompanyId);

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

        //[Authorize(AuthenticationSchemes = "Bearer")]
        [Route("company-structure/reject")]
        [HttpPut]
        public async Task<IActionResult> RejectCompanyStructureSection([FromBody] RejectCompanySectionModel obj)
        {
            //GlobalVariables.LoggedInUsername = Request.HttpContext?.User?.Claims.First(x => x.Type == "cognito:username").Value.ToString();
            var response = await _companySectionService.RejectCompanyStructureSection(obj.CompanyId);

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

        //[Authorize(AuthenticationSchemes = "Bearer")]
        [Route("company-structure/approve")]
        [HttpPut]
        public async Task<IActionResult> ApproveCompanyStructureSection([FromBody] ApproveCompanySectionModel obj)
        {
            //GlobalVariables.LoggedInUsername = Request.HttpContext?.User?.Claims.First(x => x.Type == "cognito:username").Value.ToString();
            var response = await _companySectionService.ApproveCompanyStructureSection(obj.CompanyId);

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

        [Authorize(AuthenticationSchemes = "Bearer")]
        [Route("company-beneficial-owners/submit")]
        [HttpPost]
        public async Task<IActionResult> SubmitCompanyBeneficialOwnerSection([FromBody] UpdateCompanyBeneficialOwnerSectionModel obj)
        {
            GlobalVariables.LoggedInCompanyId = Convert.ToInt32(Request.HttpContext.User.Claims.First(x => x.Type == "custom:companyId").Value.ToString());
            GlobalVariables.LoggedInUsername = Request.HttpContext.User.Claims.First(x => x.Type == "cognito:username").Value.ToString();
            var response = await _companySectionService.SubmitCompanyBeneficialOwnerSection(obj, GlobalVariables.LoggedInCompanyId);

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

        [Authorize(AuthenticationSchemes = "Bearer")]
        [Route("company-beneficial-owners/draft")]
        [HttpPost]
        public async Task<IActionResult> DraftCompanyBeneficialOwnerSection([FromBody] UpdateCompanyBeneficialOwnerSectionModel obj)
        {
            GlobalVariables.LoggedInCompanyId = Convert.ToInt32(Request.HttpContext.User.Claims.First(x => x.Type == "custom:companyId").Value.ToString());
            GlobalVariables.LoggedInUsername = Request.HttpContext.User.Claims.First(x => x.Type == "cognito:username").Value.ToString();
            var response = await _companySectionService.DraftCompanyBeneficialOwnerSection(obj, GlobalVariables.LoggedInCompanyId);

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

        //[Authorize(AuthenticationSchemes = "Bearer")]
        [Route("company-beneficial-owners/reject")]
        [HttpPut]
        public async Task<IActionResult> RejectCompanyBeneficialOwnerSection([FromBody] RejectCompanySectionModel obj)
        {
            //GlobalVariables.LoggedInUsername = Request.HttpContext?.User?.Claims.First(x => x.Type == "cognito:username").Value.ToString();
            var response = await _companySectionService.RejectCompanyBeneficialOwnerSection(obj.CompanyId);

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

        //[Authorize(AuthenticationSchemes = "Bearer")]
        [Route("company-beneficial-owners/approve")]
        [HttpPut]
        public async Task<IActionResult> ApproveCompanyBeneficialOwnerSection([FromBody] ApproveCompanySectionModel obj)
        {
            //GlobalVariables.LoggedInUsername = Request.HttpContext?.User?.Claims.First(x => x.Type == "cognito:username").Value.ToString();
            var response = await _companySectionService.ApproveCompanyBeneficialOwnerSection(obj.CompanyId);

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

        [Authorize(AuthenticationSchemes = "Bearer")]
        [Route("company-directors/submit")]
        [HttpPost]
        public async Task<IActionResult> SubmitCompanyDirectorsSection([FromBody] UpdateCompanyDirectorSectionModel obj)
        {
            GlobalVariables.LoggedInCompanyId = Convert.ToInt32(Request.HttpContext.User.Claims.First(x => x.Type == "custom:companyId").Value.ToString());
            GlobalVariables.LoggedInUsername = Request.HttpContext.User.Claims.First(x => x.Type == "cognito:username").Value.ToString();
            var response = await _companySectionService.SubmitCompanyDirectorSection(obj, GlobalVariables.LoggedInCompanyId);

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

        [Authorize(AuthenticationSchemes = "Bearer")]
        [Route("company-directors/draft")]
        [HttpPost]
        public async Task<IActionResult> DraftCompanyDirectorsSection([FromBody] UpdateCompanyDirectorSectionModel obj)
        {
            GlobalVariables.LoggedInCompanyId = Convert.ToInt32(Request.HttpContext.User.Claims.First(x => x.Type == "custom:companyId").Value.ToString());
            GlobalVariables.LoggedInUsername = Request.HttpContext.User.Claims.First(x => x.Type == "cognito:username").Value.ToString();
            var response = await _companySectionService.DraftCompanyDirectorSection(obj, GlobalVariables.LoggedInCompanyId);

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

        //[Authorize(AuthenticationSchemes = "Bearer")]
        [Route("company-directors/reject")]
        [HttpPut]
        public async Task<IActionResult> RejectCompanyDirectorsSection([FromBody] RejectCompanySectionModel obj)
        {
            //GlobalVariables.LoggedInUsername = Request.HttpContext?.User?.Claims.First(x => x.Type == "cognito:username").Value.ToString();
            var response = await _companySectionService.RejectCompanyDirectorSection(obj.CompanyId);

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

        //[Authorize(AuthenticationSchemes = "Bearer")]
        [Route("company-directors/approve")]
        [HttpPut]
        public async Task<IActionResult> ApproveCompanyDirectorsSection([FromBody] ApproveCompanySectionModel obj)
        {
            //GlobalVariables.LoggedInUsername = Request.HttpContext?.User?.Claims.First(x => x.Type == "cognito:username").Value.ToString();
            var response = await _companySectionService.ApproveCompanyDirectorSection(obj.CompanyId);

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

        [Authorize(AuthenticationSchemes = "Bearer")]
        [Route("document-types")]
        [HttpGet]
        public async Task<IActionResult> GetDocumentTypes()
        {
            GlobalVariables.LoggedInCompanyId = Convert.ToInt32(Request.HttpContext.User.Claims.First(x => x.Type == "custom:companyId").Value.ToString());
            GlobalVariables.LoggedInUsername = Request.HttpContext.User.Claims.First(x => x.Type == "cognito:username").Value.ToString();
            var response = await _documentTypeService.GetAllBRC();

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

        [Authorize(AuthenticationSchemes = "Bearer")]
        [Route("total-number-of-employees")]
        [HttpGet]
        public async Task<IActionResult> ListTotalNumberOfEmployees()
        {
            var response = await _companySectionService.ListTotalNumerOfEmployess();

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

        //[Authorize(AuthenticationSchemes = "Bearer")]
        [Route("overall-kyc-status/{companyId}")]
        [HttpGet]
        public async Task<IActionResult> GetOverallKYCStatus([FromRoute] string companyId)
        {
            var response = await _companySectionService.CheckOverallKYCStatus(Convert.ToInt32(companyId));

            return Ok(response);
        }
    }
}
