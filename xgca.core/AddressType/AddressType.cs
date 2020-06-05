using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using xgca.core.Models.AddressType;
using xgca.core.Response;
using xgca.entity.Models;
using xgca.data.AddressType;

namespace xgca.core.AddressType
{
    public class AddressType : IAddressType
    {
        private readonly xgca.data.AddressType.IAddressType _addressType;
        private readonly IGeneral _general;

        public AddressType(xgca.data.AddressType.IAddressType addressType, IGeneral general)
        {
            _addressType = addressType;
            _general = general;
        }

        public async Task<IGeneralModel> List()
        {
            var data = await _addressType.List();
            return _general.Response(new 
            {
                addressType = data.Select(t => new 
                { 
                    AddressTypeId = t.Guid, 
                    t.Name 
                }) 
            }, 200, "Configurable addresss types has been listed", true);
        }

        public async Task<IGeneralModel> Retrieve(string key)
        {
            int addressTypeId = await _addressType.GetIdByGuid(Guid.Parse(key));
            if (addressTypeId == 0)
            { return _general.Response(false, 400, "Error on updating address type", true); }
            var result = await _addressType.Retrieve(addressTypeId);
            if (result == null)
            {
                return _general.Response(null, 400, "Selected address type may have been deleted or does not exists", true);
            }

            var data = new 
            {
                AddressTypeId = result.Guid, 
                result.Name 
            };

            return _general.Response(new { addressType = data }, 200, "Configurable details for selected address type has been retrieved", true);
        }

        public async Task<IGeneralModel> Create(CreateAddressTypeModel obj)
        {
            var data = new entity.Models.AddressType
            {
                Name = obj.Name,
                Guid = Guid.NewGuid(),
                CreatedBy = 1,
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now
            };
            var result = await _addressType.Create(data);

            return result
                ? _general.Response(true, 200, "Address type created", true)
                : _general.Response(false, 400, "Error on creating address type", true);
        }

        public async Task<IGeneralModel> Update(UpdateAddressTypeModel obj)
        {
            int addressTypeId = await _addressType.GetIdByGuid(Guid.Parse(obj.AddressTypeId));
            if (addressTypeId == 0)
            { return _general.Response(false, 400, "Error on updating address type", true); }
            var data = new entity.Models.AddressType
            {
                AddressTypeId = addressTypeId,
                Name = obj.Name,
                ModifiedBy = 1,
                ModifiedOn = DateTime.Now
            };
            var result = await _addressType.Update(data);

            return result
                ? _general.Response(true, 200, "Address type updated", true)
                : _general.Response(false, 400, "Error on updating address type", true);
        }

        public async Task<IGeneralModel> Delete(string key)
        {
            int addressTypeId = await _addressType.GetIdByGuid(Guid.Parse(key));
            if (addressTypeId == 0)
            { return _general.Response(false, 400, "Error on deleting address type", true); }

            var result = await _addressType.Delete(addressTypeId);
            return result
                ? _general.Response(true, 200, "Address type deleted", true)
                : _general.Response(false, 400, "Error on deleting address type", true);
        }

        public async Task<int> RetrieveIdByName(string name)
        {
            int addressTypeId = await _addressType.RetrieveIdByName(name);
            return addressTypeId;
        }
    }
}
