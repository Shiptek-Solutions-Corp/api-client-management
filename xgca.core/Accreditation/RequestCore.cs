using AutoMapper;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using ClosedXML.Excel;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Threading.Tasks;
using xas.core._Helpers;
using xas.core._Helpers.IOptionModels;
using xas.core._ResponseModel;
using xas.core.Request;
using xas.core.Request.DTO;
using xas.data.accreditation.Request;
using System.Net;
using System.IO;
using System.Drawing;
using System.Net.Mime;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Text;
using System.Globalization;
using CsvHelper;
using Microsoft.AspNetCore.Http;
using System.Collections;
using xgca.core.Response;
using xgca.core.Helpers.Http;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using FluentValidation;
using xas.core.CustomerAccreditation.DTO;
using xgca.core.Company;
using xgca.data.Company;
using xgca.core.Helpers;
using xas.data.DataModel.TruckArea;
using xas.data.DataModel.PortArea;
using xgca.core.ResponseV2;
using xgca.data.ViewModels.Request;
using xgca.core.Models.Accreditation;
using Microsoft.Extensions.Configuration;
using Amazon.SecurityToken;

namespace xas.core.accreditation.Request
{
    public interface IRequestCore
    {
        #region Request
        Task<GeneralModel> CreateRequest(List<RequestModel> requestInfo);
        Task<GeneralModel> GetAccreditationStats(int companyId, string bound);
        Task<GeneralModel> DeleteAccreditaitonRequest(List<Guid> requestIdss);
        Task<GeneralModel> ActivateDeactivateRequest(List<Guid> requestIds, bool status);
        Task<GeneralModel> UpdateRequestStatusBulk(int companyId, List<Guid> requestId, int status);
        Task<int> GetRequestIdByGuid(string guid);
        Task<GeneralModel> GetRequestList(string bound, int pageSize, int pageNumber, Guid companyGuid, Guid serviceRoleGuid, string companyName, string companyAddress, string companyCountryName, string companyStateCityName, string portAreaResponsibility, string truckAreaResponsibility, int accreditationStatusConfigId, byte? companyStatus, string sortOrder, string sortBy, string quickSearch);
        Task<byte[]> ExportRequestListToCSV(string bound, int pageSize, int pageNumber, Guid companyGuid, Guid serviceRoleGuid, string companyName, string companyAddress, string companyCountryName, string companyStateCityName, string portAreaResponsibility, string truckAreaResponsibility, int accreditationStatusConfigId, byte? companyStatus, string sortOrder, string sortBy, string quickSearch);
        Task<byte[]> ExportRequestListToCSVTemplate();

        #endregion

        #region Customer Accreditation
        Task<dynamic> CreateCustomerAccreditation(CustomerRegistrationDTO customerRegistrationDTO, int companyId, string username, string serviceRole, string serviceRoleId);
        Task<GeneralModel> PortOfResponsibilityAccreditedCustomer(ListPortOfResponsibility obj);
        #endregion
    }

    public class RequestCore : IRequestCore
    {
        private readonly IRequestData _requestData;
        private readonly IPortAreaData _portAreaData;
        private readonly IMapper _mapper;
        private readonly IGeneralResponse _generalResponse;
        private readonly IHttpHelper _httpHelper;
        private readonly IPagedResponse _pageresponse;
        private readonly IValidator<List<RequestModel>> _validatorCreateRequest;
        private readonly ICompanyData _companyData;
        private readonly IOptions<AuthConfig> _authhConfig;
        private readonly IPaginationResponse _pagination;
        private readonly IConfiguration _config;
        private readonly IAmazonSecurityTokenService _amazonSecurityTokenService;

