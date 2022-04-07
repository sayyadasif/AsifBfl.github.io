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
    public class BranchDispatchService : IBranchDispatchService
    {
        private readonly IBranchDispatchRepository _branchDispatchRepository;
        private readonly IUserProviderService _userProviderService;

        public BranchDispatchService(
            IBranchDispatchRepository branchDispatchRepository,
            IUserProviderService userProviderService
        )
        {
            _branchDispatchRepository = branchDispatchRepository;
            _userProviderService = userProviderService;
        }

        public async Task<ResponseModel> CreateBranchDispatch(BranchDispatch entity)
        {
            entity.CreatedBy = _userProviderService.UserClaim.UserId;
            entity.UpdatedBy = _userProviderService.UserClaim.UserId;
            await _branchDispatchRepository.AddAsync(entity);
            var result = await _branchDispatchRepository.SaveChangesAsync();
            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Branch Dispatch created successfully.",
                    Data = entity
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "Branch Dispatch does not created." };
        }

        public async Task<ResponseModel> DeleteBranchDispatch(long id)
        {
            var entityResult = await GetBranchDispatchById(id);

            if (!entityResult.Success) { return entityResult; }

            var entity = entityResult.Data as BranchDispatch;
            entity.UpdatedBy = _userProviderService.UserClaim.UserId;
            entity.IsDeleted = true;
            var result = await _branchDispatchRepository.SaveChangesAsync();

            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Branch Dispatch deleted successfully.",
                    Data = entity
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "Branch Dispatch does not deleted." };
        }

        public async Task<ResponseModel> GetAllBranchDispatch(bool? isActive = null)
        {
            return new ResponseModel
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Data = await _branchDispatchRepository.Find(a => a.IsDeleted == false && (!isActive.HasValue || a.IsActive == isActive))
            };
        }

        public async Task<ResponseModel> GetBranchDispatchPaged(Pagination pagination)
        {
            return new ResponseModel
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Data = await _branchDispatchRepository.GetBranchDispatchPaged(pagination)
            };
        }

        public async Task<ResponseModel> GetBranchDispatchById(long id)
        {
            var result = await _branchDispatchRepository.SingleOrDefaultAsync(a => a.BranchDispatchId == id);
            if (result != null)
            {
                return new ResponseModel { Success = true, StatusCode = StatusCodes.Status200OK, Data = result };
            }
            else
            {
                return new ResponseModel { Success = false, StatusCode = StatusCodes.Status404NotFound, Message = "Branch Dispatch does not exists." };
            }
        }

        public async Task<ResponseModel> UpdateBranchDispatch(BranchDispatch updateEntity)
        {
            var entityResult = await GetBranchDispatchById(updateEntity.BranchDispatchId);

            if (!entityResult.Success) { return entityResult; }

            var entity = entityResult.Data as BranchDispatch;
            entity.DispatchId = updateEntity.DispatchId;
            entity.DispatchMode = updateEntity.DispatchMode;
            entity.StaffId = updateEntity.StaffId;
            entity.StaffName = updateEntity.StaffName;
            entity.StaffContactNo = updateEntity.StaffContactNo;
            entity.CourierName = updateEntity.CourierName;
            entity.WayBillNo = updateEntity.WayBillNo;
            entity.Remarks = updateEntity.Remarks;
            entity.IsActive = updateEntity.IsActive;
            entity.UpdatedDate = CommonUtils.GetDefaultDateTime();
            entity.UpdatedBy = _userProviderService.UserClaim.UserId;

            var result = await _branchDispatchRepository.SaveChangesAsync();

            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Branch Dispatch updated successfully.",
                    Data = entity
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "Branch Dispatch does not updated." };
        }
    }
}
