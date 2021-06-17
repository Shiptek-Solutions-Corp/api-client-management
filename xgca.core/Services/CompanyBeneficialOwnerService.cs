using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.Extensions.Options;
using xgca.core.Response;
using xgca.core.Models.CompanyBeneficialOwner;
using xgca.data.Repositories;
using AutoMapper;
using System.Threading.Tasks;
using xgca.data.Company;
using xgca.core.Constants;
using xgca.entity.Models;
using xgca.data.User;

namespace xgca.core.Services
{
    public interface ICompanyBeneficialOwnerService
    {
        Task<IGeneralModel> ProcessCompanyBeneficialOwners(List<GetCompanyBeneficialOwnerModel> companies, List<GetIndividualBeneficialOwnerModel> individuals);
        Task<IGeneralModel> GetByCompanyId(int companyId);
    }
    public class CompanyBeneficialOwnerService : ICompanyBeneficialOwnerService
    {
        private readonly IMapper _mapper;
        private readonly ICompanyBeneficialOwnersRepository _repository;
        private readonly IGeneral _general;
        private readonly IUserData _userRepository;

        public CompanyBeneficialOwnerService(IMapper _mapper, ICompanyBeneficialOwnersRepository _repository, IGeneral _general, IUserData _userRepository)
        {
            this._mapper = _mapper;
            this._repository = _repository;
            this._general = _general;
            this._userRepository = _userRepository;
        }

        public async Task<IGeneralModel> GetByCompanyId(int companyId)
        {
            GlobalVariables.LoggedInUserId = await _userRepository.GetIdByUsername(GlobalVariables.LoggedInUsername);

            var (companies, individuals, message) = await _repository.GetByCompanyId(companyId);

            var companyBenficialOwnerSubSection = new GetCompanyBeneficialOwnerSubSection();
            var companyBeneficialOwner = new List<GetCompanyBeneficialOwnerModel>();
            var individualBeneficialOwner = new List<GetIndividualBeneficialOwnerModel>();

            if (!(companies is null))
            {
                companies.ForEach(e =>
                {
                    companyBeneficialOwner.Add(_mapper.Map<GetCompanyBeneficialOwnerModel>(e));
                });
            }

            if (!(individuals is null))
            {
                individuals.ForEach(e =>
                {
                    individualBeneficialOwner.Add(_mapper.Map<GetIndividualBeneficialOwnerModel>(e));
                });
            }

            companyBenficialOwnerSubSection.Companies = companyBeneficialOwner;
            companyBenficialOwnerSubSection.Individuals = individualBeneficialOwner;

            return _general.Response(companyBenficialOwnerSubSection, 200, message, true);
        }

        public async Task<IGeneralModel> ProcessCompanyBeneficialOwners(List<GetCompanyBeneficialOwnerModel> companies, List<GetIndividualBeneficialOwnerModel> individuals)
        {

            var newBeneficialOwners = new List<CompanyBeneficialOwners>();
            var updateBeneficialOwners = new List<CompanyBeneficialOwners>();
            var deleteBeneficialOwners = new List<Guid>();

            foreach (var company in companies)
            {
                if (company.Id.Equals("NEW"))
                {
                    var tempCreateCompanyModel = _mapper.Map<CreateCompanyBeneficialOwnerModel>(company);
                    var createCompanyModel = _mapper.Map<CompanyBeneficialOwners>(tempCreateCompanyModel);
                    newBeneficialOwners.Add(createCompanyModel);
                }
                else
                {
                    if (company.IsUpdate)
                    {
                        var tempUpdateCompanyModel = _mapper.Map<UpdateCompanyBeneficialOwnerModel>(company);
                        var updateCompanyModel = _mapper.Map<CompanyBeneficialOwners>(tempUpdateCompanyModel);
                        updateBeneficialOwners.Add(updateCompanyModel);
                    }
                    else if (company.IsDeleted)
                    {
                        deleteBeneficialOwners.Add(Guid.Parse(company.Id));
                    }
                }    
            }

            foreach (var individual in individuals)
            {
                if (individual.Id.Equals("NEW"))
                {
                    var tempCreateIndividualModel = _mapper.Map<CreateIndividualBeneficialOwnerModel>(individual);
                    var createIndividualModel = _mapper.Map<CompanyBeneficialOwners>(tempCreateIndividualModel);
                    newBeneficialOwners.Add(createIndividualModel);
                }
                else
                {
                    if (individual.IsUpdate)
                    {
                        var tempUpdateIndividualModel = _mapper.Map<UpdateIndividualBeneficialOwnerModel>(individual);
                        var updateIndividualModel = _mapper.Map<CompanyBeneficialOwners>(tempUpdateIndividualModel);
                        updateBeneficialOwners.Add(updateIndividualModel);
                    }
                    else if (individual.IsDeleted)
                    {
                        deleteBeneficialOwners.Add(Guid.Parse(individual.Id));
                    }
                }
            }

            var (createResult, createMessage) = await _repository.CreateBeneficialOwners(newBeneficialOwners);

            if (updateBeneficialOwners.Count != 0)
            {
                foreach (var u in updateBeneficialOwners)
                {
                    var (updateResult, updateMessage) = await _repository.Update(u);
                }
            }

            var (deleteResult, deleteMessage) = await _repository.DeleteBeneficialOwners(deleteBeneficialOwners, GlobalVariables.LoggedInUsername);

            var (cboCompanies, cboIndividuals, message) = await _repository.GetByCompanyId(GlobalVariables.LoggedInCompanyId);

            var listCompanyBeneficialOwners = new List<GetCompanyBeneficialOwnerModel>();
            if (!(cboCompanies is null))
            {
                cboCompanies.ForEach(e =>
                {
                    listCompanyBeneficialOwners.Add(_mapper.Map<GetCompanyBeneficialOwnerModel>(e));
                });
            }

            var listIndividualBeneficialOwners = new List<GetIndividualBeneficialOwnerModel>();
            if (!(cboIndividuals is null))
            {
                cboIndividuals.ForEach(e =>
                {
                    listIndividualBeneficialOwners.Add(_mapper.Map<GetIndividualBeneficialOwnerModel>(e));
                });
            }

            var ultimateBenficialOwners = new
            {
                Companies = listCompanyBeneficialOwners,
                Individuals = listIndividualBeneficialOwners
            };

            return _general.Response(ultimateBenficialOwners, 200, "Ultimate Beneficial Owners retrieved", true);
        }
    }
}
