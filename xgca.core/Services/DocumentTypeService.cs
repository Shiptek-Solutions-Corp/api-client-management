using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using xgca.core.Response;
using xgca.data.Repositories;
using System.Linq;
using xgca.core.Models.DocumentType;

namespace xgca.core.Services
{
    public interface IDocumentTypeService
    {
        Task<IGeneralModel> GetAllBRC();
    }
    public class DocumentTypeService : IDocumentTypeService
    {
        private readonly IMapper _mapper;
        private readonly IDocumentTypeRepository _repository;
        private readonly IGeneral _general;

        public DocumentTypeService(IMapper _mapper, IDocumentTypeRepository _repository, IGeneral _general)
        {
            this._mapper = _mapper;
            this._repository = _repository;
            this._general = _general;
        }

        public async Task<IGeneralModel> GetAllBRC()
        {
            var (returnObj, returnMessage) = await _repository.ListAllBRC();

            if (returnObj is null)
            {
                return _general.Response(null, 200, "No document types available", true);
            }

            var displayListModel = returnObj.Select(d => _mapper.Map<GetDocumentTypeModel>(d));
            return _general.Response(new { DocumentTypes = displayListModel }, 200, "Document Types listed", true);
        }
    }
}
