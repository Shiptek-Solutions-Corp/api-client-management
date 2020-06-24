using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace xgca.entity.Models
{
    [Table("Address", Schema = "General")]
    public class Address
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AddressId { get; set; }
        [Required]
        public int AddressTypeId { get; set; }
        [StringLength(255)]
        public string AddressLine { get; set; }
        public int CityId { get; set; }
        [StringLength(100)]
        public string CityName { get; set; }
        public int StateId { get; set; }
        [StringLength(100)]
        public string StateName { get; set; }
        [StringLength(10)]
        public string ZipCode { get; set; }
        [Required]
        public int CountryId { get; set; }
        [Required]
        [StringLength(100)]
        public string CountryName { get; set; }
        [StringLength(250)]
        public string FullAddress { get; set; }
        [System.ComponentModel.DefaultValue(0)]
        public byte IsDeleted { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Guid { get; set; }
        [StringLength(10)]
        public string Longitude { get; set; }
        [StringLength(10)]
        public string Latitude { get; set; }

        public virtual AddressType AddressTypes { get; set; }
    }
}
