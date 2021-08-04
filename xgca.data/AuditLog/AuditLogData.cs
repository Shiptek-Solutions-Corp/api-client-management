using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using xgca.entity;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using LinqKit;
using System.Collections;
using Microsoft.AspNetCore.Http;

namespace xgca.data.AuditLog
{
    public interface IAuditLogData
    {
        Task<bool> Create(entity.Models.AuditLog obj);
        Task<List<entity.Models.AuditLog>> List();
        Task<entity.Models.AuditLog> Retrieve(int key);
        Task<int> GetIdByGuid(Guid key);
        Task<(ICollection, int)> ListPaginate(
            string tableName, 
            int keyFieldId,
            DateTime createdDateFrom,
            DateTime createdDateTo,
            string action,
            string username,
            string orderBy,
            string search,
            int pageNumbe,
            int pageSize);

        Task<List<entity.Models.AuditLog>> ListByTableName(string tableName);
        Task<List<entity.Models.AuditLog>> ListByTableNameAndKeyFieldId(string tableName, int keyFieldId);
        Task<List<entity.Models.AuditLog>> GetCompanyServiceRoleLogs(string type, int[] ids, int keyField);
        Task<bool> Create(List<entity.Models.AuditLog> obj);
        Task<List<int>> GetCreatedByIds(string tableName, int id);

    }
    public class AuditLogData : IMaintainable<entity.Models.AuditLog>, IAuditLogData
    {
        private readonly IXGCAContext _context;
        private readonly IHttpContextAccessor contextAccessor;
        public AuditLogData(IXGCAContext context, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            this.contextAccessor = contextAccessor;
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

        public async Task<List<int>> GetCreatedByIds(string tableName, int id)
        {
            List<int> ids = await _context.AuditLogs.AsNoTracking()
                .Where(x => x.TableName == tableName && x.KeyFieldId == id)
                .Select(c => c.CreatedBy).Distinct()
                .ToListAsync();

            return ids;
        }

        public async Task<(ICollection, int)> ListPaginate(string tableName, int keyFieldId, DateTime createdDateFrom, DateTime createdDateTo, string action, string username, string orderBy, string search, int pageNumber, int pageSize)
        {
            var isFromCms = contextAccessor.HttpContext.User.Claims.Any(t => t.Type == "custom:isCMS" && t.Value.Contains("1"));

            DateTime currentDate = DateTime.UtcNow;
            if (createdDateFrom == default)
            {
                createdDateFrom = currentDate;
                createdDateTo = currentDate;
            }

            var data = await (from a in _context.AuditLogs
                              join u in _context.Users on a.CreatedBy equals u.UserId into newUserList
                              from ul in newUserList.DefaultIfEmpty()
                              where (
                                      a.AuditLogAction.ToLower().Contains(search) ||
                                      ul.Username.ToLower().Contains(search)
                                    ) &&
                                    a.TableName.ToLower().Contains(tableName.ToLower())
                                    && a.KeyFieldId.ToString().Contains(keyFieldId.ToString())
                                    && a.AuditLogAction.ToLower().Contains(action.ToLower())
                                    && ul.Username.ToLower().Contains(username.ToLower())
                                   && (
                                   (createdDateFrom.Date == currentDate.Date ? currentDate.Date : a.CreatedOn.Date) >= (createdDateFrom.Date == currentDate.Date ? currentDate.Date : createdDateFrom.Date)
                                    && (createdDateTo.Date == currentDate.Date ? currentDate.Date : a.CreatedOn.Date) <= (createdDateTo.Date == currentDate.Date ? currentDate.Date : createdDateTo.Date)
                                   )
                              select new {
                                a.CreatedOn,
                                Module = isFromCms && tableName.Equals("company") ? "Manage Account" : "--",
                                SubModule = isFromCms && tableName.Equals("company") ? "Company" : "--",
                                a.AuditLogAction,
                                Description = "--",
                                Username = ul.Username ?? "System",
                                a.NewValue,
                                a.OldValue,
                              }).ToListAsync();


            int recordCount = data.Count();
            var result = data.Skip(pageSize * pageNumber)
                .Take(pageSize)
                .ToList();

            return (result, recordCount);
        }
    }
}
