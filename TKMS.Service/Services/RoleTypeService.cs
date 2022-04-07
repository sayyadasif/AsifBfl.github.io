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
    public class RoleTypeService : IRoleTypeService
    {
        private readonly IRoleTypeRepository _roleTypeRepository;
        private readonly IUserProviderService _userProviderService;

        public RoleTypeService(
            IRoleTypeRepository roleTypeRepository,
            IUserProviderService userProviderService
        )
        {
            _roleTypeRepository = roleTypeRepository;
            _userProviderService = userProviderService;
        }

        public async Task<ResponseModel> CreateRoleType(RoleType entity)
        {
            var existEntity = await GetRoleTypeById(entity.RoleTypeId);
            if (existEntity.Success)
            {
                return new ResponseModel
                {
                    Success = false,
                    StatusCode = StatusCodes.Status409Conflict,
                    Message = "RoleType already exists.",
                };
            }

            entity.CreatedBy = _userProviderService.UserClaim.UserId;
            entity.UpdatedBy = _userProviderService.UserClaim.UserId;
            await _roleTypeRepository.AddAsync(entity);
            var result = await _roleTypeRepository.SaveChangesAsync();
            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "RoleType created successfully.",
                    Data = entity
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "RoleType does not created." };
        }

        public async Task<ResponseModel> DeleteRoleType(long id)
        {
            var entityResult = await GetRoleTypeById(id);

            if (!entityResult.Success) { return entityResult; }

            var entity = entityResult.Data as RoleType;
            entity.UpdatedBy = _userProviderService.UserClaim.UserId;
            entity.IsDeleted = true;
            var result = await _roleTypeRepository.SaveChangesAsync();

            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "RoleType deleted successfully.",
                    Data = entity
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "RoleType does not deleted." };
        }

        public async Task<ResponseModel> GetAllRoleType(bool? isActive = null)
        {
            return new ResponseModel
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Data = await _roleTypeRepository.Find(a => a.IsDeleted == false && (!isActive.HasValue || a.IsActive == isActive))
            };
        }

        public async Task<ResponseModel> GetRoleTypePaged(Pagination pagination)
        {
            return new ResponseModel
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Data = await _roleTypeRepository.GetRoleTypePaged(pagination)
            };
        }

        public async Task<ResponseModel> GetRoleTypeById(long id)
        {
            var result = await _roleTypeRepository.SingleOrDefaultAsync(a => a.RoleTypeId == id);
            if (result != null)
            {
                return new ResponseModel { Success = true, StatusCode = StatusCodes.Status200OK, Data = result };
            }
            else
            {
                return new ResponseModel { Success = false, StatusCode = StatusCodes.Status404NotFound, Message = "RoleType does not exists." };
            }
        }

        public async Task<ResponseModel> UpdateRoleType(RoleType updateEntity)
        {
            var entityResult = await GetRoleTypeById(updateEntity.RoleTypeId);

            if (!entityResult.Success) { return entityResult; }

            var entity = entityResult.Data as RoleType;
            entity.RoleTypeName = updateEntity.RoleTypeName;
            entity.RoleTypeKey = updateEntity.RoleTypeKey;
            entity.SortOrder = updateEntity.SortOrder;
            entity.IsActive = updateEntity.IsActive;
            entity.UpdatedDate = CommonUtils.GetDefaultDateTime();
            entity.UpdatedBy = _userProviderService.UserClaim.UserId;

            var result = await _roleTypeRepository.SaveChangesAsync();

            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "RoleType updated successfully.",
                    Data = entity
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "RoleType does not updated." };
        }

        public async Task<List<DropdownModel>> GetDropdwon(long? id = null, bool? isActive = null)
        {
            return (await _roleTypeRepository.GetDropdwon(id, isActive)).Data;
        }
    }
}
