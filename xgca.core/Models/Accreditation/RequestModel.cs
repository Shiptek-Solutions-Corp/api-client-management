using System;
using System.Collections.Generic;
using System.Text;

namespace xas.core.Request
{
    public class RequestModel
    {     
        public Guid CompanyIdTo { get; set; }
        public Guid ServiceRoleIdTo { get; set; }
        public string CompanyCodeTo { get; set; }
        public string CompanyNameTo { get; set; }

        public Guid CompanyIdFrom { get; set; }
        public Guid ServiceRoleIdFrom { get; set; }
        public string CompanyCodeFrom { get; set; }
        public string CompanyNameFrom { get; set; }

    }
}
