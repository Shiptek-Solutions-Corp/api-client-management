using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using xgca.data.Guest;
using xgca.core.Helpers.Utility;
using xgca.core.Models.PreferredContact;
using xgca.core.Response;
using xgca.data.Company;
using xgca.data.PreferredContact;
using xgca.core.User;
using xgca.core.Helpers.QueryFilter;
using xgca.core.Helpers.Http;
using Microsoft.Extensions.Options;
using xgca.core.Helpers;
using Newtonsoft.Json.Linq;
using xgca.core.Constants;
using xgca.data.CompanyUser;

namespace xgca.core.PreferredContact
{
    public class PreferredContactCore : IPreferredContactCore
    {
        private readonly ICompanyData _company;
        private readonly IGuestData _guest;
        private readonly IPagedResponse _pagedResponse;
        private readonly IPreferredContactData _preferredContact;
        private readonly IPreferredContactHelper _prefConHelper;
        private readonly IUser _user;
        private readonly IGeneral _general;
        private readonly IQueryFilterHelper _query;
        private readonly IUtilityHelper _utility;
        private readonly IHttpHelper _httpHelper;
        private readonly IOptions<GlobalCmsService> _options;
        private readonly ICompanyUser _companyUser;

        public PreferredContactCore(ICompanyData company, IGuestData guest, IPagedResponse pagedResponse, IHttpHelper httpHelper, IOptions<GlobalCmsService> options,
            IPreferredContactData preferredContact, IPreferredContactHelper prefConhelper, IGeneral general, IQueryFilterHelper query, IUtilityHelper utility, ICompanyUser companyUser)
        {
            _company = company;
            _guest = guest;
            _pagedResponse = pagedResponse;
            _preferredContact = preferredContact;
            _prefConHelper = prefConhelper;
            _general = general;
            _query = query;
            _utility = utility;
            _httpHelper = httpHelper;
            _options = options;
            _companyUser = companyUser;
        }

        public async Task<IGeneralModel> Create(entity.Models.PreferredContact obj)
        {
            if (obj is null)
            {
                return _general.Response(null, 400, "Error in creating preferred contact", false);
            }

            var data = new entity.Models.PreferredContact
            {
                GuestId = obj.GuestId,
                CompanyId = obj.CompanyId,
                ProfileId = obj.ProfileId,
                ContactType = obj.ContactType,
                CreatedBy = obj.CreatedBy,
                CreatedOn = DateTime.UtcNow,
                ModifiedBy = obj.ModifiedBy,
                ModifiedOn = DateTime.UtcNow,
                Guid = Guid.NewGuid()
            };

            var result = await _preferredContact.Create(data);

            return result
                ? _general.Response(null, 200, "Preferred contact created successfully", true)
                : _general.Response(null, 400, "Error in creating preferred contact", false);
        }

        public async Task<IGeneralModel> DeleteContact(string key)
        {
            var result = await _preferredContact.Delete(key);
            if(!result)
            {
                return _general.Response(null, 400, "Contact does not exists or may have already been deleted", false);
            }

            return _general.Response(null, 200, "Contact deleted successfully", true);
        }

