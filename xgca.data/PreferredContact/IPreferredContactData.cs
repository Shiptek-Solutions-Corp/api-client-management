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
    }
}
