using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using xgca.core.Models.ContactDetail;
using xgca.core.Response;

namespace xgca.core.ContactDetail
{
    public interface IContactDetail
    {
        Task<IGeneralModel> List();
        Task<IGeneralModel> Create(CreateContactDetailModel obj);
        Task<int> CreateAndReturnId(dynamic obj, int createdById);
        Task<IGeneralModel> Update(UpdateContactDetailModel obj);
        Task<int> UpdateAndReturnId(dynamic obj, int createdById);
        Task<IGeneralModel> Retrieve(string key);
        Task<IGeneralModel> Delete(string key);
    }
}
