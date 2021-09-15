using System;
using System.Collections.Generic;

namespace xgca.entity.Models
{
    public partial class CompanyBeneficialOwners
    {
        public string BeneficialOwnersTypeCode { get; set; }
        public int CompanyId { get; set; }
        public int CompanyBeneficialOwnersId { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public string StateId { get; set; }
        public string StateName { get; set; }
        public string CityId { get; set; }
        public string CityName { get; set; }
        public string PostalCode { get; set; }
        public string CompanyAddress { get; set; }
        public string AdditionalAddress { get; set; }
        public bool? IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool IsDeleted { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? DeletedOn { get; set; }
        public Guid Guid { get; set; }
        public string PostalId { get; set; }

        public virtual BeneficialOwnersType BeneficialOwnersTypeCodeNavigation { get; set; }
        public virtual Company Company { get; set; }
    }
}