        public RequestCore(
            IRequestData requestData,
            IPortAreaData portAreaData,
            IMapper mapper,
            IGeneralResponse generalResponse,
            IHttpHelper httpHelper,
            IPagedResponse pageresponse,
            IValidator<List<RequestModel>> validatorCreateRequest,
            ICompanyData companyData,
            IOptions<AuthConfig> authhConfig,
            IPaginationResponse pagination,
            IConfiguration config,
            IAmazonSecurityTokenService amazonSecurityTokenService)
        {
            _requestData = requestData;
            _portAreaData = portAreaData;
            _mapper = mapper;
            _generalResponse = generalResponse;
            _httpHelper = httpHelper;
            _pageresponse = pageresponse;
            _validatorCreateRequest = validatorCreateRequest;
            _companyData = companyData;
            _authhConfig = authhConfig;
            _pagination = pagination;
            _config = config;
            _amazonSecurityTokenService = amazonSecurityTokenService;
        }

        #region Request        
        public async Task<GeneralModel> GetRequestList(string bound, int pageSize, int pageNumber, Guid companyGuid, Guid serviceRoleGuid, string companyName, string companyAddress, string companyCountryName, string companyStateCityName, string portAreaResponsibility, string truckAreaResponsibility, int accreditationStatusConfigId, byte? companyStatus, string sortOrder, string sortBy, string quickSearch)
        {
            var response = await _requestData.GetRequestList(bound, pageSize, pageNumber, companyGuid, serviceRoleGuid, companyName, companyAddress, companyCountryName, companyStateCityName, portAreaResponsibility, truckAreaResponsibility, accreditationStatusConfigId, companyStatus, sortOrder, sortBy, quickSearch);

            //Update Image Url for new S3 link for each record
            response.Item1.ForEach(i =>
            {
                var newCompanyLogoUrl = i.CompanyLogo.Split("/").Last();
                i.CompanyLogo = S3Helper.GetS3URL(newCompanyLogoUrl, _config, _amazonSecurityTokenService).Result;
            });

            return _generalResponse.Response(_pagination.Paginate(response.Item1, response.Item2, pageNumber, pageSize), StatusCodes.Status200OK, "Request list successfully loaded.", true);
        }

        public async Task<GeneralModel> GetAccreditationStats(int companyId, string bound)
        {
            var response = new object();

            if (bound == "incoming")
            {
                response = await _requestData.GetStatusStatisticsInbound(companyId);
            }
            else if (bound == "outgoing")
            {
                response = await _requestData.GetStatusStatisticsOutbound(companyId);
            }
            else
            {
                _generalResponse.Response(null, StatusCodes.Status404NotFound, "Invalid bound value", false);
            }

            return _generalResponse.Response(response, StatusCodes.Status200OK, "Accreditation statistics has been retrieved!", true);
        }

        public async Task<GeneralModel> CreateRequest(List<RequestModel> requestInfo)
        {
            //Validate Request 
            var validatorResponse = await _validatorCreateRequest.ValidateAsync(requestInfo);
            if (!validatorResponse.IsValid)
            {
                var errors = validatorResponse.Errors.Select(error => new ErrorField(error.PropertyName, error.ErrorMessage)).ToList();
                return _generalResponse.Response(null, errors, StatusCodes.Status400BadRequest, "Error encoutered", true);
            }

            //For Trucking
            ////Create Empty Truck Area 
            //xgca.entity.Models.TruckArea truckAreaInfo = new xgca.entity.Models.TruckArea(); ;

            //if (serviceRole.ToLower() == "trucking")
            //{
            //    truckAreaInfo = new xgca.entity.Models.TruckArea
            //    {
            //        RequestId = 0,
            //        CountryId = customerRegistrationDTO.countryId,
            //        CountryName = customerRegistrationDTO.countryName,
            //        StateId = customerRegistrationDTO.stateId,
            //        StateName = customerRegistrationDTO.stateName,
            //        CityId = customerRegistrationDTO.cityId,
            //        CityName = customerRegistrationDTO.cityName,
            //        PostalId = "-",
            //        PostalCode = customerRegistrationDTO.zipCode,
            //        Latitude = customerRegistrationDTO.latitude,
            //        Longitude = customerRegistrationDTO.longitude
            //    };
            //}

            ////Create Request
            //requestList[0].TruckArea.Add(truckAreaInfo);


            //Shipping Agency Port Area


            var requestList = _mapper.Map<List<xgca.entity.Models.Request>>(requestInfo);
            requestList.ForEach(i => { i.AccreditationStatusConfigId = 1; });

            var response = await _requestData.CreateRequest(requestList);    
            return _generalResponse.Response(response, StatusCodes.Status200OK, "Request has been submitted.", true);
        }

