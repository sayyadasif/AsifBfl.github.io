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
    public class IndentStatusService : IIndentStatusService
    {
        private readonly IIndentStatusRepository _indentStatusRepository;
        private readonly IUserProviderService _userProviderService;

        public IndentStatusService(
            IIndentStatusRepository indentStatusRepository,
            IUserProviderService userProviderService
        )
        {
            _indentStatusRepository = indentStatusRepository;
            _userProviderService = userProviderService;
        }

        public async Task<ResponseModel> CreateIndentStatus(IndentStatus entity)
        {
            var existEntity = await GetIndentStatusById(entity.IndentStatusId);
            if (existEntity.Success)
            {
                return new ResponseModel
                {
                    Success = false,
                    StatusCode = StatusCodes.Status409Conflict,
                    Message = "IndentStatus already exists.",
                };
            }

            entity.CreatedBy = _userProviderService.UserClaim.UserId;
            entity.UpdatedBy = _userProviderService.UserClaim.UserId;
            await _indentStatusRepository.AddAsync(entity);
            var result = await _indentStatusRepository.SaveChangesAsync();
            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "IndentStatus created successfully.",
                    Data = entity
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "IndentStatus does not created." };
        }

        public async Task<ResponseModel> DeleteIndentStatus(long id)
        {
            var entityResult = await GetIndentStatusById(id);

            if (!entityResult.Success) { return entityResult; }

            var entity = entityResult.Data as IndentStatus;
            entity.UpdatedBy = _userProviderService.UserClaim.UserId;
            entity.IsDeleted = true;
            var result = await _indentStatusRepository.SaveChangesAsync();

            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "IndentStatus deleted successfully.",
                    Data = entity
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "IndentStatus does not deleted." };
        }

        public async Task<ResponseModel> GetAllIndentStatus(bool? isActive = null)
        {
            return new ResponseModel
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Data = await _indentStatusRepository.Find(a => a.IsDeleted == false && (!isActive.HasValue || a.IsActive == isActive))
            };
        }

        public async Task<ResponseModel> GetIndentStatusPaged(Pagination pagination)
        {
            return new ResponseModel
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Data = await _indentStatusRepository.GetIndentStatusPaged(pagination)
            };
        }

        public async Task<ResponseModel> GetIndentStatusById(long id)
        {
            var result = await _indentStatusRepository.SingleOrDefaultAsync(a => a.IndentStatusId == id);
            if (result != null)
            {
                return new ResponseModel { Success = true, StatusCode = StatusCodes.Status200OK, Data = result };
            }
            else
            {
                return new ResponseModel { Success = false, StatusCode = StatusCodes.Status404NotFound, Message = "IndentStatus does not exists." };
            }
        }

        public async Task<ResponseModel> UpdateIndentStatus(IndentStatus updateEntity)
        {
            var entityResult = await GetIndentStatusById(updateEntity.IndentStatusId);

            if (!entityResult.Success) { return entityResult; }

            var entity = entityResult.Data as IndentStatus;
            entity.StatusName = updateEntity.StatusName;
            entity.StatusKey = updateEntity.StatusKey;
            entity.SortOrder = updateEntity.SortOrder;
            entity.IsActive = updateEntity.IsActive;
            entity.UpdatedDate = CommonUtils.GetDefaultDateTime();
            entity.UpdatedBy = _userProviderService.UserClaim.UserId;

            var result = await _indentStatusRepository.SaveChangesAsync();

            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "IndentStatus updated successfully.",
                    Data = entity
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "IndentStatus does not updated." };
        }

        public async Task<List<DropdownModel>> GetDropdwon(long? id = null, bool? isActive = null)
        {
            return (await _indentStatusRepository.GetDropdwon(id, isActive)).Data;
        }
    }
}
