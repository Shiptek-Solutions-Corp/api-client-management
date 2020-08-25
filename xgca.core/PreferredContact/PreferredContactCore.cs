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
using xgca.entity.Migrations;

namespace xgca.core.PreferredContact
{
    public class PreferredContactCore : IPreferredContactCore
    {
        private readonly ICompanyData _company;
        private readonly IGuestData _guest;
        private readonly IPagedResponse _pagedResponse;
        private readonly IPreferredContactData _preferredContact;
        private readonly IPreferredContactHelper _prefConHelper;
        private readonly IGeneral _general;

        public PreferredContactCore(ICompanyData company, IGuestData guest, IPagedResponse pagedResponse,
            IPreferredContactData preferredContact, IPreferredContactHelper prefConhelper, IGeneral general)
        {
            _company = company;
            _guest = guest;
            _pagedResponse = pagedResponse;
            _preferredContact = preferredContact;
            _prefConHelper = prefConhelper;
            _general = general;
        }

        public async Task<IGeneralModel> Create(entity.Models.PreferredContact obj)
        {
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

            if (preferredContact.ContactType == 1)
            {
                var company = await _company.Retrieve(Guid.Parse(preferredContact.CompanyId));
                return _general.Response(new 
                { 
                    Contact = new
                    {
                        PreferredContactId = preferredContact.Guid.ToString(),
                        ContactId = company.Guid.ToString(),
                        ContactName = company.CompanyName,
                        ImageURL = (company.ImageURL is null) ? "No Image" : company.ImageURL,
                        CompleteAddress = _prefConHelper.RegisteredAddress(company)
                    }
                }, 200, "Configurable details for selected contact has been displayed", true);
            }

            var guest = await _guest.Retrieve(Guid.Parse(preferredContact.GuestId));
            var contact = new
            {
                PreferredContactId = preferredContact.Guid.ToString(),
                ContactId = guest.Id.ToString(),
                ContactName = guest.GuestName,
                ImageURL = guest.Image,
                CompleteAddress = guest.AddressLine,
                City = new
                {
                    guest.CityId,
                    guest.CityName
                },
                State = new
                {
                    guest.StateId,
                    guest.StateName
                },
                Country = new
                {
                    guest.CountryId,
                    guest.CountryName,
                },
                guest.ZipCode,
                Phone = new
                {
                    guest.PhoneNumberPrefixId,
                    guest.PhoneNumberPrefix,
                    guest.PhoneNumber
                },
                Mobile = new
                {
                    guest.MobileNumberPrefixId,
                    guest.MobileNumberPrefix,
                    guest.MobileNumber
                },
                Fax = new
                {
                    guest.FaxNumberPrefixId,
                    guest.FaxNumberPrefix,
                    guest.FaxNumber
                },
                guest.FirstName,
                guest.LastName,
                guest.EmailAddress
            };
 
            return _general.Response(new { Contact = contact }, 200, "Configurable details for selected contact has been displayed", true);
        }
    }
}
