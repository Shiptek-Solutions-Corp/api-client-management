using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using xgca.entity.Models;
using xgca.entity;
using xgca.data.ViewModels.Request;
using xas.data.Request;

namespace xas.data.accreditation.Request
{
    public interface IRequestData
    {
        Task<List<xgca.entity.Models.Request>> CreateRequest(List<xgca.entity.Models.Request> entity);
        Task<dynamic> GetStatusStatistics(string bound, Guid loginCompanyGuid, Guid loginServiceRoleGuid, Guid serviceRoleGuid);
        //Task<dynamic> GetStatusStatisticsInbound(int companyId, Guid serviceRoleId);
        //Task<dynamic> GetStatusStatisticsOutbound(int companyId, Guid serviceRoleId);
        Task UpdateAccreditationRequest(Guid requestId, int companyIdTo, int status);
        Task<bool> ValidateCheckRequestIfExist(Guid CompanyIdFrom, Guid CompanyIdTo);
        Task<object> ValidateIfRequestStatusUpdateIsAllowed(Guid requestId, int companyId);
        Task DeleteRequest(List<Guid> requestIds);
        Task<int> GetRequestIdByGuid(Guid requestId);      
        Task<List<xgca.entity.Models.Request>> ActivateDeactivateRequest(List<Guid> requestIds, bool status);
        Task<(List<GetRequestModel>, int)> GetRequestList(string bound, int pageSize, int pageNumber, Guid loginCompanyGuid, Guid loginServiceRoleGuid, Guid serviceRoleGuid, string companyName, string companyAddress, string companyCountryName, string companyStateCityName, string portAreaResponsibility, string portAreaOperatingCountryName, string truckAreaResponsibility, int accreditationStatusConfigId, byte? companyStatus, string sortOrder, string sortBy, string quickSearch);
        Task<xgca.entity.Models.Request> PortOfResponsibilityAccreditedCustomer(string companyId, string portId);
    }

    public class RequestData: IRequestData
    {
        private readonly IXGCAContext _context;

        public RequestData(IXGCAContext context)
        { 
            _context = context;
        }
        
