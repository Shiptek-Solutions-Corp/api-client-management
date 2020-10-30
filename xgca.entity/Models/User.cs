using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace xgca.entity.Models
{
    [Table("User", Schema= "Users")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        [StringLength(24)]
        public string Username { get; set; }
        public int ContactDetailId { get; set; }
        [Required]
        [StringLength(100)]
        public string LastName { get; set; }
        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }
        [StringLength(100)]
        public string MiddleName { get; set; }

        [NotMapped]
        [DisplayName("Full Name")]
        public string FullName
        {
            get
            {
                string tempName =
                    (!string.IsNullOrEmpty(FirstName) ? FirstName + " " : "") +
                    (!string.IsNullOrEmpty(MiddleName) ? MiddleName + " " : "") +
                    (!string.IsNullOrEmpty(LastName) ? LastName + " " : "");

                return tempName.Trim();
            }
        }
        [StringLength(50)]
        public string Title { get; set; }
        [StringLength(100)]
        public string EmailAddress { get; set; }
        [StringLength(500)]
        public string ImageURL { get; set; }
        [Required]
        [System.ComponentModel.DefaultValue(1)]
        public byte Status { get; set; }
        [System.ComponentModel.DefaultValue(0)]
        public byte IsDeleted { get; set; }
        [Required]
        public int CreatedBy { get; set; }
        [Required]
        public DateTime CreatedOn { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Guid { get; set; }
        [System.ComponentModel.DefaultValue(0)]
        public byte IsLocked { get; set; }
        public virtual ContactDetail ContactDetails { get; set; }
        public virtual CompanyUser CompanyUsers { get; set; }
    }
}
