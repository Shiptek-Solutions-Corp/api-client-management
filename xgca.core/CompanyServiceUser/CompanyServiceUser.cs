using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using xgca.core.Models.CompanyServiceUser;
using xgca.core.Response;
using xgca.core.Helpers;
using xgca.core.Models.CompanyService;

namespace xgca.core.CompanyServiceUser
{
    public class CompanyServiceUser : ICompanyServiceUser
    {
        private readonly xgca.data.CompanyServiceRole.ICompanyServiceRole _companyServiceRole;
        private readonly xgca.data.CompanyServiceUser.ICompanyServiceUser _companyServiceUser;
        private readonly xgca.data.CompanyService.ICompanyService _companyService;
        private readonly IGeneral _general;

        public CompanyServiceUser(xgca.data.CompanyServiceRole.ICompanyServiceRole companyServiceRole,
            xgca.data.CompanyServiceUser.ICompanyServiceUser companyServiceUser,
            xgca.data.CompanyService.ICompanyService companyService, IGeneral general)
        {
            _companyServiceRole = companyServiceRole;
            _companyServiceUser = companyServiceUser;
            _companyService = companyService;
            _general = general;
        }

        public async Task<bool> CreateDefault(int companyId, int companyUserId, int createdBy)
        {
            var companyServices = await _companyService.ListByCompanyId(companyId);
            List<entity.Models.CompanyServiceUser> companyServiceUsers = new List<entity.Models.CompanyServiceUser>();
            foreach (entity.Models.CompanyService companyService in companyServices)
            {
                int companyServiceRoleId = await _companyServiceRole.RetrieveAdministratorId(companyService.CompanyServiceId);
                companyServiceUsers.Add(new entity.Models.CompanyServiceUser
                {
                    CompanyServiceId = companyService.CompanyServiceId,
                    CompanyServiceRoleId = companyServiceRoleId,
                    CompanyUserId = companyUserId,
                    CreatedBy = createdBy,
                    CreatedOn = DateTime.UtcNow,
                    ModifiedBy = createdBy,
                    ModifiedOn = DateTime.UtcNow,
                    Guid = Guid.NewGuid()
                });
            }

            var result = await _companyServiceUser.Create(companyServiceUsers);
            return result;
        }
    }
}
