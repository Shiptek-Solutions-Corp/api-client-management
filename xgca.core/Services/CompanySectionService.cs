using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using xgca.core.Models.CompanySection;
using xgca.core.Models.CompanyStructure;
using xgca.core.Models.CompanyDocument;
using xgca.core.Models.CompanyBeneficialOwner;
using xgca.core.Models.CompanyDirector;
using xgca.core.Response;
using xgca.data.Repositories;
using xgca.entity.Models;
using xgca.core.Constants;
using System.Linq;
using xgca.data.Company;
using xgca.data.User;
using Microsoft.Extensions.Configuration;
using xgca.core.Models.KYCLog;

namespace xgca.core.Services
{
    public interface ICompanySectionService
    {
        Task<IGeneralModel> CreateInitialSections();
        Task<IGeneralModel> CreateInitialSections(CreateInitialCompanySectionModel obj);
        Task<IGeneralModel> GetCompanySectionsByCompanyId(int companyId);
        Task<IGeneralModel> GetCompanySectionsByCompanyGuid(string companyGuid);
        Task<IGeneralModel> SubmitCompanyStructureSection(UpdateCompanyStructureSectionModel obj, int companyId);
        Task<IGeneralModel> DraftCompanyStructureSection(UpdateCompanyStructureSectionModel obj, int companyId);
        Task<IGeneralModel> SubmitCompanyBeneficialOwnerSection(UpdateCompanyBeneficialOwnerSectionModel obj, int companyId);
        Task<IGeneralModel> DraftCompanyBeneficialOwnerSection(UpdateCompanyBeneficialOwnerSectionModel obj, int companyId);
        Task<IGeneralModel> SubmitCompanyDirectorSection(UpdateCompanyDirectorSectionModel obj, int companyId);
        Task<IGeneralModel> DraftCompanyDirectorSection(UpdateCompanyDirectorSectionModel obj, int companyId);
        Task<IGeneralModel> ListTotalNumerOfEmployess();
        Task<IGeneralModel> RejectCompanyStructureSection(string companyGuid);
        Task<IGeneralModel> RejectCompanyBeneficialOwnerSection(string companyGuid);
        Task<IGeneralModel> RejectCompanyDirectorSection(string companyGuid);
        Task<IGeneralModel> RejectCompanySection(RejectCompanySectionModel obj);
        Task<IGeneralModel> ApproveCompanyStructureSection(ApproveCompanySectionModel obj);
        Task<IGeneralModel> ApproveCompanyBeneficialOwnerSection(ApproveCompanySectionModel obj);
        Task<IGeneralModel> ApproveCompanyDirectorSection(ApproveCompanySectionModel obj);
        Task<IGeneralModel> ReviseCompanyStructureSection(ReviseCompanySectionModel obj);
        Task<IGeneralModel> ReviseCompanyBeneficialOwnerSection(ReviseCompanySectionModel obj);
        Task<IGeneralModel> ReviseCompanyDirectorSection(ReviseCompanySectionModel obj);
        Task<string> CheckOverallKYCStatus(int companyId);

    }
    public class CompanySectionService : ICompanySectionService
    {
        private readonly ICompanyStructureService _companyStructureService;
        private readonly ICompanyDocumentService _companyDocumentService;
        private readonly ICompanyBeneficialOwnerService _companyBeneficialOwnerService;
        private readonly ICompanyDirectorService _companyDirectorService;
        private readonly IKYCLogService _kycLogService;

        private readonly IMapper _mapper;
        private readonly ICompanySectionRepository _repository;
        private readonly ISectionRepository _sectionRepository;
        private readonly ICompanyData _companyRepository;
        private readonly IUserData _userRepository;
        private readonly IGeneral _general;

        private readonly string _draftSpiel;
        private readonly string _submitSpiel;

        public CompanySectionService(IMapper _mapper, ICompanySectionRepository _repository, ISectionRepository _sectionRepository, IGeneral _general,
            ICompanyStructureService _companyStructureService, ICompanyDocumentService _companyDocumentService, ICompanyBeneficialOwnerService _companyBeneficialOwnerService, 
            ICompanyDirectorService _companyDirectorService, IKYCLogService _kycLogService, ICompanyData _companyRepository, IUserData _userRepository, IConfiguration _configuration)
        {
            this._mapper = _mapper;
            this._repository = _repository;
            this._sectionRepository = _sectionRepository;
            this._general = _general;
            this._companyStructureService = _companyStructureService;
            this._companyDocumentService = _companyDocumentService;
            this._companyBeneficialOwnerService = _companyBeneficialOwnerService;
            this._companyDirectorService = _companyDirectorService;
            this._kycLogService = _kycLogService;
            this._companyRepository = _companyRepository;
            this._userRepository = _userRepository;
            _draftSpiel = _configuration["KYCSpiels:Draft"];
            _submitSpiel = _configuration["KYCSpiels:Submit"];
        }
        public async Task<bool> CheckIfCompanyHaveCompanySections(int companyId)
        {
            var (exists, message) = await _repository.CheckIfCompanyHaveCompanySections(companyId);
            return exists;
        }
        public async Task<List<CreateCompanySectionModel>> BuildInitialSections()
        {
            var (sectionResult, sectionMessage) = await _sectionRepository.List();

            var companySections = new List<CreateCompanySectionModel>();
            foreach(var section in sectionResult)
            {
                companySections.Add(new CreateCompanySectionModel
                {
                    SectionCode = section.SectionCode
                });
            }

            return companySections;
        }

        public async Task<IGeneralModel> CreateInitialSections()
        {
            GlobalVariables.LoggedInUserId = await _userRepository.GetIdByUsername(GlobalVariables.LoggedInUsername);

            var unmappedCompanySections = await BuildInitialSections();
            if (unmappedCompanySections.Count == 0)
            {
                return _general.Response(null, 400, "Sections not configured", false);
            }

            var mappedCreateModel = new List<CompanySections>();
            unmappedCompanySections.ForEach(e =>
            {
                mappedCreateModel.Add(_mapper.Map<CompanySections>(e));
            });

            var (createResult, createMessage) = await _repository.CreateCompanySections(mappedCreateModel);
            
            if (createResult.Count == 0)
            {
                return _general.Response(null, 400, createMessage, false);
            }

            var kycLogs = new BulkCreateKYCLogModel();
            foreach (var companySection in createResult)
            {
                var (companySectionId, message) = await _repository.GetIdByGuid(companySection.Guid.ToString());
                if (companySectionId == 0)
                {
                    continue;
                }

                kycLogs.KYCLogs.Add(new CreateKYCLogModel
                {
                    CompanyId = GlobalVariables.LoggedInCompanyId,
                    CompanySectionsId = companySectionId,
                    Remarks = "Created section",
                    SectionStatusCode = companySection.SectionStatusCode
                });
            }

            await _kycLogService.BulkCreateKYCLogs(kycLogs);

            return _general.Response(createResult, 200, createMessage, true);
        }

