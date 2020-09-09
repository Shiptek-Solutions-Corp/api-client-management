using System;
using System.Collections.Generic;
using System.Text;

namespace xgca.core.Models.Email
{
    public class EmailPayload
    {
        public string to { get; set; }
        public string subject { get; set; }
        public dynamic message { get; set; }
    }
}
