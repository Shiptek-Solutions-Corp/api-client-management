using System;
using System.Collections.Generic;
using System.Text;
using xgca.core.Models.CompanyBeneficialOwner;

namespace xgca.core.Models.CompanySection
{
    public class CompanyBeneficialOwnerSectionModel
    {
        public List<GetCompanyBeneficialOwnerModel> Companies { get; set; }
        public List<GetIndividualBeneficialOwnerModel> Individual { get; set; }
    }
}
