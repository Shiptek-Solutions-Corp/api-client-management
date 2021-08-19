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

namespace xas.core.accreditation.Request
{
    public interface IRequestCore
    {
        #region Request
        Task<GeneralModel> CreateRequest(List<RequestModel> requestInfo);
        Task<GeneralModel> UpdateRequestStatus(int companyId, Guid requestId, int status);
        //Task<GeneralModel> GetAccreditationRequest(string bound, GetAccreditationRequestDTO paramData);
        //Task<GeneralModel> GetAccreditationRequestCSVFormat(string bound, GetAccreditationRequestDTO paramData);
        Task<GeneralModel> GetAccreditationStats(int companyId, string bound);
        Task<GeneralModel> DeleteAccreditaitonRequest(Guid requestId);
        Task<GeneralModel> DeleteAccreditaitonRequestBulk(List<Guid> requestId);
        Task<GeneralModel> UpdateRequestStatusBulk(int companyId, List<Guid> requestId, int status);
        Task<byte[]> GenerateExcelFile(List<ResponseDTO> companyList);
        Task<byte[]> GenerateCSVFile(List<CSVResponseDTO> companyList, string CSVtype = "");
        Task<int> GetRequestId(string companyIdFrom, string companyIdTo);
        Task<int> GetRequestIdByGuid(string guid);
        Task<GeneralModel> GetTruckingAccreditationRequest(int companyId, string bound, string serviceRoleId, string quicksearch, string company, string address, string truckArea, string orderBy, bool isDescending, int status, int pageNumber, int pageSize);
        Task<byte[]> ExportTruckingAccreditationRequest(int companyId, string bound, string serviceRoleId, string quicksearch, string company, string address, string truckArea, string orderBy, bool isDescending, int status, int pageNumber, int pageSize);
        Task<byte[]> ExportTruckingAccreditationRequestTemplate();
        Task<GeneralModel> GetAccreditationStats(int companyId, string bound, string serviceRoleId);
        #endregion

        #region Customer Accreditation
        Task<dynamic> CreateCustomerAccreditation(CustomerRegistrationDTO customerRegistrationDTO, int companyId, string username, string serviceRole, string serviceRoleId);
        #endregion
    }

    public class RequestCore : IRequestCore
    {
        private readonly IRequestData _requestData;
        private readonly IPortAreaData _portAreaData;
        private readonly IMapper _mapper;
        private readonly IGeneralResponse _generalResponse;
        private readonly IHttpHelper _httpHelper;
        private readonly IOptions<ClientManagement> _optionsClient;
        private readonly IOptions<ClientToken> _optionsToken;
        private readonly IOptions<GlobalCMS> _optionsGlobal;
        private readonly IPagedResponse _pageresponse;
        private readonly ITruckAreaData _truckAreaData;
        private readonly IValidator<List<RequestModel>> _validatorCreateRequest;
        private readonly ICompanyData _companyData;
        private readonly IOptions<AuthConfig> _authhConfig;


        public RequestCore(
            IRequestData requestData,
            IPortAreaData portAreaData,
            IMapper mapper,
            IGeneralResponse generalResponse,
            IHttpHelper httpHelper,
            IOptions<ClientManagement> optionsClient,
            IOptions<ClientToken> optionsToken,
            IOptions<GlobalCMS> optionsGlobal,
            IPagedResponse pageresponse,
            ITruckAreaData truckAreaData,
            IValidator<List<RequestModel>> validatorCreateRequest,
            ICompanyData companyData,
            IOptions<AuthConfig> authhConfig)
        {
            _requestData = requestData;
            _portAreaData = portAreaData;
            _mapper = mapper;
            _generalResponse = generalResponse;
            _httpHelper = httpHelper;
            _optionsClient = optionsClient;
            _optionsToken = optionsToken;
            _optionsGlobal = optionsGlobal;
            _pageresponse = pageresponse;
            _truckAreaData = truckAreaData;
            _validatorCreateRequest = validatorCreateRequest;
            _companyData = companyData;
            _authhConfig = authhConfig;
        }

