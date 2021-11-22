using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace xas.core.CustomerAccreditation.DTO
{
    public class CustomerAccreditationReadDTO
    {
        public string CompanyId { get; set; }
        public string AccreditedBy { get; set; }
    }
}
