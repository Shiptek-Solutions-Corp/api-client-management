using AutoMapper;
using otm.core.ResponseV2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xgca.core.Models.Company;
using xgca.data.Company;

namespace xgca.core.Services
{
    public interface ICompanyServiceV2
    {
        Task<PagedResponse<List<GetCompanyListingViewModel>>> GetCompanyList(int pageNumber = 1, int pageSize = 10, string orderBy = null, string query = null);
    }

    public class CompanyServiceV2 : ICompanyServiceV2
    {
        private readonly ICompanyDataV2 companyData;
        private readonly IMapper mapper;
        public CompanyServiceV2(ICompanyDataV2 companyData, IMapper mapper)
        {
            this.companyData = companyData;
            this.mapper = mapper;
        }

        public async Task<PagedResponse<List<GetCompanyListingViewModel>>> GetCompanyList(int pageNumber = 1, int pageSize = 10, string orderBy = null, string query = null)
        {
            var (result, totalCount, errors) = await companyData.List(orderBy, query, pageNumber, pageSize);
            var companies = mapper.Map<List<GetCompanyListingViewModel>>(result).ToList();

            return new PagedResponse<List<GetCompanyListingViewModel>>(companies, "List of companies", 200, pageNumber, pageSize, totalCount, orderBy, query, null);
        }
    }
}