        public async Task<GeneralModel> DeleteAccreditaitonRequest(List<Guid> requestIds)
        {
            await _requestData.DeleteRequest(requestIds);
            return _generalResponse.Response(null, StatusCodes.Status200OK, "Request deletion has been succesful!", true);
        }

        public async Task<GeneralModel> ActivateDeactivateRequest(List<Guid> requestIds, bool status)
        {
            var response = await _requestData.ActivateDeactivateRequest(requestIds, status);
            return _generalResponse.Response(null, StatusCodes.Status200OK, "Request status has been updated.", true);
        }

        public async Task<GeneralModel> UpdateRequestStatusBulk(int companyId, List<Guid> requestId, int status)
        {
            //Check if request exist
            foreach (var request in requestId)
            {
                if (await _requestData.ValidateIfRequestStatusUpdateIsAllowed(request, companyId) != null)
                {
                    await _requestData.UpdateAccreditationRequest(request, companyId, status);
                }
                else
                {
                    return _generalResponse.Response(null, StatusCodes.Status404NotFound, "Selected accreditation requet does not exist!", false);
                }
            }
            //update Accreditation Status

            return _generalResponse.Response(null, StatusCodes.Status200OK, "Status has been updated", true);
        }

        public async Task<int> GetRequestIdByGuid(string guid)
        {
            int requestId = await _requestData.GetRequestIdByGuid(Guid.Parse(guid));
            return requestId;
        }



        public async Task<byte[]> ExportRequestListToCSV(string bound, int pageSize, int pageNumber, Guid companyGuid, Guid serviceRoleGuid, string companyName, string companyAddress, string companyCountryName, string companyStateCityName, string portAreaResponsibility, string truckAreaResponsibility, int accreditationStatusConfigId, byte? companyStatus, string sortOrder, string sortBy, string quickSearch)
        {            
            string fileName = String.Concat(Directory.GetCurrentDirectory(), @"request_exportCSV.csv");
            if (System.IO.File.Exists(fileName)) System.IO.File.Delete(fileName);

            var data = await _requestData.GetRequestList(bound, pageSize, pageNumber, companyGuid, serviceRoleGuid, companyName, companyAddress, companyCountryName, companyStateCityName, portAreaResponsibility, truckAreaResponsibility, accreditationStatusConfigId, companyStatus, sortOrder, sortBy, quickSearch);

            MemoryStream ms = new MemoryStream();
            using (StreamWriter sw = new StreamWriter(ms, Encoding.UTF8))
            {
                CsvWriter cw = new CsvWriter(sw, System.Globalization.CultureInfo.CurrentCulture);

                cw.WriteHeader<ExportRequestCSVModel>();
                cw.NextRecord();

                foreach (var p in data.Item1)
                {
                    var request = new ExportRequestCSVModel
                    {
                           CompanyName = p.CompanyName 
                         , CompanyFullAddress = p.CompanyFullAddress 
                         , CompanyCountryName = p.CompanyCountryName
                         , CompanyStateCityName = p.CompanyStateCityName 
                         , PortAreaList = p.PortAreaList 
                         , TruckAreaList = p.TruckAreaList 
                         , RequestStatus = p.AccreditationStatusConfigDescription
                    };

                    cw.WriteRecord(request);
                    cw.NextRecord();
                }
                sw.Flush();
            }

            ms.Close();
            byte[] profileToExport = ms.ToArray();
            return profileToExport;
        }

