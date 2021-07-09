using AutoMapper;
using otm.core.ResponseV2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xgca.core.Models.Company;
using xgca.data.Company;
using Microsoft.AspNetCore.JsonPatch;
using xgca.core.Helpers;
using xgca.core.AuditLog;

namespace xgca.core.Services
{
    public interface ICompanyServiceV2
    {
        Task<PagedResponse<List<GetCompanyListingViewModel>>> GetCompanyList(bool isFromSettings = false, int pageNumber = 1, int pageSize = 10, string orderBy = null, string query = null);
        Task<GenericResponse<GetCompanyViewModel>> GetCompany(Guid guid);
        Task<GenericResponse<GetCompanyViewModel>> Put(UpdateCompanyViewModel payload);
        Task<GenericResponse<GetCompanyViewModel>> Patch(Guid guid, JsonPatchDocument<UpdateCompanyViewModel> payload);
    }

    public class CompanyServiceV2 : ICompanyServiceV2
    {
        private readonly ICompanyDataV2 companyData;
        private readonly IMapper mapper;
        private readonly IGLobalCmsService gLobalCmsService;
        private readonly IAuditLogCore auditLog;

        public CompanyServiceV2(IAuditLogCore auditLog, IGLobalCmsService gLobalCmsService, ICompanyDataV2 companyData, IMapper mapper)
        {
            this.companyData = companyData;
            this.mapper = mapper;
            this.gLobalCmsService = gLobalCmsService;
            this.auditLog = auditLog;
        }

        public async Task<GenericResponse<GetCompanyViewModel>> GetCompany(Guid guid)
        {
            var (company, errors) = await companyData.Show(guid);

            if (errors != null)
                return new GenericResponse<GetCompanyViewModel>(null, errors.Select(e => new ErrorField("message", e)).ToList(), "Error on fetching company details", 400);

            var response = mapper.Map<GetCompanyViewModel>(company);
            var services = await gLobalCmsService.GetAllService();
            response.CompanyServices.Select(c => { c.ImageUrl = services.Where(s => s.IntServiceId.Equals(c.ServiceId)).FirstOrDefault()?.ImageURL; return c; }).ToList();

            return new GenericResponse<GetCompanyViewModel>(response, "Company retreived successfully.", 200);
        }

        public async Task<PagedResponse<List<GetCompanyListingViewModel>>> GetCompanyList(bool isFromSettings = false, int pageNumber = 1, int pageSize = 10, string orderBy = null, string query = null)
        {
            var (result, totalCount, errors) = await companyData.List(isFromSettings, orderBy, query, pageNumber, pageSize);
            var companies = mapper.Map<List<GetCompanyListingViewModel>>(result).ToList();

            var services = await gLobalCmsService.GetAllService();

            var serviceGuids = result.SelectMany(c => c.CompanyServices).Select(s => new
            {
                CompanyGuid = s.Companies.Guid,
                Service = new Dictionary<string, string>() 
                {
                    { s.ServiceName,  services.Where(e => e.ServiceName.Equals(s.ServiceName)).FirstOrDefault()?.ServiceId.ToString() } 
                }
            });

            companies.All(c =>
            {
                c.Services = serviceGuids.Where(s => s.CompanyGuid.Equals(Guid.Parse(c.Guid))).Select(s => s.Service).ToList();
                return true;
            });

            return new PagedResponse<List<GetCompanyListingViewModel>>(companies, "List of companies", 200, pageNumber, pageSize, totalCount, orderBy, query, null);
        }

        public async Task<GenericResponse<GetCompanyViewModel>> Patch(Guid guid, JsonPatchDocument<UpdateCompanyViewModel> payload)
        {
            var (company, errors) = await companyData.Get(guid);

            if (errors != null)
                return new GenericResponse<GetCompanyViewModel>(null, errors.Select(e => new ErrorField("message", e)).ToList(), "Error on updating company details", 400);

            var (oldCompany, fetchError) = await companyData.Show(guid);

            var companyToPatch = mapper.Map<UpdateCompanyViewModel>(company);

            payload.ApplyTo(companyToPatch);

            mapper.Map(companyToPatch, company);

            var (result, patchErrors) = await companyData.Patch(company); 

            if (patchErrors != null)
                return new GenericResponse<GetCompanyViewModel>(null, patchErrors.Select(e => new ErrorField("message", e)).ToList(), "Error on updating company details", 400);
            
            var (newCompany, newFetchError) = await companyData.Show(guid);

            // Audit Logs
            await CreateAuditLog(oldCompany, newCompany);

            return new GenericResponse<GetCompanyViewModel>(mapper.Map<GetCompanyViewModel>(company), "Company updated successfully.", 200);
        }

        public async Task<GenericResponse<GetCompanyViewModel>> Put(UpdateCompanyViewModel payload)
        {
            if (payload.CompanyTaxSettings.Count > 0)
            {
                var duplicates = payload.CompanyTaxSettings.GroupBy(c => c.TaxTypeDescription).Any(g => g.Count() > 1);

                if(duplicates)
                    return new GenericResponse<GetCompanyViewModel>(null, new List<ErrorField> { new ErrorField("message", "Duplicate tax settings found. Please remove the duplicate tax settings") }, "Error on updating company details", 400);
            }
            var (oldCompany, fetchError) = await companyData.Show(Guid.Parse(payload.Guid));

            var (result, errors) = await companyData.Put(mapper.Map<entity.Models.Company>(payload));

            if (errors != null)
                return new GenericResponse<GetCompanyViewModel>(null, errors.Select(e => new ErrorField("message", e)).ToList(), "Error on updating company details", 400);

            // Audit Logs
            await CreateAuditLog(oldCompany, result);

            return new GenericResponse<GetCompanyViewModel>(mapper.Map<GetCompanyViewModel>(result), "Company updated successfully.", 200);
        }
        private async Task CreateAuditLog(entity.Models.Company oldCompany, entity.Models.Company newCompany)
        {
            var services = await gLobalCmsService.GetAllService();

            var oldValue = CompanyHelper.BuildCompanyValue(oldCompany, BuildCompanyService(oldCompany.CompanyServices, services));

            var newValue = CompanyHelper.BuildCompanyValue(newCompany, BuildCompanyService(newCompany.CompanyServices, services));

            await auditLog.CreateAuditLog("Update", newCompany.GetType().Name, newCompany.CompanyId, 0, oldValue, newValue);
        }

        private dynamic BuildCompanyService(ICollection<entity.Models.CompanyService> companyServices, List<Models.GlobalCms.ServicesModel> services)
        {
            return companyServices.Select(c => new
            {
                CompanyServiceId = c.Guid,
                services.Where(s => s.IntServiceId.Equals(c.ServiceId)).FirstOrDefault()?.ServiceId,
                Code = services.Where(s => s.IntServiceId.Equals(c.ServiceId)).FirstOrDefault()?.ServiceCode,
                Name = services.Where(s => s.IntServiceId.Equals(c.ServiceId)).FirstOrDefault()?.ServiceName,
                services.Where(s => s.IntServiceId.Equals(c.ServiceId)).FirstOrDefault()?.ImageURL,
                StaticId = services.Where(s => s.IntServiceId.Equals(c.ServiceId)).FirstOrDefault()?.IntServiceId,
                c.Status
            }).ToList();
        }
    }
}
