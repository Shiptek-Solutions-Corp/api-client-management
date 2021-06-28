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
        Task<(List<entity.Models.Company>, int, string[])> List(string orderBy, string query, int pageNumber = 1, int pageSize = 10);
        Task<(entity.Models.Company, string[])> Show(Guid guid);
        Task<(entity.Models.Company, string[])> Put(entity.Models.Company company);
        Task<(entity.Models.Company, string[])> Patch(Guid guid, entity.Models.Company company);
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

        public async Task<(List<entity.Models.Company>, int, string[])> List(string orderBy, string query, int pageNumber = 1, int pageSize = 10)
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
                    companies = companies.Where(
                        $"Addresses.CountryName.ToLower().Contains(@0) or " +
                        $"CompanyName.ToLower().Contains(@0) or " +
                        $"CompanyServices.Any(c => c.ServiceName.ToLower().Contains(@0)) or " +
                        $"StatusName.ToLower().Contains(@0) or " +
                        $"KycStatusCode.ToLower().Contains(@0)",
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
                                companies = companies.Where(c => c.Addresses.CountryName.ToLower().Contains(filterValue));
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

        public Task<(entity.Models.Company, string[])> Patch(Guid guid, entity.Models.Company company)
        {
            throw new NotImplementedException();
        }

        public Task<(entity.Models.Company, string[])> Put(entity.Models.Company company)
        {
            throw new NotImplementedException();
        }

        public async Task<(entity.Models.Company, string[])> Show(Guid guid)
        {

            var company = await context.Companies
                .Include(c => c.CompanyTaxSettings) // Get Only Active
                .Include(c => c.Addresses)
                    .ThenInclude(a => a.AddressTypes)
                .Include(c => c.ContactDetails)
                .Include(c => c.CompanyServices)
                .Where(c => c.Guid == guid)
                .Select(c => new entity.Models.Company
                {
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
