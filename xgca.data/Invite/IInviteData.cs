using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace xgca.data.Invite
{
    public interface IInviteData : IMaintainable<entity.Models.Invite>
    {
        Task<bool> BatchCreate(List<entity.Models.Invite> invites);
        Task<entity.Models.Invite> CheckInviteCode(string inviteCode, int inviteType);
        Task<bool> DeleteInvite(int id);
        Task<bool> DeleteInvites();
    }
}
