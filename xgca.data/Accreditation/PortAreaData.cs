using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xas.data._IOptionsModel;
using xgca.entity;

namespace xas.data.accreditation.PortArea
{
    public interface IPortAreaData
    {
        Task AddPortResponsibility(List<xgca.entity.Models.PortArea> data);
        Task<bool> CheckIfPortExists(Guid portId, Guid requestId);
        Task ReAddPortResponsibility(xgca.entity.Models.PortArea data);
        Task RemovePortResponsibility(Guid portAreaId);
        Task<List<xgca.entity.Models.PortArea>> GetPortList(Guid requestId);
        Task<bool> CheckIfPortIsDeleted(Guid portId);
        Task<bool> RemovePortAccreditation(Guid portId);
    }

    public class PortAreaData : IPortAreaData
    {
        private readonly IXGCAContext _context;
        private readonly IOptions<ClientTokenData> _clientToken;

        public PortAreaData(IXGCAContext context, IOptions<ClientTokenData> clientToken)
        {
            _context = context;
            _clientToken = clientToken;
        }

        public async Task<List<xgca.entity.Models.PortArea>> GetPortList(Guid requestId)
        {
            var requestGuid = await _context.Request.Where(t => t.Guid == requestId).FirstOrDefaultAsync();
            var ports = await _context.PortArea.Where(t => t.Request.RequestId == requestGuid.RequestId && t.IsDeleted == false).ToListAsync();
            return ports;
        }

        public async Task AddPortResponsibility(List<xgca.entity.Models.PortArea> data)
        {
            await _context.PortArea.AddRangeAsync(data);
            await _context.SaveChangesAsync();
        }

        public async Task RemovePortResponsibility(Guid portAreaId)
        {
            var d = await _context.PortArea.Where(t => t.Guid == portAreaId && t.IsDeleted == false).FirstOrDefaultAsync();
            d.IsDeleted = true;
            d.DeletedBy = _clientToken.Value.GetUsername;
            d.UpdatedBy = _clientToken.Value.GetUsername;   
            d.UpdatedOn = DateTime.UtcNow;
            d.DeletedOn = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }

        public async Task ReAddPortResponsibility(xgca.entity.Models.PortArea data)
        {
            var d = await _context.PortArea.Where(t => t.PortId == data.PortId && t.IsDeleted == true).FirstOrDefaultAsync();
            d.IsDeleted = false;
            await _context.SaveChangesAsync();
        }

        public async Task<bool> CheckIfPortIsDeleted(Guid portId)
        {
            var d = await _context.PortArea.Where(t => t.Guid == portId && t.IsDeleted == false).ToListAsync();
            bool response = d.Count > 0 ? true : false;
            return response;
        }

        public async Task<bool> CheckIfPortExists(Guid portId, Guid requestId)
        {
            var reqId = await _context.Request.Where(t => t.Guid == requestId).FirstOrDefaultAsync();
            var d = await _context.PortArea.Where(t => t.PortId == portId && t.RequestId == reqId.RequestId && t.IsDeleted == false).ToListAsync();
            bool response = d.Count > 0 ? true: false;
            return response;
        }

        public async Task<bool> RemovePortAccreditation(Guid portId)
        {
            var d = await _context.PortArea.Where(p => p.PortId == portId && p.IsDeleted == false).FirstOrDefaultAsync();
            d.IsDeleted = true;
            d.DeletedBy = "System";
            d.DeletedOn = DateTime.UtcNow;
            d.UpdatedBy = "System";
            d.UpdatedOn = DateTime.UtcNow;

            var result = await _context.SaveChangesAsync();
            if (result <= 0)
            {
                return false;
            }

            return true;
        }
    }
}
