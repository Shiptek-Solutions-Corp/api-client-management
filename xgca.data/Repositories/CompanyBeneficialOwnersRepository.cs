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
    public interface ICompanyBeneficialOwnersRepository : IRepository<CompanyBeneficialOwners>
    {
        Task<(List<CompanyBeneficialOwners>, string)> CreateBeneficialOwners(List<CompanyBeneficialOwners> obj);
        Task<(List<CompanyBeneficialOwners>, string)> UpdateBeneficialOwners(List<CompanyBeneficialOwners> obj);
        Task<(bool, string)> DeleteBeneficialOwners(List<Guid> ids, string username);
        Task<(List<CompanyBeneficialOwners>, List<CompanyBeneficialOwners>, string)> GetByCompanyId(int companyId);
    }
    public class CompanyBeneficialOwnersRepository : ICompanyBeneficialOwnersRepository
    {
        private readonly IXGCAContext _context;

        public CompanyBeneficialOwnersRepository(IXGCAContext _context)
        {
            this._context = _context;
        }

        public async Task<(CompanyBeneficialOwners, string)> Create(CompanyBeneficialOwners obj)
        {
            await _context.CompanyBeneficialOwners.AddAsync(obj);
            var result = await _context.SaveChangesAsync();
            return (result > 0)
                ? (obj, "Company Beneficial Owner created successfully")
                : (null, "Error in creating Company Beneficial Owner");
        }

        public async Task<(List<CompanyBeneficialOwners>, string)> CreateBeneficialOwners(List<CompanyBeneficialOwners> obj)
        {
            await _context.CompanyBeneficialOwners.AddRangeAsync(obj);
            var result = await _context.SaveChangesAsync();
            return (result > 0)
                ? (obj, "Company Beneficial Owners created successfully")
                : (null, "Error in creating Company Beneficial Owners");
        }

        public async Task<(bool, string)> Delete(CompanyBeneficialOwners obj)
        {
            var record = await _context.CompanyBeneficialOwners
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
                ? (true, "Company Beneficial Owner deleted successfully")
                : (false, "Error in deleting Company beneficial Owner");
        }

        public async Task<(bool, string)> DeleteBeneficialOwners(List<Guid> ids, string username)
        {
            var records = await _context.CompanyBeneficialOwners
                .Where(x => ids.Contains(x.Guid) && x.IsDeleted == false)
                .ToListAsync();

            if (records.Count == 0)
            {
                return (false, "Record(s) does not exists or may have been deleted");
            }

            records.ForEach(e =>
            {
                e.IsDeleted = false;
                e.DeletedBy = username;
                e.DeletedOn = DateTime.UtcNow;
            });

            var result = await _context.SaveChangesAsync();

            return (result > 0)
                ? (true, "Company Beneficial Owners deleted successfully")
                : (false, "Error in deleting Company Beneficial Owners");

        }

        public Task<(CompanyBeneficialOwners, string)> Get(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<(List<CompanyBeneficialOwners>, List<CompanyBeneficialOwners>, string)> GetByCompanyId(int companyId)
        {
            var records = await _context.CompanyBeneficialOwners.AsNoTracking()
                .Where(x => x.CompanyId == companyId && x.IsDeleted == false && x.IsActive == true)
                .ToListAsync();

            if (records.Count == 0)
            {
                return (null, null, "No beneficial owners found");
            }

            var companies = records.Where(x => x.BeneficialOwnersTypeCode == "C").ToList();
            var individuals = records.Where(x => x.BeneficialOwnersTypeCode == "I").ToList();

            return (companies, individuals, "Company Beneficial Owners retrieved");
        }

        public Task<(List<CompanyBeneficialOwners>, string)> List()
        {
            throw new NotImplementedException();
        }

        public async Task<(CompanyBeneficialOwners, string)> Update(CompanyBeneficialOwners obj)
        {
            var record = await _context.CompanyBeneficialOwners
                .Where(x => x.Guid == obj.Guid && x.IsDeleted == false)
                .FirstOrDefaultAsync();

            if (record is null)
            {
                return (null, "Record does not exists or may have been deleted");
            }

            record.BeneficialOwnersTypeCode = obj.BeneficialOwnersTypeCode;
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

            var result = await _context.SaveChangesAsync();

            return (result > 0)
                ? (obj, "Company Beneficial Owner updated successfully")
                : (null, "Error in updating Company Beneficial Owner");
        }

        public async Task<(List<CompanyBeneficialOwners>, string)> UpdateBeneficialOwners(List<CompanyBeneficialOwners> obj)
        {
            await _context.CompanyBeneficialOwners.BulkUpdateAsync(obj);
            var result = await _context.SaveChangesAsync();
            return (result > 0)
                ? (obj, "Company Beneficial Owner(s) updated successfully")
                : (null, "Error in updating Company Beneficial Owner(s)");
        }
    }
}
