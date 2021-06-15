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

namespace xgca.core.Services
{
    public interface ICompanyDirectorService
    {
        Task<IGeneralModel> ProcessCompanyDirectors(List<GetCompanyDirectorModel> directors);
    }
    public class CompanyDirectorService : ICompanyDirectorService
    {
        private readonly IMapper _mapper;
        private readonly ICompanyDirectorRepository _repository;
        private IGeneral _general;

        public CompanyDirectorService(IMapper _mapper, ICompanyDirectorRepository _repository, IGeneral _general)
        {
            this._mapper = _mapper;
            this._repository = _repository;
            this._general = _general;
        }

        public async Task<IGeneralModel> ProcessCompanyDirectors(List<GetCompanyDirectorModel> directors)
        {
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

            var (createResult, createMessage) = await _repository.BulkCreate(newDirectors);
            var (updateResult, updateMessage) = await _repository.BulkUpdate(updateDirectors);
            var (deleteResult, deleteMessage) = await _repository.BulkDelete(deleteDirectors, GlobalVariables.LoggedInUsername);

            var listDirectorsModel = new List<GetCompanyDirectorModel>();
            if (!(createResult is null))
            {
                createResult.ForEach(e =>
                {
                    listDirectorsModel.Add(_mapper.Map<GetCompanyDirectorModel>(e));
                });
            }
            if (!(updateResult is null))
            {
                updateResult.ForEach(e =>
                {
                    listDirectorsModel.Add(_mapper.Map<GetCompanyDirectorModel>(e));
                });
            }

            return _general.Response(new { CompanyDirectors = listDirectorsModel }, 200, "Company Directors retrieved", true);
        }
    }
}