        public async Task<IGeneralModel> CreateInitialSections(CreateInitialCompanySectionModel obj)
        {
            if (Guid.Parse(obj.CompanyId) == Guid.Empty)
            {
                return _general.Response(null, 400, "Invalid Company Id", false);
            }


            GlobalVariables.LoggedInUserId = await _userRepository.GetIdByUsername(GlobalVariables.LoggedInUsername);
            GlobalVariables.LoggedInCompanyId = await _companyRepository.GetIdByGuid(Guid.Parse(obj.CompanyId));

            var unmappedCompanySections = await BuildInitialSections();
            if (unmappedCompanySections.Count == 0)
            {
                return _general.Response(null, 400, "Sections not configured", false);
            }

            var mappedCreateModel = new List<CompanySections>();
            unmappedCompanySections.ForEach(e =>
            {
                mappedCreateModel.Add(_mapper.Map<CompanySections>(e));
            });

            var (createResult, createMessage) = await _repository.CreateCompanySections(mappedCreateModel);

            if (createResult.Count == 0)
            {
                return _general.Response(null, 400, createMessage, false);
            }

            var kycLogs = new BulkCreateKYCLogModel();
            foreach (var companySection in createResult)
            {
                var (companySectionId, message) = await _repository.GetIdByGuid(companySection.Guid.ToString());
                if (companySectionId == 0)
                {
                    continue;
                }

                kycLogs.KYCLogs.Add(new CreateKYCLogModel
                {
                    CompanyId = GlobalVariables.LoggedInCompanyId,
                    CompanySectionsId = companySectionId,
                    Remarks = "Created section",
                    SectionStatusCode = companySection.SectionStatusCode
                });
            }

            await _kycLogService.BulkCreateKYCLogs(kycLogs);

            var companySections = await GetCompanySection(GlobalVariables.LoggedInCompanyId);

            return _general.Response(new { CompanySections = companySections }, 200, createMessage, true);
        }

        public async Task<GetCompanySectionModel> GetCompanySection(int companyId)
        {
            var exists = await CheckIfCompanyHaveCompanySections(companyId);
            if (!(exists))
            {
                await CreateInitialSections();
            }

            var (companySectionResult, message) = await _repository.GetListByCompanyId(companyId);

            var tempCompanyStructureSection = companySectionResult.Find(x => x.SectionCode == Enum.GetName(typeof(Enums.Section), Enums.Section.CS));
            var companyStructureSection = new GetCompanyStructureSectionModel();
            if (!(tempCompanyStructureSection is null))
            {
                companyStructureSection = _mapper.Map<GetCompanyStructureSectionModel>(tempCompanyStructureSection);

                var companyStructureResponse = await _companyStructureService.GetCompanyStrucureDetailsByCompanyId(companyId);
                var companyStructure = (GetCompanyStructureModel)companyStructureResponse.data.CompanyStructure;

                var companyDocumentResponse = await _companyDocumentService.GetCompanyDocumentsByCompanyId(companyId);
                var businessRegistrationCertificate = (List<GetCompanyDocumentModel>)companyDocumentResponse.data.BusinessRegistrationCertificates;
                var proofOfBusinessAddress = (GetPBADocumentModel)companyDocumentResponse.data.ProofOfBusinessAddress;
                var organizationalChart = (GetOCDocumentModel)companyDocumentResponse.data.OrganizationalChart;

                string latestRemarks = "-";
                if (tempCompanyStructureSection.KYCLogs.Count != 0)
                {
                    latestRemarks = tempCompanyStructureSection.KYCLogs.OrderByDescending(o => o.CreatedOn).FirstOrDefault().Remarks.ToString();
                }

                companyStructureSection.Details = companyStructure;
                companyStructureSection.BusinessRegistrationCertificates = businessRegistrationCertificate;
                companyStructureSection.ProofOfBusinessAddress = proofOfBusinessAddress;
                companyStructureSection.OrganizationalChart = organizationalChart;
                companyStructureSection.LatestRemarks = latestRemarks;

            }

            var tempCompanyBeneficialOwnerSection = companySectionResult.Find(x => x.SectionCode == Enum.GetName(typeof(Enums.Section), Enums.Section.BO));
            var companyBeneficialOwnerSection = new GetCompanyBeneficialOwnerSectionModel();
            if (!(tempCompanyBeneficialOwnerSection is null))
            {
                companyBeneficialOwnerSection = _mapper.Map<GetCompanyBeneficialOwnerSectionModel>(tempCompanyBeneficialOwnerSection);

                var companyBeneficialOwnerResponse = await _companyBeneficialOwnerService.GetByCompanyId(companyId);
                var companies = (List<GetCompanyBeneficialOwnerModel>)companyBeneficialOwnerResponse.data.Companies;
                var individuals = (List<GetIndividualBeneficialOwnerModel>)companyBeneficialOwnerResponse.data.Individuals;

                string latestRemarks = "-";
                if (tempCompanyBeneficialOwnerSection.KYCLogs.Count != 0)
                {
                    latestRemarks = tempCompanyBeneficialOwnerSection.KYCLogs.OrderByDescending(o => o.CreatedOn).FirstOrDefault().Remarks.ToString();
                }

                companyBeneficialOwnerSection.Company = companies;
                companyBeneficialOwnerSection.Individual = individuals;
                companyBeneficialOwnerSection.LatestRemarks = latestRemarks;
            }

            var tempCompanyDirectorSection = companySectionResult.Find(x => x.SectionCode == Enum.GetName(typeof(Enums.Section), Enums.Section.CD));
            var companyDirectorSection = new GetCompanyDirectorSectionModel();
            if (!(tempCompanyDirectorSection is null))
            {
                companyDirectorSection = _mapper.Map<GetCompanyDirectorSectionModel>(tempCompanyDirectorSection);

                var companyDirectorResponse = await _companyDirectorService.GetByCompanyId(companyId);
                var directors = (List<GetCompanyDirectorModel>)companyDirectorResponse.data.Directors;

                string latestRemarks = "-";
                if (tempCompanyDirectorSection.KYCLogs.Count != 0)
                {
                    latestRemarks = tempCompanyDirectorSection.KYCLogs.OrderByDescending(o => o.CreatedOn).FirstOrDefault().Remarks.ToString();
                }

                companyDirectorSection.Directors = directors;
                companyDirectorSection.LatestRemarks = latestRemarks;
            }

            var companySections = new GetCompanySectionModel
            {
                CompanyStructure = companyStructureSection,
                UltimateBeneficialOwners = companyBeneficialOwnerSection,
                CompanyDirectors = companyDirectorSection
            };

            return companySections;
        }

