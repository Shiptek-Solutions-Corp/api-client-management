using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace xas.core.PortArea.DTO
{
    public class PortAreaDTO
    {
        [JsonProperty("portAreas")]
        public List<PortArea> PortAreas { get; set; }
        [JsonProperty("companyId")]
        public Guid CompanyId { get; set; }
        [JsonProperty("requestId")]
        public Guid RequestId { get; set; }
    }

    public class PortArea
    {
        [JsonProperty("portId")]
        public Guid PortId { get; set; }
        [JsonProperty("countryId")]
        public int CountryAreaId { get; set; }
        [JsonProperty("portOfLoading")]
        public int PortOfLoading { get; set; }
        [JsonProperty("portOfDischarge")]   
        public int PortOfDischarge { get; set; }
    }
}