        public async Task<IGeneralModel> List(string companyId, int pageNumber, int pageSize)
        {
            int profileId = await _company.GetIdByGuid(Guid.Parse(companyId));
            int recordCount = await _preferredContact.GetRecordCount(profileId);
            var data = await _preferredContact.List(profileId, pageNumber, pageSize);

            if (data is null)
            {
                return _general.Response(null, 200, "Configurable preferred contacts has been listed", true);
            }

            List<ViewPreferredContact> preferredContacts = new List<ViewPreferredContact>();
            foreach (entity.Models.PreferredContact d in data)
            {
                string contactId = "";
                string contactName = "";
                string imageURL = "";
                string cityProvince = "";
                string country = "";

                switch (d.ContactType)
                {
                    case 1:
                        {
                            var company = await _company.Retrieve(Guid.Parse(d.CompanyId));

                            if (company is null)
                            {
                                contactId = "00000000-0000-0000-000000000000";
                                contactName = "-";
                                imageURL = "No Image";
                                cityProvince = "-";
                                country = "-";
                                break;
                            }

                            contactId = company.Guid.ToString();
                            contactName = company.CompanyName;
                            imageURL = company.ImageURL;
                            cityProvince = _prefConHelper.RegisteredCityState(company);
                            country = company.Addresses.CountryName;
                            break;
                        }

                    case 2:
                        {
                            var guest = await _guest.Retrieve(Guid.Parse(d.GuestId));

                            if (guest is null)
                            {
                                contactId = "00000000-0000-0000-000000000000";
                                contactName = "-";
                                imageURL = "No Image";
                                cityProvince = "-";
                                country = "-";
                                break;
                            }

                            contactId = guest.Id.ToString();
                            contactName = guest.GuestName;
                            imageURL = guest.Image;
                            cityProvince = _prefConHelper.GuestCityState(guest);
                            country = guest.CountryName;
                            break;
                        }
                }

                preferredContacts.Add(new ViewPreferredContact
                {
                    PreferredContactId = d.Guid.ToString(),
                    ContactId = contactId,
                    ContactName = contactName,
                    ImageURL = imageURL,
                    CityProvince = cityProvince,
                    Country = country,
                    ContactType = d.ContactType
                });
            }

            var pagedResponse = _pagedResponse.Paginate(preferredContacts, recordCount, pageNumber, pageSize);
            return _general.Response(pagedResponse, 200, "Configurable preferred contacts has been listed", true);
        }

        public async Task<IGeneralModel> List(int profileId, int pageNumber, int pageSize)
        {
            int recordCount = await _preferredContact.GetRecordCount(profileId);
            var data = await _preferredContact.List(profileId, pageNumber, pageSize);

            if (data is null)
            {
                return _general.Response(null, 200, "Configurable preferred contacts has been listed", true);
            }

            List<ViewPreferredContact> preferredContacts = new List<ViewPreferredContact>();
            foreach (entity.Models.PreferredContact d in data)
            {
                string contactId = "";
                string contactName = "";
                string imageURL = "";
                string cityProvince = "";
                string country = "";

                switch (d.ContactType)
                {
                    case 1:
                        {
                            var company = await _company.Retrieve(Guid.Parse(d.CompanyId));

                            if(company is null)
                            {
                                contactId = "00000000-0000-0000-000000000000";
                                contactName = "-";
                                imageURL = "No Image";
                                cityProvince = "-";
                                country = "-";
                                break;
                            }

                            contactId = company.Guid.ToString();
                            contactName = company.CompanyName;
                            imageURL = company.ImageURL;
                            cityProvince = _prefConHelper.RegisteredCityState(company);
                            country = company.Addresses.CountryName;
                            break;
                        }

                    case 2:
                        {
                            var guest = await _guest.Retrieve(Guid.Parse(d.GuestId));

                            if (guest is null)
                            {
                                contactId = "00000000-0000-0000-000000000000";
                                contactName = "-";
                                imageURL = "No Image";
                                cityProvince = "-";
                                country = "-";
                                break;
                            }

                            contactId = guest.Id.ToString();
                            contactName = guest.GuestName;
                            imageURL = guest.Image;
                            cityProvince = _prefConHelper.GuestCityState(guest);
                            country = guest.CountryName;
                            break;
                        }
                }

                preferredContacts.Add(new ViewPreferredContact
                {
                    PreferredContactId = d.Guid.ToString(),
                    ContactId = contactId,
                    ContactName = contactName,
                    ImageURL = imageURL,
                    CityProvince = cityProvince,
                    Country = country,
                    ContactType = d.ContactType
                });
            }

            var pagedResponse = _pagedResponse.Paginate(preferredContacts, recordCount, pageNumber, pageSize);
            return _general.Response(pagedResponse, 200, "Configurable preferred contacts has been listed", true);
        }

