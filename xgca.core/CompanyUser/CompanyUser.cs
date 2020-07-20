using System.IO;
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
using xgca.core.User;
using System.Data;
using ClosedXML.Excel;
using CsvHelper;

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
            var companyUsers = await _companyUser.ListByCompanyId(companyId);
            if (companyUsers == null)
            {
                return _general.Response(false, 400, "Error on listing company users", false);
            }

            List<int> userIds = new List<int>();
            foreach(var companyUser in companyUsers)
            {
                userIds.Add(companyUser.UserId);
            }

            int totalUsersCount = await _userData.GetTotalUsers(userIds);
            int totalActiveUsers = await _userData.GetTotalActiveUsers(userIds);
            int totalInactiveUsers = await _userData.GetTotalInactiveUsers(userIds);
            int totalLockedUsers = await _userData.GetTotalLockedUsers(userIds);
            int totalUnlockedUsers = await _userData.GetTotalUnlockedUsers(userIds);

            var data = new
            {
                companyUsers = companyUsers.Select(t => new
                {
                    CompanyUserId = t.Guid,
                    UserId = t.Users.Guid,
                    Fullname = _userHelper.GetUserFullname(t.Users),
                    ImageURL = (t.Users.ImageURL is null) ? "No Image" : t.Users.ImageURL,
                    EmailAddress = (t.Users.EmailAddress is null) ? "Email not set" : t.Users.EmailAddress,
                    t.Users.Status,
                    t.Users.IsLocked,
                    Username = (t.Users.Username is null) ? "Not set" : t.Users.Username,

                }),
                TotalUsersCount = totalUsersCount,
                TotalActiveUsers = totalActiveUsers,
                TotalInactiveUsers = totalInactiveUsers,
                TotalLockUsers = totalLockedUsers,
                TotalUnlockUsers = totalUnlockedUsers
            };

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
            var companyUsers = await _companyUser.ListByCompanyId(companyId);
            if (companyUsers == null)
            {
                return _general.Response(false, 400, "Error on listing company users", false);
            }

            List<dynamic> newCompanyUsers = new List<dynamic>();
            int totalUsersCount = 0;
            int totalActiveUsers = 0;
            int totalInactiveUsers = 0;
            int totalLockedUsers = 0;
            int totalUnlockedUsers = 0;
            foreach (var companyUser in companyUsers)
            {
                if (companyUser.Users.IsDeleted == 0)
                {
                    newCompanyUsers.Add(companyUser);
                    //Total Users 
                    totalUsersCount++;
                }

                //Total Active Users
                if (companyUser.Users.Status == 1 && companyUser.Users.IsDeleted == 0)
                {
                    totalActiveUsers++;
                }

                //Total Inactive Users
                if (companyUser.Users.Status == 0 && companyUser.Users.IsDeleted == 0)
                {
                    totalInactiveUsers++;
                }

                //Total Locked Users
                if (companyUser.Users.IsLocked == 1 && companyUser.Users.IsDeleted == 0)
                {
                    totalLockedUsers++;
                }

                //Total UnLocked Users
                if (companyUser.Users.IsLocked == 0 && companyUser.Users.IsDeleted == 0)
                {
                    totalUnlockedUsers++;
                }

            }


            var data = new
            {
                companyUsers = newCompanyUsers.Select(t => new
                {
                    CompanyUserId = t.Guid,
                    UserId = t.Users.Guid,
                    Fullname = _userHelper.GetUserFullname(t.Users),
                    ImageURL = (t.Users.ImageURL is null) ? "No Image" : t.Users.ImageURL,
                    EmailAddress = (t.Users.EmailAddress is null) ? "Email not set" : t.Users.EmailAddress,
                    t.Users.Status,
                    t.Users.IsLocked,
                    Username = (t.Users.Username is null) ? "Not set" : t.Users.Username,

                }),
                TotalUsersCount = totalUsersCount,
                TotalActiveUsers = totalActiveUsers,
                TotalInactiveUsers = totalInactiveUsers,
                TotalLockUsers = totalLockedUsers,
                TotalUnlockUsers = totalUnlockedUsers
            };

            return _general.Response(new { companyUsers = data }, 200, "Configurable company users have been listed", true);
        }


        public async Task<IGeneralModel> ListByCompanyIdAndFilter(string query, int companyId)
        {
            //Start: Please move this on helpers
            var queryParams = query.Split(",")
                                    .Select(p => p.Split(':'))
                                    .ToDictionary(p => p[0], p => p.Length > 1 ? Uri.EscapeDataString(p[1]) : null).FirstOrDefault();
            //End
            var companyUsers = await _companyUser.ListByCompanyId(companyId);
            if (companyUsers == null)
            {
                return _general.Response(false, 400, "Error on listing company users", false);
            }

            List<dynamic> newCompanyUsers = new List<dynamic>();
            int totalUsersCount = 0;
            int totalActiveUsers = 0;
            int totalInactiveUsers = 0;
            int totalLockedUsers = 0;
            int totalUnlockedUsers = 0;
            foreach (var companyUser in companyUsers)
            {
                if (queryParams.Key == "Status") {
                    if (companyUser.Users.Status == Convert.ToInt32(queryParams.Value) && companyUser.Users.IsDeleted == 0)
                    {
                        newCompanyUsers.Add(companyUser);
                    }
                }
                if (queryParams.Key == "IsLocked")
                {
                    if (companyUser.Users.IsLocked == Convert.ToInt32(queryParams.Value) && companyUser.Users.IsDeleted == 0)
                    {
                        newCompanyUsers.Add(companyUser);
                    }
                }

                //Total Users 
                if (companyUser.Users.IsDeleted == 0)
                {
                    totalUsersCount++;
                }

                //Total Active Users
                if (companyUser.Users.Status == 1 && companyUser.Users.IsDeleted == 0)
                {
                    totalActiveUsers ++;
                }

                //Total Inactive Users
                if (companyUser.Users.Status == 0 && companyUser.Users.IsDeleted == 0)
                {
                    totalInactiveUsers++;
                }

                //Total Locked Users
                if (companyUser.Users.IsLocked == 1 && companyUser.Users.IsDeleted == 0)
                {
                    totalLockedUsers++;
                }

                //Total UnLocked Users
                if (companyUser.Users.IsLocked == 0 && companyUser.Users.IsDeleted == 0)
                {
                    totalUnlockedUsers++;
                }

            }

            var data = new
            {
                companyUsers = newCompanyUsers.Select(t => new
                {
                    CompanyUserId = t.Guid,
                    UserId = t.Users.Guid,
                    Fullname = _userHelper.GetUserFullname(t.Users),
                    ImageURL = (t.Users.ImageURL is null) ? "No Image" : t.Users.ImageURL,
                    EmailAddress = (t.Users.EmailAddress is null) ? "Email not set" : t.Users.EmailAddress,
                    t.Users.Status,
                    t.Users.IsLocked,
                    Username = (t.Users.Username is null) ? "Not set" : t.Users.Username,
                }),
                TotalUsersCount = totalUsersCount,
                TotalActiveUsers = totalActiveUsers,
                TotalInactiveUsers = totalInactiveUsers,
                TotalLockUsers = totalLockedUsers,
                TotalUnlockUsers = totalUnlockedUsers
            };

            return _general.Response(new { companyUsers = data }, 200, "Configurable company users have been listed", true);
        }

        public async Task<byte[]> DownloadUsersExcel(string query, int companyId)
        {
            //Start: Please move this on helpers
            var queryParams = query.Split(",")
                                    .Select(p => p.Split(':'))
                                    .ToDictionary(p => p[0], p => p.Length > 1 ? Uri.EscapeDataString(p[1]) : null);

            var queryString = "";

            foreach (var v in queryParams)
            {
                if (v.Key != "download" && v.Value != "all") {
                    queryString += $"{v.Key} = '{v.Value}' and ";
                }
            }
            //End

            var companyUsers = await _companyUser.ListByCompanyId(companyId);
            List<int> userIds = new List<int>();
            foreach (var companyUser in companyUsers)
            {
                userIds.Add(companyUser.UserId);
            }
            var userDetails = await _userData.FilterWithCompanyUsersId(queryString, string.Join(",", userIds));
            int counttest = userDetails.Count();

            var table = new DataTable { TableName = "ServiceRates" };
            table.Columns.Add("Name", typeof(string));
            table.Columns.Add("UserName", typeof(string));
            table.Columns.Add("EmailAddress", typeof(string));
            table.Columns.Add("Title", typeof(string));

            foreach (var companyUser in userDetails)
            {
                table.Rows.Add(companyUser.FirstName + " " + companyUser.LastName, companyUser.Username, companyUser.EmailAddress, companyUser.Title + ";");
            }
            var wb = new XLWorkbook();
            wb.Worksheets.Add(table);
            await using var memoryStream = new MemoryStream();
            wb.SaveAs(memoryStream);
            return memoryStream.ToArray();
        }

        public async Task<int> GetIdByUserId(int key)
        {
            int companyUserId = await _companyUser.GetIdByUserId(key);
            return companyUserId;
        }
    }
}
