using xgca.core.Models.CompanyBeneficialOwner;
using System.Collections.Generic;

namespace xgca.core.Models.CompanySection
{
    public class GetCompanyBeneficialOwnerSectionModel
    {
        public string Id { get; set; }
        public string SectionStatusCode { get; set; }
        public string SectionStatusName { get; set; }
        public string SectionCode { get; set; }
        public string SectionName { get; set; }
        public bool IsDraft { get; set; }
        public List<GetCompanyBeneficialOwnerModel> Company { get; set; }
        public List<GetIndividualBeneficialOwnerModel> Individual { get; set; }
    }
}
