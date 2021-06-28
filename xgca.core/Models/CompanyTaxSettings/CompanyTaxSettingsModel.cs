using System;
using System.Collections.Generic;
using System.Text;

namespace xgca.core.Models.CompanyTaxSettings
{
    public class CreateCompanyTaxSettingsModel
    {
        public string CompanyId { get; set; }
        public string TaxTypeId { get; set; }
        public string TaxTypeDescription { get; set; }
        public decimal TaxPercentageRate { get; set; }
        public bool IsTaxExcempted { get; set; }
    }

    public class UpdateCompanyTaxSettingsModel : CreateCompanyTaxSettingsModel
    {
        public int Status { get; set; }
        public string StatusName { get; set; }
    }

    public class GetCompanyTaxSettingsModel : UpdateCompanyTaxSettingsModel
    {
    }
}
