using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using xgca.entity.Models;

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

        public Task<int> SaveChangesAsync() => base.SaveChangesAsync();
        
    }
}
