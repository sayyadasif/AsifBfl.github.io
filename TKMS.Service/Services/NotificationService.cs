using Core.Repository.Models;
using Core.Security;
using Core.Utility.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TKMS.Abstraction.ComplexModels;
using TKMS.Abstraction.Constants;
using TKMS.Abstraction.Enums;
using TKMS.Abstraction.Models;
using TKMS.Repository.Interfaces;
using TKMS.Service.Interfaces;

namespace TKMS.Service.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IUserProviderService _userProviderService;
        private readonly ISettingService _settingService;
        private readonly INotificationTypeService _notificationTypeService;

        public NotificationService(
            INotificationRepository notificationRepository,
            IUserProviderService userProviderService,
            ISettingService settingService,
            INotificationTypeService notificationTypeService
        )
        {
            _notificationRepository = notificationRepository;
            _userProviderService = userProviderService;
            _settingService = settingService;
            _notificationTypeService = notificationTypeService;
        }

        public async Task<ResponseModel> CreateNotification(Notification entity)
        {
            entity.CreatedBy = _userProviderService.UserClaim.UserId;
            entity.UpdatedBy = _userProviderService.UserClaim.UserId;
            await _notificationRepository.AddAsync(entity);
            var result = await _notificationRepository.SaveChangesAsync();
            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Notification created successfully.",
                    Data = entity
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "Notification does not created." };
        }

        public async Task<ResponseModel> CreateNotifications(List<Notification> entities)
        {
            foreach (var entity in entities)
            {
                entity.CreatedBy = _userProviderService.UserClaim.UserId;
                entity.UpdatedBy = _userProviderService.UserClaim.UserId;
                await _notificationRepository.AddAsync(entity);
            }
            var result = await _notificationRepository.SaveChangesAsync();
            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Notifications created successfully."
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "Notifications does not created." };
        }

        public async Task<ResponseModel> DeleteNotification(long id)
        {
            var entityResult = await GetNotificationById(id);

            if (!entityResult.Success) { return entityResult; }

            var entity = entityResult.Data as Notification;
            entity.UpdatedBy = _userProviderService.UserClaim.UserId;
            entity.IsDeleted = true;
            var result = await _notificationRepository.SaveChangesAsync();

            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Notification deleted successfully.",
                    Data = entity
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "Notification does not deleted." };
        }

        public async Task<ResponseModel> GetAllNotification(bool? isActive = null)
        {
            return new ResponseModel
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Data = await _notificationRepository.Find(a => a.IsDeleted == false && (!isActive.HasValue || a.IsActive == isActive))
            };
        }

        public async Task<List<NotificationModel>> GetUserNotifications(bool? isActive = true)
        {
            dynamic filters = new ExpandoObject();
            filters.isActive = isActive;

#if DEBUG
#else
            filters.bfilBranchId = _userProviderService.UserClaim.BranchId;
#endif

            var notificationResult = await _notificationRepository.GetNotificationPaged(
                new Pagination
                {
                    SortOrderBy = "desc",
                    SortOrderColumn = "NotificationDate",
                    Filters = filters
                });

            var notificationResults = notificationResult.Data as List<NotificationModel>;

            var notifications = notificationResults.Where(n => _userProviderService.UserClaim.RoleIds.Any(ur => n.VisibleRoles.Contains(ur))).ToList();

            return notifications;
        }

        public async Task<ResponseModel> GetNotificationPaged(Pagination pagination)
        {
            return new ResponseModel
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Data = await _notificationRepository.GetNotificationPaged(pagination)
            };
        }

        public async Task<ResponseModel> GetNotificationById(long id)
        {
            var result = await _notificationRepository.SingleOrDefaultAsync(a => a.NotificationId == id);
            if (result != null)
            {
                return new ResponseModel { Success = true, StatusCode = StatusCodes.Status200OK, Data = result };
            }
            else
            {
                return new ResponseModel { Success = false, StatusCode = StatusCodes.Status404NotFound, Message = "Notification does not exists." };
            }
        }

        public async Task<ResponseModel> UpdateNotification(Notification updateEntity)
        {
            Notification entity = null;
            if (updateEntity.DispatchId.HasValue)
            {
                entity = await _notificationRepository.SingleOrDefaultAsync(n =>
                            n.IsActive &&
                            n.DispatchId == updateEntity.DispatchId &&
                            n.NotificationTypeId == updateEntity.NotificationTypeId);
            }
            if (updateEntity.KitId.HasValue)
            {
                entity = await _notificationRepository.SingleOrDefaultAsync(n =>
                            n.IsActive &&
                            n.KitId == updateEntity.KitId &&
                            n.NotificationTypeId == updateEntity.NotificationTypeId);
            }

            if (entity != null)
            {
                entity.IsActive = false;
                entity.UpdatedDate = CommonUtils.GetDefaultDateTime();
                entity.UpdatedBy = _userProviderService.UserClaim.UserId;

                var result = await _notificationRepository.SaveChangesAsync();

                if (result > 0)
                {
                    return new ResponseModel
                    {
                        Success = true,
                        StatusCode = StatusCodes.Status200OK,
                        Message = "Notification updated successfully.",
                        Data = entity
                    };
                }
            }

            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "Notification does not updated." };
        }

        public async Task<ResponseModel> UpdateNotifications(List<Notification> updateEntities, long notificationTypeId)
        {
            var kits = updateEntities.Select(n => n.KitId).ToList();

            var notifications = await _notificationRepository.Find(n => kits.Contains(n.KitId) && n.IsActive && n.NotificationTypeId == notificationTypeId);

            foreach (var entity in notifications)
            {

                entity.IsActive = false;
                entity.UpdatedDate = CommonUtils.GetDefaultDateTime();
                entity.UpdatedBy = _userProviderService.UserClaim.UserId;
            }

            var result = await _notificationRepository.SaveChangesAsync();

            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Notifications updated successfully."
                };
            }

            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "Notifications does not updated." };
        }

        public async Task GenerateNotifactions()
        {

            var notificationTypeResult = await _notificationTypeService.GetAllNotificationType();
            var notificationTypes = notificationTypeResult.Data as List<NotificationType>;

            var notifications = new List<Notification>();

            var receiptConfirmationBm = notificationTypes.FirstOrDefault(n => n.NotificationTypeId == 1);
            var receiptConfirmationRo = notificationTypes.FirstOrDefault(n => n.NotificationTypeId == 2);

            var disptaches = await DispatchNotifications();
            disptaches.ForEach(d =>
            {
                notifications.Add(GetNotification(
                    receiptConfirmationBm,
                    string.Format(receiptConfirmationBm.NotificationTemplate, d.IndentNo, CommonUtils.GetFormatedDate(d.BranchDispatchDate.Value), d.ReferenceNo),
                    UserNotificationTypes.ReceiptConfirmation,
                    d.BranchId,
                    d.DispatchId,
                    null,
                    d.DispatchId
                    ));

                notifications.Add(GetNotification(
                    receiptConfirmationRo,
                    string.Format(receiptConfirmationRo.NotificationTemplate, d.IndentNo, CommonUtils.GetFormatedDate(d.BranchDispatchDate.Value), d.ReferenceNo),
                    UserNotificationTypes.ReceiptConfirmation,
                    d.BranchId,
                    d.DispatchId,
                    null,
                    d.DispatchId
                    ));
            });

            var scanKitConfirmation = notificationTypes.FirstOrDefault(n => n.NotificationTypeId == 3);

            var scans = await ScanNotifications();
            scans.ForEach(d =>
            {
                notifications.Add(GetNotification(
                    scanKitConfirmation,
                    string.Format(scanKitConfirmation.NotificationTemplate, d.IndentNo, CommonUtils.GetFormatedDate(d.BranchDispatchDate.Value)),
                    UserNotificationTypes.ScanKitConfirmation,
                    d.BranchId,
                    d.DispatchId,
                    null,
                    d.DispatchId
                    ));
            });

            var kitRetunAfterAllocationBm = notificationTypes.FirstOrDefault(n => n.NotificationTypeId == 4);
            var kitRetunAfterAllocationStaff = notificationTypes.FirstOrDefault(n => n.NotificationTypeId == 5);
            var kitRetunAfterAllocation = await _settingService.GetSettingsByKey(SettingKeys.KitRetunAfterAllocation);

            var kits = await KitReturnNotifications(Convert.ToInt32(kitRetunAfterAllocation.SettingValue));
            var currentDate = CommonUtils.GetDefaultDateTime();

            kits.ForEach(k =>
            {
                var days = (currentDate - k.AllocatedDate.Value).Days;

                notifications.Add(GetNotification(
                    kitRetunAfterAllocationBm,
                    string.Format(kitRetunAfterAllocationBm.NotificationTemplate, k.AccountNo, k.AllocatedDate.Value, days),
                    UserNotificationTypes.KitRetunAfterAllocation,
                    k.BranchId,
                    null,
                    k.KitId,
                    k.KitId.Value
                    ));

                notifications.Add(GetNotification(
                    kitRetunAfterAllocationStaff,
                    string.Format(kitRetunAfterAllocationStaff.NotificationTemplate, k.AccountNo, k.AllocatedDate.Value, days),
                    UserNotificationTypes.KitRetunAfterAllocation,
                    k.BranchId,
                    null,
                    k.KitId,
                    k.KitId.Value
                    ));
            });

            if (notifications.Any())
            {
                var result = await CreateNotifications(notifications);
            }
        }

        private Notification GetNotification(NotificationType notificationType, string message, UserNotificationTypes type, long branchId, long? dispatchId, long? kitId, long redirectId)
        {
            return new Notification
            {
                Title = notificationType.NotificationTypeName,
                Description = message,
                BranchId = branchId,
                KitId = kitId,
                DispatchId = dispatchId,
                VisibleToRoles = notificationType.VisibleToRoles,
                NotificationTypeId = type.GetHashCode(),
                RedirectUrl = string.Format(notificationType.UrlTemplate, redirectId),
            };
        }

        private async Task<List<DispatchNotificationModel>> DispatchNotifications()
        {
            var dispatchConfirmation = await _settingService.GetSettingsByKey(SettingKeys.DispatchConfirmation);
            return await _notificationRepository.DispatchConfirmations(
                Convert.ToInt32(dispatchConfirmation.SettingValue),
                _userProviderService.UserClaim.BranchId,
                UserNotificationTypes.ReceiptConfirmation.GetHashCode());
        }

        private async Task<List<DispatchNotificationModel>> ScanNotifications()
        {
            var scanKitConfirmation = await _settingService.GetSettingsByKey(SettingKeys.ScanKitConfirmation);
            return await _notificationRepository.ScanConfirmations(
                Convert.ToInt32(scanKitConfirmation.SettingValue),
                _userProviderService.UserClaim.BranchId,
                UserNotificationTypes.ScanKitConfirmation.GetHashCode());
        }

        private async Task<List<DispatchNotificationModel>> KitReturnNotifications(int days)
        {
            return await _notificationRepository.KitReturnNotifications(
                days,
                _userProviderService.UserClaim.BranchId,
                UserNotificationTypes.KitRetunAfterAllocation.GetHashCode());

        }
    }
}
