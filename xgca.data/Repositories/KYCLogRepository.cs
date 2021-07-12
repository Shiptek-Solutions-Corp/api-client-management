using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using xgca.entity;

namespace xgca.data.Repositories
{
    public interface IKYCLogRepository : IRepository<entity.Models.KYCLog>, IRetrievableRepository<entity.Models.KYCLog>, IBulkRepository<entity.Models.KYCLog>
    {
        Task<(List<entity.Models.KYCLog>, string)> ListByCompanyId(int companyId);
        Task<(entity.Models.KYCLog, string)> Get(int companySectionId, int companyId);
        Task<(entity.Models.KYCLog, string)> GetLatest(string id);
        Task<(entity.Models.KYCLog, string)> GetLatest(int companySectionId, int companyId);

    }
    public class KYCLogRepository : IKYCLogRepository
    {
        private readonly IXGCAContext _context;
        public KYCLogRepository(IXGCAContext _context)
        {
            this._context = _context;
        }

        public async Task<(List<entity.Models.KYCLog>, string)> BulkCreate(List<entity.Models.KYCLog> list)
        {
            await _context.KYCLogs.AddRangeAsync(list);
            var result = await _context.SaveChangesAsync();
            return (result > 0)
                ? (list, "KYC Logs created successfully")
                : (null, "Error in creating KYC Logs");
        }

        public Task<(bool, string)> BulkDelete(List<string> ids, string username)
        {
            throw new NotImplementedException();
        }

        public Task<(List<entity.Models.KYCLog>, string)> BulkUpdate(List<entity.Models.KYCLog> list)
        {
            throw new NotImplementedException();
        }

        public async Task<(entity.Models.KYCLog, string)> Create(entity.Models.KYCLog obj)
        {
            await _context.KYCLogs.AddAsync(obj);
            var result = await _context.SaveChangesAsync();
            return (result > 0)
                ? (obj, "KYC Log created successfully")
                : (null, "Error in creating KYC Log");
        }

        public Task<(bool, string)> Delete(entity.Models.KYCLog obj)
        {
            throw new NotImplementedException();
        }

        public async Task<(entity.Models.KYCLog, string)> Get(string id)
        {
            var record = await _context.KYCLogs.AsNoTracking()
                .Where(x => x.Guid == Guid.Parse(id))
                .FirstOrDefaultAsync();

            return (record is null)
                ? (null, "Record does not exists or may have been deleted")
                : (record, "KYC Log retrieved successfully");
        }

        public async Task<(entity.Models.KYCLog, string)> Get(int companySectionId, int companyId)
        {
            var record = await _context.KYCLogs.AsNoTracking()
                .Where(x => x.CompanySectionsId == companySectionId && x.CompanyId == companyId)
                .FirstOrDefaultAsync();

            return (record is null)
                ? (null, "Record does not exists or may have been deleted")
                : (record, "KYC Log retrieved successfully");
        }

        public async Task<(string, string)> GetGuidById(int id)
        {
            string guid = await _context.KYCLogs.AsNoTracking()
                .Where(x => x.KYCLogId == id)
                .Select(c => c.Guid.ToString())
                .FirstOrDefaultAsync();

            return (guid is null)
                ? (null, "Record does not exists or may have been deleted")
                : (guid, "KYC Log GUID retrieved successfully");
        }

        public async Task<(int, string)> GetIdByGuid(string id)
        {
            int intId = await _context.KYCLogs.AsNoTracking()
                .Where(x => x.Guid == Guid.Parse(id))
                .Select(c => c.KYCLogId)
                .FirstOrDefaultAsync();

            return (intId == 0)
                ? (0, "Record does not exists or may have been deleted")
                : (intId, "KYC Log Id retrieved successfully");
        }

        public async Task<(entity.Models.KYCLog, string)> GetLatest(string id)
        {
            var record = await _context.KYCLogs.AsNoTracking()
                .Where(x => x.Guid == Guid.Parse(id))
                .OrderByDescending(o => o.CreatedOn)
                .FirstOrDefaultAsync();

            return (record is null)
                ? (null, "Record does not exists or may have been deleted")
                : (record, "KYC Log retrieved successfully");
        }

        public async Task<(entity.Models.KYCLog, string)> GetLatest(int companySectionId, int companyId)
        {
            var record = await _context.KYCLogs.AsNoTracking()
                .Where(x => x.CompanySectionsId == companySectionId && x.CompanyId == companyId)
                .OrderByDescending(o => o.CreatedOn)
                .FirstOrDefaultAsync();

            return (record is null)
                ? (null, "Record does not exists or may have been deleted")
                : (record, "KYC Log retrieved successfully");
        }

        public async Task<(List<entity.Models.KYCLog>, string)> List()
        {
            var records = await _context.KYCLogs.AsNoTracking()
                .ToListAsync();

            return (records, "KYC Logs listed successfully");
        }

        public async Task<(List<entity.Models.KYCLog>, string)> ListByCompanyId(int companyId)
        {
            var records = await _context.KYCLogs.AsNoTracking()
                .Where(x => x.CompanyId == companyId)
                .ToListAsync();

            return (records, "KYC Logs listed successfully");
        }

        public Task<(entity.Models.KYCLog, string)> Update(entity.Models.KYCLog obj)
        {
            throw new NotImplementedException();
        }
    }
}
