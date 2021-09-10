using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using xas.core._Helpers;
using xas.core._Helpers.IOptionModels;
using xas.core._ResponseModel;
using xas.core.TruckArea.Models;
using xas.data.DataModel.TruckArea;
using System.Linq;
using System.IO;
using CsvHelper;
using System.Collections;
using Newtonsoft.Json;
using xgca.core.Response;
using xas.core.accreditation.Request;
using xgca.core.Helpers.Http;
using xgca.core.Constants;
using xgca.data.Company;
using xgca.data.ViewModels.TruckArea;
using xgca.core.ResponseV2;
using xgca.core.Models.Accreditation;

namespace xas.core.TruckArea
{
    public interface ITruckAreaCore
    {
        Task<GeneralModel> CreateTruckArea(CreateTruckArea obj);
        Task<GeneralModel> UpdateTruckArea(UpdateTruckArea obj);
        Task<GeneralModel> List(Guid requestGuid, string search, string city, string state, string country, string postal, string sortBy, string sortOrder, int pageNumber, int pageSize);
        Task<GeneralModel> PaginateList(List<GetTruckArea> list, int recordCount, int pageNumber, int pageSize);
        Task<GeneralModel> Delete(string id);
        Task<GeneralModel> DeleteMultiple(DeleteMultipleTruckArea list);
        Task<byte[]> ExportToCSV(Guid requestGuid, string search, string city, string state, string country, string postal, string sortBy, string sortOrder, int pageNumber, int pageSize);
        Task<byte[]> ExportToTemplate();
    }

    public class TruckAreaCore : ITruckAreaCore
    {
        private readonly IOptions<ClientManagement> _optionsClient;
        private readonly IOptions<ClientToken> _optionsToken;
        private readonly ITruckAreaData _repository;
        private readonly IRequestCore _requestCore;
        private readonly IGeneralResponse _general;
        private readonly IMapper _mapper;
        private readonly IHttpHelper _httpHelpers;
        private readonly IPagedResponse _pageResponse;
        private readonly ICompanyData _companyData;
        private readonly IPaginationResponse _pagination;

        public TruckAreaCore(
            IOptions<ClientManagement> optionsClient,
            IOptions<ClientToken> optionsToken,
            ITruckAreaData repository,
            IRequestCore requestCore,
            IGeneralResponse general,
            IMapper mapper,
            IHttpHelper httpHelpers,
            IPagedResponse pageResponse,
            ICompanyData companyData,
            IPaginationResponse pagination)
        {
            _optionsClient = optionsClient;
            _optionsToken = optionsToken;
            _repository = repository;
            _requestCore = requestCore;
            _general = general;
            _mapper = mapper;
            _httpHelpers = httpHelpers;
            _pageResponse = pageResponse;
            _companyData = companyData;
            _pagination = pagination;
        }

        public async Task<GeneralModel> CreateTruckArea(CreateTruckArea obj)
        {
            if (obj is null)
            {
                return _general.Response(null, StatusCodes.Status400BadRequest, "Data cannot be null", false);
            }

            int requestId = await _requestCore.GetRequestIdByGuid(obj.RequestId);

            var createModel = _mapper.Map<xgca.entity.Models.TruckArea>(obj);
            createModel.RequestId = requestId;
            var (model, message) = await _repository.Create(createModel);
            if (model is null)
            {
                return _general.Response(null, StatusCodes.Status400BadRequest, message, false);
            }

            var displayModel = _mapper.Map<GetTruckArea>(createModel);

            return _general.Response(new { AreaOfResponsibility = displayModel }, StatusCodes.Status200OK, message, true);
        }

        public async Task<GeneralModel> Delete(string id)
        {
            if(id is null) return _general.Response(null, StatusCodes.Status400BadRequest, "Invalid id", false);
            if (Guid.Parse(id) == Guid.Empty)return _general.Response(null, StatusCodes.Status400BadRequest, "Invalid id", false);

            var (result, message) = await _repository.Delete(Guid.Parse(id));

            if (!(result)) return _general.Response(null, StatusCodes.Status400BadRequest, message, result);
            return _general.Response(null, StatusCodes.Status200OK, message, result);
        }

