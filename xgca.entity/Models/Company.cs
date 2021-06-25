using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace xgca.entity.Models
{
    [Table("Company", Schema = "Company")]
    public class Company
    {
        public Company()
        {
            CompanyBeneficialOwners = new HashSet<CompanyBeneficialOwners>();
            CompanyDirectors = new HashSet<CompanyDirectors>();
            CompanyDocuments = new HashSet<CompanyDocuments>();
            CompanySections = new HashSet<CompanySections>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CompanyId { get; set; }
        [System.ComponentModel.DefaultValue(1)]
        public int ClientId { get; set; }
        [StringLength(10)]
        public string CompanyCode { get; set; }
        [Required]
        [StringLength(160)]
        public string CompanyName { get; set; }
        public int AddressId { get; set; }
        public int ContactDetailId { get; set; }
        [StringLength(500)]
        public string ImageURL { get; set; }
        [StringLength(74)]
        public string EmailAddress { get; set; }
        [StringLength(50)]
        public string WebsiteURL { get; set; }
        [System.ComponentModel.DefaultValue(1)]
        public byte Status { get; set; }
        public string StatusName { get; set; }
        [System.ComponentModel.DefaultValue(0)]
        public byte IsDeleted { get; set; }
        public byte TaxExemption { get; set; }
        public byte TaxExemptionStatus { get; set; }
        public string CUCC { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Guid { get; set; }
        public string AccreditedBy { get; set; }
        public string KycStatusCode { get; set; }

        public virtual Address Addresses { get; set; }
        public virtual ContactDetail ContactDetails { get; set; }
        public virtual ICollection<CompanyService> CompanyServices { get; set; }
        public virtual ICollection<CompanyUser> CompanyUsers { get; set; }

        public virtual KycStatus KycStatusCodeNavigation { get; set; }
        public virtual CompanyStructure CompanyStructure { get; set; }
        public virtual ICollection<CompanyBeneficialOwners> CompanyBeneficialOwners { get; set; }
        public virtual ICollection<CompanyDirectors> CompanyDirectors { get; set; }
        public virtual ICollection<CompanyDocuments> CompanyDocuments { get; set; }
        public virtual ICollection<CompanySections> CompanySections { get; set; }
    }
}