        #region Request
        public async Task<GeneralModel> GetAccreditationStats(int companyId, string bound)
        {
            ////Get Company Guid ID from Client CMS
            //var objectResponse = (JObject)await _httpHelper.Get(_optionsClient.Value.BasePath +
            //    _optionsClient.Value.CompanyGuidById.Replace("{companyId}", companyId.ToString()), null, _optionsToken.Value.GetToken.Split(" ")[1].ToString());
            //var companyGuid = Guid.Parse(objectResponse["data"]["companyId"].ToString());

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

       
        //public async Task<GeneralModel> GetAccreditationRequest(string bound, GetAccreditationRequestDTO paramData)
        //{
        //ReBuildObject:
        //    #region LISTING
        //    //Get GUID ID of the logged on company from token
        //    var objectResponse = (JObject)await _httpHelper.Get(_optionsClient.Value.BasePath +
        //        _optionsClient.Value.CompanyGuidById.Replace("{companyId}", paramData.companyId.ToString()), null, _optionsToken.Value.GetToken.Split(" ")[1].ToString());


        //    //Transport Bound validations
        //    ICollection<xgca.entity.Models.Request> data = new List<xgca.entity.Models.Request>();
        //    if (bound == "incomming")
        //    {
        //        data = await _requestData.GetAllIncommingRequest(Guid.Parse(objectResponse["data"]["companyId"].ToString()));
        //    }
        //    else if (bound == "outgoing")
        //    {
        //        data = await _requestData.GetAllOutgoingRequest(Guid.Parse(objectResponse["data"]["companyId"].ToString()));
        //    }
        //    else
        //    {
        //        return _generalResponse.Response(null, 400, "Invalid bound value", false);
        //    }

        //    //Fetch all company ids
        //    List<string> outGoingCompanies = new List<string>();

        //    data.ToList().ForEach(t =>
        //    {
        //        var cidT = bound == "incomming" ? t.CompanyIdFrom.ToString() : t.CompanyIdTo.ToString();
        //        outGoingCompanies.Add(cidT);
        //    });

        //    //Get All Company Details
        //    var d = (JObject)(await _httpHelper.Post(_optionsClient.Value.BasePath + _optionsClient.Value.CompanyDetailsByGuids,
        //        _optionsToken.Value.GetToken.Split(" ")[1].ToString(),
        //        new
        //        {
        //            companyIDs = outGoingCompanies //[]Array List of companies
        //        })).Data;

        //    //SerializeToObject
        //    List<CompanyDTO> companyList = JsonConvert.DeserializeObject<List<CompanyDTO>>(JsonConvert.SerializeObject(d["companies"]));

        //    //Re Generate Response
        //    List<ResponseDTO> responseData = new List<ResponseDTO>();
        //    data.ToList().ForEach(t =>
        //    {
        //        var cid = bound == "incomming" ? t.CompanyIdFrom.ToString() : t.CompanyIdTo.ToString();
        //        var cMatch = companyList.Where(g => g.companyId == cid).FirstOrDefault();
        //        responseData.Add(new ResponseDTO
        //        {
        //            requestId = t.Guid,
        //            companyName = cMatch.companyName,
        //            fullAddress = cMatch.fullAddress,
        //            companyDetails = _mapper.Map<xas.core.Request.CompanyDetails>(cMatch),
        //            portAreas = _mapper.Map<List<PortAreaResponse>>(t.PortArea),
        //            status = t.AccreditationStatusConfigId == 1 ? "New" : t.AccreditationStatusConfigId == 2 ? "Approved" : t.AccreditationStatusConfigId == 3 ? "Rejected" : "Unknown",
        //        });
        //    });

        //    var isModified = false;

        //    //Setting PortArea Details
        //    foreach (var rDto in responseData)
        //    {
        //        foreach (var ports in rDto.portAreas)
        //        {
        //            var portDetails = (JObject)await _httpHelper.Get(_optionsGlobal.Value.BasePath +
        //            _optionsGlobal.Value.PortDetailsBulk.Replace("{portIdsCSV}", ports.PortId.ToString()), null, _optionsToken.Value.GetToken.Split(" ")[1].ToString());

        //            var portArr = portDetails["data"]["ports"];

        //            if (portDetails["data"]["ports"].Count() <= 0)
        //            {
        //                var portArea = await _portAreaData.RemovePortAccreditation(ports.PortId);
        //                isModified = true;
        //                continue;
        //            }

        //            ports.CountryCode = portDetails["data"]["ports"][0]["countryCode"].ToString();
        //            ports.LoCode = portDetails["data"]["ports"][0]["locode"].ToString();
        //            ports.Name = portDetails["data"]["ports"][0]["name"].ToString();
        //            ports.Location = portDetails["data"]["ports"][0]["location"].ToString();
        //            ports.Latitude = portDetails["data"]["ports"][0]["latitude"].ToString();
        //            ports.Longitude = portDetails["data"]["ports"][0]["longitude"].ToString();
        //            ports.CountryName = portDetails["data"]["ports"][0]["countryName"].ToString();
        //            ports.CityName = portDetails["data"]["ports"][0]["cityName"].ToString();
        //        }
        //    }

        //    if (isModified)
        //    {
        //        goto ReBuildObject;
        //    }

        //    #endregion
        //    #region SORTING AND SEARCH FEATURE
        //    //Filtering
        //    responseData = responseData.Select(i => new ResponseDTO
        //    {
        //        requestId = i.requestId,
        //        companyName = i.companyName,
        //        fullAddress = i.fullAddress,
        //        imageURL = i.imageURL,
        //        companyDetails = i.companyDetails,
        //        portAreas = i.portAreas.Where(a => a.CountryName.ToUpper().Contains(paramData.Coperating.ToUpper())
        //                                        && a.Name.ToUpper().Contains(paramData.CportResp.ToUpper())
        //                                        && a.LoCode.ToUpper().Contains(paramData.CLocode.ToUpper())
        //                                        && a.IsDeleted == 0).ToList(),
        //        status = i.status
        //    }
        //                                    ).Where(t => t.companyName.ToUpper().Contains(paramData.CcompanyName.ToUpper())
        //                                    && t.fullAddress.ToUpper().Contains(paramData.CfullAddress.ToUpper())
        //                                    && t.status.ToUpper().Contains(paramData.Cstatus.ToUpper())
        //                                    && (t.companyName + t.fullAddress + t.status + t.portAreas.Select(a => a.CountryName) + t.portAreas.Select(a => a.Name) + t.portAreas.Select(a => a.LoCode)).ToUpper().Contains(paramData.search.ToUpper())
        //                                    ).ToList();

        //    //TotalRecordCount
        //    int recordCount = responseData.Count();

        //    //Sorting
        //    if (paramData.sort.ToUpper() == "ASC") responseData = responseData.OrderBy(i => typeof(ResponseDTO).GetProperty(paramData.columnSort).GetValue(i).ToString()).ToList();
        //    if (paramData.sort.ToUpper() == "DESC") responseData = responseData.OrderByDescending(i => typeof(ResponseDTO).GetProperty(paramData.columnSort).GetValue(i).ToString()).ToList();
        //    #endregion
        //    #region PAGINATION
        //    //Pagination
        //    responseData = responseData.Skip(paramData.rowPerPage * paramData.pageNumber).Take(paramData.rowPerPage).ToList();

        //    //response
        //    var PageResponseData = _pageresponse.Paginate(responseData, recordCount, paramData.pageNumber, paramData.rowPerPage);
        //    return _generalResponse.Response(PageResponseData, 200, $"{bound} request has been listed!", true);
        //    #endregion
        //}

        public async Task<GeneralModel> DeleteAccreditaitonRequest(Guid requestId)
        {
            await _requestData.DeleteRequest(requestId);
            return _generalResponse.Response(null, StatusCodes.Status200OK, "Request deletion has been succesful!", true);
        }

        public async Task<GeneralModel> DeleteAccreditaitonRequestBulk(List<Guid> requestId)
        {
               //loop requestId
            foreach (var request in requestId)
            {
                await _requestData.DeleteRequest(request);
            }

            return _generalResponse.Response(null, StatusCodes.Status200OK, "Request deletion has been succesful!", true);
        }

        public async Task<GeneralModel> UpdateRequestStatus(int companyId, Guid requestId, int status)
        {
            //Check if request exist
            if (await _requestData.ValidateIfRequestStatusUpdateIsAllowed(requestId, companyId) != null)
            {
                await _requestData.UpdateAccreditationRequest(requestId, companyId, status);
            }
            else
            {
                return _generalResponse.Response(null, StatusCodes.Status404NotFound, "Selected accreditation requet does not exist!", false);
            }

            //update Accreditation Status
            return _generalResponse.Response(null, StatusCodes.Status200OK, "Status has been updated", true);
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

        public async Task<byte[]> GenerateExcelFile(List<ResponseDTO> companyList)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage pack = new ExcelPackage())
            {
                var worksheet = pack.Workbook.Worksheets.Add("companyList");

                string[] Header = { "NO.", "COMPANY NAME", "ADDRESS", "OPERATING COUNTRY", "PORT RESPONSIBILITY", "STATUS", "IMAGE URL" };
                int count = 1;
                foreach (var header in Header)
                {
                    worksheet.Cells[1, count].Value = header;
                    worksheet.Cells[1, count].Style.Font.Size = 12;
                    worksheet.Cells[1, count].Style.Font.Bold = true;
                    worksheet.Cells[1, count].Style.Border.Top.Style = ExcelBorderStyle.Hair;
                    count += 1;
                }
                int row = 2;
                int rowcount = 1;
                foreach (var resP in companyList)
                {
                    foreach (var ports in resP.portAreas)
                    {
                        worksheet.Cells[row, 4].Value += ports.CountryName + "\n";
                        worksheet.Cells[row, 5].Value += ports.Name + ports.LoCode + "\n";
                    }
                    worksheet.Cells[row, 1].Value = rowcount;
                    worksheet.Cells[row, 2].Value = resP.companyName;
                    worksheet.Cells[row, 3].Value = resP.fullAddress;
                    worksheet.Cells[row, 7].Value = resP.imageURL;
                    worksheet.Cells[row, 6].Value = resP.status;
                    row += 1;
                    rowcount += 1;
                }


                //worksheet.Cells.AutoFitColumns();

                return await pack.GetAsByteArrayAsync();
            }
        }

