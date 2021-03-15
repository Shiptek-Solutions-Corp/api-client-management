using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using xgca.core.Models.CompanyServiceRole;
using xgca.core.Response;
using xgca.core.Helpers;
using xgca.core.Models.CompanyService;
using AutoMapper;
using xgca.data.Company;
using xgca.core.Services;
using xgca.entity.Models;
using xgca.core.CompanyServiceUser;
using Castle.Core.Internal;
using xgca.core.AuditLog;
using xgca.data.AuditLog;
using xgca.data.User;
using ClosedXML.Excel;
using System.IO;
using System.Data;

namespace xgca.core.CompanyServiceRole
{
    public interface ICompanyServiceRole
    {
        Task<IGeneralModel> Create(CreateCompanyServiceRoleModel obj);
        Task<IGeneralModel> CreateDefault(int companyId, int userId);
        Task<IGeneralModel> ListByCompanyServiceId(string key);
        Task<byte[]> DownloadByCompanyServiceId(string key);
        Task<IGeneralModel> ListByCompany(string key, int status);
        Task<IGeneralModel> Show(Guid companyServiceRoleId);
        Task<IGeneralModel> Update(UpdateCompanyServiceRoleModel updateCompanyServiceRoleModel, Guid companyServiceRoleId);
        Task<IGeneralModel> CreateGroupPermissionUser(CreateGroupPermissionUserModel createGroupPermissionUser);
        Task<IGeneralModel> BatchUpdate(BatchUpdateCompanyServiceRoleModel batchUpdateCompanyServiceRoleModel);

    }

    public class CompanyServiceRole : ICompanyServiceRole
    {
        private readonly data.CompanyServiceRole.ICompanyServiceRole _companyServiceRole;
        private readonly data.CompanyService.ICompanyService _companyService;
        private readonly ICompanyData _companyData;
        private readonly IGeneral _general;
        private readonly IMapper _mapper;
        private readonly IGLobalCmsService gLobalCmsService;
        private readonly data.CompanyServiceUser.ICompanyServiceUser companyServiceUser;
        private readonly data.CompanyGroupResource.ICompanyGroupResourceData companyGroupResourceData;
        private readonly IUserData userData;
        private readonly IAuditLogCore auditLogCore;
        public CompanyServiceRole(xgca.data.CompanyServiceRole.ICompanyServiceRole companyServiceRole,
            xgca.data.CompanyService.ICompanyService companyService, IGeneral general, 
            IMapper mapper, 
            ICompanyData companyData,
            IGLobalCmsService gLobalCmsService,
            data.CompanyServiceUser.ICompanyServiceUser companyServiceUser,
            data.CompanyGroupResource.ICompanyGroupResourceData companyGroupResourceData,
            IAuditLogHelper auditLogHelper,
            IAuditLogData auditLogData,
            IUserData userData,
            IAuditLogCore auditLogCore)
        {
            _companyServiceRole = companyServiceRole;
            _companyService = companyService;
            _general = general;
            _mapper = mapper;
            _companyData = companyData;
            this.gLobalCmsService = gLobalCmsService;
            this.companyServiceUser = companyServiceUser;
            this.companyGroupResourceData = companyGroupResourceData;
            this.userData = userData;
            this.auditLogCore = auditLogCore;
        }

        public async Task<IGeneralModel> CreateDefault(int companyId, int userId)
        {

            var companyServices = await _companyService.ListByCompanyId(companyId);
            List<entity.Models.CompanyServiceRole> companyServiceRoles = new List<entity.Models.CompanyServiceRole>();
            foreach (var companyService in companyServices)
            {
                companyServiceRoles.Add(new entity.Models.CompanyServiceRole
                {
                    CompanyServiceId = companyService.CompanyServiceId,
                    Name = "Administrator",
                    Description = "Service administrator",
                    CreatedBy = userId,
                    CreatedOn = DateTime.UtcNow,
                    ModifiedBy = userId,
                    ModifiedOn = DateTime.UtcNow,
                    Guid = Guid.NewGuid()
                });
            }

            var result = await _companyServiceRole.Create(companyServiceRoles);
            return result
                ? _general.Response(true, 200, "Default company service roles have been created created", true)
                : _general.Response(false, 400, "Error on creating default company service roles", false);
        }

        public async Task<IGeneralModel> Create(CreateCompanyServiceRoleModel obj)
        {
            var request = _mapper.Map<entity.Models.CompanyServiceRole>(obj);
            var result = await _companyServiceRole.Create(request);

            return result
                ? _general.Response(true, 200, "Company Service Role Created", true)
                : _general.Response(false, 400, "Error on creating company service role", false);
        }

