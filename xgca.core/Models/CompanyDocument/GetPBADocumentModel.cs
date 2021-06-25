using System;
using System.Collections.Generic;
using System.Text;

namespace xgca.core.Models.CompanyDocument
{
    public class GetPBADocumentModel
    {
        public string Id { get; set; }
        public string FileUrl { get; set; }
        public bool IsUpdated { get; set; } = false;
    }
}
