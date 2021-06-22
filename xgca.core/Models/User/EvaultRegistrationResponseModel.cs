using System;
using System.Collections.Generic;
using System.Text;

namespace xgca.core.Models.User
{
    public class EvaultRegistrationResponseModel
    {
        public string WalletCode { get; set; }
        public string CompanyCode { get; set; }
        public string MasterMerchantName { get; set; }
        public string MerchantId { get; set; }
    }
}
