using Microsoft.AspNetCore.Http;
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
    public partial class XGCAContext : DbContext
    {
        private readonly IHttpContextAccessor contextAccessor;

        public XGCAContext(DbContextOptions<XGCAContext> options, IHttpContextAccessor contextAccessor)
         : base(options)
        { this.contextAccessor = contextAccessor; }

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
        public DbSet<CompanyTaxSettings> CompanyTaxSettings { get; set; }
        public DbSet<KYCLog> KYCLogs { get; set; }

        //Accreditation
        public virtual DbSet<AccreditationStatusConfig> AccreditationStatusConfig { get; set; }
        public virtual DbSet<PortArea> PortArea { get; set; }
        public virtual DbSet<Request> Request { get; set; }
        public virtual DbSet<ServiceRoleConfig> ServiceRoleConfig { get; set; }
        public virtual DbSet<TruckArea> TruckArea { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CompanyTaxSettings>(entity => {

                entity
                .Property(cts => cts.Guid)
                .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasDefaultValue(1);

                entity.Property(e => e.StatusName)
                    .IsRequired()
                    .HasDefaultValue("Active");

                entity
                .Property(cgr => cgr.CreatedOn)
                .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasDefaultValue("Admin");
            });

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


            #region KYC
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

                entity.Property(e => e.PostalCode).HasMaxLength(10);

                entity.Property(e => e.StateName).HasMaxLength(100);

                entity.Property(e => e.UpdatedBy)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("(N'ADMIN')");

                entity.Property(e => e.UpdatedOn).HasDefaultValueSql("(getutcdate())");

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

                entity.Property(e => e.PostalCode).HasMaxLength(10);

                entity.Property(e => e.StateName).HasMaxLength(100);

                entity.Property(e => e.UpdatedBy)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("(N'ADMIN')");

                entity.Property(e => e.UpdatedOn).HasDefaultValueSql("(getutcdate())");

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

                entity.HasOne(d => d.DocumentType)
                    .WithMany(p => p.CompanyDocuments)
                    .HasForeignKey(d => d.DocumentTypeId)
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

                entity.Property(e => e.PostalCode).HasMaxLength(10);

                entity.Property(e => e.RegistrationNumber)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.StateName).HasMaxLength(100);

                entity.Property(e => e.UpdatedBy)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("(N'ADMIN')");

                entity.Property(e => e.UpdatedOn).HasDefaultValueSql("(getutcdate())");

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
                entity.ToTable("DocumentType", "Settings");

                entity.HasIndex(e => e.Description)
                    .HasName("IX_DocumentType")
                    .IsUnique();

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

            modelBuilder.Entity<KYCLog>(entity =>
            {
                entity.ToTable("KYCLog", "Settings");

                entity.HasIndex(e => e.CompanyId);

                entity.HasIndex(e => e.CompanySectionsId);

                entity.Property(e => e.KyclogId).HasColumnName("KYCLogId");

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.KYCLogs)
                    .HasForeignKey(d => d.CompanyId);

                entity.HasOne(d => d.CompanySections)
                    .WithMany(p => p.Kyclog)
                    .HasForeignKey(d => d.CompanySectionsId);
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

            #endregion

            //Accreditation
            modelBuilder.Entity<AccreditationStatusConfig>(entity =>
            {
                entity.ToTable("AccreditationStatusConfig", "Accreditation");

                entity.HasIndex(e => e.Guid)
                    .HasName("IX_AccreditationStatusConfig")
                    .IsUnique();

                entity.Property(e => e.CreatedBy).IsRequired();

                entity.Property(e => e.Guid).HasDefaultValueSql("(newid())");
            });

            modelBuilder.Entity<PortArea>(entity =>
            {
                entity.ToTable("PortArea", "Accreditation");

                entity.HasIndex(e => e.Guid)
                    .HasName("IX_PortArea")
                    .IsUnique();

                entity.Property(e => e.CityName).HasMaxLength(150);

                entity.Property(e => e.CountryCode).HasMaxLength(2);

                entity.Property(e => e.CountryName).HasMaxLength(100);

                entity.Property(e => e.Guid).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Latitude).HasMaxLength(20);

                entity.Property(e => e.Location).HasMaxLength(500);

                entity.Property(e => e.Locode).HasMaxLength(15);

                entity.Property(e => e.Longitude).HasMaxLength(20);

                entity.Property(e => e.PortCode).HasMaxLength(10);

                entity.Property(e => e.PortName).HasMaxLength(100);

                entity.Property(e => e.StateName).HasMaxLength(100);

                entity.HasOne(d => d.Request)
                    .WithMany(p => p.PortArea)
                    .HasForeignKey(d => d.RequestId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PortArea_Request");
            });

            modelBuilder.Entity<Request>(entity =>
            {
                entity.ToTable("Request", "Accreditation");

                entity.HasIndex(e => e.Guid)
                    .HasName("IX_Request")
                    .IsUnique();

                entity.Property(e => e.CompanyCodeFrom).HasMaxLength(10);

                entity.Property(e => e.CompanyCodeTo).HasMaxLength(10);

                entity.Property(e => e.CompanyNameFrom).HasMaxLength(160);

                entity.Property(e => e.CompanyNameTo).HasMaxLength(160);

                entity.Property(e => e.CreatedBy).IsRequired();

                entity.Property(e => e.Guid).HasDefaultValueSql("(newid())");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.AccreditationStatusConfig)
                    .WithMany(p => p.Request)
                    .HasForeignKey(d => d.AccreditationStatusConfigId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Request_AccreditationStatusConfig");
            });

            modelBuilder.Entity<ServiceRoleConfig>(entity =>
            {
                entity.ToTable("ServiceRoleConfig", "Accreditation");

                entity.HasIndex(e => e.Guid)
                    .HasName("IX_ServiceRoleConfig")
                    .IsUnique();

                entity.Property(e => e.CreatedBy).IsRequired();

                entity.Property(e => e.Guid).HasDefaultValueSql("(newid())");
            });

            modelBuilder.Entity<TruckArea>(entity =>
            {
                entity.ToTable("TruckArea", "Accreditation");

                entity.HasIndex(e => e.Guid)
                    .HasName("IX_TruckArea")
                    .IsUnique();

                entity.Property(e => e.CreatedBy).IsRequired();

                entity.Property(e => e.Guid).HasDefaultValueSql("(newid())");

                entity.HasOne(d => d.Request)
                    .WithMany(p => p.TruckArea)
                    .HasForeignKey(d => d.RequestId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TruckArea_Request");
            });


            modelBuilder.SeedDefaultValues();

            base.OnModelCreating(modelBuilder);
        }
       
    }
}
