using System;
using System.Collections.Generic;
using System.Text;

namespace xgca.core.Models.ContactDetail
{
    public class GetContactDetailsModel
    {
        public string Guid { get; set; }
        public string PhonePrefix { get; set; }
        public string Phone { get; set; }
        public string MobilePrefix { get; set; }
        public string Mobile { get; set; }
        public string FaxPrefix { get; set; }
        public string Fax { get; set; }
    }
}
