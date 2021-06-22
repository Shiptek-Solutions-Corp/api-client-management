using System;
using System.Collections.Generic;
using System.Text;

namespace xgca.core.Constants
{
    public class EvaultEndPoints
    {
        public string evaultRegister { get; set; }
        public string evaultPartnerToken { get; set; }
        public string evaultSecretAccessKeys { get; set; }
        public string evaultUpdateTransaction { get; set; }
        public string evaultCountryInfo { get; set; }
        public string evaultPartnerAuthentication { get; set; }
        public EvaultSetting EvaultSetting { get; set; }
    }

    public class EvaultSetting
    {
        public string WalletProviderName { get; set; }
        public EvaultAuthentication Authentication { get; set; }
        public string MasterMerchant { get; set; }
    }

    public class EvaultAuthentication
    {
        public string Property1 { get; set; }
        public string Property2 { get; set; }
    }
}
