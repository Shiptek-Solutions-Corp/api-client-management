using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xgca.core.CompanyService;
using xgca.core.CompanyServiceRole;
using xgca.core.Models.CompanyGroupResource;
using xgca.core.Response;
using xgca.core.Services;
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
        Task<IGeneralModel> Get(int id);
        Task<IGeneralModel> GetAll(string companyServiceRoleGuid);
        Task<IGeneralModel> GetAuthorizationDetails(string username);

    }
    public class CompanyGroupResource : ICompanyGroupResource
    {
        private readonly ICompanyGroupResourceData _companyGroupResourceData;
        private readonly IMapper _mapper;
        private readonly IGeneral _general;
        private readonly data.CompanyService.ICompanyService _companyServiceData;
        private readonly data.CompanyServiceRole.ICompanyServiceRole _companyServiceRoleData;
        private readonly ICompanyServiceUser _companyServiceUserData;
        private readonly IGLobalCmsService gLobalCmsService;
        public CompanyGroupResource(
            ICompanyGroupResourceData companyGroupResourceData, 
            IMapper mapper, 
            IGeneral general,
            data.CompanyService.ICompanyService companyService,
            data.CompanyServiceRole.ICompanyServiceRole companyServiceRole,
            ICompanyServiceUser companyServiceUser,
            IGLobalCmsService gLobalCmsService
            )
        {
            _companyGroupResourceData = companyGroupResourceData;
            _mapper = mapper;
            _general = general;
            _companyServiceData = companyService;
            _companyServiceRoleData = companyServiceRole;
            _companyServiceUserData = companyServiceUser;
            this.gLobalCmsService = gLobalCmsService;
        }

        public async Task<IGeneralModel> Create(CreateCompanyGroupResource createCompanyGroupResource)
        {
            var companyGroupResource = _mapper.Map<entity.Models.CompanyGroupResource>(createCompanyGroupResource);
            var result = await _companyGroupResourceData.Create(companyGroupResource);
            var viewCompanyGroupResource = _mapper.Map<GetCompanyGroupResource>(companyGroupResource);

            return _general.Response(viewCompanyGroupResource, 200, "Created successfuly.", true);
        }

        public async Task<IGeneralModel> Get(int id)
        {
            var moduleGroup = await _companyGroupResourceData.Retrieve(id);
            if (moduleGroup != null)
            {
                var viewModuleGroup = _mapper.Map<GetCompanyGroupResource>(moduleGroup);
                return _general.Response(viewModuleGroup, 200, "Retreived successfuly", true);
            }
            return _general.Response(null, 400, "Invalid Module Group", false);
        }

        public async Task<IGeneralModel> GetAll(string companyServiceRoleGuid)
        {
            int companyServiceRoleId = 0;

            if (companyServiceRoleGuid != "")
            {
                companyServiceRoleId = await _companyServiceRoleData.GetIdByGuid(Guid.Parse(companyServiceRoleGuid));
            }

            var result = await _companyGroupResourceData.List(companyServiceRoleId);
            var viewModuleGroups = result.Select(d => _mapper.Map<GetCompanyGroupResource>(d)).ToList();

            return _general.Response(viewModuleGroups, 200, "Module Groups listed successfuly", true);
        }

        public async Task<IGeneralModel> GetAuthorizationDetails(string username)
        {
            var result = await _companyGroupResourceData.GetAuthorizationDetails(username);
            var resources = await gLobalCmsService.GetAllResourceByGroupResourceIds(result);
            return _general.Response(resources, 200, "Success", true);
        }
    }
}
