using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using xgca.core.Models.PreferredContact;
using xgca.core.Response;

namespace xgca.core.PreferredContact
{
    public interface IPreferredContactCore
    {
        Task<IGeneralModel> List(int profileId, int pageNumber, int pageSize);
        Task<IGeneralModel> List(string companyId, int pageNumber, int pageSize);
        Task<IGeneralModel> Create(entity.Models.PreferredContact obj);
        Task<IGeneralModel> ShowDetails(string preferredContactId);
        Task<IGeneralModel> QuickSearch(string search, int profileId, int pageNumber, int pageSize, int recordCount);
        Task<IGeneralModel> DeleteContact(string key, string username);
        Task<IGeneralModel> List(int profileId, string search, string name, string country, string stateCity, int type, string contact, string sortBy, string sortOrder, int pageNumber, int pageSize);
    }
}
