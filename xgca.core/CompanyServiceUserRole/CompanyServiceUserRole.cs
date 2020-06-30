using AutoMapper;
using System;
using System.Collections.Generic;
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
    }
}
