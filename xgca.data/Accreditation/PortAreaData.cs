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
using xgca.data.ViewModels.PortArea;
using xgca.entity;

namespace xas.data.DataModel.PortArea
{
    public interface IPortAreaData
    {
        Task<List<xgca.entity.Models.PortArea>> AddPortResponsibility(List<xgca.entity.Models.PortArea> data);
        Task<bool> CheckIfPortExists(Guid portId, Guid requestId);
        Task ReAddPortResponsibility(xgca.entity.Models.PortArea data);
        Task<xgca.entity.Models.PortArea> RemovePortResponsibility(Guid portAreaId);
        Task<List<PortAreaResponseModel>> GetPortList(Guid requestId);
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

        public async Task<List<PortAreaResponseModel>> GetPortList(Guid requestId)
        {
            var portList = await (from p in _context.PortArea
                                     join r in _context.Request on p.RequestId equals r.RequestId
                                     where r.Guid == requestId && r.IsDeleted == false 
                                     select new PortAreaResponseModel
                                     {
                                         CityName = p.CityName 
                                         , CountryAreaId = p.CountryAreaId 
                                         , CountryCode = p.CountryCode 
                                         , CountryName = p.CountryName 
                                         , IsDeleted = p.IsDeleted 
                                         , Latitude = p.Latitude
                                         , Location = p.Location 
                                         , LoCode = p.Locode 
                                         , Longitude = p.Longitude 
                                         , Name = ""
                                         , PortAreaId = p.PortAreaId
                                         , PortId = p.PortId
                                         , PortOfDischarge = (p.PortOfDischarge == 1 ? "yes" : "no")                                        
                                         , PortOfLoading = (p.PortOfLoading == 1 ? "yes" : "no")
                                         , PortAreaGuid = p.Guid
                                     }).ToListAsync();
                
       
            return portList;
        }

        public async Task<List<xgca.entity.Models.PortArea>> AddPortResponsibility(List<xgca.entity.Models.PortArea> data)
        {
            _context.PortArea.AddRange(data);
            await _context.SaveChangesAsync(null, true);
            return data;
        }

        public async Task<xgca.entity.Models.PortArea> RemovePortResponsibility(Guid portAreaId)
        {
            var portitem = await _context.PortArea.Where(t => t.Guid == portAreaId).SingleOrDefaultAsync();
            _context.PortArea.Remove(portitem);

            await _context.SaveChangesAsync(null, true);
            return portitem;
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
