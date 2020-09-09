using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xgca.entity;

namespace xgca.data.Invite
{
    public class InviteData : IInviteData
    {
        private readonly IXGCAContext _context;

        public InviteData(IXGCAContext context)
        {
            _context = context;
        }

        public async Task<bool> BatchCreate(List<entity.Models.Invite> invites)
        {
            _context.Invites.AddRange(invites);
            var result = await _context.SaveChangesAsync();
            return result > 0 ? true : false;
        }

        public async Task<entity.Models.Invite> CheckInviteCode(string inviteCode, int inviteType)
        {
            var invite = await _context.Invites.AsNoTracking()
                .Include(c => c.Company)
                    .ThenInclude(a => a.Addresses)
                .Where(i => i.InviteCode == inviteCode && i.InviteType == inviteType && i.ExpiresOn >= DateTime.UtcNow).FirstOrDefaultAsync();

            return invite;
        }

        public Task<bool> Create(entity.Models.Invite obj)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteInvite(int id)
        {
            var invite = await _context.Invites.SingleOrDefaultAsync(x => x.InviteId == id);
            _context.Invites.Remove(invite);
            var result = await _context.SaveChangesAsync();
            return result > 0 ? true : false;
        }

        public async Task<bool> DeleteInvites()
        {
            _context.Invites.RemoveRange(_context.Invites.Where(x => x.ExpiresOn <= DateTime.UtcNow));
            var result = await _context.SaveChangesAsync();
            return result > 0 ? true : false;
        }

        public Task<List<entity.Models.Invite>> List()
        {
            throw new NotImplementedException();
        }

        public Task<entity.Models.Invite> Retrieve(int key)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(entity.Models.Invite obj)
        {
            throw new NotImplementedException();
        }
    }
}
