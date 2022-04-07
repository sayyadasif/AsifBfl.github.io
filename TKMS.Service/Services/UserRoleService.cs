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
    public class UserRoleService : IUserRoleService
    {
        private readonly IUserRoleRepository _userRoleRepository;

        public UserRoleService(
            IUserRoleRepository userRoleRepository
        )
        {
            _userRoleRepository = userRoleRepository;
        }

        public async Task<ResponseModel> CreateUserRole(UserRole entity)
        {
            var existEntity = await GetUserRoleById(entity.UserRoleId);
            if (existEntity.Success)
            {
                return new ResponseModel
                {
                    Success = false,
                    StatusCode = StatusCodes.Status409Conflict,
                    Message = "User Role already exists.",
                };
            }

            await _userRoleRepository.AddAsync(entity);
            var result = await _userRoleRepository.SaveChangesAsync();
            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "User Role created successfully.",
                    Data = entity
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "User Role does not created." };
        }

        public async Task<ResponseModel> DeleteUserRole(long id)
        {
            var entityResult = await GetUserRoleById(id);

            if (!entityResult.Success) { return entityResult; }

            var entity = entityResult.Data as UserRole;
            var result = await _userRoleRepository.SaveChangesAsync();

            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "User Role deleted successfully.",
                    Data = entity
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "User Role does not deleted." };
        }

        public async Task<ResponseModel> GetAllUserRole(long userId)
        {
            return new ResponseModel
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Data = await _userRoleRepository.Find(a => a.UserId == userId)
            };
        }

        public async Task<ResponseModel> GetUserRolePaged(Pagination pagination)
        {
            return new ResponseModel
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Data = await _userRoleRepository.GetUserRolePaged(pagination)
            };
        }

        public async Task<ResponseModel> GetUserRoleById(long id)
        {
            var result = await _userRoleRepository.SingleOrDefaultAsync(a => a.UserRoleId == id);
            if (result != null)
            {
                return new ResponseModel { Success = true, StatusCode = StatusCodes.Status200OK, Data = result };
            }
            else
            {
                return new ResponseModel { Success = false, StatusCode = StatusCodes.Status404NotFound, Message = "User Role does not exists." };
            }
        }

        public async Task<ResponseModel> UpdateUserRole(UserRole updateEntity)
        {
            var entityResult = await GetUserRoleById(updateEntity.UserRoleId);

            if (!entityResult.Success) { return entityResult; }

            var entity = entityResult.Data as UserRole;
            entity.UserId = updateEntity.UserId;
            entity.RoleId = updateEntity.RoleId;

            var result = await _userRoleRepository.SaveChangesAsync();

            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "User Role updated successfully.",
                    Data = entity
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "User Role does not updated." };
        }
    }
}
