using System;
using System.Collections.Generic;
using System.Text;
using xgca.core.Models.CompanyStructure;
using xgca.core.Models.CompanyDocument;

namespace xgca.core.Models.CompanySection
{
    public class CompanyStructureSectionModel
    {
        public GetCompanyStructureModel Details { get; set; }
        public List<GetCompanyDocumentModel> BusinessRegistrationCertificate { get; set; }
        public GetPBADocumentModel ProofOfBusinessAddress { get; set; }
        public GetOCDocumentModel OrganizationalChart { get; set; }
    }
}
