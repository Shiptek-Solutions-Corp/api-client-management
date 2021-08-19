using System;
using System.Collections.Generic;
using System.Text;

namespace xgca.data.ViewModels.PortArea
{
    public class PortAreaResponseModel
    {
        public int PortAreaId { get; set; }
        public Guid PortId { get; set; }
        public int CountryAreaId { get; set; }
        public string CountryCode { get; set; }
        public string LoCode { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string PortOfLoading { get; set; }
        public string PortOfDischarge { get; set; }
        public string CountryName { get; set; }
        public string CityName { get; set; }
        public bool IsDeleted { get; set; }
    }
}
