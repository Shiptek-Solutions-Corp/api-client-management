using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using xgca.core.Models.CompanyServiceRole;
using xgca.core.Response;
using xgca.core.Helpers;
using xgca.core.Models.CompanyService;
using AutoMapper;

namespace xgca.core.CompanyServiceRole
{
    public class CompanyServiceRole : ICompanyServiceRole
    {
        private readonly xgca.data.CompanyServiceRole.ICompanyServiceRole _companyServiceRole;
        private readonly xgca.data.CompanyService.ICompanyService _companyService;
        private readonly IGeneral _general;
        private readonly IMapper _mapper;
        public CompanyServiceRole(xgca.data.CompanyServiceRole.ICompanyServiceRole companyServiceRole,
            xgca.data.CompanyService.ICompanyService companyService, IGeneral general, IMapper mapper)
        {
            _companyServiceRole = companyServiceRole;
            _companyService = companyService;
            _general = general;
            _mapper = mapper;
        }

        public async Task<IGeneralModel> CreateDefault(int companyId, int userId)
        {

            var companyServices = await _companyService.ListByCompanyId(companyId);
            List<entity.Models.CompanyServiceRole> companyServiceRoles = new List<entity.Models.CompanyServiceRole>();
            foreach (var companyService in companyServices)
            {
                companyServiceRoles.Add(new entity.Models.CompanyServiceRole
                {
                    CompanyServiceId = companyService.CompanyServiceId,
                    Name = "Administrator",
                    Description = "Service administrator",
                    CreatedBy = userId,
                    CreatedOn = DateTime.UtcNow,
                    ModifiedBy = userId,
                    ModifiedOn = DateTime.UtcNow,
                    Guid = Guid.NewGuid()
                });
            }

            var result = await _companyServiceRole.Create(companyServiceRoles);
            return result
                ? _general.Response(true, 200, "Default company service roles have been created created", true)
                : _general.Response(false, 400, "Error on creating default company service roles", false);
        }

        public async Task<IGeneralModel> Create(CreateCompanyServiceRoleModel obj)
        {
            var request = _mapper.Map<entity.Models.CompanyServiceRole>(obj);
            request.CreatedBy = 1;
            request.CreatedOn = DateTime.UtcNow;
            request.ModifiedBy = 1;
            request.ModifiedOn = DateTime.UtcNow;
            request.Guid = Guid.NewGuid();
            var result = await _companyServiceRole.Create(request);

            return result
                ? _general.Response(true, 200, "Company Service Role Created", true)
                : _general.Response(false, 400, "Error on creating company service role", false);
        }

        public async Task<IGeneralModel> ListByCompanyServiceId(string key)
        {
            int companyServiceId = await _companyService.GetIdByGuid(Guid.Parse(key));
            var result = await _companyServiceRole.ListByCompanyServiceId(companyServiceId);
            var data = result.Select(t => new { CompanyServiceRoleId = t.Guid, CompanyServiceId = t.CompanyServices.Guid, t.Name, t.Description });
            return _general.Response(new { companyServiceRole = data }, 200, "Configurable company service roles has been listed", true);
        }
    }
}