        public async Task<IGeneralModel> GetCompanySectionsByCompanyGuid(string companyGuid)
        {
            //GlobalVariables.LoggedInUserId = await _userRepository.GetIdByUsername(GlobalVariables.LoggedInUsername);
            int companyId = await _companyRepository.GetIdByGuid(Guid.Parse(companyGuid));
            var companySections = await GetCompanySection(companyId);

            string overallKYCStatus = await _companyRepository.GetKYCStatus(companyId);
            string reason = "-";
            if (overallKYCStatus == Enum.GetName(typeof(Enums.KYCStatus), Enums.KYCStatus.REJ))
            {
                reason = companySections.CompanyStructure.LatestRemarks;
            }

            return _general.Response(new { OverallKYCStatus = overallKYCStatus, AdditionalInformation = companySections, Reason = reason }, 200, "Company Sections retrieved successfully", true);
        }

        public async Task<IGeneralModel> GetCompanySectionsByCompanyId(int companyId)
        {
            GlobalVariables.LoggedInUserId = await _userRepository.GetIdByUsername(GlobalVariables.LoggedInUsername);
            var companySections = await GetCompanySection(companyId);

            string overallKYCStatus = await _companyRepository.GetKYCStatus(companyId);
            string reason = "-";
            if (overallKYCStatus == Enum.GetName(typeof(Enums.KYCStatus), Enums.KYCStatus.REJ))
            {
                reason = companySections.CompanyStructure.LatestRemarks;
            }

            return _general.Response(new { OverallKYCStatus = overallKYCStatus, AdditionalInformation = companySections, Reason = reason }, 200, "Company Sections retrieved successfully", true);
        }

        public async Task<string> CheckOverallKYCStatus(int companyId)
        {
            var (companySections, message) = await _repository.GetListByCompanyId(companyId);
            string kycStatus = null;

            List<string> sectionStatuses = new List<string>();
            string companyStructureStatus = companySections.SingleOrDefault(x => x.SectionCode == Enum.GetName(typeof(Enums.Section), Enums.Section.CS)).SectionStatusCode;
            sectionStatuses.Add(companyStructureStatus);
            string companyBeneficialOwnerStatus = companySections.SingleOrDefault(x => x.SectionCode == Enum.GetName(typeof(Enums.Section), Enums.Section.BO)).SectionStatusCode;
            sectionStatuses.Add(companyBeneficialOwnerStatus);
            string companyDirectorStatus = companySections.SingleOrDefault(x => x.SectionCode == Enum.GetName(typeof(Enums.Section), Enums.Section.CD)).SectionStatusCode;
            sectionStatuses.Add(companyDirectorStatus);

            
            if(sectionStatuses.Contains(Enum.GetName(typeof(Enums.SectionStatus), Enums.SectionStatus.RJ)))
            {
                kycStatus = Enum.GetName(typeof(Enums.KYCStatus), Enums.KYCStatus.REJ);
            }
            else if (sectionStatuses.Contains(Enum.GetName(typeof(Enums.SectionStatus), Enums.SectionStatus.PR)))
            {
                kycStatus = Enum.GetName(typeof(Enums.KYCStatus), Enums.KYCStatus.PER);
            }
            else if (sectionStatuses.Contains(Enum.GetName(typeof(Enums.SectionStatus), Enums.SectionStatus.PA)))
            {
                kycStatus = Enum.GetName(typeof(Enums.KYCStatus), Enums.KYCStatus.PEN);
            }
            else
            {
                int count = 0;
                foreach(var sectionStatus in sectionStatuses)
                {
                    if (sectionStatus.Equals(Enum.GetName(typeof(Enums.KYCStatus), Enums.KYCStatus.APP)) || sectionStatus.Equals(Enum.GetName(typeof(Enums.SectionStatus), Enums.SectionStatus.AP)))
                    {
                        count++;
                    }
                }

                kycStatus = (count == 3)
                    ? Enum.GetName(typeof(Enums.KYCStatus), Enums.KYCStatus.APP)
                    : Enum.GetName(typeof(Enums.KYCStatus), Enums.KYCStatus.NEW);
            }

            return kycStatus;

        }

        public async Task<GetCompanyStructureSectionModel> GetCompanyStructureSection(string id)
        {
            var (companySection, message) = await _repository.Get(id);

            var companyStructureSection = new GetCompanyStructureSectionModel();
            if (!(companySection is null))
            {
                companyStructureSection = _mapper.Map<GetCompanyStructureSectionModel>(companySection);

                var companyStructureResponse = await _companyStructureService.GetCompanyStrucureDetailsByCompanyId(companySection.CompanyId);
                var companyStructure = (GetCompanyStructureModel)companyStructureResponse.data.CompanyStructure;

                var companyDocumentResponse = await _companyDocumentService.GetCompanyDocumentsByCompanyId(companySection.CompanyId);
                var businessRegistrationCertificate = (List<GetCompanyDocumentModel>)companyDocumentResponse.data.BusinessRegistrationCertificates;
                var proofOfBusinessAddress = (GetPBADocumentModel)companyDocumentResponse.data.ProofOfBusinessAddress;
                var organizationalChart = (GetOCDocumentModel)companyDocumentResponse.data.OrganizationalChart;

                string latestRemarks = "-";
                if (companySection.KYCLogs.Count != 0)
                {
                    latestRemarks = companySection.KYCLogs.OrderByDescending(o => o.CreatedOn).FirstOrDefault().Remarks.ToString();
                }

                companyStructureSection.Details = companyStructure;
                companyStructureSection.BusinessRegistrationCertificates = businessRegistrationCertificate;
                companyStructureSection.ProofOfBusinessAddress = proofOfBusinessAddress;
                companyStructureSection.OrganizationalChart = organizationalChart;
                companyStructureSection.LatestRemarks = latestRemarks;
            }

            return companyStructureSection;
        }

        public async Task<GetCompanyBeneficialOwnerSectionModel> GetCompanyBeneficialOwnerSection(string id)
        {
            var (companySection, message) = await _repository.Get(id);

            var companyBeneficialOwnerSection = new GetCompanyBeneficialOwnerSectionModel();
            if (!(companySection is null))
            {
                companyBeneficialOwnerSection = _mapper.Map<GetCompanyBeneficialOwnerSectionModel>(companySection);

                var companyBeneficialOwnerResponse = await _companyBeneficialOwnerService.GetByCompanyId(companySection.CompanyId);
                var companies = (List<GetCompanyBeneficialOwnerModel>)companyBeneficialOwnerResponse.data.Companies;
                var individuals = (List<GetIndividualBeneficialOwnerModel>)companyBeneficialOwnerResponse.data.Individuals;

                string latestRemarks = "-";
                if (companySection.KYCLogs.Count != 0)
                {
                    latestRemarks = companySection.KYCLogs.OrderByDescending(o => o.CreatedOn).FirstOrDefault().Remarks.ToString();
                }

                companyBeneficialOwnerSection.Company = companies;
                companyBeneficialOwnerSection.Individual = individuals;
                companyBeneficialOwnerSection.LatestRemarks = latestRemarks;
            }

            return companyBeneficialOwnerSection;
        }