        public async Task<IGeneralModel> ListByCompanyServiceId(string key)
        {
            int companyServiceId = await _companyService.GetIdByGuid(Guid.Parse(key));
            var result = await _companyServiceRole.ListByCompanyServiceId(companyServiceId);
            var data = result.Select(t => new { CompanyServiceRoleId = t.Guid, CompanyServiceId = t.CompanyServices.Guid, t.Name, t.Description });
            return _general.Response(new { companyServiceRole = data }, 200, "Configurable company service roles has been listed", true);
        }

        public async Task<IGeneralModel> ListByCompany(string key, int status = -1)
        {
            int companyId = await _companyData.GetIdByGuid(Guid.Parse(key));
            var result = await _companyServiceRole.ListByCompanyId(companyId, status);
            var services = await gLobalCmsService.GetAllService();
            var viewCompanyServiceRole = result.CompanyServiceRoles.Select(c => _mapper.Map<GetCompanyServiceRoleModel>(c)).ToList();
            foreach (var companyServiceRole in viewCompanyServiceRole)
            {
                companyServiceRole.CompanyServices.ServiceName = services.Where(c => c.IntServiceId == companyServiceRole.CompanyServices.ServiceId).FirstOrDefault().ServiceName;
            }

            return _general.Response(new { viewCompanyServiceRole, result.TotalGroups, result.TotalActive, result.TotalInactive }, 200, "success", true);
        }

        public async Task<IGeneralModel> Show(Guid companyServiceRoleId)
        {
            var result = await _companyServiceRole.Retrieve(companyServiceRoleId);
            if (result == null)
            {
                return _general.Response(null, 400, "Invalid Company Service Role", false);
            }
            var services = await gLobalCmsService.GetAllService();
            var viewCompanyServiceRole = _mapper.Map<GetCompanyServiceRoleModel>(result);
            var service = services.Where(c => c.IntServiceId == viewCompanyServiceRole.CompanyServices.ServiceId).FirstOrDefault();
            viewCompanyServiceRole.CompanyServices.ServiceName = service.ServiceName;
            viewCompanyServiceRole.CompanyServices.Guid = service.ServiceId;

            return _general.Response(viewCompanyServiceRole, 200, "Success", true);
        }

        public async Task<IGeneralModel> Update(UpdateCompanyServiceRoleModel updateCompanyServiceRoleModel, Guid companyServiceRoleId)
        {
            var result = await _companyServiceRole.Get(companyServiceRoleId);
            var oldData = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(result));
            if (result == null)
            {
                return _general.Response(null, 400, "Invalid Company Service Role", false);
            }
            _mapper.Map(updateCompanyServiceRoleModel, result);
            result.CompanyServiceId = await _companyService.GetIdByGuid(Guid.Parse(updateCompanyServiceRoleModel.CompanyServiceGuid));
            bool updateResult = await _companyServiceRole.Update(result);
             
            var services = await gLobalCmsService.GetAllService();
            var viewCompanyServiceRole = _mapper.Map<GetCompanyServiceRoleModel>(result);
            
            viewCompanyServiceRole.CompanyServices.ServiceName = services.Where(c => c.IntServiceId == viewCompanyServiceRole.CompanyServices.ServiceId).FirstOrDefault().ServiceName;

            await companyGroupResourceData.BulkDeleteByCompanyServiceRole(result.CompanyServiceRoleId);
            
            if (!updateCompanyServiceRoleModel.Permissions.IsNullOrEmpty())
            {
                bool bulkResult = await companyGroupResourceData.BlukCreate(MapPermissions(updateCompanyServiceRoleModel.Permissions, result));
            }

            await companyServiceUser.BulkDeleteByCompanyServiceRole(result.CompanyServiceRoleId);