        public async Task<byte[]> GenerateCSVFile(List<CSVResponseDTO> companyList, string CSVtype = "")
        {
            string fileName = String.Concat(Directory.GetCurrentDirectory(), @"\DownloadedFiles\serviceCSV.csv");
            if (File.Exists(fileName)) File.Delete(fileName);

            using (StreamWriter sw = new StreamWriter(fileName, false, new UTF8Encoding(false)))
            {
                using (CsvWriter csvWriter = new CsvWriter(sw, CultureInfo.InvariantCulture))
                {
                    csvWriter.WriteHeader<CSVResponseDTO>();
                    csvWriter.NextRecord();
                    if (CSVtype == "CSV")
                    {
                        foreach (CSVResponseDTO resP in companyList)
                        {
                            csvWriter.WriteRecord<CSVResponseDTO>(resP);
                            csvWriter.NextRecord();
                        }
                    }
                }
                var wClient = new WebClient();
                byte[] arrayValu = wClient.DownloadData(fileName);
                return arrayValu;
            }
        }

        //public async Task<GeneralModel> GetAccreditationRequestCSVFormat(string bound, GetAccreditationRequestDTO paramData)
        //{
        //    #region LISTING
        //    //Get GUID ID of the logged on company from token
        //    var objectResponse = (JObject)await _httpHelper.Get(_optionsClient.Value.BasePath +
        //        _optionsClient.Value.CompanyGuidById.Replace("{companyId}", paramData.companyId.ToString()), null, _optionsToken.Value.GetToken.Split(" ")[1].ToString());


