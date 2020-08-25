using System;
using System.Collections.Generic;
using System.Text;

namespace xgca.core.Models.PreferredContact
{
    public class CreatePreferredContact
    {
        public string GuestId { get; set; }
        public string CompanyId { get; set; }
        public string ProfileId { get; set; }
        public int ContactType { get; set; } // 1 - Registered, 2 - Guest
    }
}
