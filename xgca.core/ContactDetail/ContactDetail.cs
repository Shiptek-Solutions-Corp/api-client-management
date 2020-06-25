using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using xgca.core.Response;
using xgca.entity.Models;
using xgca.data.ContactDetail;
using xgca.core.Helpers;
using xgca.core.Models.ContactDetail;

namespace xgca.core.ContactDetail
{
    public class ContactDetail : IContactDetail
    {
        private readonly xgca.data.ContactDetail.IContactDetail _contactDetail;
        private readonly IGeneral _general;

        public ContactDetail(xgca.data.ContactDetail.IContactDetail contactDetail, IGeneral general)
        {
            _contactDetail = contactDetail;
            _general = general;
        }

        public Task<IGeneralModel> Create(CreateContactDetailModel obj)
        {
            throw new NotImplementedException();
        }
        public Task<IGeneralModel> Update(UpdateContactDetailModel obj)
        {
            throw new NotImplementedException();
        }
        public Task<IGeneralModel> Delete(string key)
        {
            throw new NotImplementedException();
        }

        public Task<IGeneralModel> List()
        {
            throw new NotImplementedException();
        }

        public Task<IGeneralModel> Retrieve(string key)
        {
            throw new NotImplementedException();
        }
        public async Task<int> CreateAndReturnId(dynamic obj, int creatdById)
        {
            string json = JsonConvert.SerializeObject(obj);
            var fax = json.Contains("Fax") ? obj.Fax : null;
            var faxPrefixId = json.Contains("FaxPrefixId") ? obj.FaxPrefixId : 0;
            var faxPrefix = json.Contains("FaxPrefix") ? obj.FaxPrefix : null;

            var contactDetail = new xgca.entity.Models.ContactDetail
            {
                PhonePrefixId = obj.PhonePrefixId,
                PhonePrefix = obj.PhonePrefix,
                Phone = obj.Phone,
                MobilePrefixId = obj.MobilePrefixId,
                MobilePrefix = obj.MobilePrefix,
                Mobile = obj.Mobile,
                FaxPrefixId = faxPrefixId,
                FaxPrefix = faxPrefix,
                Fax = fax,
                CreatedBy = creatdById,
                CreatedOn = DateTime.UtcNow,
                ModifiedBy = creatdById,
                ModifiedOn = DateTime.UtcNow,
                Guid = Guid.NewGuid(),
            };
            int contactDetailId = await _contactDetail.CreateAndReturnId(contactDetail);
            return contactDetailId;
        }
        public async Task<int> UpdateAndReturnId(dynamic obj, int modifiedById)
        {
            int contactDetailId = await _contactDetail.GetIdByGuid(Guid.Parse(obj.ContactDetailId));
            var contactDetail = ContactDetailHelper.BuildExistingContactDetail(obj, contactDetailId, modifiedById);
            await _contactDetail.Update(contactDetail);
            return contactDetailId;
        }
    }
}
