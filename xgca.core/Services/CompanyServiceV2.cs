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

        public CompanyServiceV2(IGLobalCmsService gLobalCmsService, ICompanyDataV2 companyData, IMapper mapper)
        {
            this.companyData = companyData;
            this.mapper = mapper;
            this.gLobalCmsService = gLobalCmsService;
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

            return new PagedResponse<List<GetCompanyListingViewModel>>(companies, "List of companies", 200, pageNumber, pageSize, totalCount, orderBy, query, null);
        }

        public async Task<GenericResponse<GetCompanyViewModel>> Patch(Guid guid, JsonPatchDocument<UpdateCompanyViewModel> payload)
        {
            var (company, errors) = await companyData.Show(guid);

            if (errors != null)
                return new GenericResponse<GetCompanyViewModel>(null, errors.Select(e => new ErrorField("message", e)).ToList(), "Error on updating company details", 400);


            var companyToPatch = mapper.Map<UpdateCompanyViewModel>(company);

            payload.ApplyTo(companyToPatch);

            mapper.Map(companyToPatch, company);

            var (result, patchErrors) = await companyData.Patch(company); 

            if (patchErrors != null)
                return new GenericResponse<GetCompanyViewModel>(null, patchErrors.Select(e => new ErrorField("message", e)).ToList(), "Error on updating company details", 400);


            return new GenericResponse<GetCompanyViewModel>(mapper.Map<GetCompanyViewModel>(company), "Company updated successfully.", 200);
        }

        public async Task<GenericResponse<GetCompanyViewModel>> Put(UpdateCompanyViewModel payload)
        {
            var (result, errors) = await companyData.Put(mapper.Map<entity.Models.Company>(payload));

            if (errors != null)
                return new GenericResponse<GetCompanyViewModel>(null, errors.Select(e => new ErrorField("message", e)).ToList(), "Error on updating company details", 400);

            return new GenericResponse<GetCompanyViewModel>(mapper.Map<GetCompanyViewModel>(result), "Company updated successfully.", 200);
        }
    }
}
