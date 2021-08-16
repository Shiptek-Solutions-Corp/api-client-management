using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using xgca.entity.Models;
using xgca.entity;

namespace xas.data.accreditation.Request
{
    public interface IRequestData
    {
        Task<(xgca.entity.Models.Request, int)> Create(xgca.entity.Models.Request obj);
        Task Create(List<xgca.entity.Models.Request> entity);
        Task<dynamic> GetStatusStatisticsInbound(int companyId);
        Task<dynamic> GetStatusStatisticsOutbound(int companyId);
        Task UpdateAccreditationRequest(Guid requestId, Guid companyIdTo, int status);
        Task<ICollection<xgca.entity.Models.Request>> GetAllIncommingRequest(Guid CompanyId);
        Task<ICollection<xgca.entity.Models.Request>> GetAllOutgoingRequest(Guid CompanyId);
        Task<List<xgca.entity.Models.AccreditationStatusConfig>> GetAccreditationStatus();
        Task<int> CheckRequestIfExist(Guid CompanyIdFrom, Guid CompanyIdTo);
        Task<int> CheckRequestIfDeleted(Guid CompanyIdFrom, Guid CompanyIdTo);
        Task<object> ValidateIfRequestStatusUpdateIsAllowed(Guid requestId, Guid companyId);
        Task DeleteRequest(Guid companyId, Guid requestId);
        Task ReAddRequest(xgca.entity.Models.Request data);
        Task<int> CheckRequestIfExistById(Guid CompanyIdTo, Guid RequestId);
        Task<int> GetRequestIdByGuid(Guid requestId);
        Task<int> GetRequestId(string companyIdFrom, string companyIdTo);
        Task<(ICollection, int)> GetAllTruckingIncomingRequest(Guid CompanyId, string serviceRoleId, string quicksearch, string company, string address, string truckArea, string orderBy, bool isDescending, int status, int pageNumber, int pageSize);
        Task<(ICollection, int)> GetAllTruckingOutgoingRequest(Guid CompanyId, string serviceRoleId, string quicksearch, string company, string address, string truckArea, string orderBy, bool isDescending, int status, int pageNumber, int pageSize);
        Task<dynamic> GetStatisticsInbound(Guid companyId, string serviceRoleId);
        Task<dynamic> GetStatisticsOutbound(Guid companyId, string serviceRoleId);
    }

    public class RequestData: IRequestData
    {
        private readonly IXGCAContext _context;

