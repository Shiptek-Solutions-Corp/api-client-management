using System;
using System.Collections.Generic;
using System.Text;

namespace xgca.core.Models.CompanyDocument
{
    public class CreateCompanyDocumentModel
    {
        public string DocumentTypeGuid { get; set; }
        public string DocumentNo { get; set; }
        public string DocumentDescription { get; set; }
        public string FileUrl { get; set; }
    }
}