        public async Task<GetCompanyDirectorSectionModel> GetCompanyDirectorSection(string id)
        {
            var (companySection, message) = await _repository.Get(id);

            var companyDirectorSection = new GetCompanyDirectorSectionModel();
            if (!(companySection is null))
            {
                companyDirectorSection = _mapper.Map<GetCompanyDirectorSectionModel>(companySection);

                var companyDirectorResponse = await _companyDirectorService.GetByCompanyId(companySection.CompanyId);
                var directors = (List<GetCompanyDirectorModel>)companyDirectorResponse.data.Directors;

                string latestRemarks = "-";
                if (companySection.KYCLogs.Count != 0)
                {
                    latestRemarks = companySection.KYCLogs.OrderByDescending(o => o.CreatedOn).FirstOrDefault().Remarks.ToString();
                }

                companyDirectorSection.Directors = directors;
                companyDirectorSection.LatestRemarks = latestRemarks;
            }

            return companyDirectorSection;
        }

        public async Task<IGeneralModel> SubmitCompanyStructureSection(UpdateCompanyStructureSectionModel obj, int companyId)
        {
            if (obj is null)
            {
                return _general.Response(null, 400, "Data cannot be null", false);
            }

            GlobalVariables.LoggedInUserId = await _userRepository.GetIdByUsername(GlobalVariables.LoggedInUsername);



            if (obj.Details.Id.Equals("NEW"))
            {
                var createCompanyStructureModel = _mapper.Map<CreateCompanyStructureModel>(obj.Details);
                var companyStructureResponse = await _companyStructureService.CreateCompanyStructure(createCompanyStructureModel);
                if (!companyStructureResponse.isSuccessful)
                {
                    return _general.Response(null, 400, "Error in creating company structure details", false);
                }
            }
            else
            {
                var updateCompanyStructureModel = _mapper.Map<UpdateCompanyStructureModel>(obj.Details);
                var companyStructureResponse = await _companyStructureService.UpdateCompanyStructure(updateCompanyStructureModel);
                if (!companyStructureResponse.isSuccessful)
                {
                    return _general.Response(null, 400, "Error in updating company structure details", false);
                }
            }
            

            var companyDocumentResponse = await _companyDocumentService.ProcessCompanyDocuments(obj.BusinessRegistrationCertificates, obj.ProofOfBusinessAddress, obj.OrganizationalChart);
            if (!companyDocumentResponse.isSuccessful)
            {
                return _general.Response(null, 400, "Error in processing company documents", false);
            }


            var updateModel = new CompanySections
            {
                Guid = Guid.Parse(obj.Id),
                UpdatedBy = GlobalVariables.LoggedInUsername,
                UpdatedOn = DateTime.UtcNow,
                SectionStatusCode = Enum.GetName(typeof(Enums.SectionStatus), Enums.SectionStatus.PA)
            };
            var (updateResult, updateMessage) = await _repository.UpdateStatus(updateModel);
            if (updateResult is null)
            {
                return _general.Response(null, 400, "Error in updating company structure section status", false);
            }

            string kycStatus = await CheckOverallKYCStatus(companyId);
            int userId = await _userRepository.GetIdByUsername(GlobalVariables.LoggedInUsername);
            var (companyKYCStatus, message) = await _companyRepository.UpdateKYCStatus(companyId, kycStatus, userId);

            var (companySectionId, companySectionMessage) = await _repository.GetIdByGuid(obj.Id);
            if (companySectionId != 0)
            {
                await _kycLogService.CreateKYCLogs(new CreateKYCLogModel
                {
                    CompanyId = GlobalVariables.LoggedInCompanyId,
                    CompanySectionsId = companySectionId,
                    Remarks = obj.Remarks,
                    SectionStatusCode = updateResult.SectionStatusCode
                });
            }

            var companyStructureSection = await GetCompanyStructureSection(obj.Id);

            var data = new
            {
                OverallKYCStatus = companyKYCStatus,
                CompanyStructureSection = companyStructureSection
            };

            return _general.Response(data, 200, _submitSpiel, true);
        }

        public async Task<IGeneralModel> DraftCompanyStructureSection(UpdateCompanyStructureSectionModel obj, int companyId)
        {
            if (obj is null)
            {
                return _general.Response(null, 400, "Data cannot be null", false);
            }

            GlobalVariables.LoggedInUserId = await _userRepository.GetIdByUsername(GlobalVariables.LoggedInUsername);

            if (obj.Details.Id.Equals("NEW"))
            {
                var createCompanyStructureModel = _mapper.Map<CreateCompanyStructureModel>(obj.Details);
                var createCompanyStructureResponse = await _companyStructureService.CreateCompanyStructure(createCompanyStructureModel);
                if (!createCompanyStructureResponse.isSuccessful)
                {
                    return _general.Response(null, 400, "Error in creating company structure details", false);
                }
            }
            else
            {
                var updateCompanyStructureModel = _mapper.Map<UpdateCompanyStructureModel>(obj.Details);
                var updateCompanyStructureResponse = await _companyStructureService.UpdateCompanyStructure(updateCompanyStructureModel);
                if (!updateCompanyStructureResponse.isSuccessful)
                {
                    return _general.Response(null, 400, "Error in updating company structure details", false);
                }
            }
            var companyDocumentResponse = await _companyDocumentService.ProcessCompanyDocuments(obj.BusinessRegistrationCertificates, obj.ProofOfBusinessAddress, obj.OrganizationalChart);
            if (!companyDocumentResponse.isSuccessful)
            {
                return _general.Response(null, 400, "Error in processing company documents", false);
            }

            var draftModel = new CompanySections
            {
                Guid = Guid.Parse(obj.Id),
                UpdatedBy = GlobalVariables.LoggedInUsername,
                UpdatedOn = DateTime.UtcNow,
            };
            var (draftResult, draftMessage) = await _repository.SaveAsDraft(draftModel);
            if (draftResult is null)
            {
                return _general.Response(null, 400, "Error in saving company structure section as draft", false);
            }

            string kycStatus = await CheckOverallKYCStatus(companyId);
            int userId = await _userRepository.GetIdByUsername(GlobalVariables.LoggedInUsername);
            var (companyKYCStatus, message) = await _companyRepository.UpdateKYCStatus(companyId, kycStatus, userId);

            var (companySectionId, companySectionMessage) = await _repository.GetIdByGuid(obj.Id);
            if (companySectionId != 0)
            {
                await _kycLogService.CreateKYCLogs(new CreateKYCLogModel
                {
                    CompanyId = GlobalVariables.LoggedInCompanyId,
                    CompanySectionsId = companySectionId,
                    Remarks = obj.Remarks,
                    SectionStatusCode = draftResult.SectionStatusCode
                });
            }

            var companyStructureSection = await GetCompanyStructureSection(obj.Id);

            var data = new
            {
                OverallKYCStatus = companyKYCStatus,
                CompanyStructureSection = companyStructureSection
            };

            return _general.Response(data, 200, _draftSpiel, true);
        }

