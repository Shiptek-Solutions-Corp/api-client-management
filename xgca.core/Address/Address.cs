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
        private readonly IOptions<GlobalCmsService> _options;
        public Address(IAddressData addressData,
            xgca.core.AddressType.IAddressType coreAddressType, IHttpHelper httpHelpers,
            IOptions<GlobalCmsService> options, IGeneral general)
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
        public async Task<int> CreateAndReturnId(dynamic obj, int createdById)
        {
            int addressTypeId = await _coreAddressType.RetrieveIdByName("Company");
            var cityResponse = await _httpHelpers.GetIdByGuid(_options.Value.BaseUrl, $"{_options.Value.GetCity}/", obj.CityId.ToString(), AuthToken.Contra);
            var cityJson = (JObject)cityResponse;
            var stateResponse = await _httpHelpers.GetIdByGuid(_options.Value.BaseUrl, $"{_options.Value.GetState}/", obj.StateId.ToString(), AuthToken.Contra);
            var stateJson = (JObject)stateResponse;
            string fullAddress = AddressHelper.GenerateFullAddress(obj);
            //string fullAddress = AddressHelper.GenerateFullAddress(obj.AddressLine, obj.CityName, obj.StateName, obj.ZipCode, obj.CountryName);
            string json = JsonConvert.SerializeObject(obj);

            var address = new entity.Models.Address
            {
                AddressTypeId = addressTypeId,
                AddressLine = obj.AddressLine,
                CityId = Convert.ToInt32((cityJson)["data"]["cityId"]),
                CityName = obj.CityName,
                StateId = Convert.ToInt32((stateJson)["data"]["stateId"]),
                StateName = obj.StateName,
                ZipCode = obj.ZipCode,
                CountryId = Convert.ToInt32(obj.CountryId),
                CountryName = obj.CountryName,
                FullAddress = fullAddress,
                Longitude = obj.Longitude,
                Latitude = obj.Latitude,
                CreatedBy = createdById,
                CreatedOn = DateTime.UtcNow,
                ModifiedBy = createdById,
                ModifiedOn = DateTime.UtcNow,
                Guid = Guid.NewGuid(),
                AddressAdditionalInformation = obj.AddressAdditionalInformation
            };
            int addressId = await _addressData.CreateAndReturnId(address);
            return addressId;
        }
        public async Task<int> UpdateAndReturnId(dynamic obj, int modifiedById)
        {
            var addressTypeId = await _coreAddressType.RetrieveIdByName("Company");
            int addressId = await _addressData.GetIdByGuid(Guid.Parse(obj.AddressId));
            var cityResponse = await _httpHelpers.GetIdByGuid(_options.Value.BaseUrl, $"{_options.Value.GetCity}/", obj.CityId.ToString(), AuthToken.Contra);
            var cityJson = (JObject)cityResponse;
            var stateResponse = await _httpHelpers.GetIdByGuid(_options.Value.BaseUrl, $"{_options.Value.GetState}/", obj.StateId.ToString(), AuthToken.Contra);
            var stateJson = (JObject)stateResponse;
            string fullAddress = AddressHelper.GenerateFullAddress(obj);
            //string fullAddress = AddressHelper.GenerateFullAddress(obj.AddressLine, obj.CityName, obj.StateName, obj.ZipCode, obj.CountryName);
            string json = JsonConvert.SerializeObject(obj);

            var address = new entity.Models.Address
            {
                AddressId = addressId,
                AddressTypeId = addressTypeId,
                AddressLine = obj.AddressLine,
                CityId = Convert.ToInt32((cityJson)["data"]["cityId"]),
                CityName = obj.CityName,
                StateId = Convert.ToInt32((stateJson)["data"]["stateId"]),
                StateName = obj.StateName,
                ZipCode = obj.ZipCode,
                CountryId = Convert.ToInt32(obj.CountryId),
                CountryName = obj.CountryName,
                FullAddress = fullAddress,
                Longitude = obj.Longitude,
                Latitude = obj.Latitude,
                ModifiedBy = modifiedById,
                ModifiedOn = DateTime.UtcNow,
                Guid = Guid.Parse(obj.AddressId),
                AddressAdditionalInformation = obj.AddressAdditionalInformation
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
