using System;
using System.Collections.Generic;
using System.Text;

namespace xgca.core.Models.KYCLog
{
    public class GetKYCLogModel
    {
        public string Id { get; set; }
        public int CompanyId { get; set; }
        public int CompanySectionsId { get; set; }
        public string Remarks { get; set; }
        public string SectionStatusCode { get; set; }
    }
}
