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
    public class BranchTypeService : IBranchTypeService
    {
        private readonly IBranchTypeRepository _branchTypeRepository;
        private readonly IUserProviderService _userProviderService;

        public BranchTypeService(
            IBranchTypeRepository branchTypeRepository,
            IUserProviderService userProviderService
        )
        {
            _branchTypeRepository = branchTypeRepository;
            _userProviderService = userProviderService;
        }

        public async Task<ResponseModel> CreateBranchType(BranchType entity)
        {
            var existEntity = await GetBranchTypeById(entity.BranchTypeId);
            if (existEntity.Success)
            {
                return new ResponseModel
                {
                    Success = false,
                    StatusCode = StatusCodes.Status409Conflict,
                    Message = "BranchType already exists.",
                };
            }

            entity.CreatedBy = _userProviderService.UserClaim.UserId;
            entity.UpdatedBy = _userProviderService.UserClaim.UserId;
            await _branchTypeRepository.AddAsync(entity);
            var result = await _branchTypeRepository.SaveChangesAsync();
            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "BranchType created successfully.",
                    Data = entity
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "BranchType does not created." };
        }

        public async Task<ResponseModel> DeleteBranchType(long id)
        {
            var entityResult = await GetBranchTypeById(id);

            if (!entityResult.Success) { return entityResult; }

            var entity = entityResult.Data as BranchType;
            entity.UpdatedBy = _userProviderService.UserClaim.UserId;
            entity.IsDeleted = true;
            var result = await _branchTypeRepository.SaveChangesAsync();

            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "BranchType deleted successfully.",
                    Data = entity
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "BranchType does not deleted." };
        }

        public async Task<ResponseModel> GetAllBranchType(bool? isActive = null)
        {
            return new ResponseModel
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Data = await _branchTypeRepository.Find(a => a.IsDeleted == false && (!isActive.HasValue || a.IsActive == isActive))
            };
        }

        public async Task<ResponseModel> GetBranchTypePaged(Pagination pagination)
        {
            return new ResponseModel
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Data = await _branchTypeRepository.GetBranchTypePaged(pagination)
            };
        }

        public async Task<ResponseModel> GetBranchTypeById(long id)
        {
            var result = await _branchTypeRepository.SingleOrDefaultAsync(a => a.BranchTypeId == id);
            if (result != null)
            {
                return new ResponseModel { Success = true, StatusCode = StatusCodes.Status200OK, Data = result };
            }
            else
            {
                return new ResponseModel { Success = false, StatusCode = StatusCodes.Status404NotFound, Message = "BranchType does not exists." };
            }
        }

        public async Task<ResponseModel> UpdateBranchType(BranchType updateEntity)
        {
            var entityResult = await GetBranchTypeById(updateEntity.BranchTypeId);

            if (!entityResult.Success) { return entityResult; }

            var entity = entityResult.Data as BranchType;
            entity.BranchTypeName = updateEntity.BranchTypeName;
            entity.BranchTypeKey = updateEntity.BranchTypeKey;
            entity.SortOrder = updateEntity.SortOrder;
            entity.IsActive = updateEntity.IsActive;
            entity.UpdatedDate = CommonUtils.GetDefaultDateTime();
            entity.UpdatedBy = _userProviderService.UserClaim.UserId;

            var result = await _branchTypeRepository.SaveChangesAsync();

            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "BranchType updated successfully.",
                    Data = entity
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "BranchType does not updated." };
        }

        public async Task<List<DropdownModel>> GetDropdwon(bool? isAllowIndent = null, long? id = null, bool? isActive = null)
        {
            return (await _branchTypeRepository.GetDropdwon(isAllowIndent, id, isActive)).Data;
        }
    }
}
