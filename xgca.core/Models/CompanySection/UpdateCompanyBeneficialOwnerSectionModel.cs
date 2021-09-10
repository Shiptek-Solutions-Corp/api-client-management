using System;
using System.Collections.Generic;
using System.Text;
using xgca.core.Models.CompanyBeneficialOwner;

namespace xgca.core.Models.CompanySection
{
    public class UpdateCompanyBeneficialOwnerSectionModel
    {
        public string Id { get; set; }
        public List<GetCompanyBeneficialOwnerModel> Companies { get; set; }
        public List<GetIndividualBeneficialOwnerModel> Individuals { get; set; }
        public string Remarks { get; set; }
    }
}
