using System;
using System.Collections.Generic;
using System.Text;

namespace xgca.core.Models.User
{
    public class OnboardingSubMerchantModel
    {
        public string CountryCode { get; set; }
        public string CurrencyCode { get; set; }
        public OnboardingCreateAccount CompanyInfo { get; set; }
        public OnboardingCreateUser AuthorizedRepresentative { get; set; }

    }

    public class OnboardingCreateAccount
    {
        public string Name { get; set; }
        public string MobilePrefix { get; set; }
        public string MobileNumber { get; set; }
        public string LandLinePrefix { get; set; }
        public string LandLineNumber { get; set; }
        public string FaxPrefix { get; set; }
        public string FaxNumber { get; set; }
        public string EmailAddress { get; set; }
        public string WebsiteUrl { get; set; }
        public OnboardingCreateAddress CompanyAddress { get; set; }
    }

    public class OnboardingCreateUser
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string EmailAddress { get; set; }
        public string MobilePrefix { get; set; }
        public string MobileNumber { get; set; }
        public string LandLinePrefix { get; set; }
        public string LandLineNumber { get; set; }
        public string Title { get; set; }
    }

    public class OnboardingCreateAddress
    {
        public string State { get; set; }
        public string CityTown { get; set; }
        public string PostalCode { get; set; }
        public decimal? Lat { get; set; }
        public decimal? Long { get; set; }
        public string FullAddress { get; set; }
    }
}
