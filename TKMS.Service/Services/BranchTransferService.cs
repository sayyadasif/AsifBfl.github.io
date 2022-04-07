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
using TKMS.Abstraction.Enums;
using TKMS.Abstraction.Models;
using TKMS.Repository.Interfaces;
using TKMS.Service.Interfaces;

namespace TKMS.Service.Services
{
    public class BranchTransferService : IBranchTransferService
    {
        private readonly IBranchTransferRepository _branchTransferRepository;
        private readonly IKitRepository _kitRepository;
        private readonly IIWorksKitService _iWorksKitService;
        private readonly IUserProviderService _userProviderService;

        public BranchTransferService(
            IBranchTransferRepository branchTransferRepository,
            IKitRepository kitRepository,
            IIWorksKitService worksKitService,
            IUserProviderService userProviderService
        )
        {
            _branchTransferRepository = branchTransferRepository;
            _kitRepository = kitRepository;
            _iWorksKitService = worksKitService;
            _userProviderService = userProviderService;
        }

        public async Task<ResponseModel> CreateBranchTransfer(BranchTransfer entity)
        {
            entity.CreatedBy = _userProviderService.UserClaim.UserId;
            entity.UpdatedBy = _userProviderService.UserClaim.UserId;
            await _branchTransferRepository.AddAsync(entity);
            var result = await _branchTransferRepository.SaveChangesAsync();
            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Branch Transfer created successfully.",
                    Data = entity
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "Branch Transfer does not created." };
        }

        public async Task<ResponseModel> CreateBranchTransfers(TransferModel model)
        {
            var entities = await _kitRepository.Find(k => model.Kits.Contains(k.KitId));

            foreach (var entity in entities)
            {
                entity.KitStatusId = KitStatuses.Transfer.GetHashCode();
                entity.UpdatedDate = CommonUtils.GetDefaultDateTime();
                entity.UpdatedBy = _userProviderService.UserClaim.UserId;

                await _branchTransferRepository.AddAsync(new BranchTransfer
                {
                    KitId = entity.KitId,
                    FromBranchId = entity.BranchId,
                    ToBranchId = model.ToBranchId,
                    TransferById = _userProviderService.UserClaim.UserId,
                    TransferDate = CommonUtils.GetDefaultDateTime(),
                    Remarks = model.Remarks,
                    CreatedBy = _userProviderService.UserClaim.UserId,
                    UpdatedBy = _userProviderService.UserClaim.UserId
                });
            }

            var result = await _branchTransferRepository.SaveChangesAsync();
            if (result > 0)
            {
                await _iWorksKitService.UpdateIWorksStatus(entities.ToList(), IWorksStatuses.Dispatched.DescriptionAttr());

                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Branch Transfers created successfully."
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "Branch Transfers does not created." };
        }

        public async Task<ResponseModel> DeleteBranchTransfer(long id)
        {
            var entityResult = await GetBranchTransferById(id);

            if (!entityResult.Success) { return entityResult; }

            var entity = entityResult.Data as BranchTransfer;
            entity.UpdatedBy = _userProviderService.UserClaim.UserId;
            entity.IsDeleted = true;
            var result = await _branchTransferRepository.SaveChangesAsync();

            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Branch Transfer deleted successfully.",
                    Data = entity
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "Branch Transfer does not deleted." };
        }

        public async Task<ResponseModel> GetAllBranchTransfer(bool? isActive = null)
        {
            return new ResponseModel
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Data = await _branchTransferRepository.Find(a => a.IsDeleted == false && (!isActive.HasValue || a.IsActive == isActive))
            };
        }

        public async Task<ResponseModel> GetBranchTransferPaged(Pagination pagination)
        {
            return new ResponseModel
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Data = await _branchTransferRepository.GetBranchTransferPaged(pagination)
            };
        }

        public async Task<ResponseModel> GetBranchTransferById(long id)
        {
            var result = await _branchTransferRepository.SingleOrDefaultAsync(a => a.BranchTransferId == id);
            if (result != null)
            {
                return new ResponseModel { Success = true, StatusCode = StatusCodes.Status200OK, Data = result };
            }
            else
            {
                return new ResponseModel { Success = false, StatusCode = StatusCodes.Status404NotFound, Message = "Branch Transfer does not exists." };
            }
        }

        public async Task<BranchTransferModel> GetTransferDetailById(long id)
        {
            return await _branchTransferRepository.GetTransferDetailById(id);
        }

        public async Task<ResponseModel> UpdateBranchTransfer(BranchTransferUpdateRequest updateEntity)
        {
            var entityResult = await GetBranchTransferById(updateEntity.BranchTransferId);

            if (!entityResult.Success) { return entityResult; }

            var entity = entityResult.Data as BranchTransfer;

            var kit = await _kitRepository.GetByIdAsync(entity.KitId);

            kit.BranchId = entity.ToBranchId;
            kit.KitStatusId = updateEntity.KitStatusId;
            kit.KitDamageReasonId = updateEntity.KitDamageReasonId;
            kit.DamageRemarks = updateEntity.DamageRemarks;
            kit.UpdatedDate = CommonUtils.GetDefaultDateTime();
            kit.UpdatedBy = _userProviderService.UserClaim.UserId;

            entity.ReceivedById = _userProviderService.UserClaim.UserId;
            entity.ReceivedDate = CommonUtils.GetDefaultDateTime();
            entity.UpdatedDate = CommonUtils.GetDefaultDateTime();
            entity.UpdatedBy = _userProviderService.UserClaim.UserId;

            var result = await _branchTransferRepository.SaveChangesAsync();

            if (result > 0)
            {
                if (updateEntity.KitStatusId == KitStatuses.Received.GetHashCode())
                {
                    await _iWorksKitService.UpdateIWorksStatus(new List<Kit> { kit }, IWorksStatuses.Received.DescriptionAttr());
                }

                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Branch Transfer updated successfully."
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "Branch Transfer does not updated." };
        }

        public async Task<ResponseModel> UpdateBranchTransfers(List<long> ids)
        {
            var entityResults = await _branchTransferRepository.Find(bt => ids.Contains(bt.BranchTransferId));

            foreach (var entity in entityResults)
            {
                var kit = await _kitRepository.GetByIdAsync(entity.KitId);

                kit.BranchId = entity.ToBranchId;
                kit.KitStatusId = KitStatuses.Received.GetHashCode();
                kit.UpdatedDate = CommonUtils.GetDefaultDateTime();
                kit.UpdatedBy = _userProviderService.UserClaim.UserId;

                entity.ReceivedById = _userProviderService.UserClaim.UserId;
                entity.ReceivedDate = CommonUtils.GetDefaultDateTime();
                entity.UpdatedDate = CommonUtils.GetDefaultDateTime();
                entity.UpdatedBy = _userProviderService.UserClaim.UserId;
            }

            var result = await _branchTransferRepository.SaveChangesAsync();

            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Branch Transfer updated successfully."
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "Branch Transfer does not updated." };
        }

        public async Task<ResponseModel> GetTransferKitByAccountNo(string accountNo)
        {
            dynamic filters = new ExpandoObject();
            filters.accountNo = accountNo;
            filters.bfilBranchId = _userProviderService.GetBranchId();

            var kit = await _branchTransferRepository.GetTransferKitByAccountNo(filters);

            if (kit != null)
            {
                return new ResponseModel { Success = true, StatusCode = StatusCodes.Status200OK, Data = kit };
            }

            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status404NotFound, Message = "Branch Transfer Kit does not exists." };
        }
    }
}
