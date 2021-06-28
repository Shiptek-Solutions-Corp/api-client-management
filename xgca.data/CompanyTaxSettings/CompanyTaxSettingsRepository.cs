using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xgca.entity;

namespace xgca.data.CompanyTaxSettings
{
    public interface ICompanyTaxSettingsRepository
    {
        Task<(bool, string[])> Create(Guid companyGuid, entity.Models.CompanyTaxSettings companyTaxSettings);
        Task<(List<entity.Models.CompanyTaxSettings>, int, string[])> List(string orderBy, string query, int pageNumber = 1, int pageSize = 10);
        Task<(entity.Models.CompanyTaxSettings, string[])> Show(Guid guid);
        Task<(entity.Models.CompanyTaxSettings, string[])> Put(entity.Models.CompanyTaxSettings companyTaxSettings);
        Task<(bool, string[])> Patch(entity.Models.CompanyTaxSettings companyTaxSettings);
        Task<(bool, string[])> Delete(Guid guid);
    }
    public class CompanyTaxSettingsRepository : ICompanyTaxSettingsRepository
    {
        private readonly IXGCAContext context;

        public CompanyTaxSettingsRepository(IXGCAContext context)
        {
            this.context = context;
        }

        public async Task<(bool, string[])> Create(Guid companyGuid, entity.Models.CompanyTaxSettings companyTaxSettings)
        {
            var company = await context.Companies.FirstOrDefaultAsync(c => c.Guid.Equals(companyGuid));
            
            if(company == null)
                return (false, new[] { "Company not found" });

            companyTaxSettings.CompanyId = company.CompanyId; // Override company id

            await context.CompanyTaxSettings.AddAsync(companyTaxSettings);
            var result = await context.SaveChangesAsync();

            return (result == 1, null);
        }

        public async Task<(bool, string[])> Delete(Guid guid)
        {
            var companyTaxSetting = await context.CompanyTaxSettings.FirstOrDefaultAsync(c => c.Guid.Equals(guid));

            if (companyTaxSetting == null)
                return (false, new[] { "Company Tax Setting not found" });

            companyTaxSetting.Status = 0;
            companyTaxSetting.StatusName = "Inactive";
            companyTaxSetting.ModifiedOn = DateTime.UtcNow;
            companyTaxSetting.ModifiedBy = "Admin";

            var result = await context.SaveChangesAsync();

            if (result == 1)
                return (true, null);
            else
                return (false, new[] { "An error occured on deleting object" });
        }

        public Task<(List<entity.Models.CompanyTaxSettings>, int, string[])> List(string orderBy, string query, int pageNumber = 1, int pageSize = 10)
        {
            throw new NotImplementedException();
        }

        public async Task<(bool, string[])> Patch(entity.Models.CompanyTaxSettings companyTaxSettings)
        {
            context.CompanyTaxSettings.Update(companyTaxSettings);

            var result = await context.SaveChangesAsync();

            if (result < 1)
            {
                return (false, new string[] { "An error occured on updating object" });
            }

            return (true, null);
        }

        public async Task<(entity.Models.CompanyTaxSettings, string[])> Put(entity.Models.CompanyTaxSettings payload)
        {
            var companyTaxSetting = await context.CompanyTaxSettings.FirstOrDefaultAsync(c => c.Guid.Equals(payload.Guid));
            
            if (companyTaxSetting == null)
                return (null, new[] { "Company Tax Setting not found" });

            companyTaxSetting.TaxPercentageRate = payload.TaxPercentageRate;
            companyTaxSetting.TaxTypeDescription = payload.TaxTypeDescription;
            companyTaxSetting.TaxTypeId = payload.TaxTypeId;
            companyTaxSetting.IsTaxExcempted = payload.IsTaxExcempted;
            companyTaxSetting.Status = payload.Status;
            companyTaxSetting.StatusName = payload.Status == 1 ? "Active" : "Inactive";
            companyTaxSetting.ModifiedOn = DateTime.UtcNow;
            companyTaxSetting.ModifiedBy = "Admin";

            var result = await context.SaveChangesAsync();

            if (result == 1)
                return (companyTaxSetting, null);
            else
                return (null, new[] { "An error occured on updating object" });
        }

        public Task<(entity.Models.CompanyTaxSettings, string[])> Show(Guid guid)
        {
            throw new NotImplementedException();
        }
    }
}