        public async Task<IGeneralModel> SubmitCompanyBeneficialOwnerSection(UpdateCompanyBeneficialOwnerSectionModel obj, int companyId)
        {
            if (obj is null)
            {
                return _general.Response(null, 400, "Data cannot be null", false);
            }

            GlobalVariables.LoggedInUserId = await _userRepository.GetIdByUsername(GlobalVariables.LoggedInUsername);

            var companyBeneficialResponse = await _companyBeneficialOwnerService.ProcessCompanyBeneficialOwners(obj.Companies, obj.Individuals);

            var updateModel = new CompanySections
            {
                Guid = Guid.Parse(obj.Id),
                UpdatedBy = GlobalVariables.LoggedInUsername,
                UpdatedOn = DateTime.UtcNow,
                SectionStatusCode = Enum.GetName(typeof(Enums.SectionStatus), Enums.SectionStatus.PA)
            };
            var (updateResult, updateMessage) = await _repository.UpdateStatus(updateModel);
            if (updateResult is null)
            {
                return _general.Response(null, 400, "Error in updating ultimate beneficial owner section status", false);
            }

            string kycStatus = await CheckOverallKYCStatus(companyId);
            int userId = await _userRepository.GetIdByUsername(GlobalVariables.LoggedInUsername);
            var (companyKYCStatus, message) = await _companyRepository.UpdateKYCStatus(companyId, kycStatus, userId);

            var (companySectionId, companySectionMessage) = await _repository.GetIdByGuid(obj.Id);
            if (companySectionId != 0)
            {
                await _kycLogService.CreateKYCLogs(new CreateKYCLogModel
                {
                    CompanyId = GlobalVariables.LoggedInCompanyId,
                    CompanySectionsId = companySectionId,
                    Remarks = obj.Remarks,
                    SectionStatusCode = updateResult.SectionStatusCode
                });
            }

            var companyBeneficialOwnerSection = await GetCompanyBeneficialOwnerSection(obj.Id);

            var data = new
            {
                OverallKYCStatus = companyKYCStatus,
                UltimateBeneficialOwners = companyBeneficialOwnerSection
            };

            return _general.Response(data, 200, _submitSpiel, true);
        }

        public async Task<IGeneralModel> DraftCompanyBeneficialOwnerSection(UpdateCompanyBeneficialOwnerSectionModel obj, int companyId)
        {
            if (obj is null)
            {
                return _general.Response(null, 400, "Data cannot be null", false);
            }

            GlobalVariables.LoggedInUserId = await _userRepository.GetIdByUsername(GlobalVariables.LoggedInUsername);

            var companyBeneficialResponse = await _companyBeneficialOwnerService.ProcessCompanyBeneficialOwners(obj.Companies, obj.Individuals);

            var draftModel = new CompanySections
            {
                Guid = Guid.Parse(obj.Id),
                UpdatedBy = GlobalVariables.LoggedInUsername,
                UpdatedOn = DateTime.UtcNow,
            };
            var (draftResult, draftMessage) = await _repository.SaveAsDraft(draftModel);
            if (draftResult is null)
            {
                return _general.Response(null, 400, "Error in saving ultimate benficial owner section as draft", false);
            }

            string kycStatus = await CheckOverallKYCStatus(companyId);
            int userId = await _userRepository.GetIdByUsername(GlobalVariables.LoggedInUsername);
            var (companyKYCStatus, message) = await _companyRepository.UpdateKYCStatus(companyId, kycStatus, userId);

            var (companySectionId, companySectionMessage) = await _repository.GetIdByGuid(obj.Id);
            if (companySectionId != 0)
            {
                await _kycLogService.CreateKYCLogs(new CreateKYCLogModel
                {
                    CompanyId = GlobalVariables.LoggedInCompanyId,
                    CompanySectionsId = companySectionId,
                    Remarks = obj.Remarks,
                    SectionStatusCode = draftResult.SectionStatusCode
                });
            }

            var companyBeneficialOwnerSection = await GetCompanyBeneficialOwnerSection(obj.Id);

            var data = new
            {
                OverallKYCStatus = companyKYCStatus,
                UltimateBeneficialOwners = companyBeneficialOwnerSection
            };

            return _general.Response(data, 200, _draftSpiel, true);
        }

        public async Task<IGeneralModel> SubmitCompanyDirectorSection(UpdateCompanyDirectorSectionModel obj, int companyId)
        {
            if (obj is null)
            {
                return _general.Response(null, 400, "Data cannot be null", false);
            }

            GlobalVariables.LoggedInUserId = await _userRepository.GetIdByUsername(GlobalVariables.LoggedInUsername);

            var companyDirectorResponse = await _companyDirectorService.ProcessCompanyDirectors(obj.CompanyDirectors);
            if (!companyDirectorResponse.isSuccessful)
            {
                return _general.Response(null, 400, "Error in processing company directors ", false);
            }

            var updateModel = new CompanySections
            {
                Guid = Guid.Parse(obj.Id),
                UpdatedBy = GlobalVariables.LoggedInUsername,
                UpdatedOn = DateTime.UtcNow,
                SectionStatusCode = Enum.GetName(typeof(Enums.SectionStatus), Enums.SectionStatus.PA)
            };
            var (updateResult, updateMessage) = await _repository.UpdateStatus(updateModel);
            if (updateResult is null)
            {
                return _general.Response(null, 400, "Error in updating company directors section status", false);
            }

            string kycStatus = await CheckOverallKYCStatus(companyId);
            int userId = await _userRepository.GetIdByUsername(GlobalVariables.LoggedInUsername);
            var (companyKYCStatus, message) = await _companyRepository.UpdateKYCStatus(companyId, kycStatus, userId);

            var (companySectionId, companySectionMessage) = await _repository.GetIdByGuid(obj.Id);
            if (companySectionId != 0)
            {
                await _kycLogService.CreateKYCLogs(new CreateKYCLogModel
                {
                    CompanyId = GlobalVariables.LoggedInCompanyId,
                    CompanySectionsId = companySectionId,
                    Remarks = obj.Remarks,
                    SectionStatusCode = updateResult.SectionStatusCode
                });
            }

            var companyDirectorSection = await GetCompanyDirectorSection(obj.Id);

            var data = new
            {
                OverallKYCStatus = companyKYCStatus,
                CompanyDirectors = companyDirectorSection
            };

            return _general.Response(data, 200, _submitSpiel, true);
        }

