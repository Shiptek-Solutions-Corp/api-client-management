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
        Task<dynamic> GetStatusStatisticsInbound(int companyId);
        Task<dynamic> GetStatusStatisticsOutbound(int companyId);
        Task UpdateAccreditationRequest(Guid requestId, int companyIdTo, int status);
        Task<ICollection<xgca.entity.Models.Request>> GetAllIncommingRequest(Guid CompanyId);
        Task<ICollection<xgca.entity.Models.Request>> GetAllOutgoingRequest(Guid CompanyId);
        Task<List<xgca.entity.Models.AccreditationStatusConfig>> GetAccreditationStatus();
        Task<bool> ValidateCheckRequestIfExist(Guid CompanyIdFrom, Guid CompanyIdTo);
        Task<int> CheckRequestIfDeleted(Guid CompanyIdFrom, Guid CompanyIdTo);
        Task<object> ValidateIfRequestStatusUpdateIsAllowed(Guid requestId, int companyId);
        Task DeleteRequest(List<Guid> requestIds);
        Task ReAddRequest(xgca.entity.Models.Request data);
        Task<int> CheckRequestIfExistById(Guid CompanyIdTo, Guid RequestId);
        Task<int> GetRequestIdByGuid(Guid requestId);
        Task<int> GetRequestId(string companyIdFrom, string companyIdTo);
        Task<(List<TruckingResponseDataModel>, int)> GetAllTruckingIncomingRequest(int CompanyId, string serviceRoleId, string quicksearch, string company, string address, string truckArea, string orderBy, bool isDescending, int status, int pageNumber, int pageSize);
        Task<(List<TruckingResponseDataModel>, int)> GetAllTruckingOutgoingRequest(int CompanyId, string serviceRoleId, string quicksearch, string company, string address, string truckArea, string orderBy, bool isDescending, int status, int pageNumber, int pageSize);
        Task<dynamic> GetStatisticsInbound(int companyId, string serviceRoleId);
        Task<dynamic> GetStatisticsOutbound(int companyId, string serviceRoleId);
        Task<List<xgca.entity.Models.Request>> ActivateDeactivateRequest(List<Guid> requestIds, bool status);
        Task<(List<GetRequestModel>, int)> GetRequestList(string bound, int pageSize, int pageNumber, Guid companyGuid, Guid serviceRoleGuid, string companyName, string companyAddress, string companyCountryName, string companyStateCityName, string portAreaResponsibility, string truckAreaResponsibility, string sortOrder, string sortBy, string quickSearch);
        Task<xgca.entity.Models.Request> PortOfResponsibilityAccreditedCustomer(string companyId, string portId);
    }

    public class RequestData: IRequestData
    {
        private readonly IXGCAContext _context;

        public RequestData(IXGCAContext context)
        { 
            _context = context;
        }
        public async Task<List<xgca.entity.Models.Request>> CreateRequest(List<xgca.entity.Models.Request> entity)
        {
            _context.Request.AddRange(entity);
            await _context.SaveChangesAsync(null, true);

            return entity;
        }

        public async Task<dynamic> GetStatusStatisticsInbound(int companyId)
        {
            var companyGuid = await _context.Companies.Where(i => i.CompanyId == companyId).Select(x => x.Guid).SingleOrDefaultAsync();

            var data = await _context.Request.Where(t => t.CompanyIdTo == companyGuid && t.IsDeleted == false).ToListAsync();
            var stats = new
            {
                newRequest = data.Count(c => c.AccreditationStatusConfigId == 1),
                approvedRequest = data.Count(c => c.AccreditationStatusConfigId == 2),
                rejectedRequest = data.Count(c => c.AccreditationStatusConfigId == 3)
            };

            return stats;
        }

        public async Task<dynamic> GetStatusStatisticsOutbound(int companyId)
        {
            var companyGuid = await _context.Companies.Where(i => i.CompanyId == companyId).Select(x => x.Guid).SingleOrDefaultAsync();
            var data = await _context.Request.Where(t => t.CompanyIdFrom == companyGuid && t.IsDeleted == false).ToListAsync();
            var stats = new
            {
                newRequest = data.Count(c => c.AccreditationStatusConfigId == 1),
                approvedRequest = data.Count(c => c.AccreditationStatusConfigId == 2),
                rejectedRequest = data.Count(c => c.AccreditationStatusConfigId == 3)
            };

            return stats;
        }

        public async Task UpdateAccreditationRequest(Guid requestId, int companyIdTo, int status)
        {
            var companyToGuid = await _context.Companies.Where(i => i.CompanyId == companyIdTo).Select(f => f.Guid).SingleOrDefaultAsync();
            var intRequestId = await _context.Request.Where(t => t.Guid == requestId && t.IsDeleted == false).FirstOrDefaultAsync();
            var d = await _context.Request.Where(t => t.RequestId == intRequestId.RequestId && t.CompanyIdTo == companyToGuid && t.IsDeleted == false).FirstOrDefaultAsync();
            d.AccreditationStatusConfigId = status;

            _context.Request.Update(d);
            await _context.SaveChangesAsync(null, true);
        }

        public async Task<ICollection<xgca.entity.Models.Request>> GetAllIncommingRequest(Guid CompanyId)
        {
            var data = await _context.Request.Select(s => new xgca.entity.Models.Request {
                ServiceRoleIdFrom = s.ServiceRoleIdFrom,
                IsDeleted = s.IsDeleted,
                PortArea = _context.PortArea.Where(t => t.RequestId == s.RequestId && t.IsDeleted == false).ToList(),
                RequestId = s.RequestId,
                AccreditationStatusConfig = s.AccreditationStatusConfig,
                CompanyIdFrom = s.CompanyIdFrom,
                CompanyIdTo = s.CompanyIdTo,
                CreatedBy = s.CreatedBy,
                CreatedOn = s.CreatedOn,
                DeletedBy = s.DeletedBy,
                DeletedOn = s.DeletedOn,
                Guid = s.Guid,
                ServiceRoleIdTo = s.ServiceRoleIdTo,
                AccreditationStatusConfigId = s.AccreditationStatusConfigId,
                UpdatedBy = s.UpdatedBy,
                UpdatedOn = s.UpdatedOn
            }).Where(t => t.CompanyIdTo == CompanyId && t.IsDeleted == false).ToListAsync();
            return data;
        }

        public async Task<ICollection<xgca.entity.Models.Request>> GetAllOutgoingRequest(Guid CompanyId)
        {
            var data = await _context.Request.Select(s => new xgca.entity.Models.Request
            {
                ServiceRoleIdFrom = s.ServiceRoleIdFrom,
                IsDeleted = s.IsDeleted,
                PortArea = _context.PortArea.Where(t => t.RequestId == s.RequestId && t.IsDeleted == false).ToList(),
                RequestId = s.RequestId,
                AccreditationStatusConfig = s.AccreditationStatusConfig,
                CompanyIdFrom = s.CompanyIdFrom,
                CompanyIdTo = s.CompanyIdTo,
                CreatedBy = s.CreatedBy,
                CreatedOn = s.CreatedOn,
                DeletedBy = s.DeletedBy,
                DeletedOn = s.DeletedOn,
                Guid = s.Guid,
                ServiceRoleIdTo = s.ServiceRoleIdTo,
                AccreditationStatusConfigId = s.AccreditationStatusConfigId,
                UpdatedBy = s.UpdatedBy,
                UpdatedOn = s.UpdatedOn
            }).Where(t => t.CompanyIdFrom == CompanyId && t.IsDeleted == false).ToListAsync();
            return data;
        }

        //public async Task<(List<ResponseDTO>, int recordCount)> GetAllRequest(int pageNumber,int rowPerPage, string bound, int companyId, string search, string columnSort, string sort, string cCompanyName, string cFullAddress, string cStatus, string cOperating, string cPortResp, string cLocode)
        //{
        //    var requestList = await (from r in _context.Request.Include(i => i.PortArea.Where(x => x.IsDeleted == false)).Include(t => t.TruckArea.Where(i => i.IsDeleted == false))
        //                             join to in _context.Companies on r.CompanyIdTo equals to.Guid
        //                             join fr in _context.Companies on r.CompanyIdFrom equals fr.Guid 
        //                             where (bound.ToUpper() == "INCOMING" ?  to.CompanyId: 0) == (bound.ToUpper() == "INCOMING" ? companyId : 0)
        //                              && (bound.ToUpper() == "OUTGOING" ? fr.CompanyId : 0) == (bound.ToUpper() == "OUTGOING" ? companyId : 0)
        //                             select new
        //                             {
        //                                 r
        //                             }).ToListAsync();


        //}

        public async Task<List<xgca.entity.Models.AccreditationStatusConfig>> GetAccreditationStatus()
        {
            var data = await _context.AccreditationStatusConfig.Select(t => new xgca.entity.Models.AccreditationStatusConfig
            { 
                AccreditationStatusConfigId = t.AccreditationStatusConfigId,
                Value = t.Value
            }).ToListAsync();

            return data;
        }

        public async Task<bool> ValidateCheckRequestIfExist(Guid CompanyIdFrom, Guid CompanyIdTo)
        {
            bool IsDuplicate = false;
            var data = await _context.Request.Where(t => t.CompanyIdFrom == CompanyIdFrom && t.CompanyIdTo == CompanyIdTo && t.IsDeleted == false).ToListAsync();
            if (data.Count > 0) IsDuplicate = true;
            return IsDuplicate;
        }

        public async Task<int> CheckRequestIfDeleted(Guid CompanyIdFrom, Guid CompanyIdTo)
        {
            var data = await _context.Request.Where(t => t.CompanyIdFrom == CompanyIdFrom && t.CompanyIdTo == CompanyIdTo && t.IsDeleted == true).ToListAsync();
            return data.Count;
        }

        public async Task<object> ValidateIfRequestStatusUpdateIsAllowed(Guid requestId, int companyId)
        {
            var companyGuid = await _context.Companies.Where(i => i.CompanyId == companyId).Select(f => f.Guid).SingleOrDefaultAsync();
            var data = await _context.Request.Where(t => t.Guid == requestId && t.CompanyIdTo == companyGuid && t.IsDeleted == false).FirstOrDefaultAsync();
            return data;
        }

        public async Task DeleteRequest(List<Guid> requestIds)
        {
            var requestList = await _context.Request.Where(t => requestIds.Contains(t.Guid) && t.IsDeleted == false).ToListAsync();

            _context.Request.RemoveRange(requestList);
            await _context.SaveChangesAsync(null, true);
        }

        public async Task ReAddRequest(xgca.entity.Models.Request data)
        {
            var d = await _context.Request.Where(t => t.CompanyIdFrom == data.CompanyIdFrom &&
            t.CompanyIdFrom == data.CompanyIdFrom && t.IsDeleted == true).FirstOrDefaultAsync();
            d.IsDeleted = false;
            await _context.SaveChangesAsync();
        }

        public async Task<int> CheckRequestIfExistById(Guid CompanyIdTo, Guid RequestId)
        {
            var d = await _context.Request.Where(t => t.CompanyIdTo == CompanyIdTo && t.Guid == RequestId).ToListAsync();
            return d.Count;
        }

        public async Task<int> GetRequestIdByGuid(Guid requestId)
        {
            var data = await _context.Request.Where(t => t.Guid == requestId).FirstOrDefaultAsync();
            return data.RequestId;
        }

        public async Task<int> GetRequestId(string companyIdFrom, string companyIdTo)
        {
            int requestId = await _context.Request.AsNoTracking()
                .Where(x => x.CompanyIdFrom == Guid.Parse(companyIdFrom) && x.CompanyIdTo == Guid.Parse(companyIdTo) && x.IsDeleted == false)
                .Select(c => c.RequestId)
                .FirstOrDefaultAsync();

            return requestId;
        }

        public async Task<(List<TruckingResponseDataModel>, int)> GetAllTruckingIncomingRequest(int CompanyId, string serviceRoleId, string quicksearch, string company, string address, string truckArea, string orderBy, bool isDescending, int status, int pageNumber, int pageSize)
        {

            var data = await (from ir in _context.Request 
                              join cd in _context.Companies on  ir.CompanyIdFrom  equals cd.Guid
                              join ad in _context.Addresses on cd.AddressId equals ad.AddressId
                              join contact in _context.ContactDetails on cd.ContactDetailId equals contact.ContactDetailId
                              join toCo in _context.Companies on ir.CompanyIdTo equals toCo.Guid
                              join ta in _context.TruckArea on ir.RequestId equals ta.RequestId into talj from ta in talj.DefaultIfEmpty()
                              where
                              (
                                cd.CompanyName.ToLower().Contains(quicksearch.ToLower()) ||
                                ad.FullAddress.ToLower().Contains(quicksearch.ToLower()) ||
                                ta.CountryName.ToLower().Contains(quicksearch.ToLower()))
                                &&
                                cd.CompanyName.ToLower().Contains(company.ToLower()) &&
                                ad.FullAddress.ToLower().Contains(address.ToLower()) &&
                                ta.CountryName.ToLower().Contains(truckArea.ToLower()) &&
                                //ir.CompanyIdTo == CompanyId &&
                                toCo.CompanyId == CompanyId &&
                                ir.ServiceRoleIdFrom == Guid.Parse(serviceRoleId) &&
                                ir.IsDeleted == false &&
                                (status.Equals(0) ? !(ir.AccreditationStatusConfigId.Equals(0)) : ir.AccreditationStatusConfigId == status) 
                                select new TruckingResponseDataModel
                                {
                                    RequestId = ir.Guid.ToString(),
                                    CompanyId = cd.CompanyId.ToString(),
                                    CompanyName = cd.CompanyName,
                                    FullAddress = ad.FullAddress,
                                    EmailAddress = cd.EmailAddress,
                                    Fax = contact.Fax,
                                    FaxPrefix = contact.FaxPrefix,
                                    FaxPrefixId = contact.FaxPrefixId.ToString(),
                                    Mobile = contact.Mobile,
                                    MobilePrefix = contact.MobilePrefix,
                                    MobilePrefixId = contact.MobilePrefixId.ToString(),
                                    Phone = contact.Phone,
                                    PhonePrefix = contact.PhonePrefix,
                                    PhonePrefixId = contact.PhonePrefixId.ToString(),
                                    WebsiteUrl = cd.WebsiteURL,
                                    ImageUrl = cd.ImageURL,
                                    CountryId = ad.CountryId,
                                    CountryName = ad.CountryName,
                                    Latitude = ad.Latitude,
                                    Longitude = ad.Longitude,
                                    TruckArea = ta.CountryName,
                                    Status = ir.AccreditationStatusConfigId == 1 ? "New" : ir.AccreditationStatusConfigId == 2 ? "Approved" : ir.AccreditationStatusConfigId == 3 ? "Rejected" : "Unknown"
                                })
                                .Distinct()
                                .ToListAsync();

            if (orderBy != "")
            {
                if (orderBy.ToLower().Equals("company"))
                {
                    if (isDescending is false) data = data.OrderBy(d => d.CompanyName).ToList();
                    else data = data.OrderByDescending(d => d.CompanyName).ToList();
                }

                else if (orderBy.ToLower().Equals("address"))
                {
                    if (isDescending is false) data = data.OrderBy(d => d.FullAddress).ToList();
                    else data = data.OrderBy(d => d.FullAddress).ToList();
                }

                else if (orderBy.ToLower().Equals("area"))
                {
                    if (isDescending is false) data = data.OrderBy(d => d.TruckArea).ToList();
                    else data = data.OrderBy(d => d.TruckArea).ToList();
                }

                else if (orderBy.ToLower().Equals("status"))
                {
                    if (isDescending is false) data = data.OrderBy(d => d.Status).ToList();
                    else data = data.OrderBy(d => d.Status).ToList();
                }
            }

            int recordCount = data.Count();
            var result = data.Skip(pageSize * pageNumber)
                .Take(pageSize)
                .ToList();

            return (result, recordCount);
        }

        public async Task<(List<TruckingResponseDataModel>, int)> GetAllTruckingOutgoingRequest(int CompanyId, string serviceRoleId, string quicksearch, string company, string address, string truckArea, string orderBy, bool isDescending, int status, int pageNumber, int pageSize)
        {
            var data = await (from or in _context.Request
                              join foCo in _context.Companies on or.CompanyIdFrom equals foCo.Guid
                              join ad in _context.Addresses on foCo.AddressId equals ad.AddressId
                              join toCo in _context.Companies on or.CompanyIdTo equals toCo.Guid
                              join toAd in _context.Addresses on toCo.AddressId equals toAd.AddressId
                              join toContact in _context.ContactDetails on toCo.ContactDetailId equals toContact.ContactDetailId
                              join ta in _context.TruckArea on or.RequestId equals ta.RequestId into talj
                              from ta in talj.DefaultIfEmpty()
                              where
                              (
                                foCo.CompanyName.ToLower().Contains(quicksearch.ToLower()) ||
                                ad.FullAddress.ToLower().Contains(quicksearch.ToLower()) ||
                                ad.CountryName.ToLower().Contains(quicksearch.ToLower()))
                                &&
                                foCo.CompanyName.ToLower().Contains(company.ToLower()) &&
                                ad.FullAddress.ToLower().Contains(address.ToLower()) &&
                                ta.CountryName.ToLower().Contains(truckArea.ToLower()) &&
                                //or.CompanyIdFrom == CompanyId &&
                                foCo.CompanyId == CompanyId &&
                                or.ServiceRoleIdTo == Guid.Parse(serviceRoleId) &&
                                or.IsDeleted == false &&
                                (status.Equals(0) ? !(or.AccreditationStatusConfigId.Equals(0)) : or.AccreditationStatusConfigId == status)
                                        select new TruckingResponseDataModel
                                        {
                                            RequestId = or.Guid.ToString(),
                                            CompanyId = toCo.CompanyId.ToString(),
                                            CompanyName = toCo.CompanyName,
                                            FullAddress = ad.FullAddress,
                                            EmailAddress = toCo.EmailAddress,
                                            Fax = toContact.Fax,
                                            FaxPrefix = toContact.FaxPrefix,
                                            FaxPrefixId = toContact.FaxPrefixId.ToString(),
                                            Mobile = toContact.Mobile,
                                            MobilePrefix = toContact.MobilePrefix,
                                            MobilePrefixId = toContact.MobilePrefixId.ToString(),
                                            Phone = toContact.Phone,
                                            PhonePrefix = toContact.PhonePrefix,
                                            PhonePrefixId = toContact.PhonePrefixId.ToString(),
                                            WebsiteUrl = toCo.WebsiteURL,
                                            ImageUrl = toCo.ImageURL,
                                            CountryId = ad.CountryId,
                                            CountryName = ad.CountryName,
                                            Latitude = ad.Latitude,
                                            Longitude = ad.Longitude,
                                            TruckArea = ta.CountryName,
                                            Status = or.AccreditationStatusConfigId == 1 ? "New" : or.AccreditationStatusConfigId == 2 ? "Approved" : or.AccreditationStatusConfigId == 3 ? "Rejected" : "Unknown"

                                        })
                                        .Distinct()
                                        .ToListAsync();

            if (orderBy != "")
            {
                if (orderBy.ToLower().Equals("company"))
                {
                    if (isDescending is false) data = data.OrderBy(d => d.CompanyName).ToList();
                    else data = data.OrderByDescending(d => d.CompanyName).ToList();
                }

                else if (orderBy.ToLower().Equals("address"))
                {
                    if (isDescending is false) data = data.OrderBy(d => d.FullAddress).ToList();
                    else data = data.OrderBy(d => d.FullAddress).ToList();
                }

                else if (orderBy.ToLower().Equals("area"))
                {
                    if (isDescending is false) data = data.OrderBy(d => d.TruckArea).ToList();
                    else data = data.OrderBy(d => d.TruckArea).ToList();
                }

                else if (orderBy.ToLower().Equals("status"))
                {
                    if (isDescending is false) data = data.OrderBy(d => d.Status).ToList();
                    else data = data.OrderBy(d => d.Status).ToList();
                }
            }

            int recordCount = data.Count();
            var result = data.Skip(pageSize * pageNumber)
                .Take(pageSize)
                .ToList();

            return (result, recordCount);
        }

        public async Task<dynamic> GetStatisticsInbound(int companyId, string serviceRoleId)
        {
            var companyGuid = await _context.Companies.Where(i => i.CompanyId == companyId).Select(x => x.Guid).SingleOrDefaultAsync();
            var data = await _context.Request
                .Where(t => 
                    t.CompanyIdTo == companyGuid && 
                    t.ServiceRoleIdFrom == Guid.Parse(serviceRoleId) &&
                    t.IsDeleted == false)
                        .ToListAsync();

            var stats = new
            {
                newRequest = data.Count(c => c.AccreditationStatusConfigId == 1),
                approvedRequest = data.Count(c => c.AccreditationStatusConfigId == 2),
                rejectedRequest = data.Count(c => c.AccreditationStatusConfigId == 3)
            };

            return stats;
        }

        public async Task<dynamic> GetStatisticsOutbound(int companyId, string serviceRoleId)
        {
            var companyGuid = await _context.Companies.Where(i => i.CompanyId == companyId).Select(x => x.Guid).SingleOrDefaultAsync();
            var data = await _context.Request
                .Where(t =>
                    t.CompanyIdFrom == companyGuid &&
                    t.ServiceRoleIdTo == Guid.Parse(serviceRoleId) &&
                    t.IsDeleted == false)
                        .ToListAsync();

            var stats = new
            {
                newRequest = data.Count(c => c.AccreditationStatusConfigId == 1),
                approvedRequest = data.Count(c => c.AccreditationStatusConfigId == 2),
                rejectedRequest = data.Count(c => c.AccreditationStatusConfigId == 3)
            };

            return stats;
        }

        public async Task<List<xgca.entity.Models.Request>> ActivateDeactivateRequest(List<Guid> requestIds, bool status)
        {
            var lstRequest = await _context.Request.Where(i => requestIds.Contains(i.Guid)).ToListAsync();
            lstRequest.ForEach(i => { i.IsActive = status; });

            _context.Request.UpdateRange(lstRequest);
            await _context.SaveChangesAsync(null, true);

            return lstRequest;
        }

        public async Task<(List<GetRequestModel>, int)> GetRequestList(string bound, int pageSize, int pageNumber, Guid companyGuid, Guid serviceRoleGuid, string companyName, string companyAddress, string companyCountryName, string companyStateCityName, string portAreaResponsibility, string truckAreaResponsibility, string sortOrder, string sortBy, string quickSearch)
        {
            //Filter Company Info Either From or To
            Guid defaultGuid = Guid.NewGuid();
            var companyRequestInfo = await (from r in _context.Request.Include(i => i.PortArea).Include(t => t.TruckArea)
                                            .Include(i => i.AccreditationStatusConfig).AsNoTracking()
                                            join coFrom in _context.Companies.Include(i => i.Addresses).AsNoTracking() on r.CompanyIdFrom equals coFrom.Guid
                                            join coTo in _context.Companies.Include(i => i.Addresses).AsNoTracking() on r.CompanyIdTo equals coTo.Guid
                                            where r.IsDeleted == false 
                                                && (bound.ToUpper() == "INCOMING" ? r.CompanyIdTo : defaultGuid) == (bound.ToUpper() == "INCOMING" ? companyGuid : defaultGuid)
                                                && (bound.ToUpper() == "OUTGOING" ? r.CompanyIdFrom : defaultGuid) == (bound.ToUpper() == "OUTGOING" ? companyGuid : defaultGuid)
                                                && (bound.ToUpper() == "INCOMING" ? r.ServiceRoleIdTo : defaultGuid) == (bound.ToUpper() == "INCOMING" ? serviceRoleGuid : defaultGuid)
                                                && (bound.ToUpper() == "OUTGOING" ? r.ServiceRoleIdFrom : defaultGuid) == (bound.ToUpper() == "OUTGOING" ? serviceRoleGuid : defaultGuid)
                                                && (portAreaResponsibility.Length != 0? _context.PortArea.Where(i => i.PortCode.ToUpper().Contains(portAreaResponsibility.ToUpper()))
                                                                                                         .Select(x => x.RequestId)
                                                                                                         .Contains(r.RequestId)
                                                                                        : true)
                                                && (truckAreaResponsibility.Length != 0 ? _context.TruckArea.Where(i => i.CountryName.ToUpper().Contains(truckAreaResponsibility.ToUpper()))
                                                                                                        .Select(x => x.RequestId)
                                                                                                        .Contains(r.RequestId)
                                                                                        : true)
                                            select new 
                                            {
                                                
                                               RequestInfo = r
                                             , CompanyInfo = (bound.ToUpper() == "INCOMING" ? coFrom : coTo)
                                              
                                            }).ToListAsync();

            var requestListInfo = (from r in companyRequestInfo
                                   where r.CompanyInfo.CompanyName.ToUpper().Contains(companyName.ToUpper())
                                        && r.CompanyInfo.Addresses.CountryName.ToUpper().Contains(companyCountryName.ToUpper())
                                        && (r.CompanyInfo.Addresses.StateName + r.CompanyInfo.Addresses.CityName).ToUpper().Contains(companyStateCityName.ToUpper())
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
                                        , IsActive = r.RequestInfo.IsActive
                                        , PortAreaList = String.Join(" / ", r.RequestInfo.PortArea.Select(i => i.PortCode))
                                        , TruckAreaList = String.Join(" / ", r.RequestInfo.TruckArea.Select(i => i.CountryName))
                                        , CompanyGuid = r.CompanyInfo.Guid
                                        , CompanyName = r.CompanyInfo.CompanyName 
                                        , CompanyCountryName = r.CompanyInfo.Addresses.CountryName
                                        , CompanyStateCityName = (r.CompanyInfo.Addresses.StateName + "/" + r.CompanyInfo.Addresses.CityName)
                                        , CompanyFullAddress = r.CompanyInfo.Addresses.FullAddress
                                   }).ToList();

            requestListInfo = requestListInfo.Where(i => (i.CompanyName + i.CompanyCountryName + i.CompanyStateCityName + i.CompanyFullAddress + i.PortAreaList + i.TruckAreaList).ToUpper().Contains(quickSearch.ToUpper())).ToList();

            if (sortOrder.Equals("asc")) requestListInfo = requestListInfo.OrderBy(i => typeof(GetRequestModel).GetProperty(sortBy).GetValue(i).ToString()).ToList(); //Ascending
            if (sortOrder.Equals("desc")) requestListInfo = requestListInfo.OrderByDescending(i => typeof(GetRequestModel).GetProperty(sortBy).GetValue(i).ToString()).ToList(); //Descending

            var recordCount = requestListInfo.Count();
            requestListInfo = requestListInfo.Skip(pageSize * (pageNumber)).Take(pageSize).ToList();

            return (requestListInfo, recordCount);
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
