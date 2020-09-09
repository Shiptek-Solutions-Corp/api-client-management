using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace xgca.data.PreferredProvider
{
    public interface IPreferredProviderData : IMaintainable<entity.Models.PreferredProvider>
    {
        Task<int> GetRecordCount();
        Task<int> GetRecordCount(int profileId);
        Task<int> GetRecordCount(int profileId, List<string> compayServiceIds);
        Task<List<entity.Models.PreferredProvider>> List(int profileId, string companyGuid);
        Task<List<entity.Models.PreferredProvider>> ListByProfileId(int profileId);
        Task<List<entity.Models.PreferredProvider>> ListByProfileId(int profileId, int pageNumber, int pageSize);
        Task<bool> Delete(string key);
        Task<List<entity.Models.PreferredProvider>> ListByServiceId(string serviceId, int pageNumber, int pageSize);
        Task<List<entity.Models.PreferredProvider>> CreateAndReturnList(entity.Models.PreferredProvider provider);
        Task<List<string>> ListCompanyIdsByProfileId(int profileId);
        Task<List<string>> ListCompanyServiceIdsByProfileId(int profileId, List<entity.Models.PreferredProvider> filteredProviders);
        Task<bool> Create(List<entity.Models.PreferredProvider> providers);
        Task<bool> CheckIfExists(int profileId, string serviceId, string companyId, string companyServiceId);
        Task<List<string>> GetCompanyServiceIdByProfileId(int profileId);
        Task<List<entity.Models.PreferredProvider>> GetPreferredProvidersByQuickSearch(int profileId, List<string> companyServiceIds, int pageNumber, int pageSize);
        
    }
}