        public async Task<IGeneralModel> DraftCompanyDirectorSection(UpdateCompanyDirectorSectionModel obj, int companyId)
        {
            if (obj is null)
            {
                return _general.Response(null, 400, "Data cannot be null", false);
            }

            GlobalVariables.LoggedInUserId = await _userRepository.GetIdByUsername(GlobalVariables.LoggedInUsername);

            var companyDirectorResponse = await _companyDirectorService.ProcessCompanyDirectors(obj.CompanyDirectors);

            var draftModel = new CompanySections
            {
                Guid = Guid.Parse(obj.Id),
                UpdatedBy = GlobalVariables.LoggedInUsername,
                UpdatedOn = DateTime.UtcNow,
            };
            var (draftResult, draftMessage) = await _repository.SaveAsDraft(draftModel);
            if (draftResult is null)
            {
                return _general.Response(null, 400, "Error in saving ultimate benficial owner section as draft", false);
            }

            string kycStatus = await CheckOverallKYCStatus(companyId);
            int userId = await _userRepository.GetIdByUsername(GlobalVariables.LoggedInUsername);
            var companyKYCStatus = await _companyRepository.UpdateKYCStatus(companyId, kycStatus, userId);

            var (companySectionId, companySectionMessage) = await _repository.GetIdByGuid(obj.Id);
            if (companySectionId != 0)
            {
                await _kycLogService.CreateKYCLogs(new CreateKYCLogModel
                {
                    CompanyId = GlobalVariables.LoggedInCompanyId,
                    CompanySectionsId = companySectionId,
                    Remarks = obj.Remarks,
                    SectionStatusCode = draftResult.SectionStatusCode
                });
            }

            var companyDirectorSection = await GetCompanyDirectorSection(obj.Id);

            var data = new
            {
                OverallKYCStatus = companyKYCStatus,
                CompanyDirectors = companyDirectorSection
            };

            return _general.Response(data, 200, _draftSpiel, true);
        }

        public async Task<IGeneralModel> ListTotalNumerOfEmployess()
        {
            var tnoe = new List<string>();
            tnoe.Add("1-10");
            tnoe.Add("11-50");
            tnoe.Add("51-200");
            tnoe.Add("201-500");
            tnoe.Add("501-1000");
            tnoe.Add("1001-5000");
            tnoe.Add("5001-10000");
            tnoe.Add("Above 10000");

            return _general.Response(new { TotalNumberOfEmployees = tnoe }, 200, "Total number of employees range listed successfully", true);
        }

        public async Task<IGeneralModel> RejectCompanyStructureSection(string companyGuid)
        {
            if (Guid.Parse(companyGuid) == Guid.Empty)
            {
                return _general.Response(null, 400, "Invalid company id", false);
            }

            int companyId = await _companyRepository.GetIdByGuid(Guid.Parse(companyGuid));
            if (companyId == 0)
            {
                return _general.Response(null, 400, "Invalid company id", false);
            }

            var updateModel = new CompanySections
            {
                CompanyId = companyId,
                SectionCode = Enum.GetName(typeof(Enums.Section), Enums.Section.CS),
                SectionStatusCode = Enum.GetName(typeof(Enums.SectionStatus), Enums.SectionStatus.PR),
                UpdatedBy = GlobalVariables.LoggedInUsername,
                UpdatedOn = DateTime.UtcNow,
            };

            var (updateResult, updateMessage) = await _repository.UpdateStatusByCompanyId(updateModel);
            if (updateResult is null)
            {
                return _general.Response(null, 400, updateMessage, false);
            }

            string overallKYCStatus = await CheckCompanyKYCStatus(companyId);

            var data = new
            {
                OverallKYCStatus = overallKYCStatus,
                CompanyStructure = updateResult
            };

            return _general.Response(data, 200, updateMessage, true);
        }

        public async Task<IGeneralModel> RejectCompanyBeneficialOwnerSection(string companyGuid)
        {
            if (Guid.Parse(companyGuid) == Guid.Empty)
            {
                return _general.Response(null, 400, "Invalid company id", false);
            }

            int companyId = await _companyRepository.GetIdByGuid(Guid.Parse(companyGuid));
            if (companyId == 0)
            {
                return _general.Response(null, 400, "Invalid company id", false);
            }

            var updateModel = new CompanySections
            {
                CompanyId = companyId,
                SectionCode = Enum.GetName(typeof(Enums.Section), Enums.Section.BO),
                SectionStatusCode = Enum.GetName(typeof(Enums.SectionStatus), Enums.SectionStatus.PR),
                UpdatedBy = GlobalVariables.LoggedInUsername,
                UpdatedOn = DateTime.UtcNow,
            };

            var (updateResult, updateMessage) = await _repository.UpdateStatusByCompanyId(updateModel);
            if (updateResult is null)
            {
                return _general.Response(null, 400, updateMessage, false);
            }

            string overallKYCStatus = await CheckCompanyKYCStatus(companyId);

            var data = new
            {
                OverallKYCStatus = overallKYCStatus,
                UltimateBeneficialOwners = updateResult
            };

            return _general.Response(data, 200, updateMessage, true);
        }

        public async Task<IGeneralModel> RejectCompanyDirectorSection(string companyGuid)
        {
            if (Guid.Parse(companyGuid) == Guid.Empty)
            {
                return _general.Response(null, 400, "Invalid company id", false);
            }

            int companyId = await _companyRepository.GetIdByGuid(Guid.Parse(companyGuid));
            if (companyId == 0)
            {
                return _general.Response(null, 400, "Invalid company id", false);
            }

            var updateModel = new CompanySections
            {
                CompanyId = companyId,
                SectionCode = Enum.GetName(typeof(Enums.Section), Enums.Section.CD),
                SectionStatusCode = Enum.GetName(typeof(Enums.SectionStatus), Enums.SectionStatus.PR),
                UpdatedBy = GlobalVariables.LoggedInUsername,
                UpdatedOn = DateTime.UtcNow,
            };

            var (updateResult, updateMessage) = await _repository.UpdateStatusByCompanyId(updateModel);
            if (updateResult is null)
            {
                return _general.Response(null, 400, updateMessage, false);
            }

            string overallKYCStatus = await CheckCompanyKYCStatus(companyId);

            var data = new
            {
                OverallKYCStatus = overallKYCStatus,
                CompanyDirectors = updateResult
            };

            return _general.Response(data, 200, updateMessage, true);
        }

