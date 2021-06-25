namespace xgca.core.Models.CompanySection
{
    public class GetCompanySectionModel
    {
        public GetCompanyStructureSectionModel CompanyStructure { get; set; }
        public GetCompanyBeneficialOwnerSectionModel UltimateBeneficialOwners { get; set; }
        public GetCompanyDirectorSectionModel CompanyDirectors { get; set; }
    }
}
