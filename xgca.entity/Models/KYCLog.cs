using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace xgca.entity.Models
{
    [Table("KYCLog", Schema = "Settings")]
    public class KYCLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int KYCLogId { get; set; }
        public int CompanyId { get; set; }
        public int CompanySectionsId { get; set; }
        public string Remarks { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid Guid { get; set; }
        public string SectionStatusCode { get; set; }

        public virtual Company Company { get; set; }
        public virtual CompanySections CompanySections { get; set; }
        public virtual SectionStatus SectionStatus { get; set; }
    }
}
