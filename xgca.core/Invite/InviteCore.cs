using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using xgca.core.Email;
using xgca.core.Helpers.Utility;
using xgca.core.Models.Email;
using xgca.core.Models.Invite;
using xgca.core.Response;
using xgca.data.Company;
using xgca.data.Invite;
using xgca.data.PreferredContact;
using xgca.data.PreferredProvider;
using xgca.data.CompanyService;
using xgca.data.User;
using xgca.core.Helpers.Http;
using xgca.core.Models.Service;
using Microsoft.Extensions.Options;
using xgca.core.Helpers;
using xgca.core.Constants;

namespace xgca.core.Invite
{
    public class InviteCore : IInviteCore
    {
        private readonly IEmail _email;
        private readonly IInviteData _invite;
        private readonly IGeneral _general;
        private readonly ICompanyData _company;
        private readonly ICompanyService _companyService;
        private readonly IUserData _user;
        private readonly IPreferredContactHelper _prefConHelper;
        private readonly IPreferredContactData _preferredContact;
        private readonly IPreferredProviderData _preferredProvider;
        private readonly IUtilityHelper _utility;
        private readonly IHttpHelper _httpHelpers;
        private readonly IOptions<GlobalCmsService> _options;

        public InviteCore(IEmail email, IInviteData invite, IGeneral general, ICompanyData company, ICompanyService companyService, IUserData user, IPreferredContactHelper prefConHelper,
            IPreferredContactData preferredContact, IPreferredProviderData preferredProvider, IUtilityHelper utility, IHttpHelper httpHelpers, IOptions<GlobalCmsService> options)
        {
            _email = email;
            _invite = invite;
            _general = general;
            _prefConHelper = prefConHelper;
            _preferredContact = preferredContact;
            _preferredProvider = preferredProvider;
            _company = company;
            _companyService = companyService;
            _user = user;
            _utility = utility;
            _httpHelpers = httpHelpers;
            _options = options;
        }

        public async Task<IGeneralModel> AcceptContactsInvite(AcceptInviteCode accept, int profileId, string createdBy)
        {
            var invite = await _invite.CheckInviteCode(accept.InviteCode, 1);
            var companyGuid = await _company.GetGuidById(invite.CompanyId);
            var createdById = await _user.GetIdByUsername(createdBy);

            if (invite is null)
            {
                return _general.Response(null, 400, "Invite code does not exists or is already expired", false);
            }

            var isExists = await _preferredContact.CheckIfContactAlreadyAdded(companyGuid, profileId);
            if (isExists)
            {
                return _general.Response(null, 400, "Company already exists on contact list", false);
            }

            var registerdContact = new entity.Models.PreferredContact
            {
                ProfileId = profileId,
                CompanyId = companyGuid,
                ContactType = 1,
                CreatedBy = createdById,
                CreatedOn = DateTime.UtcNow,
                ModifiedBy = createdById,
                ModifiedOn = DateTime.UtcNow,
                IsDeleted = 0,
                Guid = Guid.NewGuid()
            };

            var result = await _preferredContact.Create(registerdContact);
            if (!result)
            {
                return _general.Response(null, 400, "Error on accepting invite", false);
            }

            await _invite.DeleteInvite(invite.InviteId);

            return _general.Response(null, 200, "Company has been registered to contacts", true);
        }

        public async Task<IGeneralModel> AcceptProvidersInvite(AcceptInviteCode accept, int profileId, string createdBy)
        {
            var createdById = await _user.GetIdByUsername(createdBy);

            var invite = await _invite.CheckInviteCode(accept.InviteCode, 2);
            if (invite is null)
            {
                return _general.Response(null, 400, "Invite code does not exists or is already expired", false);
            }

            var companyGuid = await _company.GetGuidById(invite.CompanyId);



            List<entity.Models.CompanyService> tmpCompanyServices = await _companyService.ListByCompanyId(invite.CompanyId);
            List<entity.Models.PreferredProvider> existingPreferredProviders = await _preferredProvider.List(profileId, companyGuid);
            List<entity.Models.CompanyService> companyServices = new List<entity.Models.CompanyService>();

            foreach(var tmpCompanyService in tmpCompanyServices)
            {
                var exists = existingPreferredProviders.Find(x => x.CompanyServiceId == tmpCompanyService.Guid.ToString());

                if (exists is null)
                {
                    companyServices.Add(new entity.Models.CompanyService
                    {
                        Guid = tmpCompanyService.Guid,
                        ServiceId = tmpCompanyService.ServiceId
                    });
                }
            }

            if (companyServices is null)
            {
                await _invite.DeleteInvite(invite.InviteId);
                return _general.Response(null, 200, "Service provider already added. Invite code will now be invalidated.", true);
            }

            var serviceResponse = await _httpHelpers.Get(_options.Value.BaseUrl, _options.Value.GetService, null, AuthToken.Contra);
            string statusCode = serviceResponse.statusCode;

            if (!(statusCode.Equals("200")))
            {
                return _general.Response(null, 400, "Error in fetching services", false);
            }

            List<ListServiceModel> services = new List<ListServiceModel>();
            foreach (var service in serviceResponse.data.services)
            {
                services.Add(new ListServiceModel
                {
                    IntServiceId = service.intServiceId,
                    ServiceId = service.serviceId,
                    ServiceCode = service.serviceCode,
                    ServiceName = service.serviceName,
                    ServiceImageURL = service.imageURL,
                    ServiceStaticId = service.serviceStaticId
                });
            }

            List<entity.Models.PreferredProvider> providers = new List<entity.Models.PreferredProvider>();
            foreach (var companyService in companyServices)
            {
                var service = services.Find(x => x.IntServiceId == companyService.ServiceId);

                providers.Add(new entity.Models.PreferredProvider
                {
                    CompanyServiceId = companyService.Guid.ToString(),
                    CompanyId = companyGuid,
                    ServiceId = service.ServiceId,
                    ProfileId = profileId,
                    Guid = Guid.NewGuid(),
                    CreatedBy = createdById,
                    CreatedOn = DateTime.UtcNow,
                    ModifiedBy = createdById,
                    ModifiedOn = DateTime.UtcNow,
                    IsDeleted = 0,
                });
            }

            var result = await _preferredProvider.Create(providers);
            
            if (!result)
            {
                return _general.Response(null, 400, "Error on accepting invite", false);
            }

            await _invite.DeleteInvite(invite.InviteId);

            return _general.Response(null, 200, "Provider has been added to preferred lists", true);
        }

