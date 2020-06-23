using System;
using System.Collections.Generic;
using System.Text;
using xgca.entity.Models;

namespace xgca.core.Helpers
{
    public class CompanyHelper
    {
        public static dynamic BuildCompanyLogValue(dynamic company, dynamic companyServices)
        {
            dynamic services = null;
            if (company.CompanyServices == null)
            { services = null; }
            else
            {
                services = CompanyServiceHelper.BuildCompanyServiceList(companyServices);
            }

            var obj = new
            {
                company.CompanyName,
                company.AddressLine,
                company.CityName,
                company.StateName,
                company.ZipCode,
                company.CountryName,
                company.ImageURL,
                company.WebsiteURL,
                company.EmailAddress,
                Phone = String.Concat(company.ContactDetails.PhonePrefix, company.ContactDetails.Phone),
                Mobile = String.Concat(company.ContactDetails.MobilePrefix, company.ContactDetails.Mobile),
                Fax = String.Concat(company.ContactDetails.FaxPrefix, company.ContactDetails.Fax),
                TaxExemption = company.TaxExemption == 1 ? "Yes" : "No",
                TaxExemptionStatus = company.TaxExemptionStatus == 1 ? "Yes" : "No",
                CompanyServices = services
            };
            return obj;
        }
        public static dynamic BuildCompanyValue(dynamic company, dynamic companyServices)
        {
            dynamic services = null;
            if (company.CompanyServices == null)
            { services = null; }
            else
            {
                services = CompanyServiceHelper.BuildCompanyServiceList(companyServices);
            }

            var obj = new
            {
                company.CompanyName,
                company.Addresses.AddressLine,
                company.Addresses.CityName,
                company.Addresses.StateName,
                company.Addresses.ZipCode,
                company.Addresses.CountryName,
                company.ImageURL,
                company.WebsiteURL,
                company.EmailAddress,
                Phone = String.Concat(company.ContactDetails.PhonePrefix, company.ContactDetails.Phone),
                Mobile = String.Concat(company.ContactDetails.MobilePrefix, company.ContactDetails.Mobile),
                Fax = String.Concat(company.ContactDetails.FaxPrefix, company.ContactDetails.Fax),
                TaxExemption = company.TaxExemption == 1 ? "Yes" : "No",
                TaxExemptionStatus = company.TaxExemptionStatus == 1 ? "Yes" : "No",
                CompanyServices = services
            };
            return obj;
        }

        public static dynamic ReturnUpdatedValue(dynamic companyObj, dynamic companyServicesObj)
        {
            string fullAddress = AddressHelper.GenerateFullAddress(companyObj.Addresses);
            var data = new
            {
                companyObj.CompanyId,
                companyObj.CompanyName,
                companyObj.ImageURL,
                companyObj.AddressId,
                companyObj.Addresses.AddressLine,
                City = new
                {
                    companyObj.Addresses.CityId,
                    companyObj.Addresses.CityName,
                },
                State = new
                {
                    companyObj.Addresses.StateId,
                    companyObj.Addresses.StateName,
                },
                Country = new
                {
                    companyObj.Addresses.CountryId,
                    companyObj.Addresses.CountryName,
                },
                companyObj.Addresses.ZipCode,
                FullAddress = fullAddress,
                companyObj.Addresses.Longitude,
                companyObj.Addresses.Latitude,
                companyObj.WebsiteURL,
                companyObj.EmailAddress,
                companyObj.ContactDetailId,
                Phone = new
                {
                    companyObj.ContactDetails.PhonePrefixId,
                    companyObj.ContactDetails.PhonePrefix,
                    companyObj.ContactDetails.Phone,
                },
                Mobile = new
                {
                    companyObj.ContactDetails.MobilePrefixId,
                    companyObj.ContactDetails.MobilePrefix,
                    companyObj.ContactDetails.Mobile,
                },
                Fax = new
                {
                    companyObj.ContactDetails.FaxPrefixId,
                    companyObj.ContactDetails.FaxPrefix,
                    companyObj.ContactDetails.Fax,
                },
                companyObj.TaxExemption,
                companyObj.TaxExemptionStatus,
                CompanyServices = companyServicesObj
            };

            return data;
        }
    }
}
