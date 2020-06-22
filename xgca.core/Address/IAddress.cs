using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using xgca.core.Models.Address;
using xgca.core.Response;

namespace xgca.core.Address
{
    public interface IAddress
    {
        Task<IGeneralModel> List();
        Task<IGeneralModel> Create(CreateAddressModel obj);
        Task<int> CreateAndReturnId(dynamic obj, int creadtedById);
        Task<IGeneralModel> Update(UpdateAddressModel obj);
        Task<int> UpdateAndReturnId(dynamic obj, int modifiedById);
        Task<IGeneralModel> Retrieve(string key);
        Task<IGeneralModel> Delete(string key);
    }
}
