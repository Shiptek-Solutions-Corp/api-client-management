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

namespace xgca.core.Services
{
    public interface ICompanyBeneficialOwnerService
    {
        Task<IGeneralModel> ProcessCompanyBeneficialOwners(List<GetCompanyBeneficialOwnerModel> companies, List<GetIndividualBeneficialOwnerModel> individuals);
    }
    public class CompanyBeneficialOwnerService : ICompanyBeneficialOwnerService
    {
        private readonly IMapper _mapper;
        private readonly ICompanyBeneficialOwnersRepository _repository;
        private readonly IGeneral _general;

        public CompanyBeneficialOwnerService(IMapper _mapper, ICompanyBeneficialOwnersRepository _repository, IGeneral _general)
        {
            this._mapper = _mapper;
            this._repository = _repository;
            this._general = _general;
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
                    var tempCreateIndividualModel = _mapper.Map<GetIndividualBeneficialOwnerModel>(individual);
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
            var (updateResult, updateMessage) = await _repository.UpdateBeneficialOwners(updateBeneficialOwners);
            var (deleteResult, deleteMessage) = await _repository.DeleteBeneficialOwners(deleteBeneficialOwners, GlobalVariables.LoggedInUsername);

            var listCompanyBeneficialOwners = new List<GetCompanyBeneficialOwnerModel>();
            if (!(createResult is null))
            {
                var tempNewCompanies = createResult.Where(x => x.BeneficialOwnersTypeCode == Enum.GetName(typeof(Enums.BeneficialOwnerType), Enums.BeneficialOwnerType.C)).ToList();

                tempNewCompanies.ForEach(e =>
                {
                    listCompanyBeneficialOwners.Add(_mapper.Map<GetCompanyBeneficialOwnerModel>(e));
                });
            }

            if (!(updateResult is null))
            {
                var tempUpdateCompanies = updateResult.Where(x => x.BeneficialOwnersTypeCode == Enum.GetName(typeof(Enums.BeneficialOwnerType), Enums.BeneficialOwnerType.C)).ToList();

                tempUpdateCompanies.ForEach(e =>
                {
                    listCompanyBeneficialOwners.Add(_mapper.Map<GetCompanyBeneficialOwnerModel>(e));
                });
            }

            var listIndividualBeneficialOwners = new List<GetIndividualBeneficialOwnerModel>();
            if (!(createResult is null))
            {
                var tempNewIndividuals = createResult.Where(x => x.BeneficialOwnersTypeCode == Enum.GetName(typeof(Enums.BeneficialOwnerType), Enums.BeneficialOwnerType.I)).ToList();

                tempNewIndividuals.ForEach(e =>
                {
                    listIndividualBeneficialOwners.Add(_mapper.Map<GetIndividualBeneficialOwnerModel>(e));
                });
            }

            if (!(updateResult is null))
            {
                var tempUpdateIndividuals = updateResult.Where(x => x.BeneficialOwnersTypeCode == Enum.GetName(typeof(Enums.BeneficialOwnerType), Enums.BeneficialOwnerType.I)).ToList();

                tempUpdateIndividuals.ForEach(e =>
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
