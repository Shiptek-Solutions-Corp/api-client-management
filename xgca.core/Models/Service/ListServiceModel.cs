using System;
using System.Collections.Generic;
using System.Text;

namespace xgca.core.Models.Service
{
    public class ListServiceModel
    {
        public int IntServiceId { get; set; }
        public string ServiceId { get; set; }
        public string ServiceCode { get; set; }
        public string ServiceName { get; set; }
        public string ServiceImageURL { get; set; }
        public int ServiceStaticId { get; set; }
    }
}
