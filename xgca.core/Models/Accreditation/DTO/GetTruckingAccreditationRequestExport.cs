using System;
using System.Collections.Generic;
using System.Text;

namespace xas.core.Request.DTO
{
    public class GetTruckingAccreditationRequestExport
    {
        public int No { get; set; }
        public string Company { get; set; }
        public string Address { get; set; }
        public string AreaOfResponsibility { get; set; }
        public string Status { get; set; }
    }
}
