using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace xgca.core.Models.CompanyServiceRole
{
    public class UpdateCompanyServiceRoleModel
    {
        [Required]
        public string CompanyServiceGuid { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public byte IsActive { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public ICollection<CreateNewUserPerGroupModuleModel> CompanyServiceUsersArray { get; set; }
    }
}