        public RequestData(IXGCAContext context)
        { 
            _context = context;
        }
        public async Task Create(List<xgca.entity.Models.Request> entity)
        {
            _context.Request.AddRange(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<(xgca.entity.Models.Request, int)> Create(xgca.entity.Models.Request obj)
        {
            await _context.Request.AddAsync(obj);

            int records = await _context.SaveChangesAsync();
            return (obj, records);
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

        public async Task UpdateAccreditationRequest(Guid requestId, Guid companyIdTo, int status)
        {
            var intRequestId = await _context.Request.Where(t => t.Guid == requestId && t.IsDeleted == false).FirstOrDefaultAsync();
            var d = await _context.Request.Where(t => t.RequestId == intRequestId.RequestId && t.CompanyIdTo == companyIdTo && t.IsDeleted == false).FirstOrDefaultAsync();
            d.AccreditationStatusConfigId = status;
            await _context.SaveChangesAsync();
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

        public async Task<List<xgca.entity.Models.AccreditationStatusConfig>> GetAccreditationStatus()
        {
            var data = await _context.AccreditationStatusConfig.Select(t => new xgca.entity.Models.AccreditationStatusConfig
            { 
                AccreditationStatusConfigId = t.AccreditationStatusConfigId,
                Value = t.Value
            }).ToListAsync();

            return data;
        }

        public async Task<int> CheckRequestIfExist(Guid CompanyIdFrom, Guid CompanyIdTo)
        {
            var data = await _context.Request.Where(t => t.CompanyIdFrom == CompanyIdFrom && t.CompanyIdTo == CompanyIdTo && t.IsDeleted == false).ToListAsync();
            return data.Count;
        }

        public async Task<int> CheckRequestIfDeleted(Guid CompanyIdFrom, Guid CompanyIdTo)
        {
            var data = await _context.Request.Where(t => t.CompanyIdFrom == CompanyIdFrom && t.CompanyIdTo == CompanyIdTo && t.IsDeleted == true).ToListAsync();
            return data.Count;
        }

        public async Task<object> ValidateIfRequestStatusUpdateIsAllowed(Guid requestId, Guid companyId)
        {
            var data = await _context.Request.Where(t => t.Guid == requestId && t.CompanyIdTo == companyId && t.IsDeleted == false).FirstOrDefaultAsync();
            return data;
        }

        public async Task DeleteRequest(Guid companyId, Guid requestId)
        {
            var data = await _context.Request.Where(t => t.Guid == requestId && t.IsDeleted == false).FirstOrDefaultAsync();
            data.IsDeleted = true;
            await _context.SaveChangesAsync();
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

        public async Task<(ICollection, int)> GetAllTruckingIncomingRequest(Guid CompanyId, string serviceRoleId, string quicksearch, string company, string address, string truckArea, string orderBy, bool isDescending, int status, int pageNumber, int pageSize)
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
                                ir.CompanyIdTo == CompanyId &&
                                ir.ServiceRoleIdFrom == Guid.Parse(serviceRoleId) &&
                                ir.IsDeleted == false &&
                                (status.Equals(0) ? !(ir.AccreditationStatusConfigId.Equals(0)) : ir.AccreditationStatusConfigId == status) 
                                        select new
                                        {
                                            AccreditationStatusConfig = ir.AccreditationStatusConfig,
                                            RequestId = ir.Guid,
                                            CompanyId = cd.CompanyId,
                                            CompanyName = cd.CompanyName,
                                            FullAddress = ad.FullAddress,
                                            EmailAddress = cd.EmailAddress,
                                            Fax = contact.Fax,
                                            FaxPrefix = contact.FaxPrefix,
                                            FaxPrefixId = contact.FaxPrefixId,
                                            Mobile = contact.Mobile,
                                            MobilePrefix= contact.MobilePrefix,
                                            MobilePrefixId = contact.MobilePrefixId,
                                            Phone = contact.Phone,
                                            PhonePrefix = contact.PhonePrefix,
                                            PhonePrefixId = contact.PhonePrefixId,
                                            WebsiteUrl = cd.WebsiteURL,
                                            ImageUrl = cd.ImageURL,
                                            CountryId = ad.CountryId,
                                            CountryName = ad.CountryName,
                                            Latitude = ad.Latitude,
                                            Longitude = ad.Longitude,
                                            ServiceRoleId = "",
                                            ServiceRoleName = "",
                                            Status = ir.AccreditationStatusConfigId,
                                            TruckArea = ta.CountryName,
                                            CompanyDetails = toCo,
                                            CompanyIdFrom = ir.CompanyIdFrom,
                                            CompanyIdTo = ir.CompanyIdTo,
                                            ServiceRoleIdFrom = ir.ServiceRoleIdFrom,
                                            ServiceRoleIdTo = ir.ServiceRoleIdTo
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

        public async Task<(ICollection, int)> GetAllTruckingOutgoingRequest(Guid CompanyId, string serviceRoleId, string quicksearch, string company, string address, string truckArea, string orderBy, bool isDescending, int status, int pageNumber, int pageSize)
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
                                or.CompanyIdFrom == CompanyId &&
                                or.ServiceRoleIdTo == Guid.Parse(serviceRoleId) &&
                                or.IsDeleted == false &&
                                (status.Equals(0) ? !(or.AccreditationStatusConfigId.Equals(0)) : or.AccreditationStatusConfigId == status)
                                        select new 
                                        {
                                            AccreditationStatusConfig = or.AccreditationStatusConfig,
                                            RequestId = or.Guid,
                                            CompanyId = toCo.CompanyId,
                                            CompanyName = toCo.CompanyName,
                                            FullAddress = toAd.FullAddress,
                                            EmailAddress = toCo.EmailAddress,
                                            Fax = toContact.Fax,
                                            FaxPrefix = toContact.FaxPrefix,
                                            FaxPrefixId = toContact.FaxPrefixId,
                                            Mobile = toContact.Mobile,
                                            MobilePrefix = toContact.MobilePrefix,
                                            MobilePrefixId = toContact.MobilePrefixId,
                                            Phone = toContact.Phone,
                                            PhonePrefix = toContact.PhonePrefix,
                                            PhonePrefixId = toContact.PhonePrefixId,
                                            WebsiteUrl = toCo.WebsiteURL,
                                            ImageUrl = toCo.ImageURL,
                                            CountryId = toAd.CountryId,
                                            CountryName = toAd.CountryName,
                                            Latitude = toAd.Latitude,
                                            Longitude = toAd.Longitude,
                                            ServiceRoleId = "",
                                            ServiceRoleName = "",
                                            Status = or.AccreditationStatusConfigId,
                                            TruckArea = ta.CountryName,
                                            CompanyDetails = toCo,
                                            CompanyIdFrom = or.CompanyIdFrom,
                                            CompanyIdTo = or.CompanyIdTo,
                                            ServiceRoleIdFrom = or.ServiceRoleIdFrom,
                                            ServiceRoleIdTo = or.ServiceRoleIdTo
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

        public async Task<dynamic> GetStatisticsInbound(Guid companyId, string serviceRoleId)
        {
            var data = await _context.Request
                .Where(t => 
                    t.CompanyIdTo == companyId && 
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

        public async Task<dynamic> GetStatisticsOutbound(Guid companyId, string serviceRoleId)
        {
            var data = await _context.Request
                .Where(t =>
                    t.CompanyIdFrom == companyId &&
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
    }
}
