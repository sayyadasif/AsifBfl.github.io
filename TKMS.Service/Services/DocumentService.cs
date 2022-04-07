using Core.Repository.Models;
using Core.Security;
using Core.Utility.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TKMS.Abstraction.ComplexModels;
using TKMS.Abstraction.Models;
using TKMS.Repository.Interfaces;
using TKMS.Service.Interfaces;

namespace TKMS.Service.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly IUserProviderService _userProviderService;

        public DocumentService(
            IDocumentRepository documentRepository,
            IUserProviderService userProviderService
        )
        {
            _documentRepository = documentRepository;
            _userProviderService = userProviderService;
        }

        public async Task<ResponseModel> CreateDocument(Document entity)
        {
            var existEntity = await GetDocumentById(entity.DocumentId);
            if (existEntity.Success)
            {
                return new ResponseModel
                {
                    Success = false,
                    StatusCode = StatusCodes.Status409Conflict,
                    Message = "Document already exists.",
                };
            }

            entity.CreatedBy = _userProviderService.UserClaim.UserId;
            entity.UpdatedBy = _userProviderService.UserClaim.UserId;
            await _documentRepository.AddAsync(entity);
            var result = await _documentRepository.SaveChangesAsync();
            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Document created successfully.",
                    Data = entity
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "Document does not created." };
        }

        public async Task<ResponseModel> DeleteDocument(long id)
        {
            var entityResult = await GetDocumentById(id);

            if (!entityResult.Success) { return entityResult; }

            var entity = entityResult.Data as Document;
            entity.UpdatedBy = _userProviderService.UserClaim.UserId;
            entity.IsDeleted = true;
            var result = await _documentRepository.SaveChangesAsync();

            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Document deleted successfully.",
                    Data = entity
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "Document does not deleted." };
        }

        public async Task<ResponseModel> GetAllDocument(bool? isActive = null)
        {
            return new ResponseModel
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Data = await _documentRepository.Find(a => a.IsDeleted == false && (!isActive.HasValue || a.IsActive == isActive))
            };
        }

        public async Task<ResponseModel> GetDocumentPaged(Pagination pagination)
        {
            return new ResponseModel
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Data = await _documentRepository.GetDocumentPaged(pagination)
            };
        }

        public async Task<ResponseModel> GetDocumentById(long id)
        {
            var result = await _documentRepository.SingleOrDefaultAsync(a => a.DocumentId == id);
            if (result != null)
            {
                return new ResponseModel { Success = true, StatusCode = StatusCodes.Status200OK, Data = result };
            }
            else
            {
                return new ResponseModel { Success = false, StatusCode = StatusCodes.Status404NotFound, Message = "Document does not exists." };
            }
        }

        public async Task<ResponseModel> UpdateDocument(Document updateEntity)
        {
            var entityResult = await GetDocumentById(updateEntity.DocumentId);

            if (!entityResult.Success) { return entityResult; }

            var entity = entityResult.Data as Document;
            entity.DocumentTypeId = updateEntity.DocumentTypeId;
            entity.DocumentType = updateEntity.DocumentType;
            entity.FileName = updateEntity.FileName;
            entity.FilePath = updateEntity.FilePath;
            entity.ContentType = updateEntity.ContentType;
            entity.IsActive = updateEntity.IsActive;
            entity.UpdatedDate = CommonUtils.GetDefaultDateTime();
            entity.UpdatedBy = _userProviderService.UserClaim.UserId;

            var result = await _documentRepository.SaveChangesAsync();

            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Document updated successfully.",
                    Data = entity
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "Document does not updated." };
        }
    }
}
