using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace xgca.entity.Models
{
    [Table("Guest")]
    public class Guest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GuestId { get; set; }
        [System.ComponentModel.DefaultValue(0)]
        public int GuestType { get; set; }
        public int CompanyId { get; set; }
        public string GuestName { get; set; }
        public string Image { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumberPrefixId { get; set; }
        public string PhoneNumberPrefix { get; set; }
        public string PhoneNumber { get; set; }
        public string MobileNumberPrefixId { get; set; }
        public string MobileNumberPrefix { get; set; }
        public string MobileNumber { get; set; }
        public string FaxNumberPrefixId { get; set; }
        public string FaxNumberPrefix { get; set; }
        public string FaxNumber { get; set; }
        public string AddressLine { get; set; }
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public string StateId { get; set; }
        public string StateName { get; set; }
        public string CityId { get; set; }
        public string CityName { get; set; }
        public string ZipCode { get; set; }
        public bool IsGuest { get; set; }
        public string CUCC { get; set; }

        public Guid Id { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        [System.ComponentModel.DefaultValue(0)]
        public byte IsDeleted { get; set; }
        public int DeletedBy { get; set; }
        public DateTime? DeletedOn { get; set; }
    }
}
