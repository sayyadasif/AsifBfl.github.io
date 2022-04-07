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
    public class SettingService : ISettingService
    {
        private readonly ISettingRepository _settingRepository;
        private readonly IUserProviderService _userProviderService;

        public SettingService(
            ISettingRepository settingRepository,
            IUserProviderService userProviderService
        )
        {
            _settingRepository = settingRepository;
            _userProviderService = userProviderService;
        }

        public async Task<ResponseModel> CreateSetting(Setting entity)
        {
            var existEntity = await GetSettingById(entity.SettingId);
            if (existEntity.Success)
            {
                return new ResponseModel
                {
                    Success = false,
                    StatusCode = StatusCodes.Status409Conflict,
                    Message = "Setting already exists.",
                };
            }

            entity.CreatedBy = _userProviderService.UserClaim.UserId;
            entity.UpdatedBy = _userProviderService.UserClaim.UserId;
            await _settingRepository.AddAsync(entity);
            var result = await _settingRepository.SaveChangesAsync();
            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Setting created successfully.",
                    Data = entity
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "Setting does not created." };
        }

        public async Task<ResponseModel> DeleteSetting(long id)
        {
            var entityResult = await GetSettingById(id);

            if (!entityResult.Success) { return entityResult; }

            var entity = entityResult.Data as Setting;
            entity.UpdatedBy = _userProviderService.UserClaim.UserId;
            entity.IsDeleted = true;
            var result = await _settingRepository.SaveChangesAsync();

            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Setting deleted successfully.",
                    Data = entity
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "Setting does not deleted." };
        }

        public async Task<ResponseModel> GetAllSetting(bool? isEditable = null)
        {
            return new ResponseModel
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Data = await _settingRepository.Find(a => !isEditable.HasValue || a.IsEditable == isEditable)
            };
        }

        public async Task<ResponseModel> GetSettingPaged(Pagination pagination)
        {
            return new ResponseModel
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Data = await _settingRepository.GetSettingPaged(pagination)
            };
        }

        public async Task<ResponseModel> GetSettingById(long id)
        {
            var result = await _settingRepository.SingleOrDefaultAsync(a => a.SettingId == id);
            if (result != null)
            {
                return new ResponseModel { Success = true, StatusCode = StatusCodes.Status200OK, Data = result };
            }
            else
            {
                return new ResponseModel { Success = false, StatusCode = StatusCodes.Status404NotFound, Message = "Setting does not exists." };
            }
        }

        public async Task<ResponseModel> UpdateSetting(Setting updateEntity)
        {
            var entityResult = await GetSettingById(updateEntity.SettingId);

            if (!entityResult.Success) { return entityResult; }

            var entity = entityResult.Data as Setting;
            entity.SettingName = updateEntity.SettingName;
            entity.SettingKey = updateEntity.SettingKey;
            entity.SettingValue = updateEntity.SettingValue;
            entity.SettingDesc = updateEntity.SettingDesc;
            entity.IsActive = updateEntity.IsActive;
            entity.UpdatedDate = CommonUtils.GetDefaultDateTime();
            entity.UpdatedBy = _userProviderService.UserClaim.UserId;

            var result = await _settingRepository.SaveChangesAsync();

            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Setting updated successfully.",
                    Data = entity
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "Setting does not updated." };
        }

        public async Task<ResponseModel> UpdateSettings(List<Setting> updateEntities)
        {
            var ids = updateEntities.Select(s => s.SettingId);
            var entities = await _settingRepository.Find(s => ids.Contains(s.SettingId));

            foreach (var entity in entities)
            {
                entity.SettingValue = updateEntities.FirstOrDefault(s => s.SettingId == entity.SettingId).SettingValue;
                entity.UpdatedDate = CommonUtils.GetDefaultDateTime();
                entity.UpdatedBy = _userProviderService.UserClaim.UserId;
            }

            var result = await _settingRepository.SaveChangesAsync();

            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Settings updated successfully."
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "Settings does not updated." };
        }

        public async Task<Setting> GetSettingsByKey(string key)
        {
            return await _settingRepository.GetSettingsByKey(key);
        }

    }
}
