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
    public class BranchService : IBranchService
    {
        private readonly IBranchRepository _branchRepository;
        private readonly IUserProviderService _userProviderService;

        public BranchService(
            IBranchRepository branchRepository,
            IUserProviderService userProviderService
        )
        {
            _branchRepository = branchRepository;
            _userProviderService = userProviderService;
        }

        public async Task<ResponseModel> CreateBranch(Branch entity)
        {
            var existEntity = await GetBranchByCode(entity.BranchCode);
            if (existEntity.Success)
            {
                return new ResponseModel
                {
                    Success = false,
                    StatusCode = StatusCodes.Status409Conflict,
                    Message = "Branch Code already exists.",
                };
            }

            entity.CreatedBy = _userProviderService.UserClaim.UserId;
            entity.UpdatedBy = _userProviderService.UserClaim.UserId;

            entity.Address.UpdatedDate = CommonUtils.GetDefaultDateTime();
            entity.Address.UpdatedBy = _userProviderService.UserClaim.UserId;

            await _branchRepository.AddAsync(entity);
            var result = await _branchRepository.SaveChangesAsync();
            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Branch created successfully.",
                    Data = entity
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "Branch does not created." };
        }


        public async Task<ResponseModel> CreateBranches(List<Branch> entities)
        {
            foreach (var entity in entities)
            {
                entity.CreatedBy = _userProviderService.UserClaim.UserId;
                entity.UpdatedBy = _userProviderService.UserClaim.UserId;
                await _branchRepository.AddAsync(entity);
            }
            var result = await _branchRepository.SaveChangesAsync();
            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Branches created successfully."
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "Branches does not created." };
        }

        public async Task<ResponseModel> IblBranchMapping(List<IblBranchMapping> entities)
        {
            var branchIds = entities.Select(b => b.BranchId).ToList();
            var branches = await _branchRepository.Find(b => branchIds.Contains(b.BranchId));

            foreach (var entity in branches)
            {
                entity.IblBranchId = entities.FirstOrDefault(b => b.BranchId == entity.BranchId).IblBranchId;
                entity.UpdatedBy = _userProviderService.UserClaim.UserId;
            }
            var result = await _branchRepository.SaveChangesAsync();
            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Branch Mapping updated successfully."
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "Branch Mapping does not updated." };
        }

        public async Task<ResponseModel> DeleteBranch(long id)
        {
            var entityResult = await GetBranchById(id);

            if (!entityResult.Success) { return entityResult; }

            var entity = entityResult.Data as Branch;
            entity.UpdatedBy = _userProviderService.UserClaim.UserId;
            entity.IsDeleted = true;
            var result = await _branchRepository.SaveChangesAsync();

            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Branch deleted successfully.",
                    Data = entity
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "Branch does not deleted." };
        }

        public async Task<ResponseModel> GetAllBranch(bool? isActive = null)
        {
            return new ResponseModel
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Data = await _branchRepository.Find(a => a.IsDeleted == false && (!isActive.HasValue || a.IsActive == isActive))
            };
        }

        public async Task<ResponseModel> GetBranchPaged(Pagination pagination)
        {
            return new ResponseModel
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Data = await _branchRepository.GetBranchPaged(pagination)
            };
        }

        public async Task<ResponseModel> GetBranchById(long id)
        {
            var result = await _branchRepository.SingleOrDefaultAsync(a => a.BranchId == id);
            if (result != null)
            {
                return new ResponseModel { Success = true, StatusCode = StatusCodes.Status200OK, Data = result };
            }
            else
            {
                return new ResponseModel { Success = false, StatusCode = StatusCodes.Status404NotFound, Message = "Branch does not exists." };
            }
        }

        public async Task<ResponseModel> GetBranchByCode(string branchCode)
        {
            var result = await _branchRepository.SingleOrDefaultAsync(a => a.BranchCode == branchCode);
            if (result != null)
            {
                return new ResponseModel { Success = true, StatusCode = StatusCodes.Status200OK, Data = result };
            }
            else
            {
                return new ResponseModel { Success = false, StatusCode = StatusCodes.Status404NotFound, Message = "Branch does not exists." };
            }
        }

        public async Task<ResponseModel> UpdateBranch(Branch updateEntity)
        {
            var entityResult = await GetBranchById(updateEntity.BranchId);

            if (!entityResult.Success) { return entityResult; }

            var entity = entityResult.Data as Branch;
            entity.BranchName = updateEntity.BranchName;
            entity.BranchCode = updateEntity.BranchCode;
            entity.IblBranchId = updateEntity.IblBranchId;
            entity.RegionId = updateEntity.RegionId;
            entity.BranchTypeId = updateEntity.BranchTypeId;
            entity.SchemeCodeId = updateEntity.SchemeCodeId;
            entity.C5CodeId = updateEntity.C5CodeId;
            entity.CardTypeId = updateEntity.CardTypeId;
            entity.IfscCode = updateEntity.IfscCode;
            entity.SortOrder = updateEntity.SortOrder;
            entity.IsActive = updateEntity.IsActive;
            entity.UpdatedDate = CommonUtils.GetDefaultDateTime();
            entity.UpdatedBy = _userProviderService.UserClaim.UserId;

            entity.Address = updateEntity.Address;
            entity.Address.UpdatedDate = CommonUtils.GetDefaultDateTime();
            entity.Address.UpdatedBy = _userProviderService.UserClaim.UserId;

            var result = await _branchRepository.SaveChangesAsync();

            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Branch updated successfully.",
                    Data = entity
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "Branch does not updated." };
        }

        public async Task<ResponseModel> UpdateBranches(List<Branch> updateEntities)
        {
            foreach (var entity in updateEntities)
            {

                entity.UpdatedDate = CommonUtils.GetDefaultDateTime();
                entity.UpdatedBy = _userProviderService.UserClaim.UserId;
            }

            var result = await _branchRepository.SaveChangesAsync();

            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Branches updated successfully."
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "Branches does not updated." };
        }

        public async Task<List<BranchDropdownModel>> GetDropdwon(long? regionId = null, long? branchId = null, long? id = null)
        {
            return (await _branchRepository.GetDropdwon(regionId, branchId, id)).Data;
        }
    }
}
