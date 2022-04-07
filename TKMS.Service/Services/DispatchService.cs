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
using TKMS.Abstraction.Constants;
using TKMS.Abstraction.Enums;
using TKMS.Abstraction.Models;
using TKMS.Repository.Interfaces;
using TKMS.Service.Interfaces;

namespace TKMS.Service.Services
{
    public class DispatchService : IDispatchService
    {
        private readonly IDispatchRepository _dispatchRepository;
        private readonly IUserProviderService _userProviderService;
        private readonly INotificationService _notificationService;

        public DispatchService(
            IDispatchRepository dispatchRepository,
            IUserProviderService userProviderService,
            INotificationService notificationService
        )
        {
            _dispatchRepository = dispatchRepository;
            _userProviderService = userProviderService;
            _notificationService = notificationService;
        }

        public async Task<ResponseModel> CreateDispatch(Dispatch entity)
        {
            var existEntity = await GetDispatchById(entity.DispatchId);
            if (existEntity.Success)
            {
                return new ResponseModel
                {
                    Success = false,
                    StatusCode = StatusCodes.Status409Conflict,
                    Message = "Dispatch already exists.",
                };
            }

            entity.CreatedBy = _userProviderService.UserClaim.UserId;
            entity.UpdatedBy = _userProviderService.UserClaim.UserId;
            await _dispatchRepository.AddAsync(entity);
            var result = await _dispatchRepository.SaveChangesAsync();
            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Dispatch created successfully.",
                    Data = entity
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "Dispatch does not created." };
        }

        public async Task<ResponseModel> CreateDispatches(List<Dispatch> dispatches)
        {
            foreach (var entity in dispatches)
            {
                entity.CreatedBy = _userProviderService.UserClaim.UserId;
                entity.UpdatedBy = _userProviderService.UserClaim.UserId;

                foreach (var wayBill in entity.DispatchWayBills)
                {
                    wayBill.CreatedBy = _userProviderService.UserClaim.UserId;
                    wayBill.UpdatedBy = _userProviderService.UserClaim.UserId;
                }

                foreach (var kit in entity.Kits)
                {
                    kit.CreatedBy = _userProviderService.UserClaim.UserId;
                    kit.UpdatedBy = _userProviderService.UserClaim.UserId;
                }

                await _dispatchRepository.AddAsync(entity);
            }

            var result = await _dispatchRepository.SaveChangesAsync();
            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Dispatch created successfully.",
                };
            }

            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "Dispatch does not created." };
        }
        public async Task<ResponseModel> DeleteDispatch(long id)
        {
            var entityResult = await GetDispatchById(id);

            if (!entityResult.Success) { return entityResult; }

            var entity = entityResult.Data as Dispatch;
            entity.UpdatedBy = _userProviderService.UserClaim.UserId;
            entity.IsDeleted = true;
            var result = await _dispatchRepository.SaveChangesAsync();

            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Dispatch deleted successfully.",
                    Data = entity
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "Dispatch does not deleted." };
        }

        public async Task<ResponseModel> GetAllDispatch(bool? isActive = null)
        {
            return new ResponseModel
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Data = await _dispatchRepository.Find(a => a.IsDeleted == false && (!isActive.HasValue || a.IsActive == isActive))
            };
        }

        public async Task<ResponseModel> GetDispatchPaged(Pagination pagination)
        {
            return new ResponseModel
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Data = await _dispatchRepository.GetDispatchPaged(pagination)
            };
        }

        public async Task<ResponseModel> GetDispatchById(long id)
        {
            var result = await _dispatchRepository.SingleOrDefaultAsync(a => a.DispatchId == id, b => b.DispatchWayBills);
            if (result != null)
            {
                return new ResponseModel { Success = true, StatusCode = StatusCodes.Status200OK, Data = result };
            }
            else
            {
                return new ResponseModel { Success = false, StatusCode = StatusCodes.Status404NotFound, Message = "Dispatch does not exists." };
            }
        }

        public async Task<ResponseModel> UpdateDispatch(Dispatch updateEntity)
        {
            var entityResult = await GetDispatchById(updateEntity.DispatchId);

            if (!entityResult.Success) { return entityResult; }

            var entity = entityResult.Data as Dispatch;
            entity.IndentId = updateEntity.IndentId;
            entity.AccountStart = updateEntity.AccountStart;
            entity.AccountEnd = updateEntity.AccountEnd;
            entity.Vendor = updateEntity.Vendor;
            entity.DispatchQty = updateEntity.DispatchQty;
            entity.DispatchDate = updateEntity.DispatchDate;
            entity.SchemeType = updateEntity.SchemeType;
            entity.ReferenceNo = updateEntity.ReferenceNo;
            entity.DispatchStatusId = updateEntity.DispatchStatusId;
            entity.Remarks = updateEntity.Remarks;
            entity.IsActive = updateEntity.IsActive;
            entity.UpdatedDate = CommonUtils.GetDefaultDateTime();
            entity.UpdatedBy = _userProviderService.UserClaim.UserId;

            var result = await _dispatchRepository.SaveChangesAsync();

            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Dispatch updated successfully.",
                    Data = entity
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "Dispatch does not updated." };
        }

        public async Task<ResponseModel> UpdateDispatchStatus(Dispatch updateEntity)
        {
            var entityResult = await GetDispatchById(updateEntity.DispatchId);

            if (!entityResult.Success) { return entityResult; }

            var entity = entityResult.Data as Dispatch;
            entity.DispatchStatusId = updateEntity.DispatchStatusId;
            entity.UpdatedDate = CommonUtils.GetDefaultDateTime();
            entity.UpdatedDate = CommonUtils.GetDefaultDateTime();
            entity.UpdatedBy = _userProviderService.UserClaim.UserId;

            if (entity.DispatchStatusId == DispatchStatuses.DispatchToBranch.GetHashCode())
            {
                entity.BranchDispatchDate = CommonUtils.GetDefaultDateTime();
            }
            if (entity.DispatchStatusId == DispatchStatuses.Received.GetHashCode())
            {
                entity.BranchReceiveDate = CommonUtils.GetDefaultDateTime();
            }

            var result = await _dispatchRepository.SaveChangesAsync();

            if (result > 0)
            {
                if (entity.DispatchStatusId == DispatchStatuses.Received.GetHashCode())
                {
                    await _notificationService.UpdateNotification(new Notification
                    {
                        DispatchId = entity.DispatchId,
                        NotificationTypeId = UserNotificationTypes.ReceiptConfirmation.GetHashCode()
                    });
                }
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Dispatch updated successfully.",
                    Data = entity
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "Dispatch does not updated." };
        }

        public async Task<bool> IsAllDispatchReceived(long indentId)
        {
            return await _dispatchRepository.IsAllDispatchReceived(indentId);
        }
    }
}
