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
        Task<(entity.Models.Company, string[])> Put(Guid guid);
        Task<(entity.Models.Company, string[])> Patch(Guid guid, entity.Models.Company company);
        Task<(bool, string[])> Delete(Guid guid);
    }

    public class CompanyDataV2 : ICompanyDataV2
    {
        private readonly XGCAContext _context;
        public CompanyDataV2(XGCAContext _context)
        {
            this._context = _context;
        }

        public Task<(bool, string[])> Delete(Guid guid)
        {
            throw new NotImplementedException();
        }

        public async Task<(List<entity.Models.Company>, int, string[])> List(string orderBy, string query, int pageNumber = 1, int pageSize = 10)
        {
            var companies = _context.Companies
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

        public Task<(entity.Models.Company, string[])> Put(Guid guid)
        {
            throw new NotImplementedException();
        }

        public Task<(entity.Models.Company, string[])> Show(Guid guid)
        {
            throw new NotImplementedException();
        }
    }
}
