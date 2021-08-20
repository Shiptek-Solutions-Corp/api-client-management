using System;
using System.Collections.Generic;

namespace xgca.entity.Models
{
    public partial class PortArea
    {
        public int RequestId { get; set; }
        public int PortAreaId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsDeleted { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? DeletedOn { get; set; }
        public Guid Guid { get; set; }
        public int CountryAreaId { get; set; }
        public Guid PortId { get; set; }
        public string PortName { get; set; }
        public int PortOfLoading { get; set; }
        public int PortOfDischarge { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public Guid? StateCode { get; set; }
        public string StateName { get; set; }
        public Guid? CityCode { get; set; }
        public string CityName { get; set; }
        public string Locode { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string Location { get; set; }

        public virtual Request Request { get; set; }
    }
}
