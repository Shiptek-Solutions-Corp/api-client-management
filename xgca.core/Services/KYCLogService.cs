using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using xgca.core.Response;
using xgca.core.Models.KYCLog;
using xgca.data.Repositories;
using AutoMapper;

namespace xgca.core.Services
{
    public interface IKYCLogService
    {
        Task<IGeneralModel> BulkCreateKYCLogs(BulkCreateKYCLogModel bulkObj);
        Task<IGeneralModel> CreateKYCLogs(CreateKYCLogModel obj);
        Task<IGeneralModel> GetLatestLog(int companySectionsId, int companyId);
        Task<IGeneralModel> GetLatestLogs(List<int> companySectionsId, int companyId);
    }
    public class KYCLogService : IKYCLogService
    {
        private readonly IKYCLogRepository _repository;
        private readonly IGeneral _general;
        private readonly IMapper _mapper;

        public KYCLogService(IKYCLogRepository _repository, IGeneral _general, IMapper _mapper)
        {
            this._repository = _repository;
            this._general = _general;
            this._mapper = _mapper;
        }

        public async Task<IGeneralModel> BulkCreateKYCLogs(BulkCreateKYCLogModel bulkObj)
        {
            if (bulkObj.KYCLogs.Count == 0)
            {
                return _general.Response(null, 400, "Data cannot be null", false);
            }

            var createBulkModel = new List<entity.Models.KYCLog>();
            bulkObj.KYCLogs.ForEach(e =>
            {
                createBulkModel.Add(_mapper.Map<entity.Models.KYCLog>(e));
            });

            var (returnList, message) = await _repository.BulkCreate(createBulkModel);
            if (returnList.Count == 0)
            {
                return _general.Response(null, 400, message, false);
            }

            var displayListModel = new List<GetKYCLogModel>();
            returnList.ForEach(e =>
            {
                displayListModel.Add(_mapper.Map<GetKYCLogModel>(e));
            });

            return _general.Response(new { KYLogs = displayListModel }, 200, message, true);
        }

        public async Task<IGeneralModel> CreateKYCLogs(CreateKYCLogModel obj)
        {
            if (obj is null)
            {
                return _general.Response(null, 400, "Data cannot be null", false);
            }

            var createModel = _mapper.Map<entity.Models.KYCLog>(obj);
            var (returnObj, message) = await _repository.Create(createModel);

            if (returnObj is null)
            {
                return _general.Response(null, 400, message, false);
            }

            var displayModel = _mapper.Map<GetKYCLogModel>(returnObj);
            return _general.Response(new { KYCLog = displayModel }, 200, message, true);
        }

        public async Task<IGeneralModel> GetLatestLog(int companySectionsId, int companyId)
        {
            var (returnObj, message) = await _repository.Get(companySectionsId, companyId);
            if (returnObj is null)
            {
                return _general.Response(null, 400, message, false);
            }

            var displayModel = _mapper.Map<GetKYCLogModel>(returnObj);
            return _general.Response(new { KYCLog = displayModel }, 200, message, true);
        }

        public async Task<IGeneralModel> GetLatestLogs(List<int> companySectionsId, int companyId)
        {
            if (companySectionsId.Count == 0)
            {
                return _general.Response(null, 400, "Company Section Ids required", false);
            }

            var listModel = new List<GetKYCLogModel>();
            foreach(int i in companySectionsId)
            {
                var (returnObj, message) = await _repository.Get(i, companyId);
                if (returnObj is null)
                {
                    continue;
                }

                listModel.Add(_mapper.Map<GetKYCLogModel>(returnObj));
            }

            if (listModel.Count == 0)
            {
                return _general.Response(null, 400, "No KYC Logs found", false);
            }

            return _general.Response(new { KYCLogs = listModel }, 200, "KYC Logs retrieved", true);
        }
    }
}
