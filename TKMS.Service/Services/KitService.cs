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
    public class KitService : IKitService
    {
        private readonly IKitRepository _kitRepository;
        private readonly IKitReturnRepository _kitReturnRepository;
        private readonly IKitAssignedCustomerRepository _kitAssignedCustomerRepository;
        private readonly IUserProviderService _userProviderService;
        private readonly INotificationService _notificationService;
        private readonly IIWorksKitService _iWorksKitService;
        private readonly IKitAuditService _kitAuditService;

        public KitService(
            IKitRepository kitRepository,
            IKitReturnRepository kitReturnRepository,
            IKitAssignedCustomerRepository kitAssignedCustomerRepository,
            IUserProviderService userProviderService,
            INotificationService notificationService,
            IIWorksKitService worksKitService,
            IKitAuditService kitAuditService
        )
        {
            _kitRepository = kitRepository;
            _kitReturnRepository = kitReturnRepository;
            _kitAssignedCustomerRepository = kitAssignedCustomerRepository;
            _userProviderService = userProviderService;
            _notificationService = notificationService;
            _iWorksKitService = worksKitService;
            _kitAuditService = kitAuditService;
        }

        public async Task<ResponseModel> CreateKit(Kit entity)
        {
            var existEntity = await GetKitById(entity.KitId);
            if (existEntity.Success)
            {
                return new ResponseModel
                {
                    Success = false,
                    StatusCode = StatusCodes.Status409Conflict,
                    Message = "Kit already exists.",
                };
            }

            entity.CreatedBy = _userProviderService.UserClaim.UserId;
            entity.UpdatedBy = _userProviderService.UserClaim.UserId;
            await _kitRepository.AddAsync(entity);
            var result = await _kitRepository.SaveChangesAsync();
            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Kit created successfully.",
                    Data = entity
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "Kit does not created." };
        }

        public async Task<ResponseModel> CreateKits(List<Kit> kits)
        {
            foreach (var entity in kits)
            {
                entity.CreatedBy = _userProviderService.UserClaim.UserId;
                entity.UpdatedBy = _userProviderService.UserClaim.UserId;
                await _kitRepository.AddAsync(entity);
            }

            var result = await _kitRepository.SaveChangesAsync();
            if (result > 0)
            {
                _kitRepository.CommitTransaction();
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Kits created successfully.",
                };
            }

            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "Kits does not created." };
        }
        public async Task<ResponseModel> DeleteKit(long id)
        {
            var entityResult = await GetKitById(id);

            if (!entityResult.Success) { return entityResult; }

            var entity = entityResult.Data as Kit;
            entity.UpdatedBy = _userProviderService.UserClaim.UserId;
            entity.IsDeleted = true;
            var result = await _kitRepository.SaveChangesAsync();

            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Kit deleted successfully.",
                    Data = entity
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "Kit does not deleted." };
        }

        public async Task<ResponseModel> GetAllKit(bool? isActive = null)
        {
            return new ResponseModel
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Data = await _kitRepository.Find(a => a.IsDeleted == false && (!isActive.HasValue || a.IsActive == isActive))
            };
        }

        public async Task<ResponseModel> GetKitPaged(Pagination pagination)
        {
            return new ResponseModel
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Data = await _kitRepository.GetKitPaged(pagination)
            };
        }

        public async Task<ResponseModel> GetKitStaffPaged(Pagination pagination)
        {
            return new ResponseModel
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Data = await _kitRepository.GetKitStaffPaged(pagination)
            };
        }

        public async Task<ResponseModel> GetKitById(long id)
        {
            var result = await _kitRepository.SingleOrDefaultAsync(a => a.KitId == id);
            if (result != null)
            {
                return new ResponseModel { Success = true, StatusCode = StatusCodes.Status200OK, Data = result };
            }
            else
            {
                return new ResponseModel { Success = false, StatusCode = StatusCodes.Status404NotFound, Message = "Kit does not exists." };
            }
        }

        public async Task<KitModel> GetKitDetailById(long id)
        {
            return await _kitRepository.GetKitDetailById(id);
        }

        public async Task<int> GetAllocatedKitCount(long staffId, long kitStatusId)
        {
            return await _kitRepository.GetAllocatedKitCount(staffId, kitStatusId);
        }

        public async Task<KitStaffDetailModel> KitStaffDetailByAllocatedId(long userId)
        {
            return await _kitRepository.KitStaffDetailByAllocatedId(userId);
        }

        public async Task<long> GetKitCount(dynamic filters = null)
        {
            return await _kitRepository.GetKitCount(filters);
        }

        public async Task<ResponseModel> GetKitByAccountNo(string accountNo, long? kitStatusId = null)
        {
            dynamic filters = new ExpandoObject();
            filters.accountNo = accountNo;
            filters.bfilBranchId = _userProviderService.GetBranchId();
            filters.kitStatusId = kitStatusId;
            filters.dispatchStatusId = DispatchStatuses.Received.GetHashCode();

            var kit = await _kitRepository.GetKitByAccountNo(filters);

            if (kit != null)
            {
                return new ResponseModel { Success = true, StatusCode = StatusCodes.Status200OK, Data = kit };
            }

            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status404NotFound, Message = "Kit does not exists." };
        }

        public async Task<ResponseModel> UpdateKit(Kit updateEntity)
        {
            var entityResult = await GetKitById(updateEntity.KitId);

            if (!entityResult.Success) { return entityResult; }

            var entity = entityResult.Data as Kit;
            entity.IndentId = updateEntity.IndentId;
            entity.CifNo = updateEntity.CifNo;
            entity.AccountNo = updateEntity.AccountNo;
            entity.KitStatusId = updateEntity.KitStatusId;
            entity.Remarks = updateEntity.Remarks;
            entity.IsActive = updateEntity.IsActive;
            entity.UpdatedDate = CommonUtils.GetDefaultDateTime();
            entity.UpdatedBy = _userProviderService.UserClaim.UserId;

            var result = await _kitRepository.SaveChangesAsync();

            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Kit updated successfully.",
                    Data = entity
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "Kit does not updated." };
        }

        public async Task<ResponseModel> UpdateKitsAllocated(KitAllocateModel updateEntity)
        {
            var entities = await _kitRepository.Find(k => updateEntity.Kits.Contains(k.KitId));

            foreach (var entity in entities)
            {
                entity.KitStatusId = KitStatuses.Allocated.GetHashCode();
                entity.AllocatedToId = updateEntity.StaffId;
                entity.AllocatedDate = CommonUtils.GetDefaultDateTime();
                entity.AllocatedById = _userProviderService.UserClaim.UserId;
                entity.UpdatedDate = CommonUtils.GetDefaultDateTime();
                entity.UpdatedBy = _userProviderService.UserClaim.UserId;
            }

            var result = await _kitRepository.SaveChangesAsync();

            if (result > 0)
            {
                await _iWorksKitService.UpdateIWorksStatus(entities.ToList(), IWorksStatuses.Allocated.DescriptionAttr());

                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Kits updated successfully."
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "Kits does not updated." };
        }

        public async Task<ResponseModel> UpdateKitsDestructed(KitDestructModel updateEntity)
        {
            var entities = await _kitRepository.Find(k => updateEntity.Kits.Contains(k.KitId));

            foreach (var entity in entities)
            {
                entity.KitStatusId = KitStatuses.Damaged.GetHashCode();
                entity.KitDamageReasonId = updateEntity.KitDamageReasonId;
                entity.DamageRemarks = updateEntity.Remarks;
                entity.AllocatedToId = null;
                entity.AllocatedDate = null;
                entity.AllocatedById = null;
                entity.UpdatedDate = CommonUtils.GetDefaultDateTime();
                entity.UpdatedBy = _userProviderService.UserClaim.UserId;
            }

            var result = await _kitRepository.SaveChangesAsync();

            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Kits updated successfully."
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "Kits does not updated." };
        }

        public async Task<ResponseModel> UpdateKitDetails(KitUpdateRequest updateEntity)
        {
            var entityResult = await GetKitById(updateEntity.KitId);

            if (!entityResult.Success) { return entityResult; }

            var entity = entityResult.Data as Kit;
            entity.KitStatusId = updateEntity.KitStatusId;
            entity.KitDamageReasonId = updateEntity.KitDamageReasonId;
            entity.DamageRemarks = updateEntity.DamageRemarks;
            entity.UpdatedDate = CommonUtils.GetDefaultDateTime();
            entity.UpdatedBy = _userProviderService.UserClaim.UserId;
            if (updateEntity.KitStatusId == KitStatuses.Received.GetHashCode())
            {
                entity.IsDestructionApproved = null;
                entity.DestructionById = null;
                entity.DestructionRemarks = null;
            }
            var result = await _kitRepository.SaveChangesAsync();

            if (result > 0)
            {
                await _notificationService.UpdateNotification(new Notification
                {
                    DispatchId = entity.DispatchId,
                    NotificationTypeId = UserNotificationTypes.ScanKitConfirmation.GetHashCode()
                });

                if (updateEntity.KitStatusId == KitStatuses.Received.GetHashCode())
                {
                    await _iWorksKitService.UpdateIWorksStatus(new List<Kit> { entity }, IWorksStatuses.Received.DescriptionAttr());
                }

                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Kit updated successfully."
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "Kit does not updated." };
        }

        public async Task<ResponseModel> UpdateKitDestruction(KitDestructionRequest updateEntity)
        {
            var entityResult = await GetKitById(updateEntity.KitId);

            if (!entityResult.Success) { return entityResult; }

            var entity = entityResult.Data as Kit;
            entity.IsDestructionApproved = updateEntity.DestructionApproved;
            entity.DestructionRemarks = updateEntity.DestructionRemarks;
            entity.DestructionById = _userProviderService.UserClaim.UserId;
            entity.UpdatedDate = CommonUtils.GetDefaultDateTime();
            entity.UpdatedBy = _userProviderService.UserClaim.UserId;

            if (updateEntity.DestructionApproved)
            {
                entity.KitStatusId = KitStatuses.Destruction.GetHashCode();
            }
            else
            {
                var lastKitStatus = await _kitAuditService.GetKitLastDestruction(updateEntity.KitId);
                entity.AllocatedToId = lastKitStatus.AllocatedToId;
                entity.AllocatedDate = lastKitStatus.AllocatedDate;
                entity.AllocatedById = lastKitStatus.AllocatedById;
                entity.KitStatusId = lastKitStatus.KitStatusId;
                entity.KitDamageReasonId = lastKitStatus.KitDamageReasonId;
                entity.DamageRemarks = null;
            }

            var result = await _kitRepository.SaveChangesAsync();

            if (result > 0)
            {
                if (updateEntity.KitStatusId == KitStatuses.Destruction.GetHashCode())
                {
                    await _iWorksKitService.UpdateIWorksStatus(new List<Kit> { entity }, IWorksStatuses.Destroyed.DescriptionAttr());
                }

                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Kit updated successfully."
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "Kit does not updated." };
        }

        public async Task<ResponseModel> UpdateKitAssigned()
        {
            var pendingAssigned = await _kitAssignedCustomerRepository.Find(k => !k.ProcessedDate.HasValue);

            if (!pendingAssigned.Any())
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "There are not any Kits pending for assignment."
                };
            }

            var acNos = pendingAssigned.Select(k => k.AccountNumber).ToList();
            var kitStatusId = KitStatuses.Allocated.GetHashCode();
            var kits = await _kitRepository.Find(k => acNos.Contains(k.AccountNo) && k.KitStatusId == kitStatusId);

            foreach (var entity in kits)
            {
                var assigned = pendingAssigned.FirstOrDefault(a => a.AccountNumber == entity.AccountNo);

                entity.AssignedDate = assigned.AssignedDate;
                entity.CustomerName = assigned.CustomerName;
                entity.KitStatusId = KitStatuses.Assigned.GetHashCode();
                entity.UpdatedDate = CommonUtils.GetDefaultDateTime();
                assigned.ProcessedDate = CommonUtils.GetDefaultDateTime();
            }

            var result = await _kitRepository.SaveChangesAsync();

            if (result > 0)
            {
                await _iWorksKitService.UpdateIWorksStatus(kits.ToList(), IWorksStatuses.Assigned.DescriptionAttr());

                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = $"Total {kits.Count()} Kits assigned successfully."
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "Kits does not updated." };
        }

        public async Task<ResponseModel> UpdateKitReturned(KitAllocateModel updateEntity)
        {
            var kits = await _kitRepository.Find(k => updateEntity.Kits.Contains(k.KitId));

            foreach (var entity in kits)
            {
                var allocateTo = entity.AllocatedToId.Value;
                await _kitReturnRepository.AddAsync(new KitReturn
                {
                    KitId = entity.KitId,
                    ReturnBy = allocateTo,
                    ReturnDate = CommonUtils.GetDefaultDateTime(),
                    ReturnAcceptedBy = _userProviderService.UserClaim.UserId,
                    CreatedBy = _userProviderService.UserClaim.UserId,
                    UpdatedBy = _userProviderService.UserClaim.UserId
                });

                entity.AllocatedToId = null;
                entity.AllocatedDate = null;
                entity.AllocatedById = null;
                entity.KitStatusId = KitStatuses.Received.GetHashCode();
                entity.UpdatedDate = CommonUtils.GetDefaultDateTime();
                entity.UpdatedBy = _userProviderService.UserClaim.UserId;
            }

            var result = await _kitRepository.SaveChangesAsync();

            if (result > 0)
            {
                await _iWorksKitService.UpdateIWorksStatus(kits.ToList(), IWorksStatuses.Received.DescriptionAttr());

                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Kits returned successfully."
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "Kits does not returned." };
        }

        public async Task<bool> IsSingleStaff(List<long> kitIds)
        {
            return await _kitRepository.IsSingleStaff(kitIds);
        }

        public async Task<List<KitModel>> GetStaffKits(List<long> kitIds)
        {
            return await _kitRepository.GetStaffKits(kitIds);
        }

        public async Task<List<string>> ValidateKitAccountNo(List<string> accountNos)
        {
            return await _kitRepository.ValidateKitAccountNo(accountNos);
        }
    }
}
