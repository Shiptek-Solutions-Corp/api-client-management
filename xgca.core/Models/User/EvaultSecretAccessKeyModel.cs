using System;
using System.Collections.Generic;
using System.Text;

namespace xgca.core.Models.User
{
    public class EvaultSecretAccessKeyModel
    {
        public string walletProvider { get; set; }        
        public string masterMerchantName { get; set; }
        public string masterMerchantCode { get; set; }
        public string accessKey { get; set; }
        public string secretKey { get; set; }

    }
}
