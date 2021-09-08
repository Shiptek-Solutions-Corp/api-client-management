using System;
using System.Collections.Generic;
using System.Text;

namespace xgca.data.ViewModels.Company
{
    public class GetCompanyExistModel
    {
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyContactNo { get; set; }
        public string CompanyLogo { get; set; }
        public Guid CompanyGuid { get; set; }

    }
}
