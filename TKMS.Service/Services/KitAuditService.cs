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
    public class KitAuditService : IKitAuditService
    {
        private readonly IKitAuditRepository _kitAuditRepository;
        private readonly IUserProviderService _userProviderService;

        public KitAuditService(
            IKitAuditRepository kitAuditRepository,
            IUserProviderService userProviderService
        )
        {
            _kitAuditRepository = kitAuditRepository;
            _userProviderService = userProviderService;
        }

        public async Task<ResponseModel> CreateKitAudit(KitAudit entity)
        {
            var existEntity = await GetKitAuditById(entity.KitAuditId);
            if (existEntity.Success)
            {
                return new ResponseModel
                {
                    Success = false,
                    StatusCode = StatusCodes.Status409Conflict,
                    Message = "KitAudit already exists.",
                };
            }

            entity.CreatedBy = _userProviderService.UserClaim.UserId;
            entity.UpdatedBy = _userProviderService.UserClaim.UserId;
            await _kitAuditRepository.AddAsync(entity);
            var result = await _kitAuditRepository.SaveChangesAsync();
            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "KitAudit created successfully.",
                    Data = entity
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "KitAudit does not created." };
        }

        public async Task<ResponseModel> DeleteKitAudit(long id)
        {
            var entityResult = await GetKitAuditById(id);

            if (!entityResult.Success) { return entityResult; }

            var entity = entityResult.Data as KitAudit;
            entity.UpdatedBy = _userProviderService.UserClaim.UserId;
            entity.IsDeleted = true;
            var result = await _kitAuditRepository.SaveChangesAsync();

            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "KitAudit deleted successfully.",
                    Data = entity
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "KitAudit does not deleted." };
        }

        public async Task<ResponseModel> GetAllKitAudit(bool? isActive = null)
        {
            return new ResponseModel
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Data = await _kitAuditRepository.Find(a => a.IsDeleted == false && (!isActive.HasValue || a.IsActive == isActive))
            };
        }

        public async Task<ResponseModel> GetKitAuditPaged(Pagination pagination)
        {
            return new ResponseModel
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Data = await _kitAuditRepository.GetKitAuditPaged(pagination)
            };
        }

        public async Task<ResponseModel> GetKitAuditById(long id)
        {
            var result = await _kitAuditRepository.SingleOrDefaultAsync(a => a.KitAuditId == id);
            if (result != null)
            {
                return new ResponseModel { Success = true, StatusCode = StatusCodes.Status200OK, Data = result };
            }
            else
            {
                return new ResponseModel { Success = false, StatusCode = StatusCodes.Status404NotFound, Message = "KitAudit does not exists." };
            }
        }

        public async Task<ResponseModel> UpdateKitAudit(KitAudit updateEntity)
        {
            var entityResult = await GetKitAuditById(updateEntity.KitAuditId);

            if (!entityResult.Success) { return entityResult; }

            var entity = entityResult.Data as KitAudit;
            entity.IsActive = updateEntity.IsActive;
            entity.UpdatedDate = CommonUtils.GetDefaultDateTime();
            entity.UpdatedBy = _userProviderService.UserClaim.UserId;

            var result = await _kitAuditRepository.SaveChangesAsync();

            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "KitAudit updated successfully.",
                    Data = entity
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "KitAudit does not updated." };
        }

        public async Task<List<DropdownModel>> GetDropdwon(long? id = null, bool? isActive = null)
        {
            return (await _kitAuditRepository.GetDropdwon(id, isActive)).Data;
        }

        public async Task<KitAudit> GetKitLastDestruction(long kitId)
        {
            return await _kitAuditRepository.GetKitLastDestruction(kitId);
        }
    }
}
