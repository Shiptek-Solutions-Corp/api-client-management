using xgca.core.Models.CompanyStructure;
using xgca.core.Models.CompanyDocument;
using System.Collections.Generic;

namespace xgca.core.Models.CompanySection
{
    public class GetCompanyStructureSectionModel
    {
        public string Id { get; set; }
        public string SectionStatusCode { get; set; }
        public string SectionStatusName { get; set; }
        public string SectionCode { get; set; }
        public string SectionName { get; set; }
        public bool IsDraft { get; set; }
        public GetCompanyStructureModel Details { get; set; }
        public List<GetCompanyDocumentModel> BusinessRegistrationCertificates { get; set; }
        public GetPBADocumentModel ProofOfBusinessAddress { get; set; }
        public GetOCDocumentModel OrganizationalChart { get; set; }
    }
}
