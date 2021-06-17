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
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class KYCController : Controller
    {
        private readonly ICompanySectionService _companySectionService;
        private readonly IDocumentTypeService _documentTypeService;

        public KYCController(ICompanySectionService _companySectionService, IDocumentTypeService _documentTypeService)
        {
            this._companySectionService = _companySectionService;
            this._documentTypeService = _documentTypeService;
        }

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

        [Route("company-sections/{companyId}")]
        [HttpGet]
        public async Task<IActionResult> GetCompanySectionsByCompanyGuid([FromRoute] string companyId)
        {
            GlobalVariables.LoggedInUsername = Request.HttpContext.User.Claims.First(x => x.Type == "cognito:username").Value.ToString();
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

        [Route("company-beneficial-owners/submit")]
        [HttpPost]
        public async Task<IActionResult> SubmitCompanyBeneficialOwner([FromBody] UpdateCompanyBeneficialOwnerSectionModel obj)
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

        [Route("company-beneficial-owners/draft")]
        [HttpPost]
        public async Task<IActionResult> DraftCompanyBeneficialOwner([FromBody] UpdateCompanyBeneficialOwnerSectionModel obj)
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

        [Route("company-directors/submit")]
        [HttpPost]
        public async Task<IActionResult> SubmitCompanyDirectors([FromBody] UpdateCompanyDirectorSectionModel obj)
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

        [Route("company-directors/draft")]
        [HttpPost]
        public async Task<IActionResult> DraftCompanyDirectors([FromBody] UpdateCompanyDirectorSectionModel obj)
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
    }
}
