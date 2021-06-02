using Amazon.S3.Model;
using Amazon.S3.Model.Internal.MarshallTransformations;
using DocumentFormat.OpenXml.Office2010.Drawing;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using xgca.core.Helpers;
using xgca.core.Helpers.Http;
using xgca.core.Models.Email;
using xgca.core.Response;

namespace xgca.core.Email
{
    public class Email : IEmail
    {
        private readonly IOptions<EmailTemplate> _emailTemplate;
        private readonly IOptions<EmailApi> _emailOptions;
        private readonly IOptions<WebsiteLinks> _website;
        private readonly IHttpHelper _httpHelper;
        private readonly IGeneral _general;

        public Email(IOptions<EmailTemplate> emailTemplate, IOptions<EmailApi> emailOptions, IOptions<WebsiteLinks> website, IHttpHelper httpHelper, IGeneral general)
        {
            _emailTemplate = emailTemplate;
            _emailOptions = emailOptions;
            _website = website;
            _httpHelper = httpHelper;
            _general = general;
        }

        public async Task<IGeneralModel> SendCompanyActivationEmail(EmailModel model)
        {
            var message = _emailTemplate.Value.BaseTemplate
               .Replace("{content}",
                   _emailTemplate.Value.SendCompanyActivationTemplate
                       .Replace("{receiver_name}", model.Payload.ReceiverName)
                       .Replace("{company_name}", model.Payload.SenderCompanyName));

            EmailPayload payload = new EmailPayload
            {
                sender = "no-reply@myxlog.com",
                to = model.Payload.EmailAddress,
                subject = "Account Activation",
                message = message
            };

            await _httpHelper.Post(_emailOptions.Value.BaseUrl, payload, null);

            return _general.Response(null, 200, "Activation email sent", true);
        }

        public async Task<IGeneralModel> SendContactInviteEmail(EmailModel model)
        {
            var message = _emailTemplate.Value.BaseTemplate
                .Replace("{content}",
                    _emailTemplate.Value.SendContactInviteTemplate
                        .Replace("{receiver_name}", model.Payload.ReceiverName)
                        .Replace("{sender_company_name}", model.Payload.SenderCompanyName)
                        .Replace("{invite_code}", model.Payload.InviteCode)
                        .Replace("login_link", _website.Value.Login));

            EmailPayload payload = new EmailPayload
            {
                sender = "no-reply@myxlog.com",
                to = model.Payload.EmailAddress,
                subject = "XLOG: Contact Invite",
                message = message
            };

            await _httpHelper.Post(_emailOptions.Value.BaseUrl, payload, null);

            return _general.Response(null, 200, "Invite email sent", true);
        }

        public async Task<IGeneralModel> SendProviderInviteEmail(EmailModel model)
        {
            var message = _emailTemplate.Value.BaseTemplate
                .Replace("{content}",
                    _emailTemplate.Value.SendProviderInviteTemplate
                        .Replace("{receiver_name}", model.Payload.ReceiverName)
                        .Replace("{sender_company_name}", model.Payload.SenderCompanyName)
                        .Replace("{invite_code}", model.Payload.InviteCode)
                        .Replace("{login_link}", _website.Value.Login));

            EmailPayload payload = new EmailPayload
            {
                sender = "no-reply@myxlog.com",
                to = model.Payload.EmailAddress,
                subject = $"XLOG: Add {model.Payload.SenderCompanyName} as preferred provider",
                message = message
            };

            await _httpHelper.Post(_emailOptions.Value.BaseUrl, payload, null);

            return _general.Response(null, 200, "Invite email sent", true);
        }
    }
}
