using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace xgca.core.Models.CompanyServiceRole
{
    public class CreateGroupPermissionUserModel : CreateCompanyServiceRoleModel
    {
        [Required]
        public string CompanyServiceGuid { get; set; }

        public ICollection<CreateNewUserPerGroupModuleModel> CompanyServiceUsersArray { get; set; }
    }

    public class CreateNewUserPerGroupModuleModel
    {
        public int CompanyServiceId { get; set; }

        [Required]
        public int CompanyUserId { get; set; }
        [Required]
        public int CompanyId { get; set; }
        public int CompanyServiceRoleId { get; set; }
    }
}