        public async Task<IGeneralModel> ApproveCompanyStructureSection(ApproveCompanySectionModel obj)
        {
            if(Guid.Parse(obj.CompanyId) == Guid.Empty)
            {
                return _general.Response(null, 400, "Invalid company id", false);
            }

            int companyId = await _companyRepository.GetIdByGuid(Guid.Parse(obj.CompanyId));
            if (companyId == 0)
            {
                return _general.Response(null, 400, "Invalid company id", false);
            }

            var updateModel = new CompanySections
            {
                CompanyId = companyId,
                SectionCode = Enum.GetName(typeof(Enums.Section), Enums.Section.CS),
                SectionStatusCode = Enum.GetName(typeof(Enums.SectionStatus), Enums.SectionStatus.AP),
                UpdatedBy = GlobalVariables.LoggedInUsername,
                UpdatedOn = DateTime.UtcNow,
            };

            var (updateResult, updateMessage) = await _repository.UpdateStatusByCompanyId(updateModel);
            if (updateResult is null)
            {
                return _general.Response(null, 400, updateMessage, false);
            }

            var (companySectionObj, companySectionMessage) = await _repository.GetCompanySection(Enum.GetName(typeof(Enums.Section), Enums.Section.CS), companyId);
            if (!(companySectionObj is null))
            {
                await _kycLogService.CreateKYCLogs(new CreateKYCLogModel
                {
                    CompanyId = companyId,
                    CompanySectionsId = companySectionObj.CompanySectionsId,
                    Remarks = obj.Remarks,
                    SectionStatusCode = updateResult.SectionStatusCode
                });
            }

            string overallKYCStatus = await CheckCompanyKYCStatus(companyId);

            var data = new
            {
                OverallKYCStatus = overallKYCStatus,
                CompanyStructure = updateResult
            };

            return _general.Response(data, 200, updateMessage, true);
        }

        public async Task<IGeneralModel> ApproveCompanyBeneficialOwnerSection(ApproveCompanySectionModel obj)
        {
            if (Guid.Parse(obj.CompanyId) == Guid.Empty)
            {
                return _general.Response(null, 400, "Invalid company id", false);
            }

            int companyId = await _companyRepository.GetIdByGuid(Guid.Parse(obj.CompanyId));
            if (companyId == 0)
            {
                return _general.Response(null, 400, "Invalid company id", false);
            }

            var updateModel = new CompanySections
            {
                CompanyId = companyId,
                SectionCode = Enum.GetName(typeof(Enums.Section), Enums.Section.BO),
                SectionStatusCode = Enum.GetName(typeof(Enums.SectionStatus), Enums.SectionStatus.AP),
                UpdatedBy = GlobalVariables.LoggedInUsername,
                UpdatedOn = DateTime.UtcNow,
            };

            var (updateResult, updateMessage) = await _repository.UpdateStatusByCompanyId(updateModel);
            if (updateResult is null)
            {
                return _general.Response(null, 400, updateMessage, false);
            }

            var (companySectionObj, companySectionMessage) = await _repository.GetCompanySection(Enum.GetName(typeof(Enums.Section), Enums.Section.BO), companyId);
            if (!(companySectionObj is null))
            {
                await _kycLogService.CreateKYCLogs(new CreateKYCLogModel
                {
                    CompanyId = companyId,
                    CompanySectionsId = companySectionObj.CompanySectionsId,
                    Remarks = obj.Remarks,
                    SectionStatusCode = updateResult.SectionStatusCode
                });
            }

            string overallKYCStatus = await CheckCompanyKYCStatus(companyId);

            var data = new
            {
                OverallKYCStatus = overallKYCStatus,
                UltimateBeneficialOwners = updateResult
            };

            return _general.Response(data, 200, updateMessage, true);
        }

        public async Task<IGeneralModel> ApproveCompanyDirectorSection(ApproveCompanySectionModel obj)
        {
            if (Guid.Parse(obj.CompanyId) == Guid.Empty)
            {
                return _general.Response(null, 400, "Invalid company id", false);
            }

            int companyId = await _companyRepository.GetIdByGuid(Guid.Parse(obj.CompanyId));
            if (companyId == 0)
            {
                return _general.Response(null, 400, "Invalid company id", false);
            }

            var updateModel = new CompanySections
            {
                CompanyId = companyId,
                SectionCode = Enum.GetName(typeof(Enums.Section), Enums.Section.CD),
                SectionStatusCode = Enum.GetName(typeof(Enums.SectionStatus), Enums.SectionStatus.AP),
                UpdatedBy = GlobalVariables.LoggedInUsername,
                UpdatedOn = DateTime.UtcNow,
            };

            var (updateResult, updateMessage) = await _repository.UpdateStatusByCompanyId(updateModel);
            if (updateResult is null)
            {
                return _general.Response(null, 400, updateMessage, false);
            }

            var (companySectionObj, companySectionMessage) = await _repository.GetCompanySection(Enum.GetName(typeof(Enums.Section), Enums.Section.CD), companyId);
            if (!(companySectionObj is null))
            {
                await _kycLogService.CreateKYCLogs(new CreateKYCLogModel
                {
                    CompanyId = companyId,
                    CompanySectionsId = companySectionObj.CompanySectionsId,
                    Remarks = obj.Remarks,
                    SectionStatusCode = updateResult.SectionStatusCode
                });
            }

            string overallKYCStatus = await CheckCompanyKYCStatus(companyId);

            var data = new
            {
                OverallKYCStatus = overallKYCStatus,
                CompanyDirectors = updateResult
            };

            return _general.Response(data, 200, updateMessage, true);
        }

        public async Task<string> CheckCompanyKYCStatus(int companyId)
        {
            string kycStatus = await CheckOverallKYCStatus(companyId);
            int userId = await _userRepository.GetIdByUsername(GlobalVariables.LoggedInUsername);
            var (companyKYCStatus, message) = await _companyRepository.UpdateKYCStatus(companyId, kycStatus, userId);

            return companyKYCStatus;
        }

