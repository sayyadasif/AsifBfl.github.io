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
    public class DispatchStatusService : IDispatchStatusService
    {
        private readonly IDispatchStatusRepository _dispatchStatusRepository;
        private readonly IUserProviderService _userProviderService;

        public DispatchStatusService(
            IDispatchStatusRepository dispatchStatusRepository,
            IUserProviderService userProviderService
        )
        {
            _dispatchStatusRepository = dispatchStatusRepository;
            _userProviderService = userProviderService;
        }

        public async Task<ResponseModel> CreateDispatchStatus(DispatchStatus entity)
        {
            var existEntity = await GetDispatchStatusById(entity.DispatchStatusId);
            if (existEntity.Success)
            {
                return new ResponseModel
                {
                    Success = false,
                    StatusCode = StatusCodes.Status409Conflict,
                    Message = "DispatchStatus already exists.",
                };
            }

            entity.CreatedBy = _userProviderService.UserClaim.UserId;
            entity.UpdatedBy = _userProviderService.UserClaim.UserId;
            await _dispatchStatusRepository.AddAsync(entity);
            var result = await _dispatchStatusRepository.SaveChangesAsync();
            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "DispatchStatus created successfully.",
                    Data = entity
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "DispatchStatus does not created." };
        }

        public async Task<ResponseModel> DeleteDispatchStatus(long id)
        {
            var entityResult = await GetDispatchStatusById(id);

            if (!entityResult.Success) { return entityResult; }

            var entity = entityResult.Data as DispatchStatus;
            entity.UpdatedBy = _userProviderService.UserClaim.UserId;
            entity.IsDeleted = true;
            var result = await _dispatchStatusRepository.SaveChangesAsync();

            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "DispatchStatus deleted successfully.",
                    Data = entity
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "DispatchStatus does not deleted." };
        }

        public async Task<ResponseModel> GetAllDispatchStatus(bool? isActive = null)
        {
            return new ResponseModel
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Data = await _dispatchStatusRepository.Find(a => a.IsDeleted == false && (!isActive.HasValue || a.IsActive == isActive))
            };
        }

        public async Task<ResponseModel> GetDispatchStatusPaged(Pagination pagination)
        {
            return new ResponseModel
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Data = await _dispatchStatusRepository.GetDispatchStatusPaged(pagination)
            };
        }

        public async Task<ResponseModel> GetDispatchStatusById(long id)
        {
            var result = await _dispatchStatusRepository.SingleOrDefaultAsync(a => a.DispatchStatusId == id);
            if (result != null)
            {
                return new ResponseModel { Success = true, StatusCode = StatusCodes.Status200OK, Data = result };
            }
            else
            {
                return new ResponseModel { Success = false, StatusCode = StatusCodes.Status404NotFound, Message = "DispatchStatus does not exists." };
            }
        }

        public async Task<ResponseModel> UpdateDispatchStatus(DispatchStatus updateEntity)
        {
            var entityResult = await GetDispatchStatusById(updateEntity.DispatchStatusId);

            if (!entityResult.Success) { return entityResult; }

            var entity = entityResult.Data as DispatchStatus;
            entity.StatusName = updateEntity.StatusName;
            entity.StatusKey = updateEntity.StatusKey;
            entity.SortOrder = updateEntity.SortOrder;
            entity.IsActive = updateEntity.IsActive;
            entity.UpdatedDate = CommonUtils.GetDefaultDateTime();
            entity.UpdatedBy = _userProviderService.UserClaim.UserId;

            var result = await _dispatchStatusRepository.SaveChangesAsync();

            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "DispatchStatus updated successfully.",
                    Data = entity
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "DispatchStatus does not updated." };
        }

        public async Task<List<DropdownModel>> GetDropdwon(long? id = null, bool? isActive = null)
        {
            return (await _dispatchStatusRepository.GetDropdwon(id, isActive)).Data;
        }
    }
}
