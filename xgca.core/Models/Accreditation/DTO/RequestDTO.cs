using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace xas.core.Request
{
    public class RequestDTO
    {
        [JsonProperty(PropertyName = "loggedOnServiceRoleId")]
        public Guid ServiceRoleIdFrom { get; set; }

        [JsonProperty(PropertyName = "selectedCompanyId")]
        public Guid CompanyIdTo { get; set; }
        [JsonProperty(PropertyName = "selectedServiceRoleId")]
        public Guid ServiceRoleIdTo { get; set; }
    }
}
