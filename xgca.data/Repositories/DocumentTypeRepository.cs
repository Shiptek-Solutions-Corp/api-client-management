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
    public interface IDocumentTypeRepository : IRepository<DocumentType>, IRetrievableRepository<DocumentType>
    {
        Task<(List<DocumentType>, string)> ListExcept(List<string> documentTypeCodes);
    }
    public class DocumentTypeRepository : IDocumentTypeRepository
    {
        private readonly IXGCAContext _context;

        public DocumentTypeRepository(IXGCAContext _context)
        {
            this._context = _context;
        }

        public async Task<(DocumentType, string)> Create(DocumentType obj)
        {
            await _context.DocumentTypes.AddAsync(obj);
            var result = await _context.SaveChangesAsync();
            return (result > 0)
                ? (obj, "Document Type created successfully")
                : (null, "Error in creating document type");
        }

        public async Task<(bool, string)> Delete(DocumentType obj)
        {
            var record = await _context.DocumentTypes
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
                ? (true, "Document Type deleted successfully")
                : (false, "Error in deleting document type");
        }

        public async Task<(DocumentType, string)> Get(string id)
        {
            var record = await _context.DocumentTypes.AsNoTracking()
                .Include(i => i.DocumentCategoryCodeNavigation)
                .Where(x => x.Guid == Guid.Parse(id) && x.IsDeleted == false)
                .FirstOrDefaultAsync();

            return (record is null)
                ? (null, "Record does not exists or may have been deleted")
                : (record, "Document Type retrieved successfully");

        }

        public async Task<(string, string)> GetGuidById(int id)
        {
            string guid = await _context.DocumentTypes.AsNoTracking()
                .Where(x => x.DocumentTypeId == id && x.IsDeleted == false)
                .Select(c => c.Guid.ToString())
                .FirstOrDefaultAsync();

            return (guid is null)
                ? (null, "Record does not exists or may have been deleted")
                : (guid, "GUID retrieved");
        }

        public async Task<(int, string)> GetIdByGuid(string id)
        {
            int intId = await _context.DocumentTypes.AsNoTracking()
                .Where(x => x.Guid == Guid.Parse(id) && x.IsDeleted == false)
                .Select(c => c.DocumentTypeId)
                .FirstOrDefaultAsync();

            return (intId == 0)
                ? (0, "Record does not exists or may have been deleted")
                : (intId, "DocumentType Id retrieved");
        }

        public async Task<(List<DocumentType>, string)> List()
        {
            var records = await _context.DocumentTypes.AsNoTracking()
                .Include(i => i.DocumentCategoryCodeNavigation)
                .Where(x => x.IsActive == true && x.IsDeleted == false)
                .ToListAsync();

            return (records, "Document Type listed successfully");
        }

        public async Task<(List<DocumentType>, string)> ListExcept(List<string> documentTypeCodes)
        {
            var records = await _context.DocumentTypes.AsNoTracking()
                .Include(i => i.DocumentCategoryCodeNavigation)
                .Where(x => !documentTypeCodes.Contains(x.DocumentCategoryCode) && x.IsActive == true && x.IsDeleted == false)
                .ToListAsync();

            return (records, "Document Type listed successfully");
        }

        public async Task<(DocumentType, string)> Update(DocumentType obj)
        {
            var record = await _context.DocumentTypes
                .Where(x => x.Guid == obj.Guid && x.IsDeleted == false)
                .FirstOrDefaultAsync();

            if (record is null)
            {
                return (null, "Record does not exists or may have been deleted");
            }

            record.Name = obj.Name;
            record.Description = obj.Description;
            record.IsActive = obj.IsActive;
            record.UpdatedBy = obj.UpdatedBy;
            record.UpdatedOn = obj.UpdatedOn;

            var result = await _context.SaveChangesAsync();

            return (result > 0)
                ? (obj, "Document Type updated successfully")
                : (null, "Error in updating Document Type");
        }
    }
}
