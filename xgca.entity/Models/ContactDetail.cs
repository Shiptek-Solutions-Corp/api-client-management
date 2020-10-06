using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace xgca.entity.Models
{
    [Table("ContactDetail")]
    public class ContactDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ContactDetailId { get; set; }
        public int PhonePrefixId { get; set; }
        [StringLength(10)]
        public string PhonePrefix { get; set; }
        [StringLength(20)]
        public string Phone { get; set; }
        public int MobilePrefixId { get; set; }
        [StringLength(10)]
        public string MobilePrefix { get; set; }
        [StringLength(20)]
        public string Mobile { get; set; }
        public int FaxPrefixId { get; set; }
        [StringLength(10)]
        public string FaxPrefix { get; set; }
        [StringLength(20)]
        public string Fax { get; set; }
        [System.ComponentModel.DefaultValue(0)]
        public byte IsDeleted { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Guid { get; set; }
    }
}