        public async Task<GeneralModel> DeleteMultiple(DeleteMultipleTruckArea list)
        {
            if (list.Ids.Count == 0)
            {
                return _general.Response(null, StatusCodes.Status400BadRequest, "No items selected", false);
            }

            var guidList = new List<Guid>();
            foreach(var id in list.Ids)
            {
                guidList.Add(Guid.Parse(id));
            }

            var (result, message) = await _repository.DeleteBulk(guidList, GlobalVariables.LoggedInUsername);

            if (!(result))
            {
                return _general.Response(null, StatusCodes.Status400BadRequest, message, result);
            }
            return _general.Response(null, StatusCodes.Status200OK, message, result);
        }

        public async Task<byte[]> ExportToCSV(Guid requestGuid, string search, string city, string state, string country, string postal, string sortBy, string sortOrder, int pageNumber, int pageSize)
        {
           string fileName = String.Concat(Directory.GetCurrentDirectory(), @"AccessManagement_UserExportListing.csv");
            if (File.Exists(fileName)) File.Delete(fileName);

            var (result, recordCount) = await _repository.List(requestGuid, search, city, state, country, postal, sortBy, sortOrder, pageNumber, pageSize);

            MemoryStream ms = new MemoryStream();
            using (StreamWriter sw = new StreamWriter(ms, Encoding.UTF8))
            {
                CsvWriter cw = new CsvWriter(sw, System.Globalization.CultureInfo.CurrentCulture);

                cw.WriteHeader<ExportCSVTruckAreaModel>();
                cw.NextRecord();

                foreach (var p in result)
                {
                    var profile = new ExportCSVTruckAreaModel
                    {                         
                           CountryName = p.CountryName       
                         , StateName = p.StateName 
                         , CityName =  p.CityName 
                         , PostalCode = p.PostalCode                    
                    };

                    cw.WriteRecord(profile);
                    cw.NextRecord();
                }
                sw.Flush();
            }

            ms.Close();
            byte[] profileToExport = ms.ToArray();
            return profileToExport;
        }

        public async Task<byte[]> ExportToTemplate()
        {
            MemoryStream ms = new MemoryStream();
            using (StreamWriter sw = new StreamWriter(ms, Encoding.UTF8))
            {
                CsvWriter cw = new CsvWriter(sw, System.Globalization.CultureInfo.CurrentCulture);

                cw.WriteHeader<TemplateTruckArea>();
                cw.NextRecord();

                sw.Flush();
            }

            ms.Close();
            byte[] truckAreaTemplate = ms.ToArray();
            return truckAreaTemplate;
        }

        public async Task<GeneralModel> List(Guid requestGuid, string search, string city, string state, string country, string postal, string sortBy, string sortOrder, int pageNumber, int pageSize)
        {
            var (result, recordCount) = await _repository.List(requestGuid, search, city, state, country, postal, sortBy, sortOrder, pageNumber, pageSize);
            return _general.Response(_pagination.Paginate(result, recordCount, pageNumber, pageSize), StatusCodes.Status200OK, "Truck Area list successfully loaded.", true);
        }

        public async Task<GeneralModel> PaginateList(List<GetTruckArea> list, int recordCount, int pageNumber, int pageSize)
        {
            var pagedResponse = _pageResponse.Paginate(list, recordCount, pageNumber, pageSize);
            return _general.Response(pagedResponse, 200, "Configurable are of responsibilities has been listed", true);
        }

        public async Task<GeneralModel> UpdateTruckArea(UpdateTruckArea obj)
        {
            if (obj is null)
            {
                return _general.Response(null, StatusCodes.Status400BadRequest, "Data cannot be null", false);
            }

            var updateModel = _mapper.Map<xgca.entity.Models.TruckArea>(obj);
            var (model, message) = await _repository.Update(updateModel);
            if (model is null)
            {
                return _general.Response(null, StatusCodes.Status400BadRequest, message, false);
            }

            var displayModel = _mapper.Map<GetTruckArea>(model);
            return _general.Response(new { AreaOfResponsibility = displayModel }, StatusCodes.Status200OK, message, true);
        }
    }
}
