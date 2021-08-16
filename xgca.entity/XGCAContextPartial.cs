using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xgca.entity.Models;
using static xgca.entity.Models._Model;

namespace xgca.entity
{
    public interface IXGCAContext
    {
        Task<int> SaveChangesAsync(string userName = null, bool IsNewStructure = false);
        Task<int> SaveChangesAuditable();

        DbSet<User> Users { get; set; }
        DbSet<Address> Addresses { get; set; }
        DbSet<AddressType> AddressTypes { get; set; }
        DbSet<AuditLog> AuditLogs { get; set; }
        DbSet<Company> Companies { get; set; }
        DbSet<ContactDetail> ContactDetails { get; set; }
        DbSet<CompanyService> CompanyServices { get; set; }
        DbSet<CompanyServiceRole> CompanyServiceRoles { get; set; }
        DbSet<CompanyServiceUser> CompanyServiceUsers { get; set; }
        DbSet<CompanyUser> CompanyUsers { get; set; }
        DbSet<CompanyGroupResource> CompanyGroupResources { get; set; }
        DbSet<CompanyServiceUserRole> CompanyServiceUserRoles { get; set; }
        DbSet<Guest> Guests { get; set; }
        DbSet<PreferredContact> PreferredContacts { get; set; }
        DbSet<PreferredProvider> PreferredProviders { get; set; }
        DbSet<Invite> Invites { get; set; }
        DbSet<BeneficialOwnersType> BeneficialOwnersTypes { get; set; }
        DbSet<CompanyBeneficialOwners> CompanyBeneficialOwners { get; set; }
        DbSet<CompanyDirectors> CompanyDirectors { get; set; }
        DbSet<CompanyDocuments> CompanyDocuments { get; set; }
        DbSet<CompanySections> CompanySections { get; set; }
        DbSet<CompanyStructure> CompanyStructures { get; set; }
        DbSet<DocumentCategory> DocumentCategories { get; set; }
        DbSet<DocumentType> DocumentTypes { get; set; }
        DbSet<KycStatus> KycStatuses { get; set; }
        DbSet<Section> Sections { get; set; }
        DbSet<SectionStatus> SectionStatuses { get; set; }
        DbSet<CompanyTaxSettings> CompanyTaxSettings { get; set; }
        DbSet<KYCLog> KYCLogs { get; set; }

        //Accreditation
        DbSet<AccreditationStatusConfig> AccreditationStatusConfig { get; set; }
        DbSet<PortArea> PortArea { get; set; }
        DbSet<Request> Request { get; set; }
        DbSet<ServiceRoleConfig> ServiceRoleConfig { get; set; }
        DbSet<TruckArea> TruckArea { get; set; }

    }
    public partial class XGCAContext: IXGCAContext
    {
        public Task<int> SaveChangesAsync(string userName = null, bool IsNewStructure = false)
        {
            if (!IsNewStructure) return base.SaveChangesAsync();

            UpdateAuditEntities(null, userName);
            return base.SaveChangesAsync();
        }

        public Task<int> SaveChangesAuditable()
        {
            UpdateAuditEntities();
            return base.SaveChangesAsync();
        }

        private void UpdateAuditEntities()
        {
            // https://medium.com/@unhandlederror/deleting-it-softly-with-ef-core-5f191db5cf72
            var modifiedEntries = ChangeTracker.Entries()
                .Where(x => (x.Entity is ISoftDeletableEntity || x.Entity is IAuditableEntity) && (x.State == EntityState.Added || x.State == EntityState.Modified || x.State == EntityState.Deleted));


            foreach (var entry in modifiedEntries)
            {
                var entity = (IAuditableEntity)entry.Entity;
                var now = DateTime.UtcNow;

                if (entry.State != EntityState.Deleted)
                {
                    if (entry.State == EntityState.Added)
                    {
                        entity.CreatedOn = now;
                        entity.CreatedBy = "calicubilly";
                    }
                    else
                    {
                        base.Entry(entity).Property(x => x.CreatedBy).IsModified = false;
                        base.Entry(entity).Property(x => x.CreatedOn).IsModified = false;
                    }

                    entity.UpdatedOn = now;
                    entity.UpdatedBy = "calicubilly";
                }
                else
                {
                    // if deleted state, change to Modified
                    entry.State = EntityState.Modified;
                    var entityDel = (ISoftDeletableEntity)entry.Entity;
                    entityDel.IsDeleted = 1;
                    entityDel.DeletedOn = now;
                    entityDel.DeletedBy = "calicubilly";
                }
            }
        }

        private void UpdateAuditEntities(dynamic type = null, string userName = null)
        {
            var username = contextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "cognito:username")?.Value;
            username = (username == null ? userName : username);

            // https://medium.com/@unhandlederror/deleting-it-softly-with-ef-core-5f191db5cf72
            var modifiedEntries = ChangeTracker.Entries()
                .Where(x => (x.State == EntityState.Added || x.State == EntityState.Modified || x.State == EntityState.Deleted));


            foreach (var entry in modifiedEntries)
            {
                dynamic entity = entry.Entity;

                var now = DateTime.UtcNow;
                if (entry.State != EntityState.Deleted)
                {
                    if (entry.State == EntityState.Added)
                    {
                        entity.CreatedOn = now;
                        entity.CreatedBy = username;
                    }
                    else
                    {
                        entity.UpdatedOn = now;
                        entity.UpdatedBy = username;
                    }
                }
                else
                {
                    if (type == null)
                    {
                        // if deleted state, change to Modified
                        entry.State = EntityState.Modified;
                        dynamic entityDel = entry.Entity;
                        entityDel.IsDeleted = true;
                        entityDel.DeletedOn = now;
                        entityDel.DeletedBy = username;
                    }
                }
            }
        }
    }
}
