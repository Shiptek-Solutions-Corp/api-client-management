using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using xgca.data.Company;

namespace xgca.data.Guest
{
    public interface IGuestData
    {
        Task<bool> Create(entity.Models.Guest obj);
        Task<int> CreateAndReturnId(entity.Models.Guest obj);
        Task<string> CreateAndReturnGuid(entity.Models.Guest obj);
        Task<List<entity.Models.Guest>> List();
        Task<List<entity.Models.Guest>> List(int companyId);
        Task<List<entity.Models.Guest>> List(string columnFilter);
        Task<List<entity.Models.Guest>> List(int companyId, string columnFilter);
        Task<entity.Models.Guest> Retrieve(int key);
        Task<entity.Models.Guest> Retrieve(Guid key);
        Task<int> GetIdByGuid(Guid guid);
        Task<string> GetGuidById(int id);
        Task<bool> Update(entity.Models.Guest obj);
        Task<bool> Delete(entity.Models.Guest obj);
        Task<List<entity.Models.Guest>> QuickSearch(string search);
        Task<List<entity.Models.Guest>> QuickSearch(string search, List<string> guids);
        Task<YourEDIActorReturn> GetGuestAndMasterUserDetails(string guestKey);
        Task<bool> SetCUCCByGuestGuid(string guestKey, string CUCC);
        Task<List<entity.Models.Guest>> SearchGuest(string search, string name, string country, string stateCity, string contact, List<string> guids);
        Task<Biller> GetGuestBiller(string guestBillerId);
        Task<Customer> GetGuestCustomer(string guestCustomerId);
    }
}
