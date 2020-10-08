using System;
using System.Collections.Generic;
using System.Text;

namespace xgca.core.Models.YourEDI
{
    public class YourEdiResponse
    {
        public YourEditResponseDetails Actors { get; set; }
    }

    public class YourEditResponseDetails
    {
        public dynamic Shipper { get; set; }
        public dynamic Consignee { get; set; }
        public dynamic BookingParty { get; set; }
        public List<dynamic> NotifyParties { get; set; }
    }
}
