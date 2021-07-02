using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using xgca.entity;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using LinqKit;
using System.Linq.Dynamic.Core;
using otm.data.Helpers;

namespace xgca.data.Company
{
    public interface ICompanyDataV2
    {
        Task<(List<entity.Models.Company>, int, string[])> List(bool isFromSettings, string orderBy, string query, int pageNumber = 1, int pageSize = 10);
        Task<(entity.Models.Company, string[])> Show(Guid guid);
        Task<(entity.Models.Company, string[])> Put(entity.Models.Company company);
        Task<(entity.Models.Company, string[])> Patch(entity.Models.Company company);
        Task<(bool, string[])> Delete(Guid guid);
    }

    public class CompanyDataV2 : ICompanyDataV2
    {
        private readonly XGCAContext context;
        public CompanyDataV2(XGCAContext context)
        {
            this.context = context;
        }

        public Task<(bool, string[])> Delete(Guid guid)
        {
            throw new NotImplementedException();
        }

        public async Task<(List<entity.Models.Company>, int, string[])> List(bool isFromSettings, string orderBy, string query, int pageNumber = 1, int pageSize = 10)
        {
            var companies = context.Companies
                .Include(c => c.Addresses)
                .Include(c => c.CompanyServices)
                .Where(c => c.IsDeleted == 0).AsNoTracking();

            // search
            if (!string.IsNullOrEmpty(query))
            {
                var queries = query.Split(",").ToDictionary(k => k.Split(":")[0], v => v.Split(":")[1]);
                var predicate = PredicateBuilder.New<entity.Models.Company>();

                if (queries.ContainsKey("all"))
                {
                    queries.TryGetValue("all", out var value);
                    string queryStrings =
                        $"Addresses.CountryName.ToLower().Contains(@0) or " +
                        $"CompanyName.ToLower().Contains(@0) or " +
                        $"CompanyServices.Any(c => c.ServiceName.ToLower().Contains(@0)) or " +
                        $"StatusName.ToLower().Contains(@0) or " +
                        $"KycStatusCode.ToLower().Contains(@0)";

                    if (isFromSettings) // Check if request is from Company Settings Tab
                        queryStrings += 
                            $" or Addresses.StateName.ToLower().Contains(@0) or " + 
                            $"PricingSettingsDescription.ToLower().Contains(@0)";

                    companies = companies.Where(queryStrings,
                        value);
                } 
                else
                {
                    DateTime dateFrom = DateTime.Now;
                    DateTime dateTo = DateTime.Now;

                    foreach (var (key, value) in queries)
                    {
                        var filterValue = value.ToLower();
                        if (key.Contains("createdOn"))
                        {
                            string[] dates = value.Split('|');
                            dateFrom = DateTime.Parse(dates[0]);
                            dateTo = DateTime.Parse(dates[1]);
                            dateTo = new DateTime(dateTo.Year, dateTo.Month, dateTo.Day, 23, 59, 59);
                        }
                        switch (key)
                        {
                            case "country":
                            case "countryName":
                                companies = companies.Where(c => c.Addresses.CountryName.ToLower().Contains(filterValue));
                                continue;
                            case "state":
                            case "stateName":
                                companies = companies.Where(c => c.Addresses.StateName.ToLower().Contains(filterValue));
                                continue;
                            case "serviceName":
                                companies = companies.Where(c => c.CompanyServices.Any(cs => cs.ServiceName.ToLower().Contains(filterValue)));
                                continue;
                            case "createdOn":
                                companies = companies.Where(p => (p.CreatedOn >= dateFrom && p.CreatedOn <= dateTo));
                                continue;
                            case "status":
                                companies = companies.Where(c => c.Status.ToString().Equals(filterValue));
                                continue;
                            default:
                                companies = companies.Where($"{key}.ToLower().Contains(@0)", filterValue);
                                break;
                        }
                    }
                }
            }

            var pagedList = new PagedList<entity.Models.Company>(companies, pageNumber, pageSize, orderBy);

            return (pagedList.Items, pagedList.TotalCount, null);
        }

        public async Task<(entity.Models.Company, string[])> Patch(entity.Models.Company company)
        {
            company.ModifiedOn = DateTime.UtcNow;
            company.ModifiedBy = 0;

            context.Companies.Update(company).State = EntityState.Modified;
            var result = await context.SaveChangesAsync();

            if (result < 1)
                return (null, new string[] { "An error occured on updating company details" });

            return (company, null);
        }

        public async Task<(entity.Models.Company, string[])> Put(entity.Models.Company payload)
        {
                var company = await context.Companies
                .Include(c => c.CompanyTaxSettings)
                .Include(c => c.Addresses)
                    .ThenInclude(a => a.AddressTypes)
                .Include(c => c.ContactDetails)
                .FirstOrDefaultAsync(c => c.Guid.Equals(payload.Guid));

            if (company == null)
                return (null, new[] { "Company not found" });

            context.CompanyTaxSettings.RemoveRange(company.CompanyTaxSettings);

            payload.Addresses.CreatedOn = company.CreatedOn;
            payload.Addresses.ModifiedOn = DateTime.UtcNow;
            context.Entry(company.Addresses).CurrentValues.SetValues(payload.Addresses);

            payload.ContactDetails.CreatedOn = company.CreatedOn;
            payload.ContactDetails.ModifiedOn = DateTime.UtcNow;
            context.Entry(company.ContactDetails).CurrentValues.SetValues(payload.ContactDetails);

            foreach (var taxSetting in payload.CompanyTaxSettings)
                taxSetting.CompanyId = company.CompanyId;

            context.CompanyTaxSettings.AddRange(payload.CompanyTaxSettings);

            company.CompanyName = payload.CompanyName;
            company.ImageURL = payload.ImageURL;
            company.EmailAddress = payload.EmailAddress;
            company.WebsiteURL = payload.WebsiteURL;
            company.ModifiedOn = DateTime.UtcNow;
            company.ModifiedBy = 0;

            await context.SaveChangesAsync();

            return (company, null);
        }

        public async Task<(entity.Models.Company, string[])> Show(Guid guid)
        {

            var company = await context.Companies
                .Include(c => c.CompanyTaxSettings)
                .Include(c => c.Addresses)
                    .ThenInclude(a => a.AddressTypes)
                .Include(c => c.ContactDetails)
                .Include(c => c.CompanyServices)
                .Where(c => c.Guid == guid)
                .Select(c => new entity.Models.Company
                {
                  CompanyId = c.CompanyId,
                  Guid = c.Guid,
                  CompanyCode = c.CompanyCode,
                  CompanyName = c.CompanyName,
                  ImageURL = c.ImageURL,
                  EmailAddress = c.EmailAddress,
                  WebsiteURL = c.WebsiteURL,
                  StatusName = c.StatusName,
                  TaxExemption =  c.TaxExemption,
                  TaxExemptionStatus = c.TaxExemptionStatus,
                  CUCC = c.CUCC,
                  AccreditedBy = c.AccreditedBy,
                  KycStatusCode = c.KycStatusCode,
                  Addresses = c.Addresses,
                  ContactDetails = c.ContactDetails,
                  CompanyServices = c.CompanyServices,
                  CompanyTaxSettings = c.CompanyTaxSettings.Where(cts => cts.Status == 1).ToList()
                })
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (company != null)
                return (company, null);

            return (null, new[] { "Company not found" });
        }
    }
}
