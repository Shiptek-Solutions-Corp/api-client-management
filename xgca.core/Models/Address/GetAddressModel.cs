using System;
using System.Collections.Generic;
using System.Text;
using xgca.core.Models.AddressType;

namespace xgca.core.Models.Address
{
    public class GetAddressModel : UpdateAddressModel
    {
        public GetAddressTypeModel AddressTypes { get; set; }
    }
}
