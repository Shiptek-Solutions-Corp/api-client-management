using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using xgca.core.CompanyService;
using xgca.core.CompanyServiceRole;
using xgca.core.Models.CompanyGroupResource;
using xgca.core.Response;
using xgca.data.CompanyGroupResource;
using xgca.data.CompanyService;
using xgca.data.CompanyServiceRole;
using xgca.data.CompanyServiceUser;
using xgca.entity.Models;

namespace xgca.core.CompanyGroupResource
{
    public interface ICompanyGroupResource
    {
        Task<IGeneralModel> Create(CreateCompanyGroupResource createCompanyGroupResource);
    }
    public class CompanyGroupResource : ICompanyGroupResource
    {
        private readonly ICompanyGroupResourceData _companyGroupResourceData;
        private readonly IMapper _mapper;
        private readonly IGeneral _general;
        private readonly data.CompanyService.ICompanyService _companyServiceData;
        private readonly data.CompanyServiceRole.ICompanyServiceRole _companyServiceRoleData;
        private readonly ICompanyServiceUser _companyServiceUserData;

        public CompanyGroupResource(
            ICompanyGroupResourceData companyGroupResourceData, 
            IMapper mapper, 
            IGeneral general,
            data.CompanyService.ICompanyService companyService,
            data.CompanyServiceRole.ICompanyServiceRole companyServiceRole,
            ICompanyServiceUser companyServiceUser
            )
        {
            _companyGroupResourceData = companyGroupResourceData;
            _mapper = mapper;
            _general = general;
            _companyServiceData = companyService;
            _companyServiceRoleData = companyServiceRole;
            _companyServiceUserData = companyServiceUser;
        }

        public async Task<IGeneralModel> Create(CreateCompanyGroupResource createCompanyGroupResource)
        {
            var companyGroupResource = _mapper.Map<entity.Models.CompanyGroupResource>(createCompanyGroupResource);
            var result = await _companyGroupResourceData.Create(companyGroupResource);
            var viewCompanyGroupResource = _mapper.Map<GetCompanyGroupResource>(companyGroupResource);

            return _general.Response(viewCompanyGroupResource, 200, "Created successfuly.", true);
        }
    }
}
