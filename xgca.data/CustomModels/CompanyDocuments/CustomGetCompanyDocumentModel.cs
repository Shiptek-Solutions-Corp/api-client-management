using System;
using System.Collections.Generic;
using System.Text;

namespace xgca.data.CustomModels.CompanyDocuments
{
    public class CustomGetCompanyDocumentModel
    {
        public int CompanyId { get; set; }
        public int DocumentTypeId { get; set; }
        public int CompanyDocumentsId { get; set; }
        public Guid Guid { get; set; }
        public string DocumentNo { get; set; }
        public string DocumentDescription { get; set; }
        public string FileUrl { get; set; }
        public bool? IsActive { get; set; }
        public string DocumentTypeGuid { get; set; }
    }
}