        public async Task<IGeneralModel> QuickSearch(string search, int profileId, int pageNumber, int pageSize, int recordCount)
        {
            var guestIds = await _preferredContact.GetGuestIds(Convert.ToInt32(profileId));
            var registeredIds = await _preferredContact.GetRegisteredIds(Convert.ToInt32(profileId));

            var filteredGuestIds = await _guest.QuickSearch(search, guestIds);
            var filteredRegisteredIds = await _company.QuickSearch(search, registeredIds);

            var data = new List<entity.Models.PreferredContact>();

            if (filteredGuestIds is null && filteredRegisteredIds is null)
            {
                data = await _preferredContact.List(profileId, pageNumber, pageSize);
            }
            else
            {
                data = await _preferredContact.GetContactsByQuickSearch(profileId, filteredGuestIds, filteredRegisteredIds, pageNumber, pageSize);
            }

            if (data is null)
            {
                return _general.Response(null, 200, "Configurable preferred contacts has been listed", true);
            }

            List<ViewPreferredContact> preferredContacts = new List<ViewPreferredContact>();
            foreach (entity.Models.PreferredContact d in data)
            {
                string contactId = "";
                string contactName = "";
                string imageURL = "";
                string cityProvince = "";
                string country = "";

                switch (d.ContactType)
                {
                    case 1:
                        {
                            var company = await _company.Retrieve(Guid.Parse(d.CompanyId));

                            if (company is null)
                            {
                                contactId = "00000000-0000-0000-000000000000";
                                contactName = "-";
                                imageURL = "No Image";
                                cityProvince = "-";
                                country = "-";
                                break;
                            }

                            contactId = company.Guid.ToString();
                            contactName = company.CompanyName;
                            imageURL = company.ImageURL;
                            cityProvince = _prefConHelper.RegisteredCityState(company);
                            country = company.Addresses.CountryName;
                            break;
                        }

                    case 2:
                        {
                            var guest = await _guest.Retrieve(Guid.Parse(d.GuestId));

                            if (guest is null)
                            {
                                contactId = "00000000-0000-0000-000000000000";
                                contactName = "-";
                                imageURL = "No Image";
                                cityProvince = "-";
                                country = "-";
                                break;
                            }

                            contactId = guest.Id.ToString();
                            contactName = guest.GuestName;
                            imageURL = guest.Image;
                            cityProvince = _prefConHelper.GuestCityState(guest);
                            country = guest.CountryName;
                            break;
                        }
                }

                preferredContacts.Add(new ViewPreferredContact
                {
                    PreferredContactId = d.Guid.ToString(),
                    ContactId = contactId,
                    ContactName = contactName,
                    ImageURL = imageURL,
                    CityProvince = cityProvince,
                    Country = country,
                    ContactType = d.ContactType
                });
            }

            pageSize = (pageSize < data.Count) ? data.Count : pageSize;

            var pagedResponse = _pagedResponse.Paginate(preferredContacts, data.Count, pageNumber, pageSize);
            return _general.Response(pagedResponse, 200, "Configurable preferred contacts has been listed", true);
        }

        public async Task<IGeneralModel> ShowDetails(string preferredContactId)
        {
            if (preferredContactId is null)
            {
                return _general.Response(null, 400, "Preferred contact id cannot be null", false);
            }

            var preferredContact = await _preferredContact.Retrieve(Guid.Parse(preferredContactId));

            if (preferredContact is null)
            {
                return _general.Response(null, 400, "Selected record may have been deleted or does not exists", false);
            }

            dynamic contact = null;

            if (preferredContact.ContactType == 1)
            {
                var company = await _company.Retrieve(Guid.Parse(preferredContact.CompanyId));
                var cityResponse = await _httpHelper.GetGuidById(_options.Value.BaseUrl, $"{_options.Value.GetCity}/", company.Addresses.CityId, AuthToken.Contra);
                var cityJson = (JObject)cityResponse;
                var stateResponse = await _httpHelper.GetGuidById(_options.Value.BaseUrl, $"{_options.Value.GetState}/", company.Addresses.StateId, AuthToken.Contra);
                var stateJson = (JObject)stateResponse;
                var city = new 
                { 
                    CityId = (cityJson)["data"]["cityId"], 
                    company.Addresses.CityName 
                };
                var state = new
                {
                    StateId = (stateJson)["data"]["stateId"],
                    company.Addresses.StateName
                };
                var masterUser = await _companyUser.GetMasterUser(company.CompanyId, (int)UserType.MasterUser);
                contact = _prefConHelper.BuildCompanyDetails(preferredContact.Guid.ToString(), company, state, city, masterUser.Users);
            }
            else
            {
                var guest = await _guest.Retrieve(Guid.Parse(preferredContact.GuestId));
                contact = _prefConHelper.BuildGuestDetails(preferredContact.Guid.ToString(), guest);
            }
 
            return _general.Response(new { Contact = contact }, 200, "Configurable details for selected contact has been displayed", true);
        }
    }
}
