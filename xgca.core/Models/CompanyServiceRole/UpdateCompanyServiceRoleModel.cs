namespace xgca.core.Models.CompanyServiceRole
{
    public class UpdateCompanyServiceRoleModel
    {
        public int CompanyServiceRoleId { get; set; }
        public int CompanyServiceId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
