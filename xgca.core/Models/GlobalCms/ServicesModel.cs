using System;
using System.Collections.Generic;
using System.Text;

namespace xgca.core.Models.GlobalCms
{
    public class ServicesModel
    {
        public int IntServiceId { get; set; }
        public Guid ServiceId { get; set; }
        public string ServiceName { get; set; }
        public string Description { get; set; }
        public string ImageURL { get; set; }
        public string ServiceCode { get; set; }
        public Guid Guid { get; set; }
    }
}
