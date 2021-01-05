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
using System.Linq;

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

        public async Task<IGeneralModel> List(int profileId, string search, string name, string country, string stateCity, int type, string contact, string sortBy, string sortOrder, int pageNumber, int pageSize)
        {
            var guests = await _preferredContact.GetGuestIds(Convert.ToInt32(profileId));
            var registered = await _preferredContact.GetRegisteredIds(Convert.ToInt32(profileId));

            List<entity.Models.Guest> filteredGuests = null;
            List<entity.Models.Company> filteredCompanies = null;

            if (type == 1)
            {
                filteredGuests = await _guest.SearchGuest(search, name, country, stateCity, contact, guests.Item2);
            }
            else if (type == 2)
            {
                filteredCompanies = await _company.SearchCompany(search, name, country, stateCity, contact, registered.Item2);
            }
            else
            {
                filteredGuests = await _guest.SearchGuest(search, name, country, stateCity, contact, guests.Item2);
                filteredCompanies = await _company.SearchCompany(search, name, country, stateCity, contact, registered.Item2);
            }

            List<ViewPreferredContact> preferredContacts = new List<ViewPreferredContact>();
            if (!(filteredGuests is null))
            {
                foreach(var g in guests.Item1)
                {
                    var guest = filteredGuests.SingleOrDefault(x => x.Id.ToString() == g.GuestId);

                    if (guest is null)
                    {
                        continue;
                    }

                    var guestCityProvince = _prefConHelper.GuestCityState(guest);

                    preferredContacts.Add(new ViewPreferredContact
                    {
                        PreferredContactId = g.PreferredContactId,
                        ContactId = g.GuestId,
                        ContactName = guest.GuestName,
                        ImageURL = (guest.Image is null) ? "-" : guest.Image,
                        CityProvince = guestCityProvince,
                        Country = guest.CountryName,
                        ContactType = 2,
                        PhoneNumber = guest.PhoneNumber,
                        MobileNumber = guest.MobileNumber,
                        FaxNumber = guest.FaxNumber
                    });
                }
            }

            if (!(filteredCompanies is null))
            {
                foreach(var r in registered.Item1)
                {
                    var company = filteredCompanies.SingleOrDefault(x => x.Guid.ToString() == r.RegisteredId);

                    if (company is null)
                    {
                        continue;
                    }

                    var registeredCityProvince = _prefConHelper.RegisteredCityState(company);

                    preferredContacts.Add(new ViewPreferredContact
                    {
                        PreferredContactId = r.PreferredContactId,
                        ContactId = r.RegisteredId,
                        ContactName = company.CompanyName,
                        ImageURL = (company.ImageURL is null) ? "-" : company.ImageURL,
                        CityProvince = registeredCityProvince,
                        Country = company.Addresses.CountryName,
                        ContactType = 1,
                        PhoneNumber = company.ContactDetails.Phone,
                        MobileNumber = company.ContactDetails.Mobile,
                        FaxNumber = company.ContactDetails.Fax
                    });
                }
            }

            var pagedResponse = _pagedResponse.Paginate(preferredContacts, preferredContacts.Count, pageNumber, pageSize);
            return _general.Response(pagedResponse, 200, "Configurable preferred contacts has been listed", true);
        }

        public async Task<IGeneralModel> QuickSearch(string search, int profileId, int pageNumber, int pageSize, int recordCount)
        {
            var guests = await _preferredContact.GetGuestIds(Convert.ToInt32(profileId));
            var registered = await _preferredContact.GetRegisteredIds(Convert.ToInt32(profileId));

            List<entity.Models.Guest> filteredGuests = null;
            List<entity.Models.Company> filteredCompanies = null;

            filteredGuests = await _guest.SearchGuest(search, null, null, null, null, guests.Item2);
            filteredCompanies = await _company.SearchCompany(search, null, null, null, null, registered.Item2);


            List<ViewPreferredContact> preferredContacts = new List<ViewPreferredContact>();
            if (!(filteredGuests is null))
            {
                foreach (var g in guests.Item1)
                {
                    var guest = filteredGuests.SingleOrDefault(x => x.Id.ToString() == g.GuestId);

                    if (guest is null)
                    {
                        continue;
                    }

                    var guestCityProvince = _prefConHelper.GuestCityState(guest);

                    preferredContacts.Add(new ViewPreferredContact
                    {
                        PreferredContactId = g.PreferredContactId,
                        ContactId = g.GuestId,
                        ContactName = guest.GuestName,
                        ImageURL = (guest.Image is null) ? "-" : guest.Image,
                        CityProvince = guestCityProvince,
                        Country = guest.CountryName,
                        ContactType = 2,
                        PhoneNumber = guest.PhoneNumber,
                        MobileNumber = guest.MobileNumber,
                        FaxNumber = guest.FaxNumber
                    });
                }
            }

            if (!(filteredCompanies is null))
            {
                foreach (var r in registered.Item1)
                {
                    var company = filteredCompanies.SingleOrDefault(x => x.Guid.ToString() == r.RegisteredId);

                    if (company is null)
                    {
                        continue;
                    }

                    var registeredCityProvince = _prefConHelper.RegisteredCityState(company);

                    preferredContacts.Add(new ViewPreferredContact
                    {
                        PreferredContactId = r.PreferredContactId,
                        ContactId = r.RegisteredId,
                        ContactName = company.CompanyName,
                        ImageURL = (company.ImageURL is null) ? "-" : company.ImageURL,
                        CityProvince = registeredCityProvince,
                        Country = company.Addresses.CountryName,
                        ContactType = 1,
                        PhoneNumber = company.ContactDetails.Phone,
                        MobileNumber = company.ContactDetails.Mobile,
                        FaxNumber = company.ContactDetails.Fax
                    });
                }
            }

            var pagedResponse = _pagedResponse.Paginate(preferredContacts, preferredContacts.Count, pageNumber, pageSize);
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
