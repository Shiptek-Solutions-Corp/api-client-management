using System;
using System.Collections.Generic;
using System.Text;

namespace xgca.core.Models.YourEDI
{
    public class YourEDIActor
    {
        public Guid Guid { get; set; }
        public string CUCC { get; set; }
        public dynamic MasterUser { get; set; }
        public string CompanyName { get; set; }
        public string Email { get; set; }
        public string ImageUrl { get; set; }
        public dynamic ContactDetails { get; set; }
        public dynamic Address { get; set; }

    }
}
