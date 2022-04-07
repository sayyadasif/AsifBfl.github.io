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
    public class NotificationTypeService : INotificationTypeService
    {
        private readonly INotificationTypeRepository _notificationTypeRepository;
        private readonly IUserProviderService _userProviderService;

        public NotificationTypeService(
            INotificationTypeRepository notificationTypeRepository,
            IUserProviderService userProviderService
        )
        {
            _notificationTypeRepository = notificationTypeRepository;
            _userProviderService = userProviderService;
        }

        public async Task<ResponseModel> CreateNotificationType(NotificationType entity)
        {
            entity.CreatedBy = _userProviderService.UserClaim.UserId;
            entity.UpdatedBy = _userProviderService.UserClaim.UserId;
            await _notificationTypeRepository.AddAsync(entity);
            var result = await _notificationTypeRepository.SaveChangesAsync();
            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Notification Type created successfully.",
                    Data = entity
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "Notification Type does not created." };
        }

        public async Task<ResponseModel> DeleteNotificationType(long id)
        {
            var entityResult = await GetNotificationTypeById(id);

            if (!entityResult.Success) { return entityResult; }

            var entity = entityResult.Data as NotificationType;
            entity.UpdatedBy = _userProviderService.UserClaim.UserId;
            entity.IsDeleted = true;
            var result = await _notificationTypeRepository.SaveChangesAsync();

            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Notification Type deleted successfully.",
                    Data = entity
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "Notification Type does not deleted." };
        }

        public async Task<ResponseModel> GetAllNotificationType(bool? isActive = null)
        {
            return new ResponseModel
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Data = await _notificationTypeRepository.Find(a => a.IsDeleted == false && (!isActive.HasValue || a.IsActive == isActive))
            };
        }

        public async Task<ResponseModel> GetNotificationTypePaged(Pagination pagination)
        {
            return new ResponseModel
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Data = await _notificationTypeRepository.GetNotificationTypePaged(pagination)
            };
        }

        public async Task<ResponseModel> GetNotificationTypeById(long id)
        {
            var result = await _notificationTypeRepository.SingleOrDefaultAsync(a => a.NotificationTypeId == id);
            if (result != null)
            {
                return new ResponseModel { Success = true, StatusCode = StatusCodes.Status200OK, Data = result };
            }
            else
            {
                return new ResponseModel { Success = false, StatusCode = StatusCodes.Status404NotFound, Message = "Notification Type does not exists." };
            }
        }

        public async Task<ResponseModel> UpdateNotificationType(NotificationType updateEntity)
        {
            var entityResult = await GetNotificationTypeById(updateEntity.NotificationTypeId);

            if (!entityResult.Success) { return entityResult; }

            var entity = entityResult.Data as NotificationType;
            entity.IsActive = updateEntity.IsActive;
            entity.UpdatedDate = CommonUtils.GetDefaultDateTime();
            entity.UpdatedBy = _userProviderService.UserClaim.UserId;

            var result = await _notificationTypeRepository.SaveChangesAsync();

            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Notification Type updated successfully.",
                    Data = entity
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "Notification Type does not updated." };
        }
    }
}
