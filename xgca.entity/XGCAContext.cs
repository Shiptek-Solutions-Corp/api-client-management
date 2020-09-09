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

            modelBuilder.Entity<Address>().HasData(
                 new Address
                 {
                     AddressId = 1,
                     AddressLine = "None",
                     AddressTypeId = 1,
                     FullAddress = "None",
                     CityId = 1,
                     CityName = "None",
                     StateId = 1,
                     StateName = "None",
                     CountryId = 1,
                     CountryName = "Global",
                     Latitude = "None",
                     Longitude = "None",
                     ZipCode = "None",
                     CreatedBy = 0,
                     CreatedOn = DateTime.UtcNow,
                     ModifiedBy = 0,
                     ModifiedOn = DateTime.UtcNow,
                     Guid = Guid.Parse("21716A6C-4DEE-422B-B2D9-74BF4B12E242"),
                     IsDeleted = 1
                 });

            modelBuilder.Entity<ContactDetail>().HasData(
                new ContactDetail
                {
                    ContactDetailId = 1,
                    PhonePrefixId = 1,
                    PhonePrefix = "None",
                    Phone = "None",
                    MobilePrefixId = 1,
                    MobilePrefix = "None",
                    Mobile = "None",
                    FaxPrefixId = 1,
                    FaxPrefix = "None",
                    Fax = "None",
                    Guid = Guid.Parse("1F5CB3B9-992B-4279-ACF7-7F0273ABBF03"),
                    CreatedBy = 0,
                    CreatedOn = DateTime.UtcNow,
                    ModifiedBy = 0,
                    ModifiedOn = DateTime.UtcNow,
                    IsDeleted = 1
                });

            modelBuilder.Entity<Company>().HasData(
                new Company
                {
                    CompanyId = 1,
                    CompanyName = "None",
                    ClientId = 1,
                    ContactDetailId = 1,
                    AddressId = 1,
                    EmailAddress = "None",
                    ImageURL = "None",
                    Status = 0,
                    Guid = Guid.Parse("3608A083-FBE5-44D5-AFEF-C7814100AEC7"),
                    WebsiteURL = "None",
                    CreatedBy = 0,
                    CreatedOn = DateTime.UtcNow,
                    ModifiedBy = 0,
                    ModifiedOn = DateTime.UtcNow,
                    IsDeleted = 1,
                    TaxExemption = 0,
                    TaxExemptionStatus = 0
                });

            modelBuilder.Entity<Guest>().HasData(
                new Guest
                {
                    GuestId = 1,
                    GuestName = "None",
                    GuestType = 0,
                    AddressLine = "None",
                    CityId = "None",
                    CityName = "None",
                    StateId = "None",
                    StateName = "None",
                    CountryId = 1,
                    CountryName = "Global",
                    EmailAddress = "None",
                    PhoneNumberPrefixId = "None",
                    PhoneNumberPrefix = "None",
                    PhoneNumber = "None",
                    MobileNumberPrefixId = "None",
                    MobileNumberPrefix = "None",
                    MobileNumber = "None",
                    FaxNumberPrefixId = "None",
                    FaxNumberPrefix = "None",
                    FaxNumber = "None",
                    CompanyId = 1,
                    Id = Guid.Parse("7BBE1F11-E04A-4A8B-AFE1-EC55EBA13D66"),
                    FirstName = "None",
                    LastName = "None",
                    IsGuest = false,
                    ZipCode = "None",
                    CreatedBy = 0,
                    CreatedOn = DateTime.UtcNow,
                    ModifiedBy = 0,
                    ModifiedOn = DateTime.UtcNow,
                    IsDeleted = 1,
                    DeletedBy = 0,
                    DeletedOn = DateTime.UtcNow
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
                .HasDefaultValue(Guid.NewGuid());

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
