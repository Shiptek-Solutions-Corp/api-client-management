using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using xgca.core.Response;
using xgca.data.Repositories;

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
            return null;
        }
    }
}