        public async Task<byte[]> ExportRequestListToCSVTemplate()
        {
            string fileName = String.Concat(Directory.GetCurrentDirectory(), @"Country_ExportListing.csv");
            if (System.IO.File.Exists(fileName)) System.IO.File.Delete(fileName);

            var data = new List<GetRequestModel>();
            data.Add(new GetRequestModel { });

            MemoryStream ms = new MemoryStream();
            using (StreamWriter sw = new StreamWriter(ms, Encoding.UTF8))
            {
                CsvWriter cw = new CsvWriter(sw, System.Globalization.CultureInfo.CurrentCulture);

                cw.WriteHeader<GetRequestModel>();
                cw.NextRecord();

                foreach (var p in data)
                {
                    var request = new GetRequestModel
                    {
                         AccreditationStatusConfigDescription = p.AccreditationStatusConfigDescription 
                         , CompanyName = p.CompanyName 
                         , CompanyFullAddress = p.CompanyFullAddress 
                    };

                    cw.WriteRecord(request);
                    cw.NextRecord();
                }
                sw.Flush();
            }

            ms.Close();
            byte[] profileToExport = ms.ToArray();
            return profileToExport;
        }

        #endregion

        #region CustomerAccreditation
        //Shipping/Cosignee = Requestor
        public async Task<dynamic> CreateCustomerAccreditation(CustomerRegistrationDTO customerRegistrationDTO, int companyId, string username, string serviceRole, string serviceRoleId)
        {
            if (! await _companyData.CheckIfExistsByCompanyName(customerRegistrationDTO.companyName))
            {
                return _generalResponse.Response(null, StatusCodes.Status404NotFound, "Error on Accreditation: Existing Company", false);
            }

            //New Company Registration
            var companyGuid = await RegisterCompany(customerRegistrationDTO);

            if (companyGuid != String.Empty)
            {
                List<xgca.entity.Models.Request> requestList = new List<xgca.entity.Models.Request>();
                var fetchedCompanyId = await _companyData.GetGuidById(companyId);
                var request = new RequestModel()
                {
                    ServiceRoleIdFrom = Guid.Parse(customerRegistrationDTO.serviceRoleId),
                    CompanyIdFrom = Guid.Parse(companyGuid),
                    ServiceRoleIdTo = Guid.Parse(serviceRoleId),
                    CompanyIdTo = Guid.Parse(fetchedCompanyId.ToString())
                };

                var requestInfo = _mapper.Map<xgca.entity.Models.Request>(request);
                requestInfo.IsActive = true;
                requestInfo.AccreditationStatusConfigId = 2; //1 - New, 2 - Accepted, 3 - Rejected
                requestList.Add(requestInfo);

                var result = await _requestData.CreateRequest(requestList);

                //Update Accredited By 
                await _companyData.SetAccreditedBy(companyGuid, fetchedCompanyId.ToString(), 0);
            }

            return _generalResponse.Response(null, StatusCodes.Status200OK, "Company Successfully Registered & Accredited", true);
        }

        public async Task<string> RegisterCompany(CustomerRegistrationDTO customerRegistrationDTO)
        {
            string reqUrl = _authhConfig.Value.BasePath + _authhConfig.Value.CustomerRegistration;
            var response = await _httpHelper.PostAsync(reqUrl, String.Empty, customerRegistrationDTO, null);

            if (response.statusCode != StatusCodes.Status200OK)
            {
                return String.Empty;
            }

            string companyId = response?.data["companyGuid"];
            return companyId;
        }

        public async Task<GeneralModel> PortOfResponsibilityAccreditedCustomer(ListPortOfResponsibility obj)
        {
            if (obj == null)
            {
                return _generalResponse.Response(null, StatusCodes.Status400BadRequest, "Company Id is required!", false);
            }

            ReturnPortOfResponsibility returnPortOfResponsibility = new ReturnPortOfResponsibility();

            var dischargeLine = await _requestData.PortOfResponsibilityAccreditedCustomer(obj.CompanyId, obj.PortOfDischargeId);
            var loadingLine = await _requestData.PortOfResponsibilityAccreditedCustomer(obj.CompanyId, obj.PortOfLoadingId);

            return _generalResponse.Response(new { companies = new ReturnPortOfResponsibility { PortOfDischargeCompanyId = dischargeLine?.CompanyIdFrom.ToString(), PortOfLoadingCompanyId = loadingLine?.CompanyIdFrom.ToString() } }, StatusCodes.Status200OK, "Accredited Companies has been Listed", true);
        }

       
        #endregion
    }
}
