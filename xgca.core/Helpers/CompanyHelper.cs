using System;
using System.Collections.Generic;
using System.Text;
using xgca.entity.Models;

namespace xgca.core.Helpers
{
    public class CompanyHelper
    {
        public static dynamic BuildCompanyValue(dynamic company)
        {
            dynamic services = null;
            if (company.CompanyServices == null)
            { services = null; }
            else
            {
                services = company.CompanyServices;
                services = CompanyServiceHelper.BuildCompanyServiceList(services);
            }

            var obj = new
            {
                CompanyId = company.CompanyId,
                CompanyName = company.CompanyName,
                AddressLine = company.Addresses.AddressLine,
                CityName = company.Addresses.CityName,
                StateName = company.Addresses.StateName,
                ZipCode = company.Addresses.ZipCode,
                CountryId = company.Addresses.CountryId,
                CountryName = company.Addresses.CountryName,
                ImageURL = company.ImageURL,
                WebsiteURL = company.WebsiteURL,
                EmailAddress = company.EmailAddress,
                Phone = String.Concat(company.ContactDetails.PhonePrefix, company.ContactDetails.Phone),
                Mobile = String.Concat(company.ContactDetails.MobilePrefix, company.ContactDetails.Mobile),
                Fax = String.Concat(company.ContactDetails.FaxPrefix, company.ContactDetails.Fax),
                TaxExemption = company.TaxExemption,
                TaxExemptionStatus = company.TaxExemptionStatus,
                Guid = company.Guid,
                Addresses = company.Addresses,
                ContactDetails = company.ContactDetails,
                CompanyServices = services
            };
            return obj;
        }

        public static dynamic ReturnUpdatedValue(dynamic companyObj, dynamic companyServicesObj)
        {
            string fullAddress = AddressHelper.GenerateFullAddress(companyObj);
            var company = new
            {
                companyObj.CompanyId,
                companyObj.CompanyName,
                companyObj.ImageURL,
                companyObj.AddressId,
                companyObj.AddressLine,
                City = new
                {
                    companyObj.CityId,
                    companyObj.CityName,
                },
                State = new
                {
                    companyObj.StateId,
                    companyObj.StateName,
                },
                Country = new
                {
                    companyObj.CountryId,
                    companyObj.CountryName,
                },
                companyObj.ZipCode,
                FullAddress = fullAddress,
                companyObj.Longitude,
                companyObj.Latitude,
                companyObj.WebsiteURL,
                companyObj.EmailAddress,
                companyObj.ContactDetailId,
                Phone = new
                {
                    companyObj.PhonePrefixId,
                    companyObj.PhonePrefix,
                    companyObj.Phone,
                },
                Mobile = new
                {
                    companyObj.MobilePrefixId,
                    companyObj.MobilePrefix,
                    companyObj.Mobile,
                },
                Fax = new
                {
                    companyObj.FaxPrefixId,
                    companyObj.FaxPrefix,
                    companyObj.Fax,
                },
                companyObj.TaxExemption,
                companyObj.TaxExemptionStatus,
                CompanyServices = companyServicesObj
            };

            return company;
        }
    }
}
