using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace xgca.data.PreferredContact
{
    public interface IPreferredContactData
    {
        Task<int> GetRecordCount();
        Task<int> GetRecordCount(int profileId);
        Task<List<entity.Models.PreferredContact>> List();
        Task<List<entity.Models.PreferredContact>> List(int profileId);
        Task<List<entity.Models.PreferredContact>> List(int profileId, int pageNumber, int pageSize);
        Task<List<entity.Models.PreferredContact>> List(int profileId, string columnFilter, int pageNumber, int pageSize);
        Task<bool> Create(entity.Models.PreferredContact obj);
        Task<entity.Models.PreferredContact> Retrieve(Guid guid);
        Task<List<string>> GetGuestIds(int profileId);
        Task<List<string>> GetRegisteredIds(int profileId);
        Task<bool> Delete(string key);
        Task<List<entity.Models.PreferredContact>> GetContactsByQuickSearch(int profileId, List<string> guestIds, List<string> registeredIds, int pageNumber, int pageSize);
        Task<bool> CheckIfContactAlreadyAdded(string companyGuid, int profileId);
    }
}
