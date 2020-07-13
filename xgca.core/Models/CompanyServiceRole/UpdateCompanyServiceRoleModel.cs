namespace xgca.core.Models.CompanyServiceRole
{
    public class UpdateCompanyServiceRoleModel
    {
        public int CompanyServiceRoleId { get; set; }
        public string CompanyServiceGuid { get; set; }
        public string Name { get; set; }
        public byte IsActive { get; set; }
        public string Description { get; set; }
    }
}
