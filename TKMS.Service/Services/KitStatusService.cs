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
    public class KitStatusService : IKitStatusService
    {
        private readonly IKitStatusRepository _kitStatusRepository;
        private readonly IUserProviderService _userProviderService;

        public KitStatusService(
            IKitStatusRepository kitStatusRepository,
            IUserProviderService userProviderService
        )
        {
            _kitStatusRepository = kitStatusRepository;
            _userProviderService = userProviderService;
        }

        public async Task<ResponseModel> CreateKitStatus(KitStatus entity)
        {
            var existEntity = await GetKitStatusById(entity.KitStatusId);
            if (existEntity.Success)
            {
                return new ResponseModel
                {
                    Success = false,
                    StatusCode = StatusCodes.Status409Conflict,
                    Message = "KitStatus already exists.",
                };
            }

            entity.CreatedBy = _userProviderService.UserClaim.UserId;
            entity.UpdatedBy = _userProviderService.UserClaim.UserId;
            await _kitStatusRepository.AddAsync(entity);
            var result = await _kitStatusRepository.SaveChangesAsync();
            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "KitStatus created successfully.",
                    Data = entity
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "KitStatus does not created." };
        }

        public async Task<ResponseModel> DeleteKitStatus(long id)
        {
            var entityResult = await GetKitStatusById(id);

            if (!entityResult.Success) { return entityResult; }

            var entity = entityResult.Data as KitStatus;
            entity.UpdatedBy = _userProviderService.UserClaim.UserId;
            entity.IsDeleted = true;
            var result = await _kitStatusRepository.SaveChangesAsync();

            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "KitStatus deleted successfully.",
                    Data = entity
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "KitStatus does not deleted." };
        }

        public async Task<ResponseModel> GetAllKitStatus(bool? isActive = null)
        {
            return new ResponseModel
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Data = await _kitStatusRepository.Find(a => a.IsDeleted == false && (!isActive.HasValue || a.IsActive == isActive))
            };
        }

        public async Task<ResponseModel> GetKitStatusPaged(Pagination pagination)
        {
            return new ResponseModel
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Data = await _kitStatusRepository.GetKitStatusPaged(pagination)
            };
        }

        public async Task<ResponseModel> GetKitStatusById(long id)
        {
            var result = await _kitStatusRepository.SingleOrDefaultAsync(a => a.KitStatusId == id);
            if (result != null)
            {
                return new ResponseModel { Success = true, StatusCode = StatusCodes.Status200OK, Data = result };
            }
            else
            {
                return new ResponseModel { Success = false, StatusCode = StatusCodes.Status404NotFound, Message = "KitStatus does not exists." };
            }
        }

        public async Task<ResponseModel> UpdateKitStatus(KitStatus updateEntity)
        {
            var entityResult = await GetKitStatusById(updateEntity.KitStatusId);

            if (!entityResult.Success) { return entityResult; }

            var entity = entityResult.Data as KitStatus;
            entity.StatusName = updateEntity.StatusName;
            entity.StatusKey = updateEntity.StatusKey;
            entity.SortOrder = updateEntity.SortOrder;
            entity.IsActive = updateEntity.IsActive;
            entity.UpdatedDate = CommonUtils.GetDefaultDateTime();
            entity.UpdatedBy = _userProviderService.UserClaim.UserId;

            var result = await _kitStatusRepository.SaveChangesAsync();

            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "KitStatus updated successfully.",
                    Data = entity
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "KitStatus does not updated." };
        }

        public async Task<List<DropdownModel>> GetDropdwon(long? id = null, bool? isActive = null)
        {
            return (await _kitStatusRepository.GetDropdwon(id, isActive)).Data;
        }
    }
}
