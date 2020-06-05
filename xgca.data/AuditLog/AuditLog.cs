using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using xgca.entity;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace xgca.data.AuditLog
{
    public class AuditLog : IMaintainable<entity.Models.AuditLog>, IAuditLog
    {
        private readonly IXGCAContext _context;
        public AuditLog(IXGCAContext context)
        {
            _context = context;
        }

        public async Task<bool> Create(entity.Models.AuditLog obj)
        {
            await _context.AuditLogs.AddAsync(obj);
            var result = await _context.SaveChangesAsync();
            return result > 0 ? true : false;
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
            var data = await _context.AuditLogs.Where(c => c.AuditLogId == key).FirstOrDefaultAsync();
            return data;
        }

        public Task<bool> Update(entity.Models.AuditLog obj)
        {
            throw new NotImplementedException();
        }
    }
}
