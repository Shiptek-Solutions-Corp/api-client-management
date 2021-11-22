using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.Extensions.Options;
using xgca.core.Response;
using xgca.core.Models.CompanyDirector;
using xgca.data.Repositories;
using AutoMapper;
using System.Threading.Tasks;
using xgca.data.Company;
using xgca.core.Constants;
using xgca.entity.Models;
using xgca.data.User;

namespace xgca.core.Services
{
    public interface ICompanyDirectorService
    {
        Task<IGeneralModel> ProcessCompanyDirectors(List<GetCompanyDirectorModel> directors);
        Task<IGeneralModel> GetByCompanyId(int companyId);
    }
    public class CompanyDirectorService : ICompanyDirectorService
    {
        private readonly IMapper _mapper;
        private readonly ICompanyDirectorRepository _repository;
        private IGeneral _general;
        private readonly IUserData _userRepository;

        public CompanyDirectorService(IMapper _mapper, ICompanyDirectorRepository _repository, IGeneral _general, IUserData _userRepository)
        {
            this._mapper = _mapper;
            this._repository = _repository;
            this._general = _general;
            this._userRepository = _userRepository;
        }

        public async Task<IGeneralModel> GetByCompanyId(int companyId)
        {
            //GlobalVariables.LoggedInUserId = await _userRepository.GetIdByUsername(GlobalVariables.LoggedInUsername);

            var (directors, message) = await _repository.GetByCompanyId(companyId);

            var companyDirectorSubSection = new GetCompanyDirectorSubSection();
            var companyDirectors = new List<GetCompanyDirectorModel>();

            if (!(directors is null))
            {
                directors.ForEach(e =>
                {
                    companyDirectors.Add(_mapper.Map<GetCompanyDirectorModel>(e));
                });
            }
            companyDirectorSubSection.Directors = companyDirectors;

            return _general.Response(companyDirectorSubSection, 200, message, true);
        }

        public async Task<IGeneralModel> ProcessCompanyDirectors(List<GetCompanyDirectorModel> directors)
        {
            GlobalVariables.LoggedInUserId = await _userRepository.GetIdByUsername(GlobalVariables.LoggedInUsername);

            var newDirectors = new List<CompanyDirectors>();
            var updateDirectors = new List<CompanyDirectors>();
            var deleteDirectors = new List<string>();

            foreach(var director in directors)
            {
                if (director.Id.Equals("NEW"))
                {
                    var tempCreateDirectorModel = _mapper.Map<CreateCompanyDirectorModel>(director);
                    var createDirectorModel = _mapper.Map<CompanyDirectors>(tempCreateDirectorModel);
                    newDirectors.Add(createDirectorModel);
                }
                else
                {
                    if (director.IsUpdated)
                    {
                        var tempUpdateDirectorModel = _mapper.Map<UpdateCompanyDirectorModel>(director);
                        var updateDirectorModel = _mapper.Map<CompanyDirectors>(tempUpdateDirectorModel);
                        updateDirectors.Add(updateDirectorModel);
                    }
                    else if (director.IsDeleted)
                    {
                        deleteDirectors.Add(director.Id);
                    }
                }
            }

            if (newDirectors.Count != 0)
            {
                var (createResult, createMessage) = await _repository.BulkCreate(newDirectors);
            }
            
            if (updateDirectors.Count != 0)
            {
                foreach (var u in updateDirectors)
                {
                    var (updateResult, updateMessage) = await _repository.Update(u);
                }
            }

            var (deleteResult, deleteMessage) = await _repository.BulkDelete(deleteDirectors, GlobalVariables.LoggedInUsername);

            var (companyDirectors, message) = await _repository.GetByCompanyId(GlobalVariables.LoggedInCompanyId);

            var listDirectorsModel = new List<GetCompanyDirectorModel>();
            if (!(companyDirectors is null))
            companyDirectors.ForEach(e =>
            {
                listDirectorsModel.Add(_mapper.Map<GetCompanyDirectorModel>(e));
            });

            return _general.Response(new { CompanyDirectors = listDirectorsModel }, 200, "Company Directors retrieved", true);
        }
    }
}
