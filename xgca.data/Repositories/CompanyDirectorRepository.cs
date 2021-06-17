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
    public interface ICompanyDirectorRepository : IRepository<CompanyDirectors>, IBulkRepository<CompanyDirectors>
    {
        Task<(List<CompanyDirectors>, string)> GetByCompanyId(int companyId);
    }
    public class CompanyDirectorRepository : ICompanyDirectorRepository
    {
        private readonly IXGCAContext _context;

        public CompanyDirectorRepository(IXGCAContext _context)
        {
            this._context = _context;
        }

        public async Task<(List<CompanyDirectors>, string)> BulkCreate(List<CompanyDirectors> list)
        {
            await _context.CompanyDirectors.AddRangeAsync(list);
            var result = await _context.SaveChangesAsync();
            return (result > 0)
                ? (list, "Company Director(s) created successfully")
                : (null, "Error in creating Company Director(s)");
        }

        public async Task<(bool, string)> BulkDelete(List<string> ids, string username)
        {
            var records = await _context.CompanyDirectors
                .Where(x => ids.Contains(x.Guid.ToString()) && x.IsDeleted == false)
                .ToListAsync();

            if (records is null)
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
                ? (true, "Company Director(s) deleted successfully")
                : (false, "Error in deleting Company Director(s)");
        }

        public async Task<(List<CompanyDirectors>, string)> BulkUpdate(List<CompanyDirectors> list)
        {
            await _context.CompanyDirectors.BulkUpdateAsync(list);
            var result = await _context.SaveChangesAsync();
            return (result > 0)
                ? (list, "Company Director(s) updated successfully")
                : (null, "Error in updating Company Director(s)");
        }

        public async Task<(CompanyDirectors, string)> Create(CompanyDirectors obj)
        {
            await _context.CompanyDirectors.AddAsync(obj);
            var result = await _context.SaveChangesAsync();
            return (result > 0)
                ? (obj, "Company Director created successfully")
                : (null, "Error in creating Company Director");
        }

        public async Task<(bool, string)> Delete(CompanyDirectors obj)
        {
            var record = await _context.CompanyDirectors
                .Where(x => x.Guid == obj.Guid && x.IsDeleted == false)
                .FirstOrDefaultAsync();

            if (record is null)
            {
                return (false, "record does not exists or may have been deleted");
            }

            record.IsDeleted = true;
            record.DeletedBy = obj.DeletedBy;
            record.DeletedOn = obj.DeletedOn;

            var result = await _context.SaveChangesAsync();

            return (result > 0)
                ? (true, "Company Director deleted successfully")
                : (false, "Error in deleting Company Director");
        }

        public async Task<(CompanyDirectors, string)> Get(string id)
        {
            var record = await _context.CompanyDirectors.AsNoTracking()
                .Where(x => x.Guid == Guid.Parse(id) && x.IsDeleted == false)
                .FirstOrDefaultAsync();

            return (record is null)
                ? (null, "Record does not exists or may have been deleted")
                : (record, "Company Director retrieved");
        }

        public async Task<(List<CompanyDirectors>, string)> GetByCompanyId(int companyId)
        {
            var records = await _context.CompanyDirectors.AsNoTracking()
                .Where(x => x.CompanyId == companyId && x.IsDeleted == false && x.IsActive == true)
                .ToListAsync();

            return (records, "Company Directors retrieved");
        }

        public Task<(List<CompanyDirectors>, string)> List()
        {
            throw new NotImplementedException();
        }

        public async Task<(CompanyDirectors, string)> Update(CompanyDirectors obj)
        {
            var record = await _context.CompanyDirectors
                .Where(x => x.Guid == obj.Guid && x.IsDeleted == false)
                .FirstOrDefaultAsync();

            if (record is null)
            {
                return (null, "Record does not exists or may have been deleted");
            }

            record.Name = obj.Name;
            record.DateOfBirth = obj.DateOfBirth;
            record.CompanyAddress = obj.CompanyAddress;
            record.AdditionalAddress = obj.AdditionalAddress;
            record.CityId = obj.CityId;
            record.CityName = obj.CityName;
            record.StateId = obj.StateId;
            record.StateName = obj.StateName;
            record.CountryId = obj.CountryId;
            record.CountryName = obj.CountryName;
            record.PostalId = obj.PostalId;
            record.PostalCode = obj.PostalCode;
            record.UpdatedBy = obj.UpdatedBy;
            record.UpdatedOn = obj.UpdatedOn;

            var result = await _context.SaveChangesAsync();

            return (result > 0)
                ? (obj, "Company Director updated successfully")
                : (null, "Error in updating Company Director");
        }
    }
}
