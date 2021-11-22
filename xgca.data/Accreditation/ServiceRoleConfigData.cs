using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xas.data.DataModel.ServiceRoleConfig;
using xgca.entity;

namespace xas.data.DataModel.ServiceRoleConfig
{
    public interface IServiceRoleConfigData
    {
        Task<List<xgca.entity.Models.ServiceRoleConfig>> GetAllowedServices(Guid serviceType);
    }
    public class ServiceRoleConfigData : IServiceRoleConfigData
    {
        private readonly IXGCAContext _context;

        public ServiceRoleConfigData(IXGCAContext context)
        {
            _context = context;
        }

        public async Task<List<xgca.entity.Models.ServiceRoleConfig>> GetAllowedServices(Guid serviceRole)
        {            
            return await _context.ServiceRoleConfig.Where(t => t.ServiceRoleId == serviceRole).ToListAsync();
        }
    }
}
