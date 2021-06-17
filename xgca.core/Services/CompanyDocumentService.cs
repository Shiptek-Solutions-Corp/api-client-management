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
using xgca.data.User;

namespace xgca.core.Services
{
    public interface ICompanyDocumentService
    {
        Task<IGeneralModel> ProcessCompanyDocuments(List<GetCompanyDocumentModel> companyDocuments, GetPBADocumentModel pba, GetOCDocumentModel oc);
        Task<IGeneralModel> GetCompanyDocumentsByCompanyId(int companyId);
    }
    public class CompanyDocumentService : ICompanyDocumentService
    {
        private readonly IMapper _mapper;
        private readonly ICompanyDocumentRepository _repository;
        private readonly IDocumentTypeRepository _documentTypeRepository;
        private readonly IGeneral _general;
        private readonly IUserData _userRepository;

        public CompanyDocumentService(IMapper _mapper, ICompanyDocumentRepository _repository, IDocumentTypeRepository _documentTypeRepository, IGeneral _general, IUserData _userRepository)
        {
            this._mapper = _mapper;
            this._repository = _repository;
            this._documentTypeRepository = _documentTypeRepository;
            this._general = _general;
            this._userRepository = _userRepository;
        }

        public async Task<IGeneralModel> GetCompanyDocumentsByCompanyId(int companyId)
        {
            GlobalVariables.LoggedInUserId = await _userRepository.GetIdByUsername(GlobalVariables.LoggedInUsername);

            var (companyDocuments, pbaDoc, ocDoc, message) = await _repository.GetCompanyDocumentsByCompanyId(companyId);

            var businessRegistrationCertificates = new List<GetCompanyDocumentModel>();
            var proofOfBusinessAddress = new GetPBADocumentModel();
            var organizationalChart = new GetOCDocumentModel();
            var companyDocumentsSubSection = new GetCompanyDocumentSubSectionModel();
            var businessRegisrationCertificates = new List<GetCompanyDocumentModel>();

            if (!(companyDocuments is null))
            {
                companyDocuments.ForEach(e =>
                {
                    businessRegisrationCertificates.Add(_mapper.Map<GetCompanyDocumentModel>(e));
                });
            }

            if (!(pbaDoc is null))
            {
                companyDocumentsSubSection.ProofOfBusinessAddress = _mapper.Map<GetPBADocumentModel>(pbaDoc);
            }

            if (!(ocDoc is null))
            {
                companyDocumentsSubSection.OrganizationalChart = _mapper.Map<GetOCDocumentModel>(ocDoc);
            }

            companyDocumentsSubSection.BusinessRegistrationCertificates = businessRegisrationCertificates;

            return _general.Response(companyDocumentsSubSection, 200, message, true);
        }