        public async Task<IGeneralModel> RejectCompanySection(RejectCompanySectionModel obj)
        {
            if (Guid.Parse(obj.CompanyId) == Guid.Empty)
            {
                return _general.Response(null, 400, "Invalid company GUID", false);
            }

            int companyId = await _companyRepository.GetIdByGuid(Guid.Parse(obj.CompanyId));
            if (companyId == 0)
            {
                return _general.Response(null, 400, "Invalid company GUID", false);
            }

            var rejectModel = new CompanySections
            {
                CompanyId = companyId,
                SectionStatusCode = Enum.GetName(typeof(Enums.SectionStatus), Enums.SectionStatus.RJ),
                UpdatedBy = GlobalVariables.LoggedInUsername,
                UpdatedOn = DateTime.UtcNow,
            };

            var (rejectResult, rejectMessage) = await _repository.RejectCompanySectionsByCompanyId(rejectModel);
            if (!(rejectResult))
            {
                return _general.Response(null, 400, rejectMessage, false);
            }

            var (updateKYCStatusReturn, updateKYCStatusMessage) = await _companyRepository.UpdateKYCStatus(companyId, Enum.GetName(typeof(Enums.KYCStatus), Enums.KYCStatus.REJ), GlobalVariables.LoggedInUserId);

            if (updateKYCStatusReturn is null)
            {
                return _general.Response(null, 400, updateKYCStatusMessage, false);
            }

            var (companySectionsResult, companySectionsMessage) = await _repository.GetListByCompanyId(companyId);
            if (companySectionsResult.Count != 0)
            {
                var listModel = new List<CreateKYCLogModel>();
                companySectionsResult.ForEach(e =>
                {
                    listModel.Add(new CreateKYCLogModel
                    {
                        CompanyId = companyId,
                        CompanySectionsId = e.CompanySectionsId,
                        Remarks = obj.Remarks,
                        SectionStatusCode = Enum.GetName(typeof(Enums.SectionStatus), Enums.SectionStatus.RJ)
                    });
                });

                var bulkModel = new BulkCreateKYCLogModel();
                bulkModel.KYCLogs = listModel;
                await _kycLogService.BulkCreateKYCLogs(bulkModel);
            }

            var data = new
            {
                OverallKYCStatus = updateKYCStatusReturn,
                Remarks = obj.Remarks
            };

            return _general.Response(data, 200, rejectMessage, true);
        }

        public async Task<IGeneralModel> ReviseCompanyStructureSection(ReviseCompanySectionModel obj)
        {
            if (Guid.Parse(obj.CompanyId) == Guid.Empty)
            {
                return _general.Response(null, 400, "Invalid company id", false);
            }

            int companyId = await _companyRepository.GetIdByGuid(Guid.Parse(obj.CompanyId));
            if (companyId == 0)
            {
                return _general.Response(null, 400, "Invalid company id", false);
            }

            var updateModel = new CompanySections
            {
                CompanyId = companyId,
                SectionCode = Enum.GetName(typeof(Enums.Section), Enums.Section.CS),
                SectionStatusCode = Enum.GetName(typeof(Enums.SectionStatus), Enums.SectionStatus.PR),
                UpdatedBy = GlobalVariables.LoggedInUsername,
                UpdatedOn = DateTime.UtcNow,
            };

            var (updateResult, updateMessage) = await _repository.UpdateStatusByCompanyId(updateModel);
            if (updateResult is null)
            {
                return _general.Response(null, 400, updateMessage, false);
            }

            var (companySectionObj, companySectionMessage) = await _repository.GetCompanySection(Enum.GetName(typeof(Enums.Section), Enums.Section.CS), companyId);
            if (!(companySectionObj is null))
            {
                await _kycLogService.CreateKYCLogs(new CreateKYCLogModel
                {
                    CompanyId = companyId,
                    CompanySectionsId = companySectionObj.CompanySectionsId,
                    Remarks = obj.Remarks,
                    SectionStatusCode = updateResult.SectionStatusCode
                });
            }

            string overallKYCStatus = await CheckCompanyKYCStatus(companyId);

            var data = new
            {
                OverallKYCStatus = overallKYCStatus,
                CompanyStructure = updateResult
            };

            return _general.Response(data, 200, updateMessage, true);
        }

        public async Task<IGeneralModel> ReviseCompanyBeneficialOwnerSection(ReviseCompanySectionModel obj)
        {
            if (Guid.Parse(obj.CompanyId) == Guid.Empty)
            {
                return _general.Response(null, 400, "Invalid company id", false);
            }

            int companyId = await _companyRepository.GetIdByGuid(Guid.Parse(obj.CompanyId));
            if (companyId == 0)
            {
                return _general.Response(null, 400, "Invalid company id", false);
            }

            var updateModel = new CompanySections
            {
                CompanyId = companyId,
                SectionCode = Enum.GetName(typeof(Enums.Section), Enums.Section.BO),
                SectionStatusCode = Enum.GetName(typeof(Enums.SectionStatus), Enums.SectionStatus.PR),
                UpdatedBy = GlobalVariables.LoggedInUsername,
                UpdatedOn = DateTime.UtcNow,
            };

            var (updateResult, updateMessage) = await _repository.UpdateStatusByCompanyId(updateModel);
            if (updateResult is null)
            {
                return _general.Response(null, 400, updateMessage, false);
            }

            var (companySectionObj, companySectionMessage) = await _repository.GetCompanySection(Enum.GetName(typeof(Enums.Section), Enums.Section.BO), companyId);
            if (!(companySectionObj is null))
            {
                await _kycLogService.CreateKYCLogs(new CreateKYCLogModel
                {
                    CompanyId = companyId,
                    CompanySectionsId = companySectionObj.CompanySectionsId,
                    Remarks = obj.Remarks,
                    SectionStatusCode = updateResult.SectionStatusCode
                });
            }

            string overallKYCStatus = await CheckCompanyKYCStatus(companyId);

            var data = new
            {
                OverallKYCStatus = overallKYCStatus,
                UltimateBeneficialOwners = updateResult
            };

            return _general.Response(data, 200, updateMessage, true);
        }

        public async Task<IGeneralModel> ReviseCompanyDirectorSection(ReviseCompanySectionModel obj)
        {
            if (Guid.Parse(obj.CompanyId) == Guid.Empty)
            {
                return _general.Response(null, 400, "Invalid company id", false);
            }

            int companyId = await _companyRepository.GetIdByGuid(Guid.Parse(obj.CompanyId));
            if (companyId == 0)
            {
                return _general.Response(null, 400, "Invalid company id", false);
            }

            var updateModel = new CompanySections
            {
                CompanyId = companyId,
                SectionCode = Enum.GetName(typeof(Enums.Section), Enums.Section.CD),
                SectionStatusCode = Enum.GetName(typeof(Enums.SectionStatus), Enums.SectionStatus.PR),
                UpdatedBy = GlobalVariables.LoggedInUsername,
                UpdatedOn = DateTime.UtcNow,
            };

            var (updateResult, updateMessage) = await _repository.UpdateStatusByCompanyId(updateModel);
            if (updateResult is null)
            {
                return _general.Response(null, 400, updateMessage, false);
            }

            var (companySectionObj, companySectionMessage) = await _repository.GetCompanySection(Enum.GetName(typeof(Enums.Section), Enums.Section.CD), companyId);
            if (!(companySectionObj is null))
            {
                await _kycLogService.CreateKYCLogs(new CreateKYCLogModel
                {
                    CompanyId = companyId,
                    CompanySectionsId = companySectionObj.CompanySectionsId,
                    Remarks = obj.Remarks,
                    SectionStatusCode = updateResult.SectionStatusCode
                });
            }

            string overallKYCStatus = await CheckCompanyKYCStatus(companyId);

            var data = new
            {
                OverallKYCStatus = overallKYCStatus,
                CompanyDirectors = updateResult
            };

            return _general.Response(data, 200, updateMessage, true);
        }
    }
}
