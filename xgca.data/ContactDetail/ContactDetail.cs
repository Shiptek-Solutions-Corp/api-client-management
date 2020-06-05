using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using xgca.entity;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace xgca.data.ContactDetail
{
    public class ContactDetail : IMaintainable<entity.Models.ContactDetail>, IContactDetail
    {
        private readonly IXGCAContext _context;

        public ContactDetail(IXGCAContext context)
        {
            _context = context;
        }

        public async Task<int> CreateAndReturnId(entity.Models.ContactDetail obj)
        {
            await _context.ContactDetails.AddAsync(obj);
            var result = await _context.SaveChangesAsync();
            return obj.ContactDetailId;
        }
        public async Task<entity.Models.ContactDetail> Retrieve(int key)
        {
            var data = await _context.ContactDetails
                .Where(c => c.ContactDetailId == key && c.IsDeleted == 0)
                .FirstOrDefaultAsync();
            return data;
        }
        public async Task<int> GetIdByGuid(Guid guid)
        {
            var data = await _context.ContactDetails
                .Where(c => c.Guid == guid).FirstOrDefaultAsync();
            if (data == null) { return 0; }
            return data.ContactDetailId;
        }
        public Task<bool> Create(entity.Models.ContactDetail obj)
        {
            throw new NotImplementedException();
        }
        public Task<List<entity.Models.ContactDetail>> List()
        {
            throw new NotImplementedException();
        }
        public async Task<bool> Update(entity.Models.ContactDetail obj)
        {
            var data = await _context.ContactDetails.Where(c => c.ContactDetailId == obj.ContactDetailId).FirstOrDefaultAsync();
            if (data == null)
            {
                return false;
            }
            data.PhonePrefixId = obj.PhonePrefixId;
            data.PhonePrefix = obj.PhonePrefix;
            data.Phone = obj.Phone;
            data.MobilePrefixId = obj.MobilePrefixId;
            data.MobilePrefix = obj.MobilePrefix;
            data.Mobile = obj.Mobile;
            data.FaxPrefixId = obj.FaxPrefixId;
            data.FaxPrefix = obj.FaxPrefix;
            data.Fax = obj.Fax;
            data.ModifiedBy = obj.ModifiedBy;
            data.ModifiedOn = obj.ModifiedOn;
            var result = await _context.SaveChangesAsync();
            return result > 0 ? true : false;
        }
        public Task<bool> Delete(int key)
        {
            throw new NotImplementedException();
        }
    }
}
