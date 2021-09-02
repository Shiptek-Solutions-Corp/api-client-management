using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xas.data.CustomModel.TruckArea;
using xgca.data.ViewModels.TruckArea;
using xgca.entity;

namespace xas.data.DataModel.TruckArea
{
    public interface ITruckAreaData
    { 
        Task<(xgca.entity.Models.TruckArea, string)> Create(xgca.entity.Models.TruckArea obj);
        Task<(xgca.entity.Models.TruckArea, string)> Update(xgca.entity.Models.TruckArea obj);
        Task<List<xgca.entity.Models.TruckArea>> List(int requestId);
        Task<(List<GetTruckAreaModel>, int)> List(Guid requestGuid, string search, string city, string state, string country, string postal, string sortBy, string sortOrder, int pageNumber, int pageSize);
        Task<(bool, string)> Delete(Guid truckAreaId);
        Task<(bool, string)> DeleteBulk(List<Guid> ids, string deletedBy);
    }

    public class TruckAreaData : ITruckAreaData
    {
        private readonly IXGCAContext _context;

        public TruckAreaData(IXGCAContext context)
        {
            _context = context;
        }

        public async Task Create(ICollection<xgca.entity.Models.TruckArea> entity)
        {
            await _context.TruckArea.AddRangeAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<(xgca.entity.Models.TruckArea, string)> Create(xgca.entity.Models.TruckArea obj)
        {
            await _context.TruckArea.AddAsync(obj);
            var result = await _context.SaveChangesAsync();

            return (result > 0)
                ? (obj, "New area of responsibility created successfully")
                : (null, "Error in creating area of responsibility");
        }

        public async Task Delete(int id)
        {
           var data = await _context.TruckArea
                .Where(d => d.TruckAreaId == id)
                .SingleOrDefaultAsync();

            data.IsDeleted = true;
        }

        public async Task<(bool, string)> DeleteBulk(List<Guid> ids, string deletedBy)
        {
            var records = await _context.TruckArea
                .Where(x => ids.Contains(x.Guid) && x.IsDeleted == false)
                .ToListAsync();

            if (records.Count == 0)
            {
                return (false, "Records do not exists or may have been deleted");
            }

            records.ForEach(e =>
            {
                e.IsDeleted = true;
                e.DeletedBy = deletedBy;
                e.DeletedOn = DateTime.UtcNow;
            });

            var result = await _context.SaveChangesAsync();

            return (result > 0)
                ? (true, "Records deleted successfully")
                : (false, "Error in deleting records");
        }

        public async Task<(bool, string)> Delete(Guid truckAreaId)
        {
            var record = await _context.TruckArea
                .Where(x => x.Guid == truckAreaId && x.IsDeleted == false)
                .SingleOrDefaultAsync();

            _context.TruckArea.Remove(record);
            var result = await _context.SaveChangesAsync(null, true);

            return (result > 0)
                ? (true, "Record deleted successfully")
                : (false, "Error in deleting record");
        }

        public async Task<xgca.entity.Models.TruckArea> GetById(int id)
        {
            var data = await _context.TruckArea
                .Where(d => d.TruckAreaId == id)
                .SingleOrDefaultAsync();

            return data;
        }

        public async Task<List<xgca.entity.Models.TruckArea>> List(int requestId)
        {
            var data = await _context.TruckArea
                .Where(d => d.RequestId == requestId)
                .ToListAsync();

            return data;
        }

        public async Task<(List<GetTruckAreaModel>, int)> List(Guid requestGuid, string search, string city, string state, string country, string postal, string sortBy, string sortOrder, int pageNumber, int pageSize)
        {
            int requestId = await _context.Request.Where(i => i.Guid == requestGuid).Select(x => x.RequestId).SingleOrDefaultAsync();
            var tempRecords = await (from r in _context.TruckArea
                                     where r.IsDeleted == false && r.RequestId == requestId
                                     && (r.CountryName + r.StateName + r.CityName + r.PostalCode).ToUpper().Contains(search.ToUpper())
                                     && r.CountryName.ToUpper().Contains(country.ToUpper()) 
                                     && r.StateName.ToUpper().Contains(state.ToUpper())
                                     && r.CityName.ToUpper().Contains(city.ToUpper())
                                     && r.PostalCode.ToUpper().Contains(postal.ToUpper())
                                     select new GetTruckAreaModel
                                     {
                                         CityId = r.CityId
                                         , CityName = r.CityName 
                                         , CountryId = r.CountryId 
                                         , CountryName = r.CountryName 
                                         , PostalCode = r.PostalCode 
                                         , PostalId = r.PostalId 
                                         , StateId = r.StateId 
                                         , StateName = r.StateName 
                                         , TruckAreaId = r.TruckAreaId
                                         , TruckAreaGuid = r.Guid
                                     }).ToListAsync();

            if (sortOrder.Equals("asc")) tempRecords = tempRecords.OrderBy(i => typeof(GetTruckAreaModel).GetProperty(sortBy).GetValue(i).ToString()).ToList(); //Ascending
            if (sortOrder.Equals("desc")) tempRecords = tempRecords.OrderByDescending(i => typeof(GetTruckAreaModel).GetProperty(sortBy).GetValue(i).ToString()).ToList(); //Descending

            var recordCount = tempRecords.Count();
            var records = tempRecords = tempRecords.Skip(pageSize * (pageNumber)).Take(pageSize).ToList();

            return (records, recordCount);
        }

        public async Task Update(int id, xgca.entity.Models.TruckArea entity)
        {
            var data = await _context.TruckArea
                .Where(d => d.TruckAreaId == id)
                .SingleOrDefaultAsync();

            data.CountryId = entity.CountryId;
            data.CountryName = entity.CountryName;
            data.StateId = entity.StateId;
            data.StateName = entity.StateName;
            data.CityId = entity.CityId;
            data.CityName = entity.CityName;
            data.PostalId = entity.PostalCode;
            data.Longitude = entity.Longitude;
            data.Latitude = entity.Latitude;
        }

        public async Task<(xgca.entity.Models.TruckArea, string)> Update(xgca.entity.Models.TruckArea obj)
        {
            var record = await _context.TruckArea
                .Where(x => x.Guid == obj.Guid && x.IsDeleted == false)
                .SingleOrDefaultAsync();

            if (record is null)
            {
                return (null, "Record does not exists or may have been deleted");
            }

            record.CountryId = obj.CountryId;
            record.CountryName = obj.CountryName;
            record.StateId = obj.StateId;
            record.StateName = obj.StateName;
            record.CityId = obj.CityId;
            record.CityName = obj.CityName;
            record.PostalId = obj.PostalId;
            record.PostalCode = obj.PostalCode;

            var result = await _context.SaveChangesAsync();

            return (result > 0)
                ? (obj, "Area of Responsibility updated successfully")
                : (null, "Error in updating Area of Responsibility");
        }
    }
}
