using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.Extensions.Options;
using xgca.core.Response;
using xgca.core.Models.CompanyStructure;
using xgca.data.Repositories;
using AutoMapper;
using System.Threading.Tasks;
using xgca.data.Company;
using xgca.core.Constants;
using xgca.data.User;

namespace xgca.core.Services
{
    public interface ICompanyStructureService
    {
        Task<IGeneralModel> CreateCompanyStructure(CreateCompanyStructureModel obj);
        Task<IGeneralModel> UpdateCompanyStructure(UpdateCompanyStructureModel obj);
        Task<IGeneralModel> GetCompanyStrucureDetailsByCompanyProfile();
        Task<IGeneralModel> GetCompanyStrucureDetailsByCompanyId(int companyId);
    }
    public class CompanyStructureService : ICompanyStructureService
    {
        private readonly IMapper _mapper;
        private readonly ICompanyStructureRepository _repository;
        private readonly IGeneral _general;
        private readonly ICompanyData _companyRepository;
        private readonly IUserData _userRepository;

        public CompanyStructureService(IMapper _mapper, ICompanyStructureRepository _repository, IGeneral _general, ICompanyData _companyRepository, IUserData _userRepository)
        {
            this._mapper = _mapper;
            this._repository = _repository;
            this._general = _general;
            this._companyRepository = _companyRepository;
            this._userRepository = _userRepository;
        }

        public async Task<IGeneralModel> CreateCompanyStructure(CreateCompanyStructureModel obj)
        {
            if (obj is null)
            {
                return _general.Response(null, 400, "Data cannot be null", false);
            }

            GlobalVariables.LoggedInUserId = await _userRepository.GetIdByUsername(GlobalVariables.LoggedInUsername);

            var createModel = _mapper.Map<entity.Models.CompanyStructure>(obj);
            var (returnObj, message) = await _repository.Create(createModel);

            int statusCode = (returnObj is null) ? 400 : 200;
            bool isSuccessful = (returnObj is null) ? false : true;

            var displayModel = new GetCompanyStructureModel();
            if (!(returnObj is null))
            {
                displayModel = _mapper.Map<GetCompanyStructureModel>(returnObj);
            }

            return _general.Response(new { CompanyStructure = displayModel }, statusCode, message, isSuccessful);
        }

        public async Task<IGeneralModel> GetCompanyStrucureDetailsByCompanyId(int companyId)
        {
            var (returnObj, message) = await _repository.GetByCompanyId(companyId);

            int statusCode = (returnObj is null) ? 400 : 200;
            bool isSuccessful = (returnObj is null) ? false : true;

            var displayModel = new GetCompanyStructureModel();
            if (!(returnObj is null))
            {
                displayModel = _mapper.Map<GetCompanyStructureModel>(returnObj);
            }

            return _general.Response(new { CompanyStructure = displayModel }, statusCode, message, isSuccessful);
        }

        public async Task<IGeneralModel> GetCompanyStrucureDetailsByCompanyProfile()
        {
            var (returnObj, message) = await _repository.GetByCompanyId(GlobalVariables.LoggedInCompanyId);

            int statusCode = (returnObj is null) ? 400 : 200;
            bool isSuccessful = (returnObj is null) ? false : true;

            var displayModel = new GetCompanyStructureModel();
            if (!(returnObj is null))
            {
                displayModel = _mapper.Map<GetCompanyStructureModel>(returnObj);
            }

            return _general.Response(new { CompanyStructure = displayModel }, statusCode, message, isSuccessful);
        }

        public async Task<IGeneralModel> UpdateCompanyStructure(UpdateCompanyStructureModel obj)
        {
            if (obj is null)
            {
                return _general.Response(null, 400, "Data cannot be null", false);
            }

            GlobalVariables.LoggedInUserId = await _userRepository.GetIdByUsername(GlobalVariables.LoggedInUsername);

            var updateModel = _mapper.Map<entity.Models.CompanyStructure>(obj);
            var (returnObj, message) = await _repository.Update(updateModel);

            int statusCode = (returnObj is null) ? 400 : 200;
            bool isSuccessful = (returnObj is null) ? false : true;

            var displayModel = new GetCompanyStructureModel();
            if (!(returnObj is null))
            {
                displayModel = _mapper.Map<GetCompanyStructureModel>(returnObj);
            }

            return _general.Response(new { CompanyStructure = displayModel }, statusCode, message, isSuccessful);
        }
    }
}
