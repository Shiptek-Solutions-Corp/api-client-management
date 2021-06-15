using System;
using System.Collections.Generic;

namespace xgca.entity.Models
{
    public partial class CompanyDocuments
    {
        public int CompanyId { get; set; }
        public string DocumentTypeCode { get; set; }
        public int CompanyDocumentsId { get; set; }
        public Guid Guid { get; set; }
        public string DocumentNo { get; set; }
        public string DocumentDescription { get; set; }
        public string FileUrl { get; set; }
        public bool? IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool IsDeleted { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? DeletedOn { get; set; }

        public virtual Company Company { get; set; }
        public virtual DocumentType DocumentTypeCodeNavigation { get; set; }
    }
}
