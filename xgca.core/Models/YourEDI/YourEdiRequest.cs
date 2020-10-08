using System;
using System.Collections.Generic;
using System.Text;

namespace xgca.core.Models.YourEDI
{
    public class YourEdiRequest
    {
        public YourEdiRequestActors Actors { get; set; }
    }

    public class YourEdiRequestActors
    {
        public string ShipperId { get; set; }
        public string ConsigneeId { get; set; }
        public string BookingPartyId { get; set; }
        public List<string> NotifyPartyIds { get; set; }
    }
}
