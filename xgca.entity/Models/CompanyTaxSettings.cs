using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace xgca.entity.Models
{
    [Table("CompanyTaxSettings", Schema = "Company")]
    public class CompanyTaxSettings
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CompanyTaxSettingsId { get; set; }
        public int CompanyId { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Guid { get; set; }
        public string TaxTypeId { get; set; }
        public string TaxTypeDescription { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TaxPercentageRate { get; set; }
        public bool IsTaxExcempted { get; set; }
        public int Status { get; set; }
        public string StatusName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public virtual Company Company{ get; set; }
    }
}
