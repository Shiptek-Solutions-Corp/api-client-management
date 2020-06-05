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
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CompanyId { get; set; }
        [System.ComponentModel.DefaultValue(1)]
        public int ClientId { get; set; }
        [Required]
        [StringLength(100)]
        public string CompanyName { get; set; }
        public int AddressId { get; set; }
        public int ContactDetailId { get; set; }
        [StringLength(500)]
        public string ImageURL { get; set; }
        [StringLength(100)]
        public string EmailAddress { get; set; }
        [StringLength(100)]
        public string WebsiteURL { get; set; }
        [System.ComponentModel.DefaultValue(1)]
        public byte Status { get; set; }
        [System.ComponentModel.DefaultValue(0)]
        public byte IsDeleted { get; set; }
        public byte TaxExemption { get; set; }
        public byte TaxExemptionStatus { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Guid { get; set; }

        public virtual Address Addresses { get; set; }
        public virtual ContactDetail ContactDetails { get; set; }
        public virtual ICollection<CompanyService> CompanyServices { get; set; }
        public virtual ICollection<CompanyUser> CompanyUsers { get; set; }
    }
}