        public async Task<(List<GetRequestModel>, int)> GetRequestList(string bound, int pageSize, int pageNumber, Guid loginCompanyGuid, Guid loginServiceRoleGuid, Guid serviceRoleGuid, string companyName, string companyAddress, string companyCountryName, string companyStateCityName, string portAreaResponsibility, string portAreaOperatingCountryName, string truckAreaResponsibility, int accreditationStatusConfigId, byte? companyStatus, string sortOrder, string sortBy, string quickSearch)
        {
            //Filter Company Info Either From or To
            Guid defaultGuid = Guid.NewGuid();
            var companyRequestInfo = await (from r in _context.Request.Include(i => i.PortArea).Include(t => t.TruckArea)
                                            .Include(i => i.AccreditationStatusConfig).AsNoTracking()
                                            join coFrom in _context.Companies.Include(i => i.Addresses).Include(c => c.ContactDetails).AsNoTracking() on r.CompanyIdFrom equals coFrom.Guid
                                            join coTo in _context.Companies.Include(i => i.Addresses).Include(c => c.ContactDetails).AsNoTracking() on r.CompanyIdTo equals coTo.Guid
                                            where r.IsDeleted == false 
                                                && (bound.ToUpper() == "INCOMING" ? r.CompanyIdTo : defaultGuid) == (bound.ToUpper() == "INCOMING" ? loginCompanyGuid : defaultGuid)
                                                && (bound.ToUpper() == "OUTGOING" ? r.CompanyIdFrom : defaultGuid) == (bound.ToUpper() == "OUTGOING" ? loginCompanyGuid : defaultGuid)
                                                && (bound.ToUpper() == "INCOMING" ? r.ServiceRoleIdTo : defaultGuid) == (bound.ToUpper() == "INCOMING" ? loginServiceRoleGuid : defaultGuid)
                                                && (bound.ToUpper() == "OUTGOING" ? r.ServiceRoleIdFrom : defaultGuid) == (bound.ToUpper() == "OUTGOING" ? loginServiceRoleGuid : defaultGuid)

                                                //For Requestor
                                                && (bound.ToUpper() == "INCOMING" ? r.ServiceRoleIdFrom : defaultGuid) == (bound.ToUpper() == "INCOMING" ? serviceRoleGuid : defaultGuid)
                                                && (bound.ToUpper() == "OUTGOING" ? r.ServiceRoleIdTo : defaultGuid) == (bound.ToUpper() == "OUTGOING" ? serviceRoleGuid : defaultGuid)
                                                && (portAreaResponsibility.Length != 0? _context.PortArea.Where(i => (i.Locode + "-" + i.PortCode + "-" + i.PortName).ToUpper().Contains(portAreaResponsibility.ToUpper()))
                                                                                                         .Select(x => x.RequestId)
                                                                                                         .Contains(r.RequestId)
                                                                                        : true)
                                                && (portAreaOperatingCountryName.Length != 0 ? _context.PortArea.Where(i => i.CountryName.ToUpper().Contains(portAreaOperatingCountryName.ToUpper()))
                                                                                                                    .Select(x => x.RequestId)
                                                                                                                    .Contains(r.RequestId)
                                                : true)
                                                && (truckAreaResponsibility.Length != 0 ? _context.TruckArea.Where(i => i.CountryName.ToUpper().Contains(truckAreaResponsibility.ToUpper()))
                                                                                                        .Select(x => x.RequestId)
                                                                                                        .Contains(r.RequestId)
                                                                                        : true)
                                                && (accreditationStatusConfigId == 0? 0:r.AccreditationStatusConfigId) == (accreditationStatusConfigId == 0 ? 0 : accreditationStatusConfigId)
                                            select new 
                                            {
                                                
                                               RequestInfo = r
                                             , CompanyInfo = (bound.ToUpper() == "INCOMING" ? coFrom : coTo)
                                              
                                            }).ToListAsync();

            var requestListInfo = (from r in companyRequestInfo
                                   where r.CompanyInfo.CompanyName.ToUpper().Contains(companyName.ToUpper())
                                        && r.CompanyInfo.Addresses.CountryName.ToUpper().Contains(companyCountryName.ToUpper())
                                        && (r.CompanyInfo.Addresses.StateName + r.CompanyInfo.Addresses.CityName).ToUpper().Contains(companyStateCityName.ToUpper())
                                        && r.CompanyInfo.Addresses.FullAddress.ToUpper().Contains(companyAddress.ToUpper())
                                        && (companyStatus == null? 0:r.CompanyInfo.Status) == (companyStatus == null ? 0 : companyStatus)
                                   select new GetRequestModel
                                   {
                                       AccreditationStatusConfigId = r.RequestInfo.AccreditationStatusConfigId
                                        , AccreditationStatusConfigDescription = r.RequestInfo.AccreditationStatusConfig.Description
                                        , RequestGuid = r.RequestInfo.Guid
                                        , RequestId = r.RequestInfo.RequestId
                                        , ServiceRoleIdFrom = r.RequestInfo.ServiceRoleIdFrom
                                        , CompanyIdFrom = r.RequestInfo.CompanyIdFrom
                                        , ServiceRoleIdTo = r.RequestInfo.ServiceRoleIdTo
                                        , CompanyIdTo = r.RequestInfo.CompanyIdTo
                                        , RequestIsActive = r.RequestInfo.IsActive
                                        , PortAreaList = String.Join(" / ", r.RequestInfo.PortArea.Where(i => i.IsDeleted == false).Select(i => (i.Locode + "-"+ i.PortCode + "-" + i.PortName)))
                                        , PortAreaOperatingCountries = r.RequestInfo.PortArea.Where(i => i.IsDeleted == false).Select(i => i.CountryName).Distinct().ToList()
                                        , TruckAreaList = String.Join(" / ", r.RequestInfo.TruckArea.Where(i => i.IsDeleted == false).Select(i => i.CountryName))
                                        , CompanyLogo = r.CompanyInfo.ImageURL
                                        , CompanyGuid = r.CompanyInfo.Guid
                                        , CompanyName = r.CompanyInfo.CompanyName 
                                        , CompanyCountryName = r.CompanyInfo.Addresses.CountryName
                                        , CompanyStateCityName = (r.CompanyInfo.Addresses.StateName + "/" + r.CompanyInfo.Addresses.CityName)
                                        , CompanyStateName = r.CompanyInfo.Addresses.StateName 
                                        , CompanyCityName = r.CompanyInfo.Addresses.CityName 
                                        , CompanyPostalCode = r.CompanyInfo.Addresses.ZipCode 
                                        , CompanyFullAddress = r.CompanyInfo.Addresses.FullAddress
                                        , CompanyCUCC = r.CompanyInfo.CUCC
                                        , CompanyFaxPrefix = r.CompanyInfo.ContactDetails.FaxPrefix
                                        , CompanyFaxNumber = r.CompanyInfo.ContactDetails.Fax
                                        , CompanyMobilePrefix = r.CompanyInfo.ContactDetails.MobilePrefix
                                        , CompanyMobileNumber = r.CompanyInfo.ContactDetails.Mobile
                                        , CompanyPhonePrefix = r.CompanyInfo.ContactDetails.PhonePrefix
                                        , CompanyPhoneNumber = r.CompanyInfo.ContactDetails.Phone
                                        , CompanyWebsiteURL = r.CompanyInfo.WebsiteURL
                                        , CompanyStatus = (r.CompanyInfo.Status == 1? "Active":"Inactive")
                                        , CompanyEmailAddress = r.CompanyInfo.EmailAddress
                                        
                                   }).ToList();

            requestListInfo = requestListInfo.Where(i => (i.CompanyName + i.CompanyCountryName + i.CompanyStateCityName + i.CompanyFullAddress + i.PortAreaList + i.TruckAreaList).ToUpper().Contains(quickSearch.ToUpper())).ToList();

            if (sortOrder.Equals("asc")) requestListInfo = requestListInfo.OrderBy(i => typeof(GetRequestModel).GetProperty(sortBy).GetValue(i).ToString()).ToList(); //Ascending
            if (sortOrder.Equals("desc")) requestListInfo = requestListInfo.OrderByDescending(i => typeof(GetRequestModel).GetProperty(sortBy).GetValue(i).ToString()).ToList(); //Descending

            var recordCount = requestListInfo.Count();
            requestListInfo = requestListInfo.Skip(pageSize * (pageNumber)).Take(pageSize).ToList();

            return (requestListInfo, recordCount);
        }

