using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xgca.core.Constants;
using xgca.core.Helpers.Guest;
using xgca.core.Helpers.Utility;
using xgca.core.Models.Guest;
using xgca.core.PreferredContact;
using xgca.core.Response;
using xgca.core.User;
using xgca.data.Guest;

namespace xgca.core.Guest
{
    public class GuestCore : IGuestCore
    {
        private readonly IPreferredContactCore _preferredContact;
        private readonly IUser _user;

        private readonly IGuestData _guest;

        private readonly IGeneral _general;
        private readonly IGuestHelper _guestHelper;
        private readonly IMapper _mapper;
        private readonly IPreferredContactHelper _prefConHelper;
        
        public GuestCore(IPreferredContactCore preferredContact, IUser user, IGuestData guest,
            IGeneral general, IGuestHelper guestHelper, IMapper mapper, IPreferredContactHelper prefConHelper)
        {
            _preferredContact = preferredContact;
            _user = user;
            _guest = guest;
            _general = general;
            _guestHelper = guestHelper;
            _mapper = mapper;
            _prefConHelper = prefConHelper;
        }

        public async Task<IGeneralModel> Create(CreateGuest obj, string username, int companyId)
        {
            if (obj is null)
            {
                return _general.Response(null, 400, "Data cannot be null", false);
            }

            int userId = await _user.GetIdByUsername(username);
            userId = (userId == 0) ? GlobalVariables.SystemUserId : userId;

            var mappedModel = _mapper.Map<entity.Models.Guest>(obj);
            mappedModel.CompanyId = companyId;
            mappedModel.Id = Guid.NewGuid();
            mappedModel.CreatedBy = userId;
            mappedModel.CreatedOn = DateTime.UtcNow;
            mappedModel.ModifiedBy = userId;
            mappedModel.ModifiedOn = DateTime.UtcNow;
            mappedModel.IsGuest = true;

            string newGuestId = await _guest.CreateAndReturnGuid(mappedModel);

            var preferredContact = _prefConHelper.ParseObject(newGuestId, null, companyId, 2, userId);
            await _preferredContact.Create(preferredContact);

            return newGuestId.Length > 0
                ? _general.Response(null, 200, "New guest created successfully", true)
                : _general.Response(null, 400, "Error on creating new guest", false);
        }

        public async Task<IGeneralModel> CreateAndReturnId(CreateGuest obj, string username, int companyId)
        {
            if (obj is null)
            {
                return _general.Response(null, 400, "Data cannot be null", false);
            }

            int userId = await _user.GetIdByUsername(username);

            var mappedModel = _mapper.Map<entity.Models.Guest>(obj);
            mappedModel.CompanyId = companyId;
            mappedModel.Id = Guid.NewGuid();
            mappedModel.CreatedBy = (userId == 0) ? GlobalVariables.SystemUserId : userId;
            mappedModel.CreatedOn = DateTime.UtcNow;
            mappedModel.ModifiedBy = (userId == 0) ? GlobalVariables.SystemUserId : userId;
            mappedModel.ModifiedOn = DateTime.UtcNow;
            mappedModel.IsGuest = true;

            int newGuestId = await _guest.CreateAndReturnId(mappedModel);

            return newGuestId > 0
                ? _general.Response(new { GuestId = newGuestId }, 200, "New guest created successfully", true)
                : _general.Response(null, 400, "Error on creating new guest", false);
        }
        public async Task<IGeneralModel> CreateAndReturnGuid(CreateGuest obj, string username, int companyId)
        {
            if (obj is null)
            {
                return _general.Response(null, 400, "Data cannot be null", false);
            }

            int userId = await _user.GetIdByUsername(username);

            var mappedModel = _mapper.Map<entity.Models.Guest>(obj);
            mappedModel.CompanyId = companyId;
            mappedModel.Id = Guid.NewGuid();
            mappedModel.CreatedBy = (userId == 0) ? GlobalVariables.SystemUserId : userId;
            mappedModel.CreatedOn = DateTime.UtcNow;
            mappedModel.ModifiedBy = (userId == 0) ? GlobalVariables.SystemUserId : userId;
            mappedModel.ModifiedOn = DateTime.UtcNow;
            mappedModel.IsGuest = true;

            string newGuestId = await _guest.CreateAndReturnGuid(mappedModel);

            return newGuestId.Length > 0
                ? _general.Response(new { GuestId = newGuestId }, 200, "New guest created successfully", true)
                : _general.Response(null, 400, "Error on creating new guest", false);
        }