            if (!updateCompanyServiceRoleModel.CompanyServiceUsersArray.IsNullOrEmpty())
            {
                List<entity.Models.CompanyServiceUser> companyServiceUsers = new List<entity.Models.CompanyServiceUser>();

                foreach (CreateNewUserPerGroupModuleModel obj in updateCompanyServiceRoleModel.CompanyServiceUsersArray)
                {
                    obj.CompanyServiceId = (int) result.CompanyServiceId;
                    obj.CompanyServiceRoleId = result.CompanyServiceRoleId;
                    var companyServiceUser = _mapper.Map<entity.Models.CompanyServiceUser>(obj);

                    // TODO: Change createdBy by logged in user
                    companyServiceUser.IsActive = 1;
                    companyServiceUser.CreatedBy = 1;
                    companyServiceUser.CreatedOn = DateTime.UtcNow;
                    companyServiceUser.ModifiedBy = 1;
                    companyServiceUser.ModifiedOn = DateTime.UtcNow;
                    companyServiceUser.Guid = Guid.NewGuid();
                    companyServiceUsers.Add(companyServiceUser);
                }

                var companyServiceUserResult = await companyServiceUser.BulkCreate(companyServiceUsers);
            }

            await auditLogCore.CreateAuditLog(
                "Update",
                _companyServiceRole.GetType().Name,
                result.CompanyServiceRoleId,
                await userData.GetIdByUsername(Constant.loggedInUserName),
                _mapper.Map<GetCompanyServiceRoleModel>(oldData),
                _mapper.Map<GetCompanyServiceRoleModel>(result));

            return updateResult ?
                _general.Response(viewCompanyServiceRole, 200, "Updated successfuly", true)
                :
                _general.Response(null, 400, "An error has occured", false);
        }

        public async Task<IGeneralModel> CreateGroupPermissionUser(CreateGroupPermissionUserModel createGroupPermissionUser)
        {
            var companyServiceId = await _companyService.GetIdByGuid(Guid.Parse(createGroupPermissionUser.CompanyServiceGuid));
            if (companyServiceId > 0 == false)
            {
                return _general.Response(null, 400, "Invalid company service guid", false);
            }
            var isGroupNameUnique = await _companyServiceRole.CheckGroupNameIfExists(companyServiceId, createGroupPermissionUser.Name);
            if (isGroupNameUnique)
            {
                return _general.Response(null, 400, "Group name already exists. Please enter other group name", false);
            }
            createGroupPermissionUser.CompanyServiceId = companyServiceId;
            var companyServiceRole = _mapper.Map<entity.Models.CompanyServiceRole>(createGroupPermissionUser);
            var companyServiceRoleResult = await _companyServiceRole.Create(companyServiceRole);

            if (!createGroupPermissionUser.Permissions.IsNullOrEmpty())
            {
                bool result = await companyGroupResourceData.BlukCreate(MapPermissions(createGroupPermissionUser.Permissions, companyServiceRole));
            }

            if (!createGroupPermissionUser.CompanyServiceUsersArray.IsNullOrEmpty())
            {

                List<entity.Models.CompanyServiceUser> companyServiceUsers = new List<entity.Models.CompanyServiceUser>();

                foreach (CreateNewUserPerGroupModuleModel obj in createGroupPermissionUser.CompanyServiceUsersArray)
                {
                    obj.CompanyServiceId = companyServiceId;
                    obj.CompanyServiceRoleId = companyServiceRole.CompanyServiceRoleId;
                    var companyServiceUser = _mapper.Map<entity.Models.CompanyServiceUser>(obj);

                    // TODO: Change createdBy by logged in user
                    companyServiceUser.IsActive = 1;
                    companyServiceUser.CreatedBy = await userData.GetIdByUsername(Constant.loggedInUserName);
                    companyServiceUser.CreatedOn = DateTime.UtcNow;
                    companyServiceUser.ModifiedBy = 1;
                    companyServiceUser.ModifiedOn = DateTime.UtcNow;
                    companyServiceUser.Guid = Guid.NewGuid();
                    companyServiceUsers.Add(companyServiceUser);
                }

                var companyServiceUserResult = await companyServiceUser.BulkCreate(companyServiceUsers);
            }
            await auditLogCore.CreateAuditLog(
                "Create", 
                companyServiceRole.GetType().Name, 
                companyServiceRole.CompanyServiceRoleId, 
                await userData.GetIdByUsername(Constant.loggedInUserName),
                _mapper.Map<GetCompanyServiceRoleModel>(companyServiceRole), 
                null);
            
            return _general.Response(null, 200, "Created successfuly", true);
        }

