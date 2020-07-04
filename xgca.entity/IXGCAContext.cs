using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using xgca.entity.Models;

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

    }
}