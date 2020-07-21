using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace xgca.core.Models.CompanyServiceRole
{
    // -- BASE CLASS (GROUP, PERMISSION, USERS)
    public class CreateGroupPermissionUserModel : CreateCompanyServiceRoleModel
    {
        [Required]
        public string CompanyServiceGuid { get; set; }
        public ICollection<ClientMenuModel> Permissions { get; set; }
        public ICollection<CreateNewUserPerGroupModuleModel> CompanyServiceUsersArray { get; set; }
    }
    // -- USER
    public class CreateNewUserPerGroupModuleModel
    {
        public int CompanyServiceId { get; set; }

        [Required]
        public int CompanyUserId { get; set; }
        [Required]
        public int CompanyId { get; set; }
        public int CompanyServiceRoleId { get; set; }
    }
    // -- PERMISSIONS
    public class ClientMenuModel
    {
        public string Display { get; set; }
        public string Description { get; set; }
        public string Guid { get; set; }
        public string Index { get; set; }
        public ICollection<ClientSubMenuModel> ClientSubMenus { get; set; }
        public ICollection<ClientMenuModuleModel> ClientMenuModules { get; set; }
    }
    public class ClientSubMenuModel
    {
        public string Display { get; set; }
        public string Description { get; set; }
        public string Guid { get; set; }
        public string Index { get; set; }
        public ICollection<ClientMenuModuleModel> ClientMenuModules { get; set; }
    }
    public class ClientMenuModuleModel
    {
        public string Guid { get; set; }
        public int ClientMenuId { get; set; }
        public int ClientSubMenuId { get; set; }
        public int ModuleId { get; set; }
        public ModuleModel Module { get; set; }
    }
    public class ModuleModel
    {
        public int ModuleId { get; set; }
        public string ModuleName { get; set; }
        public string Description { get; set; }
        public int ResourceTypeId { get; set; }
        public ICollection<ModuleGroupModel> ModuleGroups { get; set; }
    }
    public class ModuleGroupModel
    {
        public int ModuleGroupId { get; set; }
        public int ModuleId { get; set; }
        public int ResourceGroupId { get; set; }
        public bool IsChecked { get; set; }
        public ResourceGroupModel ResourceGroups { get; set; }
        public ICollection<GroupResourceModel> GroupResources { get; set; }
    }
    public class ResourceGroupModel
    {
        public int ResourceGroupId { get; set; }
        public string ResourceGroupName { get; set; }
        public string Description { get; set; }
        public string Guid { get; set; }
    }
    public class GroupResourceModel
    {
        public int GroupResourceId { get; set; }
        public int ModuleGroupId { get; set; }
        public int ResourceId { get; set; }
        public string Guid { get; set; }
        public ResourceModel Resources { get; set; }
    }
    public class ResourceModel
    {
        public int ResourceId { get; set; }
        public string ResourceName { get; set; }
        public string Path { get; set; }
        public string method { get; set; }
        public int AllowAnonymous { get; set; }
        public string Description { get; set; }
        public string Guid { get; set; }
    }
}
