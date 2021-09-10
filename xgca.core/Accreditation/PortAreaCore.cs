using AutoMapper;
using Microsoft.AspNetCore.Http;
using CsvHelper;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using xas.core._Helpers;
using xas.core._Helpers.IOptionModels;
using xas.core._ResponseModel;
using xas.core.PortArea.DTO;
using xas.core.Request;
using xas.data._IOptionsModel;
using xas.data.DataModel.PortArea;
using xgca.core.Response;
using xas.data.accreditation.Request;
using xgca.core.Helpers.Http;
using xgca.data.Company;
using xgca.data.ViewModels.PortArea;
using xgca.core.Models.Accreditation.PortArea;
using xgca.core.Helpers;
using FluentValidation;
using System.Linq;
using xgca.core.Validators.Request;

namespace xas.core.PortArea
{
    public interface IPortAreaCore
    {
        Task<GeneralModel> AddPortOfResponsibility(List<CreatePortAreaModel> portInfoList);
        Task<GeneralModel> GetListofPorts(Guid requestId);
        Task<byte[]> GenerateExcelFile(Guid requestId);
        Task<GeneralModel> RemovePortResponsibility(Guid portAreaId);
        Task<byte[]> GenerateCSVFile(Guid requestId, string CSVtype = "");
        Task<GeneralModel> GetPortList(string portName, string countryName, string unloCode);
       // Task<GeneralModel> GetListofPortsCSVFormat(Guid requestId);
    }
    public class PortAreaCore : IPortAreaCore
    {
        private readonly IPortAreaData _portAreaData;
        private readonly IGeneralResponse _generalResponse;
        private readonly IMapper _mapper;
        private readonly IHttpHelper _httpHelper;
        private readonly IOptions<GlobalCmsService> _options;
        private readonly IValidator<List<CreatePortAreaModel>> _validatorCreatePortAreaResponsibility;

        public PortAreaCore(IPortAreaData portAreaData, IGeneralResponse generalResponse,  IMapper mapper, IHttpHelper httpHelper, IOptions<GlobalCmsService> options, IValidator<List<CreatePortAreaModel>> validatorCreatePortAreaResponsibility)
        {
            _portAreaData = portAreaData;
            _generalResponse = generalResponse;
            _mapper = mapper;
            _httpHelper = httpHelper;
            _options = options;
            _validatorCreatePortAreaResponsibility = validatorCreatePortAreaResponsibility;
        }

        public async Task<GeneralModel> AddPortOfResponsibility(List<CreatePortAreaModel> portInfoList)
        {

            //Validate Request 
            var validatorResponse = await _validatorCreatePortAreaResponsibility.ValidateAsync(portInfoList);
            if (!validatorResponse.IsValid)
            {
                var errors = validatorResponse.Errors.Select(error => new ErrorField(error.PropertyName, error.ErrorMessage)).ToList();
                return _generalResponse.Response(null, errors, StatusCodes.Status400BadRequest, "Error encoutered", true);
            }


            var lstPorts = _mapper.Map<List<xgca.entity.Models.PortArea>>(portInfoList);
            var response = await _portAreaData.AddPortResponsibility(lstPorts);

            return _generalResponse.Response(response, StatusCodes.Status200OK, "Successfully Added Port", true);
        }

        public async Task<byte[]> GenerateCSVFile(Guid requestId, string CSVtype = "")
        {
            string fileName = String.Concat(Directory.GetCurrentDirectory(), @"portarea_list.csv");
            if (File.Exists(fileName)) File.Delete(fileName);

            var data = await _portAreaData.GetPortList(requestId);
            if (CSVtype == "TEMPLATE") data.RemoveAll(i => i.PortId != null);


            MemoryStream ms = new MemoryStream();
            using (StreamWriter sw = new StreamWriter(ms, Encoding.UTF8))
            {
                CsvWriter cw = new CsvWriter(sw, System.Globalization.CultureInfo.CurrentCulture);

                cw.WriteHeader<ExportPortAreaModel>();
                cw.NextRecord();

                foreach (var p in data)
                {
                    var profile = new ExportPortAreaModel
                    {
                        CityName = p.CityName 
                        , CountryCode = p.CountryCode 
                        , CountryName = p.CountryName 
                        , Latitude = p.Latitude
                        , Location = p.Location 
                        , LoCode = p.LoCode 
                        , Longitude = p.Longitude 
                        , PortName = p.PortName
                        , PortOfDischarge = p.PortOfDischarge                               
                        , PortOfLoading = p.PortOfLoading
                    };

                    cw.WriteRecord(profile);
                    cw.NextRecord();
                }
                sw.Flush();
            }

            ms.Close();
            byte[] portAreas = ms.ToArray();
            return portAreas;
        }

