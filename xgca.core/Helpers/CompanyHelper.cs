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
                company.Status,
                company.StatusName,
                Phone = String.Concat(company.ContactDetails.PhonePrefix, company.ContactDetails.Phone),
                Mobile = String.Concat(company.ContactDetails.MobilePrefix, company.ContactDetails.Mobile),
                Fax = String.Concat(company.ContactDetails.FaxPrefix, company.ContactDetails.Fax),
                TaxExemption = company.TaxExemption == 1 ? "Yes" : "No",
                TaxExemptionStatus = company.TaxExemptionStatus == 1 ? "Yes" : "No",
                CompanyServices = services
            };
            return obj;
        }

        public static dynamic ReturnUpdatedValue(dynamic companyObj, string cityId, string stateId, dynamic companyServicesObj, string kycStatus = "NEW")
        {
            string fullAddress = AddressHelper.GenerateFullAddress(companyObj.Addresses);
            var data = new
            {
                CompanyId = companyObj.Guid,
                companyObj.CompanyName,
                companyObj.ImageURL,
                AddressId = companyObj.Addresses.Guid,
                companyObj.Addresses.AddressLine,
                City = new
                {
                    CityId = cityId,
                    companyObj.Addresses.CityName,
                },
                State = new
                {
                    StateId = stateId,
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
                companyObj.Addresses.AddressAdditionalInformation,
                companyObj.WebsiteURL,
                companyObj.EmailAddress,
                ContactDetailId = companyObj.ContactDetails.Guid,
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
                CUCC = companyObj.CUCC,
                companyObj.TaxExemption,
                companyObj.TaxExemptionStatus,
                CompanyServices = companyServicesObj,
                Status = (companyObj.Status == 1) ? "Active" : "Inactive",
                KYCStatus = (kycStatus is null) ? "NEW" : kycStatus
            };

            return data;
        }

        public static string GenerateCompanyCode(string companyName, int tries, int numOfChar = 5)
        {
            string[] names = companyName.Split(" ");
            string companyCode = "";

            string GetPadZeroes(int num, int len)
            {
                return num.ToString().PadLeft(len, '0');
            }

            foreach (string name in names)
            {
                companyCode += name;
                if (companyCode.Length > numOfChar)
                {
                    continue;
                }
            }

            
            if (companyCode.Length > numOfChar)
            {
                if (tries == 0)
                {
                    companyCode = companyCode.Substring(0, numOfChar);
                }
                else
                {
                    int len = Convert.ToString(tries).Length;
                    companyCode = $"{companyCode.Substring(0, (numOfChar - len))}{Convert.ToString(tries)}";
                }
            }
            else
            {
                int missingLen = numOfChar - names[0].Length;

                companyCode = $"{companyCode}{GetPadZeroes(tries, missingLen)}";
            }

           

            return companyCode.ToUpper();
        }

        public static string ParseCompanydAddress(dynamic obj)
        {
            string companyAddress = "";

            companyAddress = (!(obj.Addresses.AddressLine is null) ? obj.Addresses.AddressLine : "");
            companyAddress += companyAddress.Length != 0 ? ", " : "";
            companyAddress += (!(obj.Addresses.CityName is null) ? obj.Addresses.CityName : "");
            companyAddress += companyAddress.Length != 0 ? ", " : "";
            companyAddress += (!(obj.Addresses.StateName is null) ? obj.Addresses.StateName : "");
            companyAddress += companyAddress.Length != 0 ? ", " : "";
            companyAddress += obj.Addresses.CountryName;

            return companyAddress;
        }
    }
}
