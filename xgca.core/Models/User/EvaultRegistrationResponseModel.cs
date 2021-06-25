using System;
using System.Collections.Generic;
using System.Text;

namespace xgca.core.Models.User
{
    public class EvaultRegistrationResponseModel
    {
        public string walletCode { get; set; }
        public string companyCode { get; set; }
        public string masterMerchantName { get; set; }
        public string merchantId { get; set; }
        public Guid companyGuid { get; set; }
        public string currencyCode { get; set; }
        public string countryCode { get; set; }
        public string countryName { get; set; }
        public string currencyName { get; set; }
    }
}
