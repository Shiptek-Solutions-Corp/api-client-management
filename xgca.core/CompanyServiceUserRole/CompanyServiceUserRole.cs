using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xgca.core.Models.CompanyServiceUserRole;
using xgca.core.Response;
using xgca.data.CompanyServiceUserRole;
using xgca.entity;

namespace xgca.core.CompanyServiceUserRole
{
    public interface ICompanyServiceUserRole
    {
        Task<IGeneralModel> Create(CreateCompanyServiceUserRole createCompanyServiceUserRole);
        Task<IGeneralModel> GetAll();
        Task<IGeneralModel> Get(int id);

    }
    public class CompanyServiceUserRole : ICompanyServiceUserRole
    {
        private readonly ICompanyServiceUserRoleData _companyServiceUserRoleData;
        private readonly IMapper _mapper;
        private readonly IGeneral _general;
        public CompanyServiceUserRole(ICompanyServiceUserRoleData companyServiceUserRoleData, IMapper mapper, IGeneral general) {
            _companyServiceUserRoleData = companyServiceUserRoleData;
            _mapper = mapper;
            _general = general;
        }

        public async Task<IGeneralModel> Create(CreateCompanyServiceUserRole createCompanyServiceUserRole)
        {
            var companyServiceUserRole = _mapper.Map<entity.Models.CompanyServiceUserRole>(createCompanyServiceUserRole);
            var result = await _companyServiceUserRoleData.Create(companyServiceUserRole);
            var viewCompanyServiceUserRole = _mapper.Map<GetCompanyServiceUserRole>(companyServiceUserRole);

            return _general.Response(viewCompanyServiceUserRole, 200, "Created successfuly.", true);
        }

        public async Task<IGeneralModel> Get(int id)
        {
            var moduleGroup = await _companyServiceUserRoleData.Retrieve(id);
            if (moduleGroup != null)
            {
                var viewModuleGroup = _mapper.Map<GetCompanyServiceUserRole>(moduleGroup);
                return _general.Response(viewModuleGroup, 200, "Retreived successfuly", true);
            }
            return _general.Response(null, 400, "Invalid Module Group", false);
        }

        public async Task<IGeneralModel> GetAll()
        {
            var result = await _companyServiceUserRoleData.List();
            var viewModuleGroups = result.Select(d => _mapper.Map<GetCompanyServiceUserRole>(d)).ToList();

            return _general.Response(viewModuleGroups, 200, "Module Groups listed successfuly", true);
        }
    }
}
