using System;
using System.Collections.Generic;
using System.Text;


namespace xgca.core.Models.CompanyDocument
{
    public class GetCompanyDocumentSubSectionModel
    {
        public List<GetCompanyDocumentModel> BusinessRegistrationCertificates { get; set; }
        public GetPBADocumentModel ProofOfBusinessAddress { get; set; }
        public GetOCDocumentModel OrganizationalChart { get; set; }
    }
}
