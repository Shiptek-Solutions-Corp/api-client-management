using System;
using System.Collections.Generic;
using System.Text;
using xgca.core.Models.AuditLog;
using xgca.core.Models.Company;
using xgca.core.Models.CompanyGroupResource;
using xgca.core.Models.CompanyService;
using xgca.core.Models.CompanyServiceRole;
using xgca.core.Models.CompanyServiceUser;
using xgca.core.Models.CompanyServiceUserRole;
using xgca.core.Models.CompanyUser;
using xgca.core.Models.GroupResource;
using xgca.core.Models.Guest;
using xgca.core.Models.MenuModule;
using xgca.core.Models.ModuleGroup;
using xgca.core.Models.PreferredProvider;
using xgca.core.Models.User;
using xgca.core.Models.CompanyStructure;
using xgca.core.Models.CompanyBeneficialOwner;
using xgca.core.Models.CompanyDirector;
using xgca.core.Models.CompanyDocument;
using xgca.core.Models.CompanySection;
using xgca.core.Models.DocumentType;
using xgca.entity.Models;
using xgca.core.Constants;
using xgca.core.Enums;
using System.Linq;
using xgca.core.Models.Address;
using xgca.core.Models.AddressType;
using xgca.core.Models.ContactDetail;
using xgca.core.Models.CompanyTaxSettings;

