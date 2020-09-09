using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using xgca.core.Models.Invite;
using xgca.core.Response;

namespace xgca.core.Invite
{
    public interface IInviteCore
    {
        Task<IGeneralModel> SendContactsInvites(ListReceiverEmails receivers, int companyId);
        Task<IGeneralModel> CheckInviteCode(string inviteCode, int type);
        Task<IGeneralModel> AcceptContactsInvite(AcceptInviteCode accept, int profileId, string createdBy);
        Task<IGeneralModel> AcceptProvidersInvite(AcceptInviteCode accept, int profileId, string createdBy);
        Task<IGeneralModel> SendProvidersInvites(ListReceiverEmails receivers, int companyId);
    }
}
