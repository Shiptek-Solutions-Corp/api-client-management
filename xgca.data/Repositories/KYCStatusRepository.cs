using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using xgca.entity;
using xgca.entity.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace xgca.data.Repositories
{
    public interface IKYCStatusRepository : IRepository<KycStatus>
    {
        Task<(KycStatus, string)> GetByKycStatusCode(string code);
    }
    public class KYCStatusRepository  : IKYCStatusRepository
    {
        private readonly IXGCAContext _context;
        public KYCStatusRepository(IXGCAContext _context)
        {
            this._context = _context;
        }

        public Task<(KycStatus, string)> Create(KycStatus obj)
        {
            throw new NotImplementedException();
        }

        public Task<(bool, string)> Delete(KycStatus obj)
        {
            throw new NotImplementedException();
        }

        public Task<(KycStatus, string)> Get(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<(KycStatus, string)> GetByKycStatusCode(string code)
        {
            var record = await _context.KycStatuses.AsNoTracking()
                .Where(x => x.KycStatusCode == code && x.IsDeleted == false)
                .FirstOrDefaultAsync();

            return (record is null)
                ? (null, "Record does not exists or may have been deleted")
                : (record, "KYC Status retrieved");
        }

        public Task<(List<KycStatus>, string)> List()
        {
            throw new NotImplementedException();
        }

        public Task<(KycStatus, string)> Update(KycStatus obj)
        {
            throw new NotImplementedException();
        }
    }
}
