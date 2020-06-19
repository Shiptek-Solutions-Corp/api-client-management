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
    }
}
