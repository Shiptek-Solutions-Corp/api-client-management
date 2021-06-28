using AutoMapper;
using otm.core.ResponseV2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xgca.core.Models.CompanyTaxSettings;
using xgca.data.CompanyTaxSettings;
using xgca.entity.Models;

namespace xgca.core.Services
{
    public interface ICompanyTaxSettingsService
    {
        Task<GenericResponse<GetCompanyTaxSettingsModel>> Create(CreateCompanyTaxSettingsModel payload);
        Task<GenericResponse<GetCompanyTaxSettingsModel>> Put(UpdateCompanyTaxSettingsModel payload);
    }
    public class CompanyTaxSettingsService : ICompanyTaxSettingsService
    {
        private readonly IMapper mapper;
        private readonly ICompanyTaxSettingsRepository companyTaxSettingsRepository;

        public CompanyTaxSettingsService(IMapper mapper, ICompanyTaxSettingsRepository companyTaxSettingsRepository)
        {
            this.mapper = mapper;
            this.companyTaxSettingsRepository = companyTaxSettingsRepository;
        }

        public async Task<GenericResponse<GetCompanyTaxSettingsModel>> Create(CreateCompanyTaxSettingsModel payload)
        {
            var (result, errors) = await companyTaxSettingsRepository.Create(payload.CompanyGuid, mapper.Map<CompanyTaxSettings>(payload));
                
            return result
                ? new GenericResponse<GetCompanyTaxSettingsModel>(null, "Tax setting created successfully.", 200)
                : new GenericResponse<GetCompanyTaxSettingsModel>(null, errors.Select(e => new ErrorField("message", e)).ToList(), "An error occured on creating tax setting.", 400);
        }

        public async Task<GenericResponse<GetCompanyTaxSettingsModel>> Put(UpdateCompanyTaxSettingsModel payload)
        {
            var (result, errors) = await companyTaxSettingsRepository.Put(mapper.Map<CompanyTaxSettings>(payload));
            
            return result != null
            ? new GenericResponse<GetCompanyTaxSettingsModel>(mapper.Map<GetCompanyTaxSettingsModel>(result), "Tax setting updated successfully.", 200)
            : new GenericResponse<GetCompanyTaxSettingsModel>(null, errors.Select(e => new ErrorField("message", e)).ToList(), "An error occured on update tax setting.", 400);
        }
    }
}
