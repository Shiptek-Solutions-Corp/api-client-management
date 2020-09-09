using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using xgca.core.Response;
using xgca.core.Guest;
using xgca.core.Models.Guest;

namespace xgca.core.Guest
{
    public interface IGuestCore
    {
        Task<IGeneralModel> Create(CreateGuest obj, string username, int companyId);
        Task<IGeneralModel> CreateAndReturnId(CreateGuest obj, string username, int companyId);
        Task<IGeneralModel> CreateAndReturnGuid(CreateGuest obj, string username, int companyId);
        Task<IGeneralModel> Update(UpdateGuestContact obj, string username, int companyId);
        Task<IGeneralModel> Update(UpdateGuest obj, string username, int companyId);
        Task<IGeneralModel> ShowDetails(int key);
        Task<IGeneralModel> ShowDetails(string key);
        Task<IGeneralModel> List();
        Task<IGeneralModel> List(string columnFilter);
        Task<IGeneralModel> Delete(string key, string username);
        Task<IGeneralModel> GetGuidById(int key);
        Task<IGeneralModel> GetIdByGuid(string key);
        Task<IGeneralModel> QuickSearch(string search);

    }
}
