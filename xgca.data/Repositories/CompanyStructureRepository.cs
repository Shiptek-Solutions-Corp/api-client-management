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
    public interface ICompanyStructureRepository : IRepository<CompanyStructure>, IRetrievableRepository<CompanyStructure>
    {
        Task<(CompanyStructure, string)> GetByCompanyId(int companyId);

    }
    public class CompanyStructureRepository : ICompanyStructureRepository
    {
        private readonly IXGCAContext _context;

        public CompanyStructureRepository(IXGCAContext _context)
        {
            this._context = _context;
        }

        public async Task<(CompanyStructure, string)> Create(CompanyStructure obj)
        {
            await _context.CompanyStructures.AddAsync(obj);
            var result = await _context.SaveChangesAsync();
            return (result > 0)
                ? (obj, "Company Structure created successfully")
                : (null, "Error in creating Company Structure");
        }

        public async Task<(bool, string)> Delete(CompanyStructure obj)
        {
            var record = await _context.CompanyStructures
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
                ? (true, "Company structure deleted successfully")
                : (false, "Error in deleting company structure");
        }

        public async Task<(CompanyStructure, string)> Get(string id)
        {
            var record = await _context.CompanyStructures.AsNoTracking()
                .Where(x => x.Guid == Guid.Parse(id) && x.IsDeleted == false)
                .FirstOrDefaultAsync();

            return (record is null)
                ? (null, "Record does not exists or may have been deleted")
                : (record, "Company Structure record retrieved");
        }

        public async Task<(CompanyStructure, string)> GetByCompanyId(int companyId)
        {
            var record = await _context.CompanyStructures.AsNoTracking()
                .Where(x => x.CompanyId == companyId && x.IsDeleted == false)
                .FirstOrDefaultAsync();

            return (record is null)
                ? (null, "Record does not exists or may have been deleted")
                : (record, "Company Structure retrieved");
        }

        public async Task<(string, string)> GetGuidById(int id)
        {
            var guid = await _context.CompanyStructures.AsNoTracking()
                .Where(x => x.CompanyId == id)
                .Select(c => c.Guid)
                .FirstOrDefaultAsync();

            return (guid == Guid.Empty)
                ? (null, "Record does not exists or may have been deleted")
                : (guid.ToString(), "Company Structure GUID retrieved");
        }

        public Task<(int, string)> GetIdByGuid(string id)
        {
            throw new NotImplementedException();
        }

        public Task<(List<CompanyStructure>, string)> List()
        {
            throw new NotImplementedException();
        }

        public async Task<(CompanyStructure, string)> Update(CompanyStructure obj)
        {
            var record = await _context.CompanyStructures
                .Where(x => x.Guid == obj.Guid && x.IsDeleted == false)
                .FirstOrDefaultAsync();

            if (record is null)
            {
                return (null, "Record does not exists or may have been deleted");
            }

            record.RegistrationNumber = obj.RegistrationNumber;
            record.TotalEmployeeNo = obj.TotalEmployeeNo;
            record.CompanyAddress = obj.CompanyAddress;
            record.AdditionalAddress = obj.AdditionalAddress;
            record.CityId = obj.CityId;
            record.CityName = obj.CityName;
            record.StateId = obj.StateId;
            record.StateName = obj.StateName;
            record.CountryId = obj.CountryId;
            record.CountryName = obj.CountryName;
            record.PostalCode = obj.PostalCode;
            record.PostalId = obj.PostalId;
            record.UpdatedBy = obj.UpdatedBy;
            record.UpdatedOn = obj.UpdatedOn;

            var result = await _context.SaveChangesAsync();

            return (result > 0)
                ? (obj, "Company Structure updated successfully")
                : (null, "Error in updating company structure");
        }
    }
}
