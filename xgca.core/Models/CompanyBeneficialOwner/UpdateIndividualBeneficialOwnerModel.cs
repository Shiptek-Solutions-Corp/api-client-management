using System;
using System.Collections.Generic;
using System.Text;

namespace xgca.core.Models.CompanyBeneficialOwner
{
    public class UpdateIndividualBeneficialOwnerModel
    {
        public string Id { get; set; }
        public string BeneficialOwnersTypeCode { get; set; }
        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public string StateId { get; set; }
        public string StateName { get; set; }
        public string CityId { get; set; }
        public string CityName { get; set; }
        public string PostalId { get; set; }
        public string PostalCode { get; set; }
        public string PersonalAddress { get; set; }
        public string AdditionalAddress { get; set; }
        public bool? IsActive { get; set; }
    }
}