        public async Task<List<xgca.entity.Models.Request>> CreateRequest(List<xgca.entity.Models.Request> entity)
        {
            _context.Request.AddRange(entity);
            await _context.SaveChangesAsync(null, true);

            return entity;
        }

        public async Task DeleteRequest(List<Guid> requestIds)
        {
            var requestList = await _context.Request.Where(t => requestIds.Contains(t.Guid) && t.IsDeleted == false).ToListAsync();

            _context.Request.RemoveRange(requestList);
            await _context.SaveChangesAsync(null, true);
        }
        public async Task<dynamic> GetStatusStatistics(string bound, Guid loginCompanyGuid, Guid loginServiceRoleGuid, Guid serviceRoleGuid)
        {
            Guid defaultGuid = Guid.NewGuid();
            var companyRequestInfo = await (from r in _context.Request
                                            where r.IsDeleted == false
                                                && (bound.ToUpper() == "INCOMING" ? r.CompanyIdTo : defaultGuid) == (bound.ToUpper() == "INCOMING" ? loginCompanyGuid : defaultGuid)
                                                && (bound.ToUpper() == "OUTGOING" ? r.CompanyIdFrom : defaultGuid) == (bound.ToUpper() == "OUTGOING" ? loginCompanyGuid : defaultGuid)
                                                && (bound.ToUpper() == "INCOMING" ? r.ServiceRoleIdTo : defaultGuid) == (bound.ToUpper() == "INCOMING" ? loginServiceRoleGuid : defaultGuid)
                                                && (bound.ToUpper() == "OUTGOING" ? r.ServiceRoleIdFrom : defaultGuid) == (bound.ToUpper() == "OUTGOING" ? loginServiceRoleGuid : defaultGuid)

                                                //For Requestor
                                                && (bound.ToUpper() == "INCOMING" ? r.ServiceRoleIdFrom : defaultGuid) == (bound.ToUpper() == "INCOMING" ? serviceRoleGuid : defaultGuid)
                                                && (bound.ToUpper() == "OUTGOING" ? r.ServiceRoleIdTo : defaultGuid) == (bound.ToUpper() == "OUTGOING" ? serviceRoleGuid : defaultGuid)
                                            select r).ToListAsync();
            var stats = new
            {
                newRequest = companyRequestInfo.Count(c => c.AccreditationStatusConfigId == 1),
                approvedRequest = companyRequestInfo.Count(c => c.AccreditationStatusConfigId == 2),
                rejectedRequest = companyRequestInfo.Count(c => c.AccreditationStatusConfigId == 3),
                all = companyRequestInfo.Count
            };

            return stats;
        }