        public async Task<IGeneralModel> CheckInviteCode(string code, int type)
        {
            var invite = await _invite.CheckInviteCode(code, type);

            if(invite is null)
            {
                return _general.Response(null, 400, "Invite code does not exists or is already expired", false);
            }

            var senderDetails = new
            {
                CompanyId = invite.Company.Guid,
                invite.Company.CompanyName,
                CompanyAddress = _prefConHelper.RegisteredAddress(invite.Company),
                ImageURL = (invite.Company.ImageURL is null) ? "No Image" : invite.Company.ImageURL
            };

            return _general.Response(new { Sender = senderDetails }, 200, "Sender details displayed", true);
        }

        public async Task<IGeneralModel> SendContactsInvites(ListReceiverEmails receivers, int companyId)
        {
            if (receivers is null)
            {
                return _general.Response(null, 400, "Receiver email address(es) cannot be empty", false);
            }

            List<entity.Models.Invite> invites = new List<entity.Models.Invite>();

            foreach (var email in receivers.Receivers)
            {
                string inviteCode = _utility.GeteRandomStrings(5);

                invites.Add(new entity.Models.Invite
                {
                    Id = Guid.NewGuid(),
                    InviteCode = inviteCode,
                    InviteType = 1,
                    ReceiverEmail = email,
                    CompanyId = companyId,
                    CreatedOn = DateTime.UtcNow,
                    ExpiresOn = DateTime.UtcNow.AddHours(48)
                });
            }

            await _invite.DeleteInvites();
            await _invite.BatchCreate(invites);
            
            var sender = await _company.Retrieve(companyId);
            foreach(var invite in invites)
            {
                var masterUser = await _user.GetUserByEmail(invite.ReceiverEmail);

                string receiverName = (masterUser is null)  ? "User" :  $"{masterUser.FirstName} {masterUser.LastName}";

                var payload = new
                {
                    EmailAddress = invite.ReceiverEmail,
                    ReceiverName = receiverName,
                    SenderCompanyName = sender.CompanyName,
                    invite.InviteCode
                };

                EmailModel emailPayload = new EmailModel()
                {
                    Payload = payload,
                    Additionals = null,
                };

                var response = await _email.SendContactInviteEmail(emailPayload);
                if (!response.isSuccessful)
                {
                    return _general.Response(response.message, 400, "Error", false);
                }
            }

            return _general.Response(null, 200, "Email invites sent successsfully", true);
        }

        public async Task<IGeneralModel> SendProvidersInvites(ListReceiverEmails receivers, int companyId)
        {
            if (receivers is null)
            {
                return _general.Response(null, 400, "Receiver email address(es) cannot be empty", false);
            }

            List<entity.Models.Invite> invites = new List<entity.Models.Invite>();

            foreach (var email in receivers.Receivers)
            {
                string inviteCode = _utility.GeteRandomStrings(5);

                invites.Add(new entity.Models.Invite
                {
                    Id = Guid.NewGuid(),
                    InviteCode = inviteCode,
                    InviteType = 2,
                    ReceiverEmail = email,
                    CompanyId = companyId,
                    CreatedOn = DateTime.UtcNow,
                    ExpiresOn = DateTime.UtcNow.AddHours(48)
                });
            }

            await _invite.DeleteInvites();
            await _invite.BatchCreate(invites);

            var sender = await _company.Retrieve(companyId);
            
            foreach (var invite in invites)
            {
                var masterUser = await _user.GetUserByEmail(invite.ReceiverEmail);
                string receiverName = (masterUser is null) ? "User" : $"{masterUser.FirstName} {masterUser.LastName}";

                var payload = new
                {
                    EmailAddress = invite.ReceiverEmail,
                    ReceiverName = receiverName,
                    SenderCompanyName = sender.CompanyName,
                    invite.InviteCode
                };

                EmailModel emailPayload = new EmailModel()
                {
                    Payload = payload,
                    Additionals = null,
                };

                var response = await _email.SendProviderInviteEmail(emailPayload);
                if(!response.isSuccessful)
                {
                    return _general.Response(response.message, 400, "Error", false);
                }
            }

            return _general.Response(null, 200, "Email invites sent successfully", true);
        }
    }
}
