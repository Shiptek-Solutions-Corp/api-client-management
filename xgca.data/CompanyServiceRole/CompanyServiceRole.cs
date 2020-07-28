using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using xgca.entity;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using LinqKit;
using Z.EntityFramework.Plus;

namespace xgca.data.CompanyServiceRole
{
    public interface ICompanyServiceRole
    {
        Task<bool> Create(List<entity.Models.CompanyServiceRole> obj);
        Task<bool> Create(entity.Models.CompanyServiceRole obj);
        Task<List<entity.Models.CompanyServiceRole>> List();
        Task<List<entity.Models.CompanyServiceRole>> ListByCompanyServiceId(int companyServiceId);
        Task<entity.Models.CompanyServiceRole> Retrieve(int key);
        Task<entity.Models.CompanyServiceRole> Retrieve(Guid key);
        Task<int> RetrieveAdministratorId(int key);
        Task<int> GetIdByGuid(Guid guid);
        Task<bool> Update(entity.Models.CompanyServiceRole obj);
        Task<bool> ChangeStatus(entity.Models.CompanyServiceRole obj);
        Task<bool> Delete(int key);
        Task<ReturnObject> ListByCompanyId(int companyID, int status);
        Task<bool> CheckGroupNameIfExists(int companyServiceId, string groupName);
        Task<bool> BulkUpdate(ICollection<Guid> ids, string type);
    }

    public class ReturnObject
    {
        public List<entity.Models.CompanyServiceRole> CompanyServiceRoles { get; set; }
        public int TotalGroups { get; set; }
        public int TotalActive { get; set; }
        public int TotalInactive { get; set; }
    }
    public class CompanyServiceRole : IMaintainable<entity.Models.CompanyServiceRole>, ICompanyServiceRole
    {
        private readonly IXGCAContext _context;

        public CompanyServiceRole(IXGCAContext context)
        {
            _context = context;
        }

        public async Task<bool> BulkUpdate(ICollection<Guid> guids, string type)
        {
            int result = 0;

            switch (type.ToLower())
            {
                case "enable":
                    result =  _context.CompanyServiceRoles
                       .Where(csr => guids.Contains(csr.Guid))
                       .Update(csr => new entity.Models.CompanyServiceRole() { IsActive = 1 });
                    break;
                case "disable":
                    result = _context.CompanyServiceRoles
                       .Where(csr => guids.Contains(csr.Guid))
                       .Update(csr => new entity.Models.CompanyServiceRole() { IsActive = 0 });
                    break;
                case "delete":
                    //result = _context.CompanyServiceRoles
                    //   .Where(csr => guids.Contains(csr.Guid))
                    //   .Update(csr => new entity.Models.CompanyServiceRole() { IsDeleted = 1 });
                    var test = _context.CompanyServiceRoles
                       .Where(csr => guids.Contains(csr.Guid))
                       .ToList();

                    foreach (var item in test)
                    {
                        var groupResources = _context.CompanyGroupResources
                            .Where(c => c.CompanyServiceRoleId == item.CompanyServiceRoleId);
                        _context.CompanyGroupResources.RemoveRange(groupResources);

                        var companyServiceUser = _context.CompanyServiceUsers
                            .Where(c => c.CompanyServiceRoleId == item.CompanyServiceRoleId);
                        _context.CompanyServiceUsers.RemoveRange(companyServiceUser);
                    }
                    _context.CompanyServiceRoles.RemoveRange(test);

                     result = await _context.SaveChangesAsync();

                    break;
                default:
                    break;
            }

            return result > 0 ? true : false;
        }

        public Task<bool> ChangeStatus(entity.Models.CompanyServiceRole obj)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> CheckGroupNameIfExists(int companyServiceId, string groupName)
        {
            var result = await _context.CompanyServiceRoles
                .Where(c => c.CompanyServiceId == companyServiceId)
                .Where(c => c.Name == groupName)
                .Select(c => c.CompanyServiceRoleId)
                .FirstOrDefaultAsync();

            return result > 1 ? true : false;
        }