namespace xgca.core._Mapper
{
    public class XCGAProfile : AutoMapper.Profile
    {
        public XCGAProfile()
        {
            CreateMap<CreateAuditLog, entity.Models.AuditLog>();

            CreateMap<CreateCompanyGroupResource, entity.Models.CompanyGroupResource>();
            CreateMap<entity.Models.CompanyGroupResource, GetCompanyGroupResource>();


            CreateMap<CreateCompanyServiceUserRole, entity.Models.CompanyServiceUserRole>();
            CreateMap<entity.Models.CompanyServiceUserRole, GetCompanyServiceUserRole>();

            CreateMap<entity.Models.CompanyServiceUser, GetCompanyServiceUser>();
            CreateMap<CreateNewUserPerGroupModuleModel, entity.Models.CompanyServiceUser>();


            CreateMap<entity.Models.CompanyServiceRole, GetCompanyServiceRoleModel>();
            CreateMap<CreateGroupPermissionUserModel, entity.Models.CompanyServiceRole>();

            CreateMap<CreateCompanyServiceRoleModel, entity.Models.CompanyServiceRole>();
            CreateMap<UpdateCompanyServiceRoleModel, entity.Models.CompanyServiceRole>();

            CreateMap<entity.Models.CompanyService, GetCompanyService>();

            CreateMap<entity.Models.Company, GetCompanyModel>();

            CreateMap<entity.Models.CompanyUser, GetCompanyUserModel>();

            CreateMap<entity.Models.User, GetUserModel>();


            CreateMap<CreateGuest, entity.Models.Guest>();
            CreateMap<UpdateGuestContact, entity.Models.Guest>();

            CreateMap<CreatePreferredProvider, entity.Models.PreferredProvider>();

            #region Company V2 Profiles
            CreateMap<entity.Models.Company, GetCompanyListingViewModel>()
                .ForMember(c => c.ServiceName, 
                    s => s.MapFrom(c => string.Join(",", c.CompanyServices.Select(s => s.ServiceName).ToArray())))
                .ForMember(c => c.CountryName,
                    s => s.MapFrom(c => c.Addresses.CountryName))
                .ForMember(c => c.Status,
                    s => s.MapFrom(c => c.StatusName));

            CreateMap<entity.Models.Company, GetCompanyViewModel>();
            CreateMap<entity.Models.CompanyService, GetCompanyServiceModel>();

            CreateMap<entity.Models.ContactDetail, GetContactDetailsModel>();
            CreateMap<entity.Models.Address, GetAddressModel>();
            CreateMap<entity.Models.AddressType, GetAddressTypeModel>();

            CreateMap<entity.Models.CompanyTaxSettings, GetCompanyTaxSettingsModel>()
                .ForMember(c => c.CompanyGuid, 
                    s => s.Ignore());
            CreateMap<CreateCompanyTaxSettingsModel, entity.Models.CompanyTaxSettings> ();
            CreateMap<UpdateCompanyTaxSettingsModel, entity.Models.CompanyTaxSettings> ();
            #endregion

            #region KYC Mapper Profiles
            CreateMap<CreateCompanyStructureModel, entity.Models.CompanyStructure>()
                .ForMember(i => i.CompanyId,
                    d => d.MapFrom(m => GlobalVariables.LoggedInCompanyId))
                .ForMember(i => i.Guid,
                    d => d.MapFrom(m => Guid.NewGuid()))
                .ForMember(i => i.CreatedBy,
                    d => d.MapFrom(m => GlobalVariables.LoggedInUsername))
                .ForMember(i => i.CreatedOn,
                    d => d.MapFrom(m => DateTime.UtcNow))
                .ForMember(i => i.UpdatedBy,
                    d => d.MapFrom(m => GlobalVariables.LoggedInUsername))
                .ForMember(i => i.UpdatedOn,
                    d => d.MapFrom(m => DateTime.UtcNow))
                .ForMember(i => i.IsActive,
                    d => d.MapFrom(m => true))
                .ForMember(i => i.IsDeleted,
                    d => d.MapFrom(m => false));

            CreateMap<UpdateCompanyStructureModel, entity.Models.CompanyStructure>()
                .ForMember(i => i.Guid,
                    d => d.MapFrom(m => Guid.Parse(m.Id)))
                .ForMember(i => i.UpdatedBy,
                    d => d.MapFrom(m => GlobalVariables.LoggedInUsername))
                .ForMember(i => i.UpdatedOn,
                    d => d.MapFrom(m => DateTime.UtcNow));

            CreateMap<entity.Models.CompanyStructure, GetCompanyStructureModel>()
                .ForMember(i => i.Id,
                    d => d.MapFrom(m => m.Guid.ToString()));

            CreateMap<GetCompanyStructureModel, CreateCompanyStructureModel>();
            CreateMap<GetCompanyStructureModel, UpdateCompanyStructureModel>();


            CreateMap<CreateCompanyBeneficialOwnerModel, entity.Models.CompanyBeneficialOwners>()
                .ForMember(i => i.CompanyId,
                    d => d.MapFrom(m => GlobalVariables.LoggedInCompanyId))
                .ForMember(i => i.BeneficialOwnersTypeCode,
                    d => d.MapFrom(m => Enum.GetName(typeof(Enums.BeneficialOwnerType), Enums.BeneficialOwnerType.C)))
                .ForMember(i => i.Name,
                    d => d.MapFrom(m => m.CompanyName))
                .ForMember(i => i.DateOfBirth,
                    d => d.MapFrom(m => m.DateEstablished))
                .ForMember(i => i.Guid,
                    d => d.MapFrom(m => Guid.NewGuid()))
                .ForMember(i => i.CreatedBy,
                    d => d.MapFrom(m => GlobalVariables.LoggedInUsername))
                .ForMember(i => i.CreatedOn,
                    d => d.MapFrom(m => DateTime.UtcNow))
                .ForMember(i => i.UpdatedBy,
                    d => d.MapFrom(m => GlobalVariables.LoggedInUsername))
                .ForMember(i => i.UpdatedOn,
                    d => d.MapFrom(m => DateTime.UtcNow))
                .ForMember(i => i.IsActive,
                    d => d.MapFrom(m => true))
                .ForMember(i => i.IsDeleted,
                    d => d.MapFrom(m => false));

            CreateMap<CreateIndividualBeneficialOwnerModel, entity.Models.CompanyBeneficialOwners>()
                .ForMember(i => i.CompanyId,
                    d => d.MapFrom(m => GlobalVariables.LoggedInCompanyId))
                .ForMember(i => i.BeneficialOwnersTypeCode,
                    d => d.MapFrom(m => Enum.GetName(typeof(Enums.BeneficialOwnerType), Enums.BeneficialOwnerType.I)))
                .ForMember(i => i.Name,
                    d => d.MapFrom(m => m.FullName))
                .ForMember(i => i.CompanyAddress,
                    d => d.MapFrom(m => m.PersonalAddress))
                .ForMember(i => i.Guid,
                    d => d.MapFrom(m => Guid.NewGuid()))
                .ForMember(i => i.CreatedBy,
                    d => d.MapFrom(m => GlobalVariables.LoggedInUsername))
                .ForMember(i => i.CreatedOn,
                    d => d.MapFrom(m => DateTime.UtcNow))
                .ForMember(i => i.UpdatedBy,
                    d => d.MapFrom(m => GlobalVariables.LoggedInUsername))
                .ForMember(i => i.UpdatedOn,
                    d => d.MapFrom(m => DateTime.UtcNow))
                .ForMember(i => i.IsActive,
                    d => d.MapFrom(m => true))
                .ForMember(i => i.IsDeleted,
                    d => d.MapFrom(m => false));

            CreateMap<UpdateCompanyBeneficialOwnerModel, entity.Models.CompanyBeneficialOwners>()
                .ForMember(i => i.Name,
                    d => d.MapFrom(m => m.CompanyName))
                .ForMember(i => i.BeneficialOwnersTypeCode,
                    d => d.MapFrom(m => Enum.GetName(typeof(Enums.BeneficialOwnerType), Enums.BeneficialOwnerType.C)))
                .ForMember(i => i.DateOfBirth,
                    d => d.MapFrom(m => m.DateEstablished))
                .ForMember(i => i.Guid,
                    d => d.MapFrom(m => Guid.Parse(m.Id)))
                .ForMember(i => i.UpdatedBy,
                    d => d.MapFrom(m => GlobalVariables.LoggedInUsername))
                .ForMember(i => i.UpdatedOn,
                    d => d.MapFrom(m => DateTime.UtcNow));

            CreateMap<UpdateIndividualBeneficialOwnerModel, entity.Models.CompanyBeneficialOwners>()
                .ForMember(i => i.Name,
                    d => d.MapFrom(m => m.FullName))
                .ForMember(i => i.BeneficialOwnersTypeCode,
                    d => d.MapFrom(m => Enum.GetName(typeof(Enums.BeneficialOwnerType), Enums.BeneficialOwnerType.I)))
                .ForMember(i => i.CompanyAddress,
                    d => d.MapFrom(m => m.PersonalAddress))
                .ForMember(i => i.Guid,
                    d => d.MapFrom(m => Guid.Parse(m.Id)))
                .ForMember(i => i.UpdatedBy,
                    d => d.MapFrom(m => GlobalVariables.LoggedInUsername))
                .ForMember(i => i.UpdatedOn,
                    d => d.MapFrom(m => DateTime.UtcNow));

            CreateMap<entity.Models.CompanyBeneficialOwners, GetCompanyBeneficialOwnerModel>()
                .ForMember(i => i.CompanyName,
                    d => d.MapFrom(m => m.Name))
                .ForMember(i => i.DateEstablished,
                    d => d.MapFrom(m => m.DateOfBirth))
                .ForMember(i => i.Id,
                    d => d.MapFrom(m => m.Guid.ToString()));

            CreateMap<entity.Models.CompanyBeneficialOwners, GetIndividualBeneficialOwnerModel>()
                .ForMember(i => i.FullName,
                    d => d.MapFrom(m => m.Name))
                .ForMember(i => i.PersonalAddress,
                    d => d.MapFrom(m => m.CompanyAddress))
                .ForMember(i => i.Id,
                    d => d.MapFrom(m => m.Guid.ToString()));

            CreateMap<GetCompanyBeneficialOwnerModel, CreateCompanyBeneficialOwnerModel>();
            CreateMap<GetIndividualBeneficialOwnerModel, CreateIndividualBeneficialOwnerModel>();

            CreateMap<GetCompanyBeneficialOwnerModel, UpdateCompanyBeneficialOwnerModel>();
            CreateMap<GetIndividualBeneficialOwnerModel, UpdateIndividualBeneficialOwnerModel>();


            CreateMap<CreateCompanyDirectorModel, entity.Models.CompanyDirectors>()
                .ForMember(i => i.CompanyId,
                    d => d.MapFrom(m => GlobalVariables.LoggedInCompanyId))
                .ForMember(i => i.Name,
                    d => d.MapFrom(m => m.FullName))
                .ForMember(i => i.CompanyAddress,
                    d => d.MapFrom(m => m.PersonalAddress))
                .ForMember(i => i.Guid,
                    d => d.MapFrom(m => Guid.NewGuid()))
                .ForMember(i => i.CreatedBy,
                    d => d.MapFrom(m => GlobalVariables.LoggedInUsername))
                .ForMember(i => i.CreatedOn,
                    d => d.MapFrom(m => DateTime.UtcNow))
                .ForMember(i => i.UpdatedBy,
                    d => d.MapFrom(m => GlobalVariables.LoggedInUsername))
                .ForMember(i => i.UpdatedOn,
                    d => d.MapFrom(m => DateTime.UtcNow))
                .ForMember(i => i.IsActive,
                    d => d.MapFrom(m => true))
                .ForMember(i => i.IsDeleted,
                    d => d.MapFrom(m => false));

            CreateMap<UpdateCompanyDirectorModel, entity.Models.CompanyDirectors>()
                .ForMember(i => i.Name,
                    d => d.MapFrom(m => m.FullName))
                .ForMember(i => i.CompanyAddress,
                    d => d.MapFrom(m => m.PersonalAddress))
                .ForMember(i => i.Guid,
                    d => d.MapFrom(m => Guid.Parse(m.Id)))
                .ForMember(i => i.UpdatedBy,
                    d => d.MapFrom(m => GlobalVariables.LoggedInUsername))
                .ForMember(i => i.UpdatedOn,
                    d => d.MapFrom(m => DateTime.UtcNow));

            CreateMap<entity.Models.CompanyDirectors, GetCompanyDirectorModel>()
                .ForMember(i => i.FullName,
                    d => d.MapFrom(m => m.Name))
                .ForMember(i => i.PersonalAddress,
                    d => d.MapFrom(m => m.CompanyAddress))
                .ForMember(i => i.Id,
                    d => d.MapFrom(m => m.Guid.ToString()));

            CreateMap<GetCompanyDirectorModel, CreateCompanyDirectorModel>();
            CreateMap<GetCompanyDirectorModel, UpdateCompanyDirectorModel>();


            CreateMap<CreateCompanyDocumentModel, entity.Models.CompanyDocuments>()
                .ForMember(i => i.CompanyId,
                    d => d.MapFrom(m => GlobalVariables.LoggedInCompanyId))
                .ForMember(i => i.Guid,
                    d => d.MapFrom(m => Guid.NewGuid()))
                .ForMember(i => i.CreatedBy,
                    d => d.MapFrom(m => GlobalVariables.LoggedInUsername))
                .ForMember(i => i.CreatedOn,
                    d => d.MapFrom(m => DateTime.UtcNow))
                .ForMember(i => i.UpdatedBy,
                    d => d.MapFrom(m => GlobalVariables.LoggedInUsername))
                .ForMember(i => i.UpdatedOn,
                    d => d.MapFrom(m => DateTime.UtcNow))
                .ForMember(i => i.IsActive,
                    d => d.MapFrom(m => true))
                .ForMember(i => i.IsDeleted,
                    d => d.MapFrom(m => false));

            CreateMap<CreatePBADocumentModel, entity.Models.CompanyDocuments>()
                .ForMember(i => i.CompanyId,
                    d => d.MapFrom(m => GlobalVariables.LoggedInCompanyId))
                .ForMember(i => i.DocumentDescription,
                    d => d.MapFrom(m => "Proof of Business Address"))
                .ForMember(i => i.Guid,
                    d => d.MapFrom(m => Guid.NewGuid()))
                .ForMember(i => i.CreatedBy,
                    d => d.MapFrom(m => GlobalVariables.LoggedInUsername))
                .ForMember(i => i.CreatedOn,
                    d => d.MapFrom(m => DateTime.UtcNow))
                .ForMember(i => i.UpdatedBy,
                    d => d.MapFrom(m => GlobalVariables.LoggedInUsername))
                .ForMember(i => i.UpdatedOn,
                    d => d.MapFrom(m => DateTime.UtcNow))
                .ForMember(i => i.IsActive,
                    d => d.MapFrom(m => true))
                .ForMember(i => i.IsDeleted,
                    d => d.MapFrom(m => false));

            CreateMap<CreateOCDocumentModel, entity.Models.CompanyDocuments>()
                .ForMember(i => i.CompanyId,
                    d => d.MapFrom(m => GlobalVariables.LoggedInCompanyId))
                .ForMember(i => i.DocumentDescription,
                    d => d.MapFrom(m => "Organizational Chart"))
                .ForMember(i => i.Guid,
                    d => d.MapFrom(m => Guid.NewGuid()))
                .ForMember(i => i.CreatedBy,
                    d => d.MapFrom(m => GlobalVariables.LoggedInUsername))
                .ForMember(i => i.CreatedOn,
                    d => d.MapFrom(m => DateTime.UtcNow))
                .ForMember(i => i.UpdatedBy,
                    d => d.MapFrom(m => GlobalVariables.LoggedInUsername))
                .ForMember(i => i.UpdatedOn,
                    d => d.MapFrom(m => DateTime.UtcNow))
                .ForMember(i => i.IsActive,
                    d => d.MapFrom(m => true))
                .ForMember(i => i.IsDeleted,
                    d => d.MapFrom(m => false));

            CreateMap<UpdateCompanyDocumentModel, entity.Models.CompanyDocuments>()
                .ForMember(i => i.CompanyId,
                    d => d.MapFrom(m => GlobalVariables.LoggedInCompanyId))
                .ForMember(i => i.Guid,
                    d => d.MapFrom(m => Guid.Parse(m.Id)))
                .ForMember(i => i.UpdatedBy,
                    d => d.MapFrom(m => GlobalVariables.LoggedInUsername))
                .ForMember(i => i.UpdatedOn,
                    d => d.MapFrom(m => DateTime.UtcNow));

            CreateMap<UpdatePBADocumentModel, entity.Models.CompanyDocuments>()
                .ForMember(i => i.CompanyId,
                    d => d.MapFrom(m => GlobalVariables.LoggedInCompanyId))
                .ForMember(i => i.DocumentDescription,
                    d => d.MapFrom(m => "Proof of Business Address"))
                .ForMember(i => i.Guid,
                    d => d.MapFrom(m => Guid.Parse(m.Id)))
                .ForMember(i => i.UpdatedBy,
                    d => d.MapFrom(m => GlobalVariables.LoggedInUsername))
                .ForMember(i => i.UpdatedOn,
                    d => d.MapFrom(m => DateTime.UtcNow));

            CreateMap<UpdateOCDocumentModel, entity.Models.CompanyDocuments>()
                .ForMember(i => i.DocumentDescription,
                    d => d.MapFrom(m => "Organizational Chart"))
                .ForMember(i => i.Guid,
                    d => d.MapFrom(m => Guid.Parse(m.Id)))
                .ForMember(i => i.UpdatedBy,
                    d => d.MapFrom(m => GlobalVariables.LoggedInUsername))
                .ForMember(i => i.UpdatedOn,
                    d => d.MapFrom(m => DateTime.UtcNow));

            CreateMap<entity.Models.CompanyDocuments, GetCompanyDocumentModel>()
                .ForMember(i => i.Id,
                    d => d.MapFrom(m => m.Guid.ToString()));

            CreateMap<entity.Models.CompanyDocuments, GetPBADocumentModel>()
                .ForMember(i => i.Id,
                    d => d.MapFrom(m => m.Guid.ToString()));

            CreateMap<entity.Models.CompanyDocuments, GetOCDocumentModel>()
                .ForMember(i => i.Id,
                    d => d.MapFrom(m => m.Guid.ToString()));

            CreateMap<GetPBADocumentModel, CreatePBADocumentModel>();
            CreateMap<GetPBADocumentModel, UpdatePBADocumentModel>();

            CreateMap<GetOCDocumentModel, CreateOCDocumentModel>();
            CreateMap<GetOCDocumentModel, UpdateOCDocumentModel>();
            
            CreateMap<GetCompanyDocumentModel, CreateCompanyDocumentModel>();
            CreateMap<GetCompanyDocumentModel, UpdateCompanyDocumentModel>();

            CreateMap<CreateCompanySectionModel, entity.Models.CompanySections>()
                .ForMember(i => i.CompanyId,
                    d => d.MapFrom(m => GlobalVariables.LoggedInCompanyId))
                .ForMember(i => i.SectionStatusCode,
                    d => d.MapFrom(m => Enum.GetName(typeof(Enums.SectionStatus), Enums.SectionStatus.NW)))
                .ForMember(i => i.Guid,
                    d => d.MapFrom(m => Guid.NewGuid()))
                .ForMember(i => i.CreatedBy,
                    d => d.MapFrom(m => GlobalVariables.LoggedInUsername))
                .ForMember(i => i.CreatedOn,
                    d => d.MapFrom(m => DateTime.UtcNow))
                .ForMember(i => i.UpdatedBy,
                    d => d.MapFrom(m => GlobalVariables.LoggedInUsername))
                .ForMember(i => i.UpdatedOn,
                    d => d.MapFrom(m => DateTime.UtcNow))
                .ForMember(i => i.IsDraft,
                    d => d.MapFrom(m => false))
                .ForMember(i => i.IsActive,
                    d => d.MapFrom(m => true))
                .ForMember(i => i.IsDeleted,
                    d => d.MapFrom(m => false));

            CreateMap<UpdateCompanySectionModel, entity.Models.CompanySections>()
                .ForMember(i => i.CompanyId,
                    d => d.MapFrom(m => GlobalVariables.LoggedInCompanyId))
                .ForMember(i => i.SectionStatusCode,
                    d => d.MapFrom(m => Enum.GetName(typeof(Enums.SectionStatus), Enums.SectionStatus.NW)))
                .ForMember(i => i.Guid,
                    d => d.MapFrom(m => Guid.Parse(m.Id)))
                .ForMember(i => i.UpdatedBy,
                    d => d.MapFrom(m => GlobalVariables.LoggedInUsername))
                .ForMember(i => i.UpdatedOn,
                    d => d.MapFrom(m => DateTime.UtcNow));

            CreateMap<entity.Models.CompanySections, GetCompanySectionModel>();

            CreateMap<entity.Models.CompanySections, GetCompanyStructureSectionModel>()
                .ForMember(i => i.Id,
                    d => d.MapFrom(m => m.Guid.ToString()));
            CreateMap<entity.Models.CompanySections, GetCompanyBeneficialOwnerSectionModel>()
                .ForMember(i => i.Id,
                    d => d.MapFrom(m => m.Guid.ToString()));
            CreateMap<entity.Models.CompanySections, GetCompanyDirectorSectionModel>()
                .ForMember(i => i.Id,
                    d => d.MapFrom(m => m.Guid.ToString()));

            CreateMap<entity.Models.DocumentType, GetDocumentTypeModel>()
                .ForMember(i => i.Id,
                    d => d.MapFrom(m => m.Guid.ToString()));
            #endregion
        }
    }
}
