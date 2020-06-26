using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace xgca.entity.Models
{
    [Table("AuditLog", Schema = "Settings")]
    public class AuditLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AuditLogId { get; set; }
        [StringLength(20)]
        public string AuditLogAction { get; set; }
        [StringLength(50)]
        public string TableName { get; set; }
        public int KeyFieldId { get; set; }
        public string NewValue { get; set; }
        public string OldValue { get; set; }
        public int CreatedBy { get; set; }
        public int CreatedByName { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid Guid { get; set; }
    }
}
