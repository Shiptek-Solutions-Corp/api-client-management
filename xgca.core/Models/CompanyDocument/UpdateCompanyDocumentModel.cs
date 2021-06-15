using System;
using System.Collections.Generic;
using System.Text;

namespace xgca.core.Models.CompanyDocument
{
    public class UpdateCompanyDocumentModel
    {
        public string Id { get; set; }
        public string DocumentTypeCode { get; set; }
        public string DocumentNo { get; set; }
        public string DocumentDescription { get; set; }
        public string FileUrl { get; set; }
        public bool? IsActive { get; set; }
    }
}