        public async Task<IGeneralModel> ProcessCompanyDocuments(List<GetCompanyDocumentModel> companyDocuments, GetPBADocumentModel pba, GetOCDocumentModel oc)
        {
            GlobalVariables.LoggedInUserId = await _userRepository.GetIdByUsername(GlobalVariables.LoggedInUsername);

            var newDocuments = new List<CompanyDocuments>();
            var updateDocuments = new List<CompanyDocuments>();
            var deleteDocuments = new List<Guid>();

            string pbaNewId = null;
            string ocNewId = null;

            var (documentTypes, documentTypeMessage) = await _documentTypeRepository.ListAllDocumentTypes();

            if (pba.Id.Equals("NEW"))
            {
                var tempPBAModel = _mapper.Map<CreatePBADocumentModel>(pba);
                var createPBAModel = _mapper.Map<CompanyDocuments>(tempPBAModel);
                pbaNewId = createPBAModel.Guid.ToString();
                createPBAModel.DocumentTypeId = documentTypes.SingleOrDefault(x => x.DocumentTypeName.ToUpper() == createPBAModel.DocumentDescription.ToUpper()).DocumentTypeId;
                newDocuments.Add(createPBAModel);
            }
            else
            {
                if (pba.IsUpdated)
                {
                    var tempPBAModel = _mapper.Map<UpdatePBADocumentModel>(pba);
                    var updatePBAModel = _mapper.Map<CompanyDocuments>(tempPBAModel);
                    updatePBAModel.DocumentTypeId = documentTypes.SingleOrDefault(x => x.DocumentTypeName.ToUpper() == updatePBAModel.DocumentDescription.ToUpper()).DocumentTypeId;
                    updateDocuments.Add(updatePBAModel);
                }
            }

            if (oc.Id.Equals("NEW"))
            {
                var tempOCModel = _mapper.Map<CreateOCDocumentModel>(oc);
                var createOCModel = _mapper.Map<CompanyDocuments>(tempOCModel);
                ocNewId = createOCModel.Guid.ToString();
                createOCModel.DocumentTypeId = documentTypes.SingleOrDefault(x => x.DocumentTypeName.ToUpper() == createOCModel.DocumentDescription.ToUpper()).DocumentTypeId;
                newDocuments.Add(createOCModel);
            }
            else
            {
                if (oc.IsUpdated)
                {
                    var tempOCModel = _mapper.Map<UpdateOCDocumentModel>(oc);
                    var updateOCModel = _mapper.Map<CompanyDocuments>(tempOCModel);
                    updateOCModel.DocumentTypeId = documentTypes.SingleOrDefault(x => x.DocumentTypeName.ToUpper() == updateOCModel.DocumentDescription.ToUpper()).DocumentTypeId;
                    updateDocuments.Add(updateOCModel);
                }
            }

            foreach(var companyDocument in companyDocuments)
            {
                if (companyDocument.Id.Equals("NEW"))
                {
                    var tempModel = _mapper.Map<CreateCompanyDocumentModel>(companyDocument);
                    var createModel = _mapper.Map<CompanyDocuments>(tempModel);
                    createModel.DocumentTypeId = documentTypes.SingleOrDefault(x => x.DocumentTypeName.ToUpper() == tempModel.DocumentDescription.ToUpper()).DocumentTypeId;
                    newDocuments.Add(createModel);
                }
                else
                {
                    if (companyDocument.IsUpdated)
                    {
                        var tempModel = _mapper.Map<UpdateCompanyDocumentModel>(companyDocument);
                        var updateModel = _mapper.Map<CompanyDocuments>(tempModel);
                        updateModel.DocumentTypeId = documentTypes.SingleOrDefault(x => x.DocumentTypeGuid == tempModel.DocumentTypeGuid).DocumentTypeId;
                        updateDocuments.Add(updateModel);
                    }
                    else if (companyDocument.IsDeleted)
                    {
                        deleteDocuments.Add(Guid.Parse(companyDocument.Id));
                    }
                }
            }

            if (newDocuments.Count != 0)
            {
                var (createResult, createMessage) = await _repository.CreateCompanyDocuments(newDocuments);
            }
            
            if (updateDocuments.Count != 0)
            {
                foreach(var u in updateDocuments)
                {
                    var (updateResult, updateMessage) = await _repository.Update(u);
                }
            }
            
            if (deleteDocuments.Count != 0)
            {
                var (deleteResult, deleteMessage) = await _repository.DeleteCompanyDocuments(deleteDocuments, GlobalVariables.LoggedInUsername);
            }

            var (brcDocs, pbaDoc, ocDoc, message) = await _repository.GetCompanyDocumentsByCompanyId(GlobalVariables.LoggedInCompanyId);

            var displayPBAModel = new GetPBADocumentModel();
            if (!(pbaDoc is null))
            {
                displayPBAModel = _mapper.Map<GetPBADocumentModel>(pbaDoc);
            }

            var displayOCModel = new GetOCDocumentModel();
            if (!(ocDoc is null))
            {
                displayOCModel = _mapper.Map<GetOCDocumentModel>(ocDoc);
            }

            var listCompanyDocumentModel = new List<GetCompanyDocumentModel>();

            foreach(var docs in brcDocs)
            {
                var doc = _mapper.Map<GetCompanyDocumentModel>(docs);
                doc.DocumentTypeGuid = documentTypes.SingleOrDefault(x => x.DocumentTypeId == docs.DocumentTypeId).DocumentTypeGuid;
                listCompanyDocumentModel.Add(doc);
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
