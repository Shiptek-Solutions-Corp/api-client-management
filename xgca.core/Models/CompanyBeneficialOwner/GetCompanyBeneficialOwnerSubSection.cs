using System;
using System.Collections.Generic;
using System.Text;

namespace xgca.core.Models.CompanyBeneficialOwner
{
    public class GetCompanyBeneficialOwnerSubSection
    {
        public List<GetCompanyBeneficialOwnerModel> Companies { get; set; }
        public List<GetIndividualBeneficialOwnerModel> Individuals { get; set; }
    }
}
