using System;
using System.Collections.Generic;
using System.Text;

namespace xas.core.Request.DTO
{
    public class CountryDTO
    {
        public int countryId { get; set; }
        public string name { get; set; }
        public string callingCode { get; set; }
    }
}
