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
        public string xlogOnBoarding { get; set; }
    }

    public class EvaultSetting
    {
        public string walletProvider { get; set; }
        public string[] authenticationProperties { get; set; }
        public string masterMerchantName { get; set; }
    }
}