        //    //Transport Bound validations
        //    ICollection<xgca.entity.Models.Request> data = new List<xgca.entity.Models.Request>();
        //    if (bound == "incomming")
        //    {
        //        data = await _requestData.GetAllIncommingRequest(Guid.Parse(objectResponse["data"]["companyId"].ToString()));
        //    }
        //    else if (bound == "outgoing")
        //    {
        //        data = await _requestData.GetAllOutgoingRequest(Guid.Parse(objectResponse["data"]["companyId"].ToString()));
        //    }
        //    else
        //    {
        //        return _generalResponse.Response(null, 400, "Invalid bound value", false);
        //    }


        //    //Fetch all company ids
        //    List<string> outGoingCompanies = new List<string>();

        //    data.ToList().ForEach(t =>
        //    {
        //        var cidT = bound == "incomming" ? t.CompanyIdFrom.ToString() : t.CompanyIdTo.ToString();
        //        outGoingCompanies.Add(cidT);
        //    });

        //    //Get All Company Details
        //    var d = (JObject)(await _httpHelper.Post(_optionsClient.Value.BasePath + _optionsClient.Value.CompanyDetailsByGuids,
        //        _optionsToken.Value.GetToken.Split(" ")[1].ToString(),
        //        new
        //        {
        //            companyIDs = outGoingCompanies //[]Array List of companies
        //        })).Data;

