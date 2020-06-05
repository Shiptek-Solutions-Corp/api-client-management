using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using xgca.core.Models.AddressType;
using xgca.core.Response;

namespace xgca.core.AddressType
{
    public interface IAddressType
    {
        Task<IGeneralModel> List();
        Task<IGeneralModel> Create(CreateAddressTypeModel obj);
        Task<IGeneralModel> Update(UpdateAddressTypeModel obj);
        Task<IGeneralModel> Retrieve(string key);
        Task<IGeneralModel> Delete(string key);
        Task<int> RetrieveIdByName(string key);

    }
}
