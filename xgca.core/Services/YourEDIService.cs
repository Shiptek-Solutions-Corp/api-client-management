using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using xgca.core.Response;
using xgca.core.Models.YourEDI;
using xgca.data.Company;
using xgca.data.Guest;

namespace xgca.core.Services
{
    public interface IYourEDIService
    {
        Task<IGeneralModel> GetYourEIDActors(YourEdiRequest yourEdiRequest);
        Task<IGeneralModel> SetCUCC(YourEdiCUCC obj);
    }
    public class YourEDIService : IYourEDIService
    {
        private readonly ICompanyData _company;
        private readonly IGuestData _guest;
        private readonly IGeneral _general;

        public YourEDIService(ICompanyData company, IGuestData guest, IGeneral general)
        {
            _company = company;
            _guest = guest;
            _general = general;
        }

        public async Task<IGeneralModel> GetYourEIDActors(YourEdiRequest yourEdiRequest)
        {
            var shipperRegistered = await _company.GetCompanyAndMasterUserDetails(yourEdiRequest.Actors.ShipperId);
            var shipperGuest = await _guest.GetGuestAndMasterUserDetails(yourEdiRequest.Actors.ShipperId);

            var consigneeRegistered = await _company.GetCompanyAndMasterUserDetails(yourEdiRequest.Actors.ConsigneeId);
            var consigneeGuest = await _guest.GetGuestAndMasterUserDetails(yourEdiRequest.Actors.ConsigneeId);

            var bookingPartyRegistered = await _company.GetCompanyAndMasterUserDetails(yourEdiRequest.Actors.BookingPartyId);
            var bookingPartyGuest = await _guest.GetGuestAndMasterUserDetails(yourEdiRequest.Actors.BookingPartyId);

            List<dynamic> notifyParties = new List<dynamic>();

            foreach (var notifyParty in yourEdiRequest.Actors.NotifyPartyIds)
            {
                var notifyPartyRegistered = await _company.GetCompanyAndMasterUserDetails(notifyParty);
                var notifyPartyGuest = await _guest.GetGuestAndMasterUserDetails(notifyParty);

                if (!(notifyPartyRegistered is null))
                {
                    notifyParties.Add(notifyPartyRegistered);
                }
                else
                {
                    notifyParties.Add(notifyPartyGuest);
                }
            }

            var actors = new YourEditResponseDetails();
            actors.Shipper = (shipperRegistered is null) ? (dynamic)shipperGuest : (dynamic)shipperRegistered;
            actors.Consignee = (consigneeRegistered is null) ? (dynamic)consigneeGuest : (dynamic)consigneeRegistered;
            actors.BookingParty = (bookingPartyRegistered is null) ? (dynamic)bookingPartyGuest : (dynamic)bookingPartyRegistered;
            actors.NotifyParties = (notifyParties.Count == 0) ? null : notifyParties;

            var data = new YourEdiResponse();
            data.Actors = actors;

            return _general.Response(new { YourEDI = data }, 200, "Your EDI Actors retrieved", true);
        }

        public async Task<IGeneralModel> SetCUCC(YourEdiCUCC obj)
        {
            var company = await _company.Retrieve(Guid.Parse(obj.CompanyId));
            var guest = await _guest.Retrieve(Guid.Parse(obj.CompanyId));

            bool result = false;
            if (!(company is null))
            {
                result = await _company.SetCUCCByCompanyGuid(obj.CompanyId, obj.CUCC);
            }
            else
            {
                result = await _guest.SetCUCCByGuestGuid(obj.CompanyId, obj.CUCC);
            }

            return result
                ? _general.Response(null, 200, "CUCC updated!", true)
                : _general.Response(null, 400, "Error in updating CUCC", false);
        }
    }
}
