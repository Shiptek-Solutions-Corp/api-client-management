using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using xgca.entity;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using LinqKit;

namespace xgca.data.AuditLog
{
    public interface IAuditLogData
    {
        Task<bool> Create(entity.Models.AuditLog obj);
        Task<List<entity.Models.AuditLog>> List();
        Task<entity.Models.AuditLog> Retrieve(int key);
        Task<int> GetIdByGuid(Guid key);
        Task<List<entity.Models.AuditLog>> ListByTableName(string tableName);
        Task<List<entity.Models.AuditLog>> ListByTableNameAndKeyFieldId(string tableName, int keyFieldId);
        Task<List<entity.Models.AuditLog>> GetCompanyServiceRoleLogs(string type, int[] ids, int keyField);
        Task<bool> Create(List<entity.Models.AuditLog> obj);

    }
    public class AuditLogData : IMaintainable<entity.Models.AuditLog>, IAuditLogData
    {
        private readonly IXGCAContext _context;
        public AuditLogData(IXGCAContext context)
        {
            _context = context;
        }

        public async Task<bool> Create(entity.Models.AuditLog obj)
        {
            await _context.AuditLogs.AddAsync(obj);
            var result = await _context.SaveChangesAsync();
            return result > 0 ? true : false;
        }

        public async Task<int> GetIdByGuid(Guid key)
        {
            var auditLogs = await _context.AuditLogs
                .Where(al => al.Guid == key)
                .FirstOrDefaultAsync();
            return auditLogs.AuditLogId;
        }

        public async Task<List<entity.Models.AuditLog>> GetCompanyServiceRoleLogs(string type, int[] ids, int keyField)
        {
            var predicate = PredicateBuilder.New<entity.Models.AuditLog>();

            if (ids.Length > 0)
            {
                predicate = predicate.And(a => ids.Contains(a.CreatedBy));
            }

            if (keyField > 0)
            {
                predicate = predicate.And(a => a.KeyFieldId == keyField);
            }

            List<entity.Models.AuditLog> auditLogs = await _context.AuditLogs
                .Where(predicate)
                .Where(a => a.TableName == type)
                .OrderByDescending(c => c.CreatedOn)
                .ToListAsync();

            return auditLogs;
        }

        public async Task<List<entity.Models.AuditLog>> List()
        {
            var data = await _context.AuditLogs.OrderByDescending(c => c.CreatedOn).ToListAsync();
            return data;
        }

        public async Task<List<entity.Models.AuditLog>> ListByTableName(string tableName)
        {
            var data = await _context.AuditLogs
                .Where(c => c.TableName == tableName)
                .OrderByDescending(c => c.CreatedOn)
                .ToListAsync();
            return data;
        }

        public async Task<List<entity.Models.AuditLog>> ListByTableNameAndKeyFieldId(string tableName, int keyFieldId)
        {
            var data = await _context.AuditLogs
                    .Where(c => c.TableName == tableName && c.KeyFieldId == keyFieldId)
                    .OrderByDescending(c => c.CreatedOn)
                    .ToListAsync();
            return data;
        }

        public async Task<entity.Models.AuditLog> Retrieve(int key)
        {
            var data = await _context.AuditLogs
                .Where(c => c.AuditLogId == key)
                .FirstOrDefaultAsync();
            return data;
        }

        public Task<bool> Update(entity.Models.AuditLog obj)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Create(List<entity.Models.AuditLog> obj)
        {
            _context.AuditLogs.AddRange(obj);
            var result = await _context.SaveChangesAsync();
            return result > 0 ? true : false;
        }
    }
}
