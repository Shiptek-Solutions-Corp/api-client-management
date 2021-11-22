using System;
using System.Collections.Generic;
using System.Text;

namespace xgca.core.Models.KYCLog
{
    public class CreateKYCLogModel
    {
        public int CompanyId { get; set; }
        public int CompanySectionsId { get; set; }
        public string Remarks { get; set; }
        public string SectionStatusCode { get; set; }
    }
}
