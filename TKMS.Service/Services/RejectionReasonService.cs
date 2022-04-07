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
    public class RejectionReasonService : IRejectionReasonService
    {
        private readonly IRejectionReasonRepository _rejectionReasonRepository;
        private readonly IUserProviderService _userProviderService;

        public RejectionReasonService(
            IRejectionReasonRepository rejectionReasonRepository,
            IUserProviderService userProviderService
        )
        {
            _rejectionReasonRepository = rejectionReasonRepository;
            _userProviderService = userProviderService;
        }

        public async Task<ResponseModel> CreateRejectionReason(RejectionReason entity)
        {
            var existEntity = await GetRejectionReasonById(entity.RejectionReasonId);
            if (existEntity.Success)
            {
                return new ResponseModel
                {
                    Success = false,
                    StatusCode = StatusCodes.Status409Conflict,
                    Message = "RejectionReason already exists.",
                };
            }

            entity.CreatedBy = _userProviderService.UserClaim.UserId;
            entity.UpdatedBy = _userProviderService.UserClaim.UserId;
            await _rejectionReasonRepository.AddAsync(entity);
            var result = await _rejectionReasonRepository.SaveChangesAsync();
            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "RejectionReason created successfully.",
                    Data = entity
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "RejectionReason does not created." };
        }

        public async Task<ResponseModel> DeleteRejectionReason(long id)
        {
            var entityResult = await GetRejectionReasonById(id);

            if (!entityResult.Success) { return entityResult; }

            var entity = entityResult.Data as RejectionReason;
            entity.UpdatedBy = _userProviderService.UserClaim.UserId;
            entity.IsDeleted = true;
            var result = await _rejectionReasonRepository.SaveChangesAsync();

            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "RejectionReason deleted successfully.",
                    Data = entity
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "RejectionReason does not deleted." };
        }

        public async Task<ResponseModel> GetAllRejectionReason(bool? isActive = null)
        {
            return new ResponseModel
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Data = await _rejectionReasonRepository.Find(a => a.IsDeleted == false && (!isActive.HasValue || a.IsActive == isActive))
            };
        }

        public async Task<ResponseModel> GetRejectionReasonPaged(Pagination pagination)
        {
            return new ResponseModel
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Data = await _rejectionReasonRepository.GetRejectionReasonPaged(pagination)
            };
        }

        public async Task<ResponseModel> GetRejectionReasonById(long id)
        {
            var result = await _rejectionReasonRepository.SingleOrDefaultAsync(a => a.RejectionReasonId == id);
            if (result != null)
            {
                return new ResponseModel { Success = true, StatusCode = StatusCodes.Status200OK, Data = result };
            }
            else
            {
                return new ResponseModel { Success = false, StatusCode = StatusCodes.Status404NotFound, Message = "RejectionReason does not exists." };
            }
        }

        public async Task<ResponseModel> UpdateRejectionReason(RejectionReason updateEntity)
        {
            var entityResult = await GetRejectionReasonById(updateEntity.RejectionReasonId);

            if (!entityResult.Success) { return entityResult; }

            var entity = entityResult.Data as RejectionReason;
            entity.Reason = updateEntity.Reason;
            entity.SortOrder = updateEntity.SortOrder;
            entity.IsActive = updateEntity.IsActive;
            entity.UpdatedDate = CommonUtils.GetDefaultDateTime();
            entity.UpdatedBy = _userProviderService.UserClaim.UserId;

            var result = await _rejectionReasonRepository.SaveChangesAsync();

            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "RejectionReason updated successfully.",
                    Data = entity
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "RejectionReason does not updated." };
        }

        public async Task<List<DropdownModel>> GetDropdwon(long? id = null, bool? isActive = null)
        {
            return (await _rejectionReasonRepository.GetDropdwon(id, isActive)).Data;
        }
    }
}