        //public async Task<dynamic> GetStatusStatisticsInbound(int companyId, Guid serviceRoleId)
        //{
        //    var loginCompanyGuid = await _context.Companies.Where(i => i.CompanyId == companyId).Select(x => x.Guid).SingleOrDefaultAsync();

        //    var data = await _context.Request.Where(t => t.CompanyIdTo == loginCompanyGuid && t.ServiceRoleIdTo == serviceRoleId && t.IsDeleted == false).ToListAsync();
        //    var stats = new
        //    {
        //        newRequest = data.Count(c => c.AccreditationStatusConfigId == 1),
        //        approvedRequest = data.Count(c => c.AccreditationStatusConfigId == 2),
        //        rejectedRequest = data.Count(c => c.AccreditationStatusConfigId == 3),
        //        all = data.Count
        //    };

        //    return stats;
        //}

        //public async Task<dynamic> GetStatusStatisticsOutbound(int companyId, Guid serviceRoleId)
        //{
        //    var companyGuid = await _context.Companies.Where(i => i.CompanyId == companyId).Select(x => x.Guid).SingleOrDefaultAsync();
        //    var data = await _context.Request.Where(t => t.CompanyIdFrom == companyGuid && t.ServiceRoleIdFrom == serviceRoleId && t.IsDeleted == false).ToListAsync();
        //    var stats = new
        //    {
        //        newRequest = data.Count(c => c.AccreditationStatusConfigId == 1),
        //        approvedRequest = data.Count(c => c.AccreditationStatusConfigId == 2),
        //        rejectedRequest = data.Count(c => c.AccreditationStatusConfigId == 3),
        //        all = data.Count
        //    };

        //    return stats;
        //}

        public async Task UpdateAccreditationRequest(Guid requestId, int companyIdTo, int status)
        {
            var companyToGuid = await _context.Companies.Where(i => i.CompanyId == companyIdTo).Select(f => f.Guid).SingleOrDefaultAsync();
            var intRequestId = await _context.Request.Where(t => t.Guid == requestId && t.IsDeleted == false).FirstOrDefaultAsync();
            var d = await _context.Request.Where(t => t.RequestId == intRequestId.RequestId && t.CompanyIdTo == companyToGuid && t.IsDeleted == false).FirstOrDefaultAsync();
            d.AccreditationStatusConfigId = status;

            _context.Request.Update(d);
            await _context.SaveChangesAsync(null, true);
        }

        public async Task<bool> ValidateCheckRequestIfExist(Guid CompanyIdFrom, Guid CompanyIdTo)
        {
            bool IsDuplicate = false;
            var data = await _context.Request.Where(t => t.CompanyIdFrom == CompanyIdFrom && t.CompanyIdTo == CompanyIdTo && t.IsDeleted == false).ToListAsync();
            if (data.Count > 0) IsDuplicate = true;
            return IsDuplicate;
        }

        public async Task<object> ValidateIfRequestStatusUpdateIsAllowed(Guid requestId, int companyId)
        {
            var companyGuid = await _context.Companies.Where(i => i.CompanyId == companyId).Select(f => f.Guid).SingleOrDefaultAsync();
            var data = await _context.Request.Where(t => t.Guid == requestId && t.CompanyIdTo == companyGuid && t.IsDeleted == false).FirstOrDefaultAsync();
            return data;
        }

        public async Task<int> GetRequestIdByGuid(Guid requestId)
        {
            var data = await _context.Request.Where(t => t.Guid == requestId).FirstOrDefaultAsync();
            return data.RequestId;
        }

        public async Task<List<xgca.entity.Models.Request>> ActivateDeactivateRequest(List<Guid> requestIds, bool status)
        {
            var lstRequest = await _context.Request.Where(i => requestIds.Contains(i.Guid)).ToListAsync();
            lstRequest.ForEach(i => { i.IsActive = status; });

            _context.Request.UpdateRange(lstRequest);
            await _context.SaveChangesAsync(null, true);

            return lstRequest;
        }

        public async Task<xgca.entity.Models.Request> PortOfResponsibilityAccreditedCustomer(string companyId, string portId)
        {
            var data = await _context.Request
                .Where(t => t.CompanyIdTo == Guid.Parse(companyId) && t.PortArea.Any(p => p.PortId == Guid.Parse(portId) && p.IsDeleted == false))
                .Where(t => t.AccreditationStatusConfigId == 2) // Get all approved
                .FirstOrDefaultAsync();
            return data;
        }

    }
}
