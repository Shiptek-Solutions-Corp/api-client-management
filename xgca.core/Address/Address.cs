using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using xgca.core.Models.Address;
using xgca.core.Response;
using xgca.entity.Models;
using xgca.data.Address;
using xgca.core.Helpers;
using xgca.core.Helpers.Http;
using xgca.core.Constants;
using Microsoft.Extensions.Configuration;

namespace xgca.core.Address
{
    public class Address : IAddress
    {
        private readonly IAddressData _addressData;
        private readonly xgca.core.AddressType.IAddressType _coreAddressType;
        private readonly IHttpHelper _httpHelpers;
        private readonly IGeneral _general;
        private readonly IOptions<GlobalCmsApi> _options;
        public Address(IAddressData addressData,
            xgca.core.AddressType.IAddressType coreAddressType, IHttpHelper httpHelpers,
            IOptions<GlobalCmsApi> options, IGeneral general)
        {
            _addressData = addressData;
            _coreAddressType = coreAddressType;
            _httpHelpers = httpHelpers;
            _options = options;
            _general = general;
        }

        public Task<IGeneralModel> List()
        {
            throw new NotImplementedException();
        }
        public Task<IGeneralModel> Create(CreateAddressModel obj)
        {
            throw new NotImplementedException();
        }
        public async Task<int> CreateAndReturnId(dynamic obj)
        {
            int addressTypeId = await _coreAddressType.RetrieveIdByName("Company");
            string fullAddress = AddressHelper.GenerateFullAddress(obj);
            string json = JsonConvert.SerializeObject(obj);
            int createdBy = json.Contains("CreatedBy") ? (json.Contains("MasterUser") || obj.CreatedBy is null ? 0 : obj.CreatedBy) : 0;

            var address = new entity.Models.Address
            {
                AddressTypeId = addressTypeId,
                AddressLine = obj.AddressLine,
                CityName = obj.CityName,
                StateName = obj.StateName,
                ZipCode = obj.ZipCode,
                CountryId = Convert.ToInt32(obj.CountryId),
                CountryName = obj.CountryName,
                FullAddress = fullAddress,
                Longitude = obj.Longitude,
                Latitude = obj.Latitude,
                CreatedBy = createdBy,
                CreatedOn = DateTime.Now,
                ModifiedBy = createdBy,
                ModifiedOn = DateTime.Now,
                Guid = Guid.NewGuid()
            };
            int addressId = await _addressData.CreateAndReturnId(address);
            return addressId;
        }
        public async Task<int> UpdateAndReturnId(dynamic obj)
        {
            var addressTypeId = await _coreAddressType.RetrieveIdByName("Company");
            int addressId = await _addressData.GetIdByGuid(Guid.Parse(obj.AddressId));
            string fullAddress = AddressHelper.GenerateFullAddress(obj);
            string json = JsonConvert.SerializeObject(obj);
            int modifiedBy = json.Contains("userId") ? obj.UserId : 0;

            var address = new entity.Models.Address
            {
                AddressId = addressId,
                AddressTypeId = addressTypeId,
                AddressLine = obj.AddressLine,
                CityName = obj.CityName,
                StateName = obj.StateName,
                ZipCode = obj.ZipCode,
                CountryId = Convert.ToInt32(obj.CountryId),
                CountryName = obj.CountryName,
                FullAddress = fullAddress,
                Longitude = obj.Longitude,
                Latitude = obj.Latitude,
                ModifiedBy = modifiedBy,
                ModifiedOn = DateTime.Now,
                Guid = Guid.Parse(obj.AddressId)
            };
            await _addressData.Update(address);
            return addressId;
        }
        public Task<IGeneralModel> Retrieve(string key)
        {
            throw new NotImplementedException();
        }
        public Task<IGeneralModel> Delete(string key)
        {
            throw new NotImplementedException();
        }

        public Task<IGeneralModel> Update(UpdateAddressModel obj)
        {
            throw new NotImplementedException();
        }
    }
}
