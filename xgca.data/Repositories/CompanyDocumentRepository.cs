using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using xgca.entity;
using xgca.entity.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace xgca.data.Repositories
{
    public interface ICompanyDocumentRepository : IRepository<CompanyDocuments>
    {
        Task<(List<CompanyDocuments>, string)> CreateCompanyDocuments(List<CompanyDocuments> docs);
        Task<(bool, string)> DeleteCompanyDocuments(List<Guid> ids, string username);
        Task<(List<CompanyDocuments>, string)> UpdateCompanyDocuments(List<CompanyDocuments> docs);
    }
    public class CompanyDocumentRepository : ICompanyDocumentRepository
    {
        private readonly IXGCAContext _context;

        public CompanyDocumentRepository(IXGCAContext _context)
        {
            this._context = _context;
        }

        public async Task<(CompanyDocuments, string)> Create(CompanyDocuments obj)
        {
            await _context.CompanyDocuments.AddAsync(obj);
            var result = await _context.SaveChangesAsync();
            return (result > 0)
                ? (obj, "Company Document created successfully")
                : (null, "Error in creating Company Document");
        }

        public async Task<(List<CompanyDocuments>, string)> CreateCompanyDocuments(List<CompanyDocuments> docs)
        {
            await _context.CompanyDocuments.AddRangeAsync(docs);
            var result = await _context.SaveChangesAsync();
            return (result > 0)
                ? (docs, "Company Documents created successfulyy")
                : (null, "Error in creating Company Documents");
        }

        public async Task<(bool, string)> Delete(CompanyDocuments obj)
        {
            var record = await _context.CompanyDocuments
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
                ? (true, "Company Document deleted successfully")
                : (false, "Error in deleting Company Document");
        }

        public async Task<(bool, string)> DeleteCompanyDocuments(List<Guid> ids, string username)
        {
            var records = await _context.CompanyDocuments
                .Where(x => ids.Contains(x.Guid) && x.IsDeleted == false)
                .ToListAsync();

            if (records.Count == 0)
            {
                return (false, "Record(s) does not exists or may have been deleted");
            }

            records.ForEach(e =>
            {
                e.IsDeleted = true;
                e.DeletedBy = username;
                e.DeletedOn = DateTime.UtcNow;
            });

            var result = await _context.SaveChangesAsync();

            return (result > 0)
                ? (true, "Company Document(s) deleted successfully")
                : (false, "Error in deleting Company Document(s)");
        }

        public async Task<(CompanyDocuments, string)> Get(string id)
        {
            var record = await _context.CompanyDocuments.AsNoTracking()
                .Where(x => x.Guid == Guid.Parse(id) && x.IsDeleted == false)
                .FirstOrDefaultAsync();

            return (record is null)
                ? (null, "Record does not exists or may have been deleted")
                : (record, "Comany Document retrieved");
        }

        public async Task<(List<CompanyDocuments>, string)> List()
        {
            var records = await _context.CompanyDocuments.AsNoTracking()
                .Where(x => x.IsActive == true && x.IsDeleted == false)
                .ToListAsync();

            return (records, "Company Documents retrieved");
        }

        public async Task<(CompanyDocuments, string)> Update(CompanyDocuments obj)
        {
            var record = await _context.CompanyDocuments
                .Where(x => x.Guid == obj.Guid && x.IsDeleted == false)
                .FirstOrDefaultAsync();

            if (record is null)
            {
                return (null, "Record does not exists or may have been deleted");
            }

            record.DocumentDescription = obj.DocumentDescription;
            record.DocumentNo = obj.DocumentNo;
            record.DocumentTypeId = obj.DocumentTypeId;
            record.IsActive = obj.IsActive;
            record.UpdatedBy = obj.UpdatedBy;
            record.UpdatedOn = obj.UpdatedOn;

            var result = await _context.SaveChangesAsync();

            return (result > 0)
                ? (obj, "Company Document updated successfully")
                : (null, "Error in updating Company Document");
        }

        public async Task<(List<CompanyDocuments>, string)> UpdateCompanyDocuments(List<CompanyDocuments> docs)
        {
            await _context.CompanyDocuments.BulkUpdateAsync(docs);
            var result = await _context.SaveChangesAsync();
            return (result > 0)
                ? (docs, "Company Document(s) updated successfully")
                : (null, "Error in updating Company Document(s)");
        }
    }
}
