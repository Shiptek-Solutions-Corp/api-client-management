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
    public class XGCAContext : DbContext, IXGCAContext
    {
        public XGCAContext(DbContextOptions options)
            : base(options)
        { }
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

        public Task<int> SaveChangesAsync() => base.SaveChangesAsync();
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
                    entityDel.IsDeleted = true;
                    entityDel.DeletedOn = now;
                    entityDel.DeletedBy = "calicubilly";
                }
            }
        }
    }
}