        public async Task<bool> Create(List<entity.Models.CompanyServiceRole> obj)
        {
            foreach (entity.Models.CompanyServiceRole companyServiceRole in obj)
            {
                await _context.CompanyServiceRoles.AddAsync(companyServiceRole);
            }
            var result = await _context.SaveChangesAsync();
            return result > 0 ? true : false;
        }

        public async Task<bool> Create(entity.Models.CompanyServiceRole obj)
        {
            obj.IsActive = 1;
            obj.CreatedBy = 1;
            obj.CreatedOn = DateTime.UtcNow;
            obj.ModifiedBy = 1;
            obj.ModifiedOn = DateTime.UtcNow;
            obj.Guid = Guid.NewGuid();

            await _context.CompanyServiceRoles.AddAsync(obj);
            var result = await _context.SaveChangesAsync();
            return result > 0 ? true : false;
        }

        public Task<bool> Delete(int key)
        {
            throw new NotImplementedException();
        }

        public async Task<int> GetIdByGuid(Guid guid)
        {
            var data = await _context.CompanyServiceRoles
                .Where(csr => csr.Guid == guid && csr.IsDeleted == 0)
                .FirstOrDefaultAsync();
            return data.CompanyServiceRoleId;
        }

        public Task<List<entity.Models.CompanyServiceRole>> List()
        {
            throw new NotImplementedException();
        }

        public async Task<ReturnObject> ListByCompanyId(int companyID, int status)
        {
            var data = _context.CompanyServiceRoles;
            var predicate = PredicateBuilder.New<entity.Models.CompanyServiceRole>();

            predicate.And(c => c.CompanyServices.CompanyId == companyID);

            var query = data.Where(predicate)
                .Where(c => c.IsDeleted == 0)
                .Include(c => c.CompanyServices)
                    .ThenInclude(c => c.Companies);

            int totalGroups = query.Select(c => c.CompanyServiceRoleId).Count();
            int totalActive = query.Where(c => c.IsActive == 1).Select(c => c.CompanyServiceRoleId).Count();
            int totalInactive = query.Where(c => c.IsActive == 0).Select(c => c.CompanyServiceRoleId).Count();

            if (status > -1)
            {
                predicate = predicate.And(c => c.IsActive == status);
            }

            ReturnObject returnObject = new ReturnObject
            {
                CompanyServiceRoles = await query.Where(predicate).ToListAsync(),
                TotalGroups = totalGroups,
                TotalActive = totalActive,
                TotalInactive = totalInactive
            };

            return returnObject;
        }

        public async Task<List<entity.Models.CompanyServiceRole>> ListByCompanyServiceId(int companyServiceId)
        {
            var data = await _context.CompanyServiceRoles
                .Include(cs => cs.CompanyServices)
                .Where(csr => csr.CompanyServiceId == companyServiceId && csr.IsDeleted == 0)
                .ToListAsync();
            return data;
        }

        public async Task<entity.Models.CompanyServiceRole> Retrieve(int key)
        {
            var data = await _context.CompanyServiceRoles
                .Where(cs => cs.CompanyServiceRoleId == key && cs.IsDeleted == 0).FirstOrDefaultAsync();
            return data;
        }

        public async Task<entity.Models.CompanyServiceRole> Retrieve(Guid key)
        {
            var data = await _context.CompanyServiceRoles
                .Where(cs => cs.Guid == key && cs.IsDeleted == 0)
                .Include(c => c.CompanyServiceUsers)
                    .ThenInclude(c => c.CompanyUsers)
                    .ThenInclude(c => c.Users)
                .Include(c => c.CompanyServices)
                .ThenInclude(c => c.Companies)
                .FirstOrDefaultAsync();

            return data;
        }

        public async Task<int> RetrieveAdministratorId(int key)
        {
            var data = await _context.CompanyServiceRoles
                .Where(csr => csr.CompanyServiceId == key && csr.Name == "Administrator" && csr.IsDeleted == 0)
                .FirstOrDefaultAsync();
            return data.CompanyServiceRoleId;
        }

        public async Task<bool> Update(entity.Models.CompanyServiceRole obj)
        {
            _context.CompanyServiceRoles.Update(obj);
            var result = await _context.SaveChangesAsync();

            return result > 0 ? true : false;
        }
    }
}