        //    //SerializeToObject
        //    List<CompanyDTO> companyList = JsonConvert.DeserializeObject<List<CompanyDTO>>(JsonConvert.SerializeObject(d["companies"]));

        //    //Re Generate Response
        //    List<CSVResponseDTO> responseData = new List<CSVResponseDTO>();
        //    data.ToList().ForEach(t =>
        //    {
        //        var cid = bound == "incomming" ? t.CompanyIdFrom.ToString() : t.CompanyIdTo.ToString();
        //        var cMatch = companyList.Where(g => g.companyId == cid).FirstOrDefault();
        //        responseData.Add(new CSVResponseDTO
        //        {
        //            companyDetails = _mapper.Map<xas.core.Request.CSVCompanyDetails>(cMatch),
        //            status = t.AccreditationStatusConfigId == 1 ? "New" : t.AccreditationStatusConfigId == 2 ? "Approved" : t.AccreditationStatusConfigId == 3 ? "Rejected" : "Unknown"
        //        });
        //    });

        //    #endregion
        //    #region SORTING AND SEARCH FEATURE
        //    //Filtering
        //    responseData = responseData.Select(i => new CSVResponseDTO
        //    {
        //        companyDetails = i.companyDetails,
        //        status = i.status
        //    }).ToList();

        //    //TotalRecordCount
        //    int recordCount = responseData.Count();

        //    #endregion
        //    #region PAGINATION
        //    //Pagination
        //    responseData = responseData.Skip(paramData.rowPerPage * (paramData.pageNumber - 1)).Take(paramData.rowPerPage).ToList();

        //    //response
        //    var PageResponseData = _pageresponse.Paginate(responseData, recordCount, paramData.pageNumber, paramData.rowPerPage);
        //    return _generalResponse.Response(PageResponseData, 200, $"{bound} request has been listed!", true);
        //    #endregion
        //}

        public async Task<int> GetRequestId(string companyIdFrom, string companyIdTo)
        {
            int requestId = await _requestData.GetRequestId(companyIdFrom, companyIdTo);
            return requestId;
        }

        public async Task<int> GetRequestIdByGuid(string guid)
        {
            int requestId = await _requestData.GetRequestIdByGuid(Guid.Parse(guid));
            return requestId;
        }

        public async Task<GeneralModel> GetTruckingAccreditationRequest(int companyId, string bound, string serviceRoleId, string quicksearch, string company, string address, string truckArea, string orderBy, bool isDescending, int status, int pageNumber, int pageSize)
        {
            //Transport Bound validations
            ICollection data = new List<string>();
            int recordCount = 0;
            if (bound == "incoming")
            {
                var repoData = await _requestData.GetAllTruckingIncomingRequest(companyId, serviceRoleId, quicksearch, company, address, truckArea, orderBy, isDescending, status, pageNumber, pageSize);
                data = repoData.Item1;
                recordCount = repoData.Item2;
            }
            else if (bound == "outgoing")
            {
                var repoData = await _requestData.GetAllTruckingOutgoingRequest(companyId, serviceRoleId, quicksearch, company, address, truckArea, orderBy, isDescending, status, pageNumber, pageSize);
                data = repoData.Item1;
                recordCount = repoData.Item2;
            }
            else
            {
                return _generalResponse.Response(null, StatusCodes.Status400BadRequest, "Invalid Bound Value", false);
            }

 
            return _generalResponse.Response(_pageresponse.Paginate(data, recordCount, pageNumber, pageSize), StatusCodes.Status200OK, $"{bound} requests has been listed!", true);
        }