        private List<entity.Models.CompanyGroupResource> MapPermissions(
            ICollection<ClientMenuModel> clientMenuModels, 
            entity.Models.CompanyServiceRole companyServiceRole)
        {
            List<entity.Models.CompanyGroupResource> companyGroupResources = new List<entity.Models.CompanyGroupResource>();

            ICollection<GroupResourceModel> moduleGroupsMenu = clientMenuModels
            .SelectMany(p => p.ClientMenuModules)
            .Select(m => m.Module)
            .SelectMany(mg => mg.ModuleGroups)
            .Where(mg => mg.IsChecked == true)
            .SelectMany(gr => gr.GroupResources)
            .ToList();

            foreach (GroupResourceModel groupResource in moduleGroupsMenu)
            {
                companyGroupResources.Add(new entity.Models.CompanyGroupResource
                {
                    CompanyServiceRoleId = companyServiceRole.CompanyServiceRoleId,
                    GroupResourceId = groupResource.GroupResourceId,
                    Guid = Guid.NewGuid(),
                    IsAllowed = 1
                });
            }

            ICollection<GroupResourceModel> moduleGroupsSubMenu = clientMenuModels
                .SelectMany(cs => cs.ClientSubMenus)
                .SelectMany(p => p.ClientMenuModules)
                .Select(m => m.Module)
                .SelectMany(mg => mg.ModuleGroups)
                .Where(mg => mg?.IsChecked == true)
                .SelectMany(gr => gr.GroupResources)
                .ToList();

            foreach (GroupResourceModel groupResource in moduleGroupsSubMenu)
            {
                companyGroupResources.Add(new entity.Models.CompanyGroupResource
                {
                    CompanyServiceRoleId = companyServiceRole.CompanyServiceRoleId,
                    GroupResourceId = groupResource.GroupResourceId,
                    Guid = Guid.NewGuid(),
                    IsAllowed = 1
                });
            }

            return companyGroupResources;
        }

        public async Task<IGeneralModel> BatchUpdate(BatchUpdateCompanyServiceRoleModel batchUpdateCompanyServiceRoleModel)
        {
            ICollection<Guid> guids = batchUpdateCompanyServiceRoleModel.Guids.Select(x => Guid.Parse(x)).ToList();
            var oldValues = await _companyServiceRole.GetAllByGuid(guids);
            bool result = await _companyServiceRole.BulkUpdate(guids, batchUpdateCompanyServiceRoleModel.Type);
            var newValues = await _companyServiceRole.GetAllByGuid(guids);

            //TODO: Remove foreach. Change to bulk insert
            int i = -1;
            foreach (var item in oldValues)
            {
                i++;
                await auditLogCore.CreateAuditLog(
                    batchUpdateCompanyServiceRoleModel.Type == "delete" ? "Delete" : "Update",
                    _companyServiceRole.GetType().Name,
                    item.CompanyServiceRoleId,
                    await userData.GetIdByUsername(Constant.loggedInUserName),
                    _mapper.Map<GetCompanyServiceRoleModel>(item),
                    newValues.Count > 0 ? _mapper.Map<GetCompanyServiceRoleModel>(newValues[i]) : null);
            }

            return result 
                ? _general.Response(null, 200, "Updated Successfuly", true) 
                : _general.Response(null, 400, "An error occured", true);
        }

        public async Task<byte[]> DownloadByCompanyServiceId(string key)
        {
            int companyId = await _companyData.GetIdByGuid(Guid.Parse(key));
            var result = await _companyServiceRole.ListByCompanyId(companyId, -1);
            var services = await gLobalCmsService.GetAllService();
            var viewCompanyServiceRole = result.CompanyServiceRoles.Select(c => _mapper.Map<GetCompanyServiceRoleModel>(c)).ToList();
            foreach (var companyServiceRole in viewCompanyServiceRole)
            {
                companyServiceRole.CompanyServices.ServiceName = services.Where(c => c.IntServiceId == companyServiceRole.CompanyServices.ServiceId).FirstOrDefault().ServiceName;
            }

            var table = new DataTable { TableName = "ServiceRates" };
            table.Columns.Add("Service", typeof(string));
            table.Columns.Add("Group Name", typeof(string));
            table.Columns.Add("Description", typeof(string));
            table.Columns.Add("Status", typeof(string));

            for (int i = 0; i < viewCompanyServiceRole.Count; i++)
            {
                table.Rows.Add(
                    viewCompanyServiceRole[i]?.CompanyServices?.ServiceName,
                    viewCompanyServiceRole[i]?.Name,
                    viewCompanyServiceRole[i]?.Description,
                    viewCompanyServiceRole[i]?.IsActive == 1 ? "Active" : "Inactive"
                );
            }

            var wb = new XLWorkbook();
            wb.Worksheets.Add(table);
            await using var memoryStream = new MemoryStream();
            wb.SaveAs(memoryStream);
            return memoryStream.ToArray();
        }
    }
}
