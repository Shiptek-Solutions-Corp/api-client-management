using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.Extensions.Options;
using xgca.core.Response;
using xgca.core.Models.CompanyDocument;
using xgca.data.Repositories;
using AutoMapper;
using System.Threading.Tasks;
using xgca.data.Company;
using xgca.core.Constants;
using xgca.entity.Models;

namespace xgca.core.Services
{
    public interface ICompanyDocumentService
    {
        Task<IGeneralModel> ProcessCompanyDocuments(List<GetCompanyDocumentModel> companyDocuments, GetPBADocumentModel pba, GetOCDocumentModel oc);
    }
    public class CompanyDocumentService : ICompanyDocumentService
    {
        private readonly IMapper _mapper;
        private readonly ICompanyDocumentRepository _repository;
        private readonly IGeneral _general;

        public CompanyDocumentService(IMapper _mapper, ICompanyDocumentRepository _repository, IGeneral _general)
        {
            this._mapper = _mapper;
            this._repository = _repository;
            this._general = _general;
        }

        public async Task<IGeneralModel> ProcessCompanyDocuments(List<GetCompanyDocumentModel> companyDocuments, GetPBADocumentModel pba, GetOCDocumentModel oc)
        {
            var newDocuments = new List<CompanyDocuments>();
            var updateDocuments = new List<CompanyDocuments>();
            var deleteDocuments = new List<Guid>();

            string pbaNewId = null;
            string ocNewId = null;

            if (pba.Id.Equals("NEW"))
            {
                var tempPBAModel = _mapper.Map<CreatePBADocumentModel>(pba);
                var createPBAModel = _mapper.Map<CompanyDocuments>(tempPBAModel);
                pbaNewId = createPBAModel.Guid.ToString();
                newDocuments.Add(createPBAModel);
            }
            else
            {
                if (pba.IsUpdated)
                {
                    var tempPBAModel = _mapper.Map<UpdatePBADocumentModel>(pba);
                    var updatePBAModel = _mapper.Map<CompanyDocuments>(tempPBAModel);
                    updateDocuments.Add(updatePBAModel);
                }
            }

            if (oc.Id.Equals("NEW"))
            {
                var tempOCModel = _mapper.Map<CreateOCDocumentModel>(oc);
                var createOCModel = _mapper.Map<CompanyDocuments>(tempOCModel);
                ocNewId = createOCModel.Guid.ToString();
                newDocuments.Add(createOCModel);
            }
            else
            {
                if (oc.IsUpdated)
                {
                    var tempOCModel = _mapper.Map<UpdateOCDocumentModel>(oc);
                    var updateOCModel = _mapper.Map<CompanyDocuments>(tempOCModel);
                    updateDocuments.Add(updateOCModel);
                }
            }

            foreach(var companyDocument in companyDocuments)
            {
                if (companyDocument.Id.Equals("NEW"))
                {
                    var tempModel = _mapper.Map<CreateCompanyDocumentModel>(companyDocument);
                    var createModel = _mapper.Map<CompanyDocuments>(tempModel);
                    newDocuments.Add(createModel);
                }
                else
                {
                    if (companyDocument.IsUpdated)
                    {
                        var tempModel = _mapper.Map<UpdateCompanyDocumentModel>(companyDocument);
                        var updateModel = _mapper.Map<CompanyDocuments>(tempModel);
                        updateDocuments.Add(updateModel);
                    }
                    else if (companyDocument.IsDeleted)
                    {
                        deleteDocuments.Add(Guid.Parse(companyDocument.Id));
                    }
                }
            }

            var (createResult, createMessage) = await _repository.CreateCompanyDocuments(newDocuments);
            var (updateResult, updateMessage) = await _repository.UpdateCompanyDocuments(updateDocuments);
            var (deleteResult, deleteMessage) = await _repository.DeleteCompanyDocuments(deleteDocuments, GlobalVariables.LoggedInUsername);

            var tempDisplayPBAModel = (pbaNewId is null)
                ? updateResult.Find(x => x.Guid == Guid.Parse(pba.Id))
                : createResult.Find(x => x.Guid == Guid.Parse(pbaNewId));
            var displayPBAModel = new GetPBADocumentModel();
            if (!(tempDisplayPBAModel is null))
            {
                displayPBAModel = _mapper.Map<GetPBADocumentModel>(tempDisplayPBAModel);
            }

            var tempDisplayOCModel = (pbaNewId is null)
                ? updateResult.Find(x => x.Guid == Guid.Parse(oc.Id))
                : createResult.Find(x => x.Guid == Guid.Parse(ocNewId));
            var displayOCModel = new GetOCDocumentModel();
            if (!(tempDisplayOCModel is null))
            {
                displayOCModel = _mapper.Map<GetOCDocumentModel>(tempDisplayOCModel);
            }

            var listCompanyDocumentModel = new List<GetCompanyDocumentModel>();
            if (!(createResult is null))
            {
                var tempNewDocs = createResult.Where(x => x.Guid == Guid.Parse(pba.Id) || x.Guid == Guid.Parse(pbaNewId)
                    || x.Guid == Guid.Parse(oc.Id) || x.Guid == Guid.Parse(ocNewId)).ToList();

                foreach(var docs in tempNewDocs)
                {
                    listCompanyDocumentModel.Add(_mapper.Map<GetCompanyDocumentModel>(docs));
                }
            }

            if (!(updateResult is null))
            {
                var tempNewDocs = updateResult.Where(x => x.Guid == Guid.Parse(pba.Id) || x.Guid == Guid.Parse(pbaNewId)
                    || x.Guid == Guid.Parse(oc.Id) || x.Guid == Guid.Parse(ocNewId)).ToList();

                foreach (var docs in tempNewDocs)
                {
                    listCompanyDocumentModel.Add(_mapper.Map<GetCompanyDocumentModel>(docs));
                }
            }

            var companyDocumentSubSection = new GetCompanyDocumentSubSectionModel
            {
                BusinessRegistrationCertificates = listCompanyDocumentModel,
                ProofOfBusinessAddress = displayPBAModel,
                OrganizationalChart = displayOCModel
            };

            return _general.Response(companyDocumentSubSection, 200, "Successful", true);
        }
    }
}
