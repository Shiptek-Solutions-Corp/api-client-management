using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using xgca.core.Models.Email;
using xgca.core.Response;

namespace xgca.core.Email
{
    public interface IEmail
    {
        Task<IGeneralModel> SendContactInviteEmail(EmailModel model);
        Task<IGeneralModel> SendProviderInviteEmail(EmailModel model);
        Task<IGeneralModel> SendCompanyActivationEmail(EmailModel model);
    }
}
