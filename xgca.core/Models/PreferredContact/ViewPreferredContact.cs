using System;
using System.Collections.Generic;
using System.Text;

namespace xgca.core.Models.PreferredContact
{
    public class ViewPreferredContact
    {
        public string PreferredContactId { get; set; }
        public string ContactId { get; set; }
        public string ContactName { get; set; }
        public int ContactType { get; set; }
        public string ImageURL { get; set; }
        public string CityProvince { get; set; }
        public string Country { get; set; }
    }
}