        public async Task<byte[]> ExportTruckingAccreditationRequest(int companyId, string bound, string serviceRoleId, string quicksearch, string company, string address, string truckArea, string orderBy, bool isDescending, int status, int pageNumber, int pageSize)
        {
            //Transport Bound validations
            ICollection data = new List<string>();
            int recordCount = 0;
            if (bound == "incoming")
            {
                var repoData = await _requestData.GetAllTruckingIncomingRequest(companyId, serviceRoleId, quicksearch, company, address, truckArea, orderBy, isDescending, status, pageNumber, pageSize);
                data = repoData.Item1;
            }
            else if (bound == "outgoing")
            {
                var repoData = await _requestData.GetAllTruckingOutgoingRequest(companyId, serviceRoleId, quicksearch, company, address, truckArea, orderBy, isDescending, status, pageNumber, pageSize);
                data = repoData.Item1;
            }

            var sezData = JsonConvert.SerializeObject(data);
            var desData = JsonConvert.DeserializeObject<List<TruckingResponseDTO>>(sezData);

            MemoryStream ms = new MemoryStream();
            using (StreamWriter sw = new StreamWriter(ms, Encoding.UTF8))
            {
                CsvWriter cw = new CsvWriter(sw, System.Globalization.CultureInfo.CurrentCulture);

                cw.WriteHeader<GetTruckingAccreditationRequestExport>();
                cw.NextRecord();

                foreach (var companyData in desData)
                {
                    var companyRecord = new GetTruckingAccreditationRequestExport
                    {
                        No = recordCount += 1,
                        Company = companyData.CompanyName,
                        Address = companyData.FullAddress,
                        AreaOfResponsibility = companyData.TruckArea,
                        Status = companyData.Status == 1.ToString() ? "New" : companyData.Status == 2.ToString() ? "Approved" : companyData.Status == 3.ToString() ? "Rejected" : "Unknown"
                    };

                    cw.WriteRecord(companyRecord);
                    cw.NextRecord();
                }
                sw.Flush();
            }

            ms.Close();
            byte[] truckingRequestsExport = ms.ToArray();
            return truckingRequestsExport;
        }
       
        public async Task<byte[]> ExportTruckingAccreditationRequestTemplate()
        {
            MemoryStream ms = new MemoryStream();
            using (StreamWriter sw = new StreamWriter(ms, Encoding.UTF8))
            {
                CsvWriter cw = new CsvWriter(sw, System.Globalization.CultureInfo.CurrentCulture);

                cw.WriteHeader<GetTruckingAccreditationRequestExport>();
                cw.NextRecord();

                sw.Flush();
            }

            ms.Close();
            byte[] truckingRequestsExport = ms.ToArray();
            return truckingRequestsExport;
        }

        public async Task<GeneralModel> GetAccreditationStats(int companyId, string bound, string serviceRoleId)
        {            
            var response = new object();

            if (bound == "incoming")
            {
                response = await _requestData.GetStatisticsInbound(companyId, serviceRoleId);
            }
            else if (bound == "outgoing")
            {
                response = await _requestData.GetStatisticsOutbound(companyId, serviceRoleId);
            }
            else
            {
                _generalResponse.Response(null, StatusCodes.Status400BadRequest, "Invalid bound value", false);
            }

            return _generalResponse.Response(response, StatusCodes.Status200OK, "Accreditation statistics has been retrieved!", true);
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
            var json = (JObject)response;

            if (Convert.ToInt32((json)["statusCode"]) != StatusCodes.Status200OK)
            {
                return String.Empty;
            }

            string companyId = (json)["data"]["companyGuid"].ToString();
            return companyId;
        }
        #endregion
    }
}
