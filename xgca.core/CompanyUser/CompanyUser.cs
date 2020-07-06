using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using xgca.core.Models.CompanyService;
using xgca.core.Response;
using xgca.core.Company;
using xgca.core.Helpers;
using xgca.core.Models.CompanyUser;

namespace xgca.core.CompanyUser
{
    public class CompanyUser : ICompanyUser
    {
        private readonly xgca.data.CompanyUser.ICompanyUser _companyUser;
        private readonly xgca.data.Company.ICompanyData _company;
        private readonly xgca.data.User.IUserData _userData;
        private readonly IUserHelper _userHelper;
        private readonly IGeneral _general;

        public CompanyUser(xgca.data.CompanyUser.ICompanyUser companyUser,
            xgca.data.Company.ICompanyData company, xgca.data.User.IUserData userData,
            IUserHelper userHelper, IGeneral general)
        {
            _companyUser = companyUser;
            _company = company;
            _userData = userData;
            _userHelper = userHelper;
            _general = general;
        }

        public async Task<IGeneralModel> Create(CreateCompanyUserModel obj)
        {
            int userId = await _userData.GetIdByGuid(Guid.Parse(obj.UserId));
            int userTypeId = 1;
            var data = new entity.Models.CompanyUser
            {
                CompanyId = obj.CompanyId,
                UserId = userId,
                UserTypeId = userTypeId,
                Status = 1,
                CreatedBy = 0,
                CreatedOn = DateTime.UtcNow,
                ModifiedBy = 0,
                ModifiedOn = DateTime.UtcNow,
                Guid = Guid.NewGuid()
            };

            var result = await _companyUser.Create(data);
            return result
                ? _general.Response(true, 200, "User assiged to company successfully", true)
                : _general.Response(false, 400, "Error assigning user to company", true);
        }

        public async Task<int> CreateDefaultCompanyUser(int companyId, int masterUserId, int createdBy)
        {
            int userTypeId = 1;
            var data = new entity.Models.CompanyUser
            {
                CompanyId = companyId,
                UserId = masterUserId,
                UserTypeId = userTypeId,
                Status = 1,
                CreatedBy = createdBy,
                CreatedOn = DateTime.UtcNow,
                ModifiedBy = createdBy,
                ModifiedOn = DateTime.UtcNow,
                Guid = Guid.NewGuid()
            };

            int companyUserId = await _companyUser.CreateAndReturnId(data);
            return companyUserId;
        }

        public async Task<IGeneralModel> ListByCompanyId(string key)
        {
            int companyId = await _company.GetIdByGuid(Guid.Parse(key));
            var result = await _companyUser.ListByCompanyId(companyId);
            if (result == null)
            {
                return _general.Response(false, 400, "Error on listing company users", false);
            }
            var data = result.Select(t => new { CompanyUserId = t.Guid, CompanyId = t.Companies.Guid, UserId = t.Users.Guid, Fullname = String.Concat(t.Users.FirstName, " ", t.Users.LastName) });
            return _general.Response(new { companyUsers = data }, 200, "Configurable company users have been listed", true);
        }

        public async Task<IGeneralModel> Update(UpateCompanyUserModel obj)
        {
            int companyId = await _company.GetIdByGuid(Guid.Parse(obj.CompanyId));
            int userId = await _userData.GetIdByGuid(Guid.Parse(obj.User));
            int modifiedBy = await _userData.GetIdByGuid(Guid.Parse(obj.ModifiedBy));
            int userTypeId = 1;
            var data = new entity.Models.CompanyUser
            {
                CompanyId = companyId,
                UserId = userId,
                UserTypeId = userTypeId,
                Status = 1,
                ModifiedBy = modifiedBy,
                ModifiedOn = DateTime.UtcNow
            };

            var result = await _companyUser.Update(data);
            return result
                ? _general.Response(true, 200, "Company user assignment updated", true)
                : _general.Response(false, 400, "Error updating company user assignment", true);
        }
        
        public async Task<int> GetCompanyIdByUserId(int key)
        {
            int companyId = await _companyUser.GetCompanyIdByUserId(key);
            return companyId;
        }

        public async Task<IGeneralModel> CreateAndReturnId(CreateCompanyUserModel obj)
        {
            int userId = await _userData.GetIdByGuid(Guid.Parse(obj.UserId));
            int userTypeId = 1;
            var data = new entity.Models.CompanyUser
            {
                CompanyId = obj.CompanyId,
                UserId = userId,
                UserTypeId = userTypeId,
                Status = 1,
                CreatedBy = 0,
                CreatedOn = DateTime.UtcNow,
                ModifiedBy = 0,
                ModifiedOn = DateTime.UtcNow,
                Guid = Guid.NewGuid()
            };

            int companyUserId = await _companyUser.CreateAndReturnId(data);
            return companyUserId > 0
                ? _general.Response(new { companyUserId = companyUserId }, 200, "User assiged to company successfully", true)
                : _general.Response(false, 400, "Error assigning user to company", true);
        }

        public async Task<IGeneralModel> ListByCompanyId(int companyId)
        {
            var result = await _companyUser.ListByCompanyId(companyId);
            if (result == null)
            {
                return _general.Response(false, 400, "Error on listing company users", false);
            }

            var data = result.Select(t => new 
            { 
                CompanyUserId = t.Guid,
                CompanyId = t.Companies.Guid,
                UserId = t.Users.Guid, 
                Fullname = _userHelper.GetUserFullname(t.Users),
                t.Users.ImageURL,
                t.Users.EmailAddress,
                t.Users.Status,
                t.Users.IsLocked,
                Role = "Admin",
                Service = "Shipping Line",
                t.Users.Username,
            });
            
            return _general.Response(new { companyUsers = data }, 200, "Configurable company users have been listed", true);
        }
        public async Task<int> GetIdByUserId(int key)
        {
            int companyUserId = await _companyUser.GetIdByUserId(key);
            return companyUserId;
        }
    }
}
