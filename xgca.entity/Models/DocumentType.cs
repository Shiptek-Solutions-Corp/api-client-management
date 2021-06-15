using System;
using System.Collections.Generic;

namespace xgca.entity.Models
{
    public partial class DocumentType
    {
        public DocumentType()
        {
            CompanyDocuments = new HashSet<CompanyDocuments>();
        }

        public string DocumentCategoryCode { get; set; }
        public string DocumentTypeCode { get; set; }
        public Guid Guid { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool? IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool IsDeleted { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? DeletedOn { get; set; }

        public virtual DocumentCategory DocumentCategoryCodeNavigation { get; set; }
        public virtual ICollection<CompanyDocuments> CompanyDocuments { get; set; }
    }
}
