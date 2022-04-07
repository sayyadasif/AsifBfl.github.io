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
    public class IndentAuditService : IIndentAuditService
    {
        private readonly IIndentAuditRepository _indentAuditRepository;
        private readonly IUserProviderService _userProviderService;

        public IndentAuditService(
            IIndentAuditRepository indentAuditRepository,
            IUserProviderService userProviderService
        )
        {
            _indentAuditRepository = indentAuditRepository;
            _userProviderService = userProviderService;
        }

        public async Task<ResponseModel> CreateIndentAudit(IndentAudit entity)
        {
            var existEntity = await GetIndentAuditById(entity.IndentAuditId);
            if (existEntity.Success)
            {
                return new ResponseModel
                {
                    Success = false,
                    StatusCode = StatusCodes.Status409Conflict,
                    Message = "IndentAudit already exists.",
                };
            }

            entity.CreatedBy = _userProviderService.UserClaim.UserId;
            entity.UpdatedBy = _userProviderService.UserClaim.UserId;
            await _indentAuditRepository.AddAsync(entity);
            var result = await _indentAuditRepository.SaveChangesAsync();
            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "IndentAudit created successfully.",
                    Data = entity
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "IndentAudit does not created." };
        }

        public async Task<ResponseModel> DeleteIndentAudit(long id)
        {
            var entityResult = await GetIndentAuditById(id);

            if (!entityResult.Success) { return entityResult; }

            var entity = entityResult.Data as IndentAudit;
            entity.UpdatedBy = _userProviderService.UserClaim.UserId;
            entity.IsDeleted = true;
            var result = await _indentAuditRepository.SaveChangesAsync();

            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "IndentAudit deleted successfully.",
                    Data = entity
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "IndentAudit does not deleted." };
        }

        public async Task<ResponseModel> GetAllIndentAudit(bool? isActive = null)
        {
            return new ResponseModel
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Data = await _indentAuditRepository.Find(a => a.IsDeleted == false && (!isActive.HasValue || a.IsActive == isActive))
            };
        }

        public async Task<ResponseModel> GetIndentAuditPaged(Pagination pagination)
        {
            return new ResponseModel
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Data = await _indentAuditRepository.GetIndentAuditPaged(pagination)
            };
        }

        public async Task<ResponseModel> GetIndentAuditById(long id)
        {
            var result = await _indentAuditRepository.SingleOrDefaultAsync(a => a.IndentAuditId == id);
            if (result != null)
            {
                return new ResponseModel { Success = true, StatusCode = StatusCodes.Status200OK, Data = result };
            }
            else
            {
                return new ResponseModel { Success = false, StatusCode = StatusCodes.Status404NotFound, Message = "IndentAudit does not exists." };
            }
        }

        public async Task<ResponseModel> UpdateIndentAudit(IndentAudit updateEntity)
        {
            var entityResult = await GetIndentAuditById(updateEntity.IndentAuditId);

            if (!entityResult.Success) { return entityResult; }

            var entity = entityResult.Data as IndentAudit;

            var result = await _indentAuditRepository.SaveChangesAsync();

            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "IndentAudit updated successfully.",
                    Data = entity
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "IndentAudit does not updated." };
        }
    }
}
