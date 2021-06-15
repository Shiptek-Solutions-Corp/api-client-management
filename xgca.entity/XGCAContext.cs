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
        Task<int> SaveChangesAsync();
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

    }
    public static class ModelBuilderExtensions
    {
        public static void SeedDefaultValues(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AddressType>().HasData(
                new AddressType
                {
                    AddressTypeId = 1,
                    Name = "Company",
                    CreatedBy = 0,
                    CreatedOn = DateTime.UtcNow,
                    ModifiedBy = 0,
                    ModifiedOn = DateTime.UtcNow,
                    Guid = Guid.Parse("1E0621B2-B7EA-4D48-8BE2-F09980694816")
                });

            modelBuilder.Entity<AddressType>().HasData(
                new AddressType
                {
                    AddressTypeId = 2,
                    Name = "Residential",
                    CreatedBy = 0,
                    CreatedOn = DateTime.UtcNow,
                    ModifiedBy = 0,
                    ModifiedOn = DateTime.UtcNow,
                    Guid = Guid.Parse("95EC682B-074F-42BB-9FED-D4DBDE41E41D")
                });
        }
    }
    public class XGCAContext : DbContext, IXGCAContext
    {
        public XGCAContext(DbContextOptions options)
            : base(options)
        {
        }
        public DbSet<User> Users { get; set; }

        public DbSet<Address> Addresses { get; set; }
        public DbSet<AddressType> AddressTypes { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<ContactDetail> ContactDetails { get; set; }

        public DbSet<CompanyService> CompanyServices { get; set; }
        public DbSet<CompanyServiceRole> CompanyServiceRoles { get; set; }
        public DbSet<CompanyServiceUser> CompanyServiceUsers { get; set; }
        public DbSet<CompanyUser> CompanyUsers { get; set; }
        public DbSet<CompanyGroupResource> CompanyGroupResources { get; set; }
        public DbSet<CompanyServiceUserRole> CompanyServiceUserRoles { get; set; }

        public DbSet<Guest> Guests { get; set; }
        public DbSet<PreferredContact> PreferredContacts { get; set; }
        public DbSet<PreferredProvider> PreferredProviders { get; set; }
        public DbSet<Invite> Invites { get; set; }


        public virtual DbSet<BeneficialOwnersType> BeneficialOwnersTypes { get; set; }
        public virtual DbSet<CompanyBeneficialOwners> CompanyBeneficialOwners { get; set; }
        public virtual DbSet<CompanyDirectors> CompanyDirectors { get; set; }
        public virtual DbSet<CompanyDocuments> CompanyDocuments { get; set; }
        public virtual DbSet<CompanySections> CompanySections { get; set; }
        public virtual DbSet<CompanyStructure> CompanyStructures { get; set; }
        public virtual DbSet<DocumentCategory> DocumentCategories { get; set; }
        public virtual DbSet<DocumentType> DocumentTypes { get; set; }
        public virtual DbSet<KycStatus> KycStatuses { get; set; }
        public virtual DbSet<Section> Sections { get; set; }
        public virtual DbSet<SectionStatus> SectionStatuses { get; set; }

        public Task<int> SaveChangesAsync() => base.SaveChangesAsync();
        public Task<int> SaveChangesAuditable()
        {
            UpdateAuditEntities();
            return base.SaveChangesAsync();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CompanyGroupResource>()
                .Property(cgr => cgr.Guid)
                .HasDefaultValueSql("NEWID()");

            modelBuilder.Entity<CompanyGroupResource>()
                .Property(cgr => cgr.CreatedBy)
                .HasDefaultValue(1);

            modelBuilder.Entity<CompanyGroupResource>()
                .Property(cgr => cgr.CreatedOn)
                .HasDefaultValueSql("getutcdate()");

            modelBuilder.Entity<CompanyGroupResource>()
                .Property(cgr => cgr.ModifiedOn)
                .HasDefaultValueSql("getutcdate()");

            modelBuilder.Entity<CompanyGroupResource>()
                .Property(cgr => cgr.ModifiedBy)
                .HasDefaultValue(1);

            modelBuilder.Entity<CompanyGroupResource>()
                .Property(cgr => cgr.IsDeleted)
                .HasDefaultValue(0);

            modelBuilder.Entity<CompanyServiceUser>()
                .Property(cgr => cgr.IsMasterUser)
                .HasDefaultValue(0);

            modelBuilder.Entity<BeneficialOwnersType>(entity =>
            {
                entity.HasKey(e => e.BeneficialOwnersTypeCode);

                entity.ToTable("BeneficialOwnersType", "Settings");

                entity.Property(e => e.BeneficialOwnersTypeCode).HasMaxLength(10);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValueSql("(N'ADMIN')");

                entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DeletedBy).HasMaxLength(50);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.UpdatedBy)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("(N'ADMIN')");

                entity.Property(e => e.UpdatedOn).HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<Company>(entity =>
            {
                entity.ToTable("Company", "Company");

                entity.HasIndex(e => e.AddressId);

                entity.HasIndex(e => e.ContactDetailId);

                entity.Property(e => e.CompanyCode).HasMaxLength(10);

                entity.Property(e => e.CompanyName)
                    .IsRequired()
                    .HasMaxLength(160);

                entity.Property(e => e.CUCC).HasColumnName("CUCC");

                entity.Property(e => e.EmailAddress).HasMaxLength(74);

                entity.Property(e => e.ImageURL)
                    .HasColumnName("ImageURL")
                    .HasMaxLength(500);

                entity.Property(e => e.KycStatusCode).HasMaxLength(10);

                entity.Property(e => e.WebsiteURL)
                    .HasColumnName("WebsiteURL")
                    .HasMaxLength(50);

                entity.HasOne(d => d.KycStatusCodeNavigation)
                    .WithMany(p => p.Company)
                    .HasForeignKey(d => d.KycStatusCode)
                    .HasConstraintName("FK_Company_KycStatus");
            });

            modelBuilder.Entity<CompanyBeneficialOwners>(entity =>
            {
                entity.ToTable("CompanyBeneficialOwners", "Company");

                entity.Property(e => e.AdditionalAddress).HasMaxLength(1500);

                entity.Property(e => e.BeneficialOwnersTypeCode)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.CityName).HasMaxLength(100);

                entity.Property(e => e.CompanyAddress).HasMaxLength(1500);

                entity.Property(e => e.CountryName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValueSql("(N'ADMIN')");

                entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DateOfBirth).HasColumnType("date");

                entity.Property(e => e.DeletedBy).HasMaxLength(50);

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.StateName).HasMaxLength(100);

                entity.Property(e => e.UpdatedBy)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("(N'ADMIN')");

                entity.Property(e => e.UpdatedOn).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.PostalCode).HasMaxLength(10);

                entity.HasOne(d => d.BeneficialOwnersTypeCodeNavigation)
                    .WithMany(p => p.CompanyBeneficialOwners)
                    .HasForeignKey(d => d.BeneficialOwnersTypeCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CompanyBeneficialOwners_BeneficialOwnersType");

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.CompanyBeneficialOwners)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CompanyBeneficialOwners_Company");
            });

            modelBuilder.Entity<CompanyDirectors>(entity =>
            {
                entity.ToTable("CompanyDirectors", "Company");

                entity.Property(e => e.AdditionalAddress).HasMaxLength(1500);

                entity.Property(e => e.CityName).HasMaxLength(100);

                entity.Property(e => e.CompanyAddress).HasMaxLength(1500);

                entity.Property(e => e.CountryName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValueSql("(N'ADMIN')");

                entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DateOfBirth).HasColumnType("date");

                entity.Property(e => e.DeletedBy).HasMaxLength(50);

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.StateName).HasMaxLength(100);

                entity.Property(e => e.UpdatedBy)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("(N'ADMIN')");

                entity.Property(e => e.UpdatedOn).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.PostalCode).HasMaxLength(10);

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.CompanyDirectors)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CompanyDirectors_Company");
            });

            modelBuilder.Entity<CompanyDocuments>(entity =>
            {
                entity.ToTable("CompanyDocuments", "Company");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValueSql("(N'ADMIN')");

                entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DeletedBy).HasMaxLength(50);

                entity.Property(e => e.DocumentDescription).HasMaxLength(550);

                entity.Property(e => e.DocumentNo).HasMaxLength(50);

                entity.Property(e => e.DocumentTypeCode)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.FileUrl).HasMaxLength(1500);

                entity.Property(e => e.Guid).HasDefaultValueSql("(newid())");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.UpdatedBy)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("(N'ADMIN')");

                entity.Property(e => e.UpdatedOn).HasDefaultValueSql("(getutcdate())");

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.CompanyDocuments)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CompanyDocuments_Company");

                entity.HasOne(d => d.DocumentTypeCodeNavigation)
                    .WithMany(p => p.CompanyDocuments)
                    .HasForeignKey(d => d.DocumentTypeCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CompanyDocuments_DocumentType");
            });

            modelBuilder.Entity<CompanySections>(entity =>
            {
                entity.ToTable("CompanySections", "Company");

                entity.HasIndex(e => new { e.CompanyId, e.SectionCode })
                    .HasName("IX_CompanySections")
                    .IsUnique();

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValueSql("(N'ADMIN')");

                entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DeletedBy).HasMaxLength(50);

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.SectionCode)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.SectionStatusCode)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.UpdatedBy)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("(N'ADMIN')");

                entity.Property(e => e.UpdatedOn).HasDefaultValueSql("(getutcdate())");

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.CompanySections)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CompanySections_Company");

                entity.HasOne(d => d.SectionCodeNavigation)
                    .WithMany(p => p.CompanySections)
                    .HasForeignKey(d => d.SectionCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CompanySections_Section");

                entity.HasOne(d => d.SectionStatusCodeNavigation)
                    .WithMany(p => p.CompanySections)
                    .HasForeignKey(d => d.SectionStatusCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CompanySections_SectionStatus");
            });

            modelBuilder.Entity<CompanyStructure>(entity =>
            {
                entity.HasKey(e => e.CompanyId);

                entity.ToTable("CompanyStructure", "Company");

                entity.Property(e => e.CompanyId).ValueGeneratedNever();

                entity.Property(e => e.AdditionalAddress).HasMaxLength(1500);

                entity.Property(e => e.CityName).HasMaxLength(100);

                entity.Property(e => e.CompanyAddress).HasMaxLength(1500);

                entity.Property(e => e.CountryName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValueSql("(N'ADMIN')");

                entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DeletedBy).HasMaxLength(50);

                entity.Property(e => e.Guid).HasDefaultValueSql("(newid())");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.RegistrationNumber)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.StateName).HasMaxLength(100);

                entity.Property(e => e.UpdatedBy)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("(N'ADMIN')");

                entity.Property(e => e.UpdatedOn).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.PostalCode).HasMaxLength(10);

                entity.HasOne(d => d.Company)
                    .WithOne(p => p.CompanyStructure)
                    .HasForeignKey<CompanyStructure>(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CompanyStructure_Company");
            });

            modelBuilder.Entity<DocumentCategory>(entity =>
            {
                entity.HasKey(e => e.DocumentCategoryCode);

                entity.ToTable("DocumentCategory", "Settings");

                entity.Property(e => e.DocumentCategoryCode).HasMaxLength(10);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValueSql("(N'ADMIN')");

                entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DeletedBy).HasMaxLength(50);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Guid).HasDefaultValueSql("(newid())");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.UpdatedBy)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("(N'ADMIN')");

                entity.Property(e => e.UpdatedOn).HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<DocumentType>(entity =>
            {
                entity.HasKey(e => e.DocumentTypeCode);

                entity.ToTable("DocumentType", "Settings");

                entity.HasIndex(e => e.Description)
                    .HasName("IX_DocumentType")
                    .IsUnique();

                entity.Property(e => e.DocumentTypeCode).HasMaxLength(10);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValueSql("(N'ADMIN')");

                entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DeletedBy).HasMaxLength(50);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.DocumentCategoryCode)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.Guid).HasDefaultValueSql("(newid())");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.UpdatedBy)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("(N'ADMIN')");

                entity.Property(e => e.UpdatedOn).HasDefaultValueSql("(getutcdate())");

                entity.HasOne(d => d.DocumentCategoryCodeNavigation)
                    .WithMany(p => p.DocumentType)
                    .HasForeignKey(d => d.DocumentCategoryCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DocumentType_DocumentCategory");
            });

            modelBuilder.Entity<KycStatus>(entity =>
            {
                entity.HasKey(e => e.KycStatusCode);

                entity.ToTable("KycStatus", "Settings");

                entity.HasIndex(e => e.Description)
                    .HasName("IX_KycStatus")
                    .IsUnique();

                entity.Property(e => e.KycStatusCode).HasMaxLength(10);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValueSql("(N'ADMIN')");

                entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DeletedBy).HasMaxLength(50);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Guid).HasDefaultValueSql("(newid())");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.UpdatedBy)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("(N'ADMIN')");

                entity.Property(e => e.UpdatedOn).HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<Section>(entity =>
            {
                entity.HasKey(e => e.SectionCode);

                entity.ToTable("Section", "Settings");

                entity.HasIndex(e => new { e.SectionCode, e.Description })
                    .HasName("IX_Section")
                    .IsUnique();

                entity.Property(e => e.SectionCode).HasMaxLength(10);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValueSql("(N'ADMIN')");

                entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DeletedBy).HasMaxLength(50);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Guid).HasDefaultValueSql("(newid())");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.UpdatedBy)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("(N'ADMIN')");

                entity.Property(e => e.UpdatedOn).HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<SectionStatus>(entity =>
            {
                entity.HasKey(e => e.SectionStatusCode);

                entity.ToTable("SectionStatus", "Settings");

                entity.HasIndex(e => e.Description)
                    .HasName("IX_SectionStatus")
                    .IsUnique();

                entity.Property(e => e.SectionStatusCode).HasMaxLength(10);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValueSql("(N'ADMIN')");

                entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DeletedBy).HasMaxLength(50);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Guid).HasDefaultValueSql("(newid())");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.UpdatedBy)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("(N'ADMIN')");

                entity.Property(e => e.UpdatedOn).HasDefaultValueSql("(getutcdate())");
            });


            modelBuilder.SeedDefaultValues();

            base.OnModelCreating(modelBuilder);
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
    }
}