        public async Task<GeneralModel> RemovePortResponsibility(Guid portAreaId)
        {
            var response = await _portAreaData.RemovePortResponsibility(portAreaId);
            return _generalResponse.Response(response, StatusCodes.Status200OK, "Successfully deleted port", true);
        }

        public async Task<byte[]> GenerateExcelFile(Guid requestId)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var data = await _portAreaData.GetPortList(requestId); 

            using (ExcelPackage pack = new ExcelPackage())
            {
                var workSheet = pack.Workbook.Worksheets.Add("Ports");

                ////Fill data            
                workSheet.Cells["A1"].LoadFromCollection(data, true, OfficeOpenXml.Table.TableStyles.Medium1);
                return await pack.GetAsByteArrayAsync();
            }
        }

        public async Task<GeneralModel> GetListofPorts(Guid requestId)
        {
            var response = await _portAreaData.GetPortList(requestId);
            return _generalResponse.Response(response, StatusCodes.Status200OK, "List of ports has been loaded", true);
        }

        public async Task<GeneralModel> GetPortList(string portName, string countryName, string unloCode)
        {
            string url = String.Concat(_options.Value.BaseUrl, _options.Value.PortSearch, "?portName=", portName, "&countryName=", countryName, "&unloCode=", unloCode);
            var response = await _httpHelper.GetAsync(url, "");
            return _generalResponse.Response(response.data, StatusCodes.Status200OK, "Port list has been listed.", true);
        }
        //public async Task<GeneralModel> GetListofPortsCSVFormat(Guid requestId)
        //{
        //    var data = await _portAreaData.GetPortList(requestId);
        //    var newResponse = new List<CSVPortAreaDTO>();

        //    foreach (var ports in data)
        //    {
        //        var portDetails = (JObject)await _httpHelper.Get(_optionsGlobal.Value.BasePath +
        //        _optionsGlobal.Value.PortDetailsBulk.Replace("{portIdsCSV}", ports.PortId.ToString()), null, _optionsToken.Value.GetToken.Split(" ")[1].ToString());

        //        newResponse.Add(new CSVPortAreaDTO
        //        {
        //            //PortAreaId = ports.Guid,
        //            //PortId = Guid.Parse(portDetails["data"]["ports"][0]["portId"].ToString()),
        //            CountryCode = portDetails["data"]["ports"][0]["countryCode"].ToString(),
        //            CountryName = portDetails["data"]["ports"][0]["countryName"].ToString(),
        //            CityName = portDetails["data"]["ports"][0]["cityName"].ToString(),
        //            LoCode = portDetails["data"]["ports"][0]["locode"].ToString(),
        //            Name = portDetails["data"]["ports"][0]["name"].ToString(),
        //            Location = portDetails["data"]["ports"][0]["location"].ToString(),
        //            Latitude = portDetails["data"]["ports"][0]["latitude"].ToString(),
        //            Longitude = portDetails["data"]["ports"][0]["longitude"].ToString(),
        //            CountryAreaId = ports.CountryAreaId,
        //            PortOfDischarge = ports.PortOfDischarge == 1 ? "yes" : "no",
        //            PortOfLoading = ports.PortOfLoading == 1 ? "yes" : "no"
        //        });
        //    }
        //    return _generalResponse.Response(newResponse, StatusCodes.Status200OK, "List of ports has been added", true);
        //}
    }
}
