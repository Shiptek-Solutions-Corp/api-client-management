using System;
using System.Collections.Generic;
using System.Text;
using xgca.entity.Models;
using Newtonsoft.Json;

namespace xgca.core.Helpers
{
    public class ContactDetailHelper
    {
        public static xgca.entity.Models.ContactDetail BuildNewContactDetail(dynamic obj)
        {
            string json = JsonConvert.SerializeObject(obj);
            var fax = json.Contains("fax") ? obj.Fax : null;
            var faxPrefixId = json.Contains("faxPrefixId") ? obj.FaxPrefixId : 0;
            var faxPrefix = json.Contains("faxPrefix") ? obj.FaxPrefix : null;
            int createdBy = json.Contains("createdBy") ? obj.UserId : 0;

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
                CreatedBy = createdBy,
                CreatedOn = DateTime.Now,
                ModifiedBy = createdBy,
                ModifiedOn = DateTime.Now,
                Guid = Guid.NewGuid(),
            };

            return contactDetail;
        }

        public static xgca.entity.Models.ContactDetail BuildExistingContactDetail(dynamic obj, int contactDetailId)
        {
            string json = JsonConvert.SerializeObject(obj);
            var fax = json.Contains("fax") ? obj.Fax : null;
            var faxPrefixId = json.Contains("faxPrefixId") ? obj.FaxPrefixId : 0;
            var faxPrefix = json.Contains("faxPrefix") ? obj.FaxPrefix : null;
            int modifiedBy = json.Contains("modifiedBy") ? obj.UserId : 0;
            var contactDetail = new xgca.entity.Models.ContactDetail
            {
                ContactDetailId = contactDetailId,
                PhonePrefixId = obj.PhonePrefixId,
                PhonePrefix = obj.PhonePrefix,
                Phone = obj.Phone,
                MobilePrefixId = obj.MobilePrefixId,
                MobilePrefix = obj.MobilePrefix,
                Mobile = obj.Mobile,
                FaxPrefixId = faxPrefixId,
                FaxPrefix = faxPrefix,
                Fax = fax,
                ModifiedBy = modifiedBy,
                ModifiedOn = DateTime.Now,
                Guid = Guid.Parse(obj.ContactDetailId)
            };

            return contactDetail;
        }
    }
}
