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
    }
}