        public async Task<IGeneralModel> Delete(string key, string username)
        {
            if (key is null)
            {
                return _general.Response(null, 400, "Key cannot be null", false);
            }

            var guest = await _guest.Retrieve(Guid.Parse(key));
            if (guest is null)
            {
                return _general.Response(null, 400, "Selected record may have been deleted or does not exists", false);
            }

            var userId = await _user.GetIdByUsername(username);

            var data = new entity.Models.Guest
            {
                IsDeleted = 1,
                DeletedBy = (userId == 0) ? GlobalVariables.SystemUserId : userId,
                DeletedOn = DateTime.UtcNow
            };

            var result = await _guest.Delete(data);
            return result
                ? _general.Response(null, 200, "Guest deleted successfully", true)
                : _general.Response(null, 400, "Error on deleting guest", false);
        }

        public async Task<IGeneralModel> GetGuidById(int key)
        {
            string guid = await _guest.GetGuidById(key);
            return _general.Response(new { GuestId = guid }, 200, "Guest Id retrieved", true);
        }

        public async Task<IGeneralModel> GetIdByGuid(string key)
        {
            int id = await _guest.GetIdByGuid(Guid.Parse(key));
            return _general.Response(new { GuestId = id }, 200, "Guest Id retrieved", true);
        }

        public async Task<IGeneralModel> List()
        {
            var data = await _guest.List();

            var guests = data.Select(g => new
            {
                GuestId = g.Id,
                g.GuestName,
                Address = _guestHelper.ParseCompleteAddress(g),
                Image = (g.Image is null) ? "No Image" : g.Image,
                g.IsGuest
            });

            return _general.Response(new { Guests = guests }, 200, "Configurable guests has been listed", true);
        }

        public Task<IGeneralModel> List(string columnFilter)
        {
            throw new NotImplementedException();
        }

        public async Task<IGeneralModel> ShowDetails(int key)
        {
            var data = await _guest.Retrieve(key);
            return _general.Response(new { Guest = data }, 200, "Configurable details for selected guest has been displayed", true);
        }

        public async Task<IGeneralModel> ShowDetails(string key)
        {
            var data = await _guest.Retrieve(Guid.Parse(key));
            return _general.Response(new { Guest = data }, 200, "Configurable details for selected guest has been displayed", true);
        }

        public async Task<IGeneralModel> Update(UpdateGuestContact obj, string username, int companyId)
        {
            if (obj is null)
            {
                return _general.Response(null, 400, "Data cannot be null", false);
            }

            int userId = await _user.GetIdByUsername(username);

            var mappedModel = _mapper.Map<entity.Models.Guest>(obj);

            var guest = await _guest.Retrieve(Guid.Parse(obj.ContactId));
            if (guest is null)
            {
                return _general.Response(null, 400, "Selected guest may have been deleted or does not exists", false);
            }

            mappedModel.GuestId = guest.GuestId;
            mappedModel.GuestName = obj.ContactName;
            mappedModel.AddressLine = obj.CompleteAddress;
            mappedModel.CompanyId = companyId;
            mappedModel.Image = obj.ImageURL;
            mappedModel.ModifiedBy = userId;
            mappedModel.ModifiedOn = DateTime.UtcNow;

            var result = await _guest.Update(mappedModel);
            return result
                ? _general.Response(null, 200, "Guest updated successfully", true)
                : _general.Response(null, 400, "Error on updating guest", false);
        }

        public async Task<IGeneralModel> Update(UpdateGuest obj, string username, int companyId)
        {
            if (obj is null)
            {
                return _general.Response(null, 400, "Data cannot be null", false);
            }

            int userId = await _user.GetIdByUsername(username);

            var mappedModel = _mapper.Map<entity.Models.Guest>(obj);

            var guest = await _guest.Retrieve(Guid.Parse(obj.GuestId));
            if (guest is null)
            {
                return _general.Response(null, 400, "Selected guest may have been deleted or does not exists", false);
            }

            mappedModel.GuestId = guest.GuestId;
            mappedModel.CompanyId = companyId;
            mappedModel.ModifiedBy = userId;
            mappedModel.ModifiedOn = DateTime.UtcNow;

            var result = await _guest.Update(mappedModel);
            return result
                ? _general.Response(null, 200, "Guest updated successfully", true)
                : _general.Response(null, 400, "Error on updating guest", false);
        }
    }
}
