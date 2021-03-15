using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using xgca.core.Models.PreferredProvider;
using xgca.core.Response;

namespace xgca.core.PreferredProvider
{
    public interface IPreferredProviderCore
    {
        Task<IGeneralModel> List(int profileId, string filters, string sortBy, string sortOrder, int pageNumber, int pageSize, int recordCount);
        Task<IGeneralModel> AddPreferredProviders(BatchCreatePreferredProvider providers, int profileId, string createdBy);
        Task<IGeneralModel> QuickSearch(string search, int profileId, string filters, string sortBy, string sortOrder, int pageNumber, int pageSize, int recordCount);
        Task<IGeneralModel> DeleteProvider(string key, string username);
    }
}
