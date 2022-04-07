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
    public class KitDamageReasonService : IKitDamageReasonService
    {
        private readonly IKitDamageReasonRepository _kitDamageReasonRepository;
        private readonly IUserProviderService _userProviderService;

        public KitDamageReasonService(
            IKitDamageReasonRepository kitDamageReasonRepository,
            IUserProviderService userProviderService
        )
        {
            _kitDamageReasonRepository = kitDamageReasonRepository;
            _userProviderService = userProviderService;
        }

        public async Task<ResponseModel> CreateKitDamageReason(KitDamageReason entity)
        {
            var existEntity = await GetKitDamageReasonById(entity.KitDamageReasonId);
            if (existEntity.Success)
            {
                return new ResponseModel
                {
                    Success = false,
                    StatusCode = StatusCodes.Status409Conflict,
                    Message = "KitDamageReason already exists.",
                };
            }

            entity.CreatedBy = _userProviderService.UserClaim.UserId;
            entity.UpdatedBy = _userProviderService.UserClaim.UserId;
            await _kitDamageReasonRepository.AddAsync(entity);
            var result = await _kitDamageReasonRepository.SaveChangesAsync();
            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "KitDamageReason created successfully.",
                    Data = entity
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "KitDamageReason does not created." };
        }

        public async Task<ResponseModel> DeleteKitDamageReason(long id)
        {
            var entityResult = await GetKitDamageReasonById(id);

            if (!entityResult.Success) { return entityResult; }

            var entity = entityResult.Data as KitDamageReason;
            entity.UpdatedBy = _userProviderService.UserClaim.UserId;
            entity.IsDeleted = true;
            var result = await _kitDamageReasonRepository.SaveChangesAsync();

            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "KitDamageReason deleted successfully.",
                    Data = entity
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "KitDamageReason does not deleted." };
        }

        public async Task<ResponseModel> GetAllKitDamageReason(bool? isActive = null)
        {
            return new ResponseModel
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Data = await _kitDamageReasonRepository.Find(a => a.IsDeleted == false && (!isActive.HasValue || a.IsActive == isActive))
            };
        }

        public async Task<ResponseModel> GetKitDamageReasonPaged(Pagination pagination)
        {
            return new ResponseModel
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Data = await _kitDamageReasonRepository.GetKitDamageReasonPaged(pagination)
            };
        }

        public async Task<ResponseModel> GetKitDamageReasonById(long id)
        {
            var result = await _kitDamageReasonRepository.SingleOrDefaultAsync(a => a.KitDamageReasonId == id);
            if (result != null)
            {
                return new ResponseModel { Success = true, StatusCode = StatusCodes.Status200OK, Data = result };
            }
            else
            {
                return new ResponseModel { Success = false, StatusCode = StatusCodes.Status404NotFound, Message = "KitDamageReason does not exists." };
            }
        }

        public async Task<ResponseModel> UpdateKitDamageReason(KitDamageReason updateEntity)
        {
            var entityResult = await GetKitDamageReasonById(updateEntity.KitDamageReasonId);

            if (!entityResult.Success) { return entityResult; }

            var entity = entityResult.Data as KitDamageReason;
            entity.Reason = updateEntity.Reason;
            entity.SortOrder = updateEntity.SortOrder;
            entity.IsActive = updateEntity.IsActive;
            entity.UpdatedDate = CommonUtils.GetDefaultDateTime();
            entity.UpdatedBy = _userProviderService.UserClaim.UserId;

            var result = await _kitDamageReasonRepository.SaveChangesAsync();

            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "KitDamageReason updated successfully.",
                    Data = entity
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "KitDamageReason does not updated." };
        }

        public async Task<List<DropdownModel>> GetDropdwon(long? id = null, bool? isActive = null)
        {
            return (await _kitDamageReasonRepository.GetDropdwon(id, isActive)).Data;
        }
    }
}
