using System;
using System.Collections.Generic;
using System.Text;

namespace xas.core.Request
{
    public class RequestModel
    {
        public Guid CompanyIdFrom { get; set; }
        public Guid ServiceRoleIdFrom { get; set; }
        public Guid CompanyIdTo { get; set; }
        public Guid ServiceRoleIdTo { get; set; }
        public int Status { get; set; }

        public Guid Guid { get; set; }
    }
}
