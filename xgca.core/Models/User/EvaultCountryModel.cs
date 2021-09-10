using System;
using System.Collections.Generic;
using System.Text;

namespace xgca.core.Models.User
{
    public class EvaultCountryModel
    {
        public int CountryId { get; set; }
        public string Name { get; set; }
        public string ISOCode2 { get; set; }
        public string ISOCode3 { get; set; }
        public string CallingCode { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencyLeftSymbol { get; set; }
        public string CurrencyRightSymbol { get; set; }
        public string Description { get; set; }
        public byte IsActive { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
