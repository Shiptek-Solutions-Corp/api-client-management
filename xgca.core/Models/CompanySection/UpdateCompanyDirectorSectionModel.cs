using System;
using System.Collections.Generic;
using System.Text;
using xgca.core.Models.CompanyDirector;

namespace xgca.core.Models.CompanySection
{
    public class UpdateCompanyDirectorSectionModel
    {
        public string Id { get; set; }
        public List<GetCompanyDirectorModel> CompanyDirectors { get; set; }
        public string Remarks { get; set; }
    }
}
