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
    public class IblBranchService : IIblBranchService
    {
        private readonly IIblBranchRepository _iblBranchRepository;
        private readonly IUserProviderService _userProviderService;

        public IblBranchService(
            IIblBranchRepository iblBranchRepository,
            IUserProviderService userProviderService
        )
        {
            _iblBranchRepository = iblBranchRepository;
            _userProviderService = userProviderService;
        }

        public async Task<ResponseModel> CreateIblBranch(IblBranch entity)
        {
            var existEntity = await _iblBranchRepository.SingleOrDefaultAsync(a => a.IblBranchName == entity.IblBranchName);
            if (existEntity != null)
            {
                return new ResponseModel
                {
                    Success = false,
                    StatusCode = StatusCodes.Status409Conflict,
                    Message = "IBL Branch Name already exists.",
                };
            }

            entity.CreatedBy = _userProviderService.UserClaim.UserId;
            entity.UpdatedBy = _userProviderService.UserClaim.UserId;

            await _iblBranchRepository.AddAsync(entity);
            var result = await _iblBranchRepository.SaveChangesAsync();
            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "IBL Branch created successfully.",
                    Data = entity
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "IBL Branch does not created." };
        }

        public async Task<ResponseModel> CreateIblBranches(List<IblBranch> entities)
        {
            foreach (var entity in entities)
            {
                entity.CreatedBy = _userProviderService.UserClaim.UserId;
                entity.UpdatedBy = _userProviderService.UserClaim.UserId;
                await _iblBranchRepository.AddAsync(entity);
            }
            var result = await _iblBranchRepository.SaveChangesAsync();
            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "IBL Branchs created successfully."
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "IBL Branchs does not created." };
        }

        public async Task<ResponseModel> DeleteIblBranch(long id)
        {
            var entityResult = await GetIblBranchById(id);

            if (!entityResult.Success) { return entityResult; }

            var entity = entityResult.Data as IblBranch;
            entity.UpdatedBy = _userProviderService.UserClaim.UserId;
            entity.IsDeleted = true;
            var result = await _iblBranchRepository.SaveChangesAsync();

            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "IBL Branch deleted successfully.",
                    Data = entity
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "IBL Branch does not deleted." };
        }

        public async Task<ResponseModel> GetAllIblBranch(bool? isActive = null)
        {
            return new ResponseModel
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Data = await _iblBranchRepository.Find(a => a.IsDeleted == false && (!isActive.HasValue || a.IsActive == isActive))
            };
        }

        public async Task<ResponseModel> GetIblBranchPaged(Pagination pagination)
        {
            return new ResponseModel
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Data = await _iblBranchRepository.GetIblBranchPaged(pagination)
            };
        }

        public async Task<ResponseModel> GetIblBranchById(long id)
        {
            var result = await _iblBranchRepository.SingleOrDefaultAsync(a => a.IblBranchId == id);
            if (result != null)
            {
                return new ResponseModel { Success = true, StatusCode = StatusCodes.Status200OK, Data = result };
            }
            else
            {
                return new ResponseModel { Success = false, StatusCode = StatusCodes.Status404NotFound, Message = "IBL Branch does not exists." };
            }
        }

        public async Task<ResponseModel> UpdateIblBranch(IblBranch updateEntity)
        {
            var entityResult = await GetIblBranchById(updateEntity.IblBranchId);

            if (!entityResult.Success) { return entityResult; }

            var entity = entityResult.Data as IblBranch;
            entity.IblBranchCode = updateEntity.IblBranchCode;
            entity.IblBranchName = updateEntity.IblBranchName;
            entity.SortOrder = updateEntity.SortOrder;
            entity.IsActive = updateEntity.IsActive;
            entity.UpdatedDate = CommonUtils.GetDefaultDateTime();
            entity.UpdatedBy = _userProviderService.UserClaim.UserId;
            var result = await _iblBranchRepository.SaveChangesAsync();

            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "IBL Branch updated successfully.",
                    Data = entity
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "IBL Branch does not updated." };
        }

        public async Task<List<IblBranchDropdownModel>> GetDropdwon(long? id = null, bool? isActive = null)
        {
            return (await _iblBranchRepository.GetDropdwon(id, isActive)).Data;
        }
    }
}
