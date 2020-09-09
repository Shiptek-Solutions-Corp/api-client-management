using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace xgca.entity.Models
{
    [Table("Invite", Schema = "Company")]
    public class Invite
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int InviteId { get; set; }
        public string InviteCode { get; set; }
        public string ReceiverId { get; set; }
        public string ReceiverEmail { get; set; }
        public int InviteType { get; set; } // 1 - Contact, 2- Provider
        public int CompanyId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ExpiresOn { get; set; }
        public Guid Id { get; set; }

        public virtual Company Company { get; set; }
    }
}
