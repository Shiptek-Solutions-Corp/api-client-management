using System;
using System.Collections.Generic;
using System.Text;

namespace xgca.core.Models.Company
{
    public class GetReservationActorsListModel
    {
        public List<GetReservationActorsModel> Actors { get; set; }
    }

    public class GetReservationActorsModel
    {
        public string BookingPartyId { get; set; }
        public string ShipperId { get; set; }
        public string ConsigneeId { get; set; }
        public string ShippingLineId { get; set; }
        public string NotifyPartyIds { get; set; }
    }
}
