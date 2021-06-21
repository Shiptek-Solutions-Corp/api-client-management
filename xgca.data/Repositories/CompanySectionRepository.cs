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
    public interface ICompanySectionRepository : IRepository<CompanySections>, IRetrievableRepository<CompanySections>
    {
        Task<(CompanySections, string)> GetBySectionCode(string sectionCode);
        Task<(List<CompanySections>, string)> GetListByCompanyId(int companyId);
        Task<(List<CompanySections>, string)> CreateCompanySections(List<CompanySections> obj);
        Task<(bool, string)> UpdateMultipleStatusByCompanyId(CompanySections obj);
        Task<(CompanySections, string)> SaveAsDraft(CompanySections obj);
        Task<(CompanySections, string)> UpdateStatus(CompanySections obj);
        Task<(CompanySections, string)> UpdateStatusByCompanyId(CompanySections obj);
        Task<(bool, string)> CheckIfCompanyHaveCompanySections(int companyId);

    }
    public class CompanySectionRepository : ICompanySectionRepository
    {
        private readonly IXGCAContext _context;
        public CompanySectionRepository(IXGCAContext _context)
        {
            this._context = _context;
        }

        public async Task<(CompanySections, string)> Create(CompanySections obj)
        {
            await _context.CompanySections.AddAsync(obj);
            var result = await _context.SaveChangesAsync();
            return (result > 0)
                ? (obj, "Company Section created successfully")
                : (null, "Error in creating company sction");
        }

        public async Task<(List<CompanySections>, string)> CreateCompanySections(List<CompanySections> obj)
        {
            await _context.CompanySections.AddRangeAsync(obj);
            var result = await _context.SaveChangesAsync();
            return (result > 0)
                ? (obj, "Company Sections created successfully")
                : (null, "Error in creating company sections");
        }

        public async Task<(bool, string)> Delete(CompanySections obj)
        {
            var record = await _context.CompanySections
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
                ? (true, "Company Section deleted successfully")
                : (false, "Error in deleting company section");
        }

        public async Task<(CompanySections, string)> Get(string id)
        {
            var record = await _context.CompanySections.AsNoTracking()
                .Include(i => i.SectionCodeNavigation)
                .Include(i => i.SectionStatusCodeNavigation)
                .Where(x => x.Guid == Guid.Parse(id) && x.IsDeleted == false)
                .FirstOrDefaultAsync();

            return (record is null)
                ? (null, "Record does not exists or may have been deleted")
                : (record, "Company Section retrieved");
        }

        public async Task<(CompanySections, string)> GetBySectionCode(string sectionCode)
        {
            var record = await _context.CompanySections.AsNoTracking()
                .Include(i => i.SectionCodeNavigation)
                .Include(i => i.SectionStatusCodeNavigation)
                .Where(x => x.SectionStatusCode == sectionCode && x.IsDeleted == false)
                .FirstOrDefaultAsync();

            return (record is null)
                ? (null, "Record does not exists or may have been deleted")
                : (record, "Company Section retrieved");
        }

        public async Task<(List<CompanySections>, string)> GetListByCompanyId(int companyId)
        {
            var records = await _context.CompanySections.AsNoTracking()
                .Include(i => i.SectionCodeNavigation)
                .Include(i => i.SectionStatusCodeNavigation)
                .Where(x => x.CompanyId == companyId && x.IsDeleted == false)
                .ToListAsync();

            return (records is null)
                ? (null, "Records does not exists or may have been deleted")
                : (records, "Company Sections retrieved");
        }

        public Task<(string, string)> GetGuidById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<(int, string)> GetIdByGuid(string id)
        {
            throw new NotImplementedException();
        }

        public Task<(List<CompanySections>, string)> List()
        {
            throw new NotImplementedException();
        }

        public Task<(CompanySections, string)> Update(CompanySections obj)
        {
            throw new NotImplementedException();
        }

        public async Task<(bool, string)> UpdateMultipleStatusByCompanyId(CompanySections obj)
        {
            var records = await _context.CompanySections
                .Where(x => x.CompanyId == obj.CompanyId && x.IsDeleted == false)
                .ToListAsync();

            if (records.Count == 0)
            {
                return (false, "Record(s) does not exists or may have been deleted");
            }

            records.ForEach(e =>
            {
                e.SectionStatusCode = obj.SectionStatusCode;
                e.UpdatedBy = obj.UpdatedBy;
                e.UpdatedOn = e.UpdatedOn;
            });

            var result = await _context.SaveChangesAsync();

            return (result > 0)
                ? (true, "Company Sections statuses updated successfully")
                : (false, "Error in updating company section statuses");
        }

        public async Task<(CompanySections, string)> SaveAsDraft(CompanySections obj)
        {
            var record = await _context.CompanySections
                .Where(x => x.Guid == obj.Guid && x.IsDeleted == false)
                .FirstOrDefaultAsync();

            if (record is null)
            {
                return (null, "Record does not exists or may have been deleted");
            }

            record.IsDraft = true;
            record.UpdatedBy = obj.UpdatedBy;
            record.UpdatedOn = obj.UpdatedOn;

            var result = await _context.SaveChangesAsync();

            return (result > 0)
                ? (obj, "Company Section updated as draft successfully")
                : (null, "Error in updating Company Sections as draft");
        }

        public async Task<(CompanySections, string)> UpdateStatus(CompanySections obj)
        {
            var record = await _context.CompanySections
                .Where(x => x.Guid == obj.Guid && x.IsDeleted == false)
                .FirstOrDefaultAsync();

            if (record is null)
            {
                return (null, "Record does not exists or may have been deleted");
            }

            record.SectionStatusCode = obj.SectionStatusCode;
            record.UpdatedBy = obj.UpdatedBy;
            record.UpdatedOn = obj.UpdatedOn;

            var result = await _context.SaveChangesAsync();
            return (result > 0)
                ? (obj, "Company Section status updated successfully")
                : (null, "Error in updating Company Section status");
        }

        public async Task<(bool, string)> CheckIfCompanyHaveCompanySections(int companyId)
        {
            var records = await _context.CompanySections.AsNoTracking()
                .Where(x => x.CompanyId == companyId && x.IsDeleted == false && x.IsActive == true)
                .ToListAsync();

            return (records.Count > 0)
                ? (true, "Company sections exists")
                : (false, "Company sections does not exists");
        }

        public async Task<(CompanySections, string)> UpdateStatusByCompanyId(CompanySections obj)
        {
            var record = await _context.CompanySections
                .Where(x => x.CompanyId == obj.CompanyId && x.SectionCode == obj.SectionCode && x.IsDeleted == false)
                .FirstOrDefaultAsync();

            if (record is null)
            {
                return (null, "Record does not exists or may have been deleted");
            }

            record.SectionStatusCode = obj.SectionStatusCode;
            record.UpdatedBy = obj.UpdatedBy;
            record.UpdatedOn = obj.UpdatedOn;

            var result = await _context.SaveChangesAsync();
            return (result > 0)
                ? (record, "Company Section status updated successfully")
                : (null, " Error in updating Company Section status");
        }
    }
}
