using System;
using System.Collections.Generic;
using System.Text;

namespace xgca.core.Models.User
{
    public class EvaultCredentialModel
    {
        public string walletProvider { get; set; }
        public string[] authenticationProperties { get; set; }
    }
}
