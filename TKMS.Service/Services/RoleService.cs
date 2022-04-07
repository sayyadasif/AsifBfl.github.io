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
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IUserProviderService _userProviderService;

        public RoleService(
            IRoleRepository roleRepository,
            IUserProviderService userProviderService
        )
        {
            _roleRepository = roleRepository;
            _userProviderService = userProviderService;
        }

        public async Task<ResponseModel> CreateRole(Role entity)
        {
            var existEntity = await GetRoleById(entity.RoleId);
            if (existEntity.Success)
            {
                return new ResponseModel
                {
                    Success = false,
                    StatusCode = StatusCodes.Status409Conflict,
                    Message = "Role already exists.",
                };
            }

            entity.CreatedBy = _userProviderService.UserClaim.UserId;
            entity.UpdatedBy = _userProviderService.UserClaim.UserId;
            await _roleRepository.AddAsync(entity);
            var result = await _roleRepository.SaveChangesAsync();
            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Role created successfully.",
                    Data = entity
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "Role does not created." };
        }

        public async Task<ResponseModel> DeleteRole(long id)
        {
            var entityResult = await GetRoleById(id);

            if (!entityResult.Success) { return entityResult; }

            var entity = entityResult.Data as Role;
            entity.UpdatedBy = _userProviderService.UserClaim.UserId;
            entity.IsDeleted = true;
            var result = await _roleRepository.SaveChangesAsync();

            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Role deleted successfully.",
                    Data = entity
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "Role does not deleted." };
        }

        public async Task<ResponseModel> GetAllRole(bool? isActive = null)
        {
            return new ResponseModel
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Data = await _roleRepository.Find(a => a.IsDeleted == false && (!isActive.HasValue || a.IsActive == isActive))
            };
        }

        public async Task<ResponseModel> GetRolePaged(Pagination pagination)
        {
            return new ResponseModel
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Data = await _roleRepository.GetRolePaged(pagination)
            };
        }

        public async Task<ResponseModel> GetRoleById(long id)
        {
            var result = await _roleRepository.SingleOrDefaultAsync(a => a.RoleId == id);
            if (result != null)
            {
                return new ResponseModel { Success = true, StatusCode = StatusCodes.Status200OK, Data = result };
            }
            else
            {
                return new ResponseModel { Success = false, StatusCode = StatusCodes.Status404NotFound, Message = "Role does not exists." };
            }
        }

        public async Task<ResponseModel> UpdateRole(Role updateEntity)
        {
            var entityResult = await GetRoleById(updateEntity.RoleId);

            if (!entityResult.Success) { return entityResult; }

            var entity = entityResult.Data as Role;
            entity.RoleName = updateEntity.RoleName;
            entity.SortOrder = updateEntity.SortOrder;
            entity.IsActive = updateEntity.IsActive;
            entity.UpdatedDate = CommonUtils.GetDefaultDateTime();
            entity.UpdatedBy = _userProviderService.UserClaim.UserId;

            var result = await _roleRepository.SaveChangesAsync();

            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Role updated successfully.",
                    Data = entity
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "Role does not updated." };
        }

        public async Task<List<DropdownModel>> GetDropdwon(long? id = null, long? roleTypeId = null)
        {
            return (await _roleRepository.GetDropdwon(id, roleTypeId)).Data;
        }
    }
}
