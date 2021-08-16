using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xas.data.CustomModel.TruckArea;
using xgca.entity;

namespace xas.data.accreditation.TruckArea
{
    public interface ITruckAreaData
    {
        Task<(xgca.entity.Models.TruckArea, string)> Create(xgca.entity.Models.TruckArea obj);
        Task<(xgca.entity.Models.TruckArea, string)> Update(xgca.entity.Models.TruckArea obj);
        Task<List<xgca.entity.Models.TruckArea>> List(int requestId);
        Task<(List<CustomGetTruckArea>, int)> List(int requestId, string search, string city, string state, string country, string postal, string sortBy, string sortOrder, int pageNumber, int pageSize);
        Task<(bool, string)> Delete(xgca.entity.Models.TruckArea obj);
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

        public async Task<(bool, string)> Delete(xgca.entity.Models.TruckArea obj)
        {
            var record = await _context.TruckArea
                .Where(x => x.Guid == obj.Guid && x.IsDeleted == false)
                .FirstOrDefaultAsync();

            if (record is null)
            {
                return (false, "Record does not exists or may have been deleted");
            }

            record.IsDeleted = true;
            record.DeletedBy = obj.DeletedBy;
            record.DeletedOn = obj.DeletedOn;

            var result = await _context.SaveChangesAsync();

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

        public async Task<(List<CustomGetTruckArea>, int)> List(int requestId, string search, string city, string state, string country, string postal, string sortBy, string sortOrder, int pageNumber, int pageSize)
        {
            var predicate = PredicateBuilder.New<xgca.entity.Models.TruckArea>();

            if (!(search is null))
            {
                predicate = predicate.Or(x => EF.Functions.Like(x.CountryName, $"%{search}%"));
                predicate = predicate.Or(x => EF.Functions.Like(x.StateName, $"%{search}%"));
                predicate = predicate.Or(x => EF.Functions.Like(x.CityName, $"%{search}%"));
                predicate = predicate.Or(x => EF.Functions.Like(x.PostalCode, $"%{search}%"));
            }

            if (!(city is null))
            {
                predicate = predicate.And(x => EF.Functions.Like(x.CityName, $"%{city}%"));
            }

            if (!(state is null))
            {
                predicate = predicate.And(x => EF.Functions.Like(x.StateName, $"%{state}%"));
            }

            if (!(country is null))
            {
                predicate = predicate.And(x => EF.Functions.Like(x.CountryName, $"%{country}%"));
            }

            if (!(postal is null))
            {
                predicate = predicate.And(x => EF.Functions.Like(x.PostalCode, $"%{postal}%"));
            }

            predicate = predicate.And(x => x.RequestId == requestId && x.IsActive == 1 && x.IsDeleted == false);

            var recordCount = await _context.TruckArea.AsNoTracking().Where(predicate).CountAsync();

            IQueryable<xgca.entity.Models.TruckArea> tempRecords = _context.TruckArea
                .Where(predicate);


            if (sortOrder.Equals("asc"))
            {
                if (sortBy.ToLower().Equals("city")) tempRecords = tempRecords.OrderBy(o => o.CityName);
                if (sortBy.ToLower().Equals("state")) tempRecords = tempRecords.OrderBy(o => o.StateName);
                if (sortBy.ToLower().Equals("country")) tempRecords = tempRecords.OrderBy(o => o.CountryName);
                if (sortBy.ToLower().Equals("postal")) tempRecords = tempRecords.OrderBy(o => o.PostalCode);
            }

            if (sortOrder.Equals("desc"))
            {
                if (sortBy.ToLower().Equals("city")) tempRecords = tempRecords.OrderByDescending(o => o.CityName);
                if (sortBy.ToLower().Equals("state")) tempRecords = tempRecords.OrderByDescending(o => o.StateName);
                if (sortBy.ToLower().Equals("country")) tempRecords = tempRecords.OrderByDescending(o => o.CountryName);
                if (sortBy.ToLower().Equals("postal")) tempRecords = tempRecords.OrderByDescending(o => o.PostalCode);
            }

            var records = await tempRecords
                .Select(c => new CustomGetTruckArea
                {
                    Guid = c.Guid.ToString(),
                    CountryId = c.CountryId,
                    CountryName = c.CountryName,
                    StateId = c.StateId,
                    StateName = c.StateName,
                    CityId = c.CityId,
                    CityName = c.CityName,
                    PostalCode = c.PostalCode,
                    PostalId = c.PostalId
                })
                .Skip(pageSize * pageNumber)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();

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
