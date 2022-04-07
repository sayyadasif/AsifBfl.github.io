using Core.Repository.Models;
using Core.Security;
using Core.Utility.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
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
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IBranchService _branchService;
        private readonly ITokenBuilder _tokenBuilder;
        private readonly IUserProviderService _userProviderService;
        private readonly BfilAuthSettings _bfilAuthSettings;

        public UserService(
            ITokenBuilder tokenBuilder,
            IBranchService branchService,
            IUserRepository userRepository,
            IUserProviderService userProviderService,
            IOptions<BfilAuthSettings> bfilAuthSettings
        )
        {
            _tokenBuilder = tokenBuilder;
            _branchService = branchService;
            _userRepository = userRepository;
            _userProviderService = userProviderService;
            _bfilAuthSettings = bfilAuthSettings.Value;
        }

        public async Task<ResponseModel> CreateUser(User entity)
        {
            var existEntity = await GetUserByStaffId(entity.StaffId);
            if (existEntity.Success)
            {
                return new ResponseModel
                {
                    Success = false,
                    StatusCode = StatusCodes.Status409Conflict,
                    Message = "User StaffId already exists.",
                };
            }

            entity.CreatedBy = _userProviderService.UserClaim.UserId;
            entity.UpdatedBy = _userProviderService.UserClaim.UserId;

            foreach (var roleId in entity.SelectedRoles)
            {
                entity.UserRoles.Add(new UserRole { RoleId = roleId });
            }

            if (entity.RoleTypeId == RoleTypes.CPU.GetHashCode() || entity.RoleTypeId == RoleTypes.SA.GetHashCode())
            {
                entity.Salt = PasswordUtility.GenerateSalt();
                entity.Password = PasswordUtility.GenerateHash(entity.Password, entity.Salt);
            }

            await _userRepository.AddAsync(entity);
            var result = await _userRepository.SaveChangesAsync();
            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "User created successfully.",
                    Data = entity
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "User does not created." };
        }

        public async Task<ResponseModel> CreateUsers(List<User> entities)
        {
            foreach (var entity in entities)
            {
                entity.CreatedBy = _userProviderService.UserClaim.UserId;
                entity.UpdatedBy = _userProviderService.UserClaim.UserId;
                await _userRepository.AddAsync(entity);
            }
            var result = await _userRepository.SaveChangesAsync();
            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Users created successfully."
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "Users does not created." };
        }

        public async Task<ResponseModel> DeleteUser(long id)
        {
            var entityResult = await GetUserById(id);

            if (!entityResult.Success) { return entityResult; }

            var entity = entityResult.Data as User;
            entity.UpdatedBy = _userProviderService.UserClaim.UserId;
            entity.IsDeleted = true;
            var result = await _userRepository.SaveChangesAsync();

            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "User deleted successfully.",
                    Data = entity
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "User does not deleted." };
        }

        public async Task<ResponseModel> GetAllUser(bool? isActive = null)
        {
            return new ResponseModel
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Data = await _userRepository.Find(a => a.IsDeleted == false && (!isActive.HasValue || a.IsActive == isActive))
            };
        }

        public async Task<ResponseModel> GetUserPaged(Pagination pagination)
        {
            return new ResponseModel
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Data = await _userRepository.GetUserPaged(pagination)
            };
        }

        public async Task<ResponseModel> GetUserById(long id)
        {
            var result = await _userRepository.SingleOrDefaultAsync(a => a.UserId == id, b => b.UserRoles);
            if (result != null)
            {
                return new ResponseModel { Success = true, StatusCode = StatusCodes.Status200OK, Data = result };
            }
            else
            {
                return new ResponseModel { Success = false, StatusCode = StatusCodes.Status404NotFound, Message = "User does not exists." };
            }
        }

        public async Task<ResponseModel> GetUserByStaffId(string staffId)
        {
            var result = (await _userRepository.SingleOrDefaultAsync(a => a.StaffId == staffId, b => b.UserRoles));
            if (result != null)
            {
                if (result.BranchId > 0)
                {
                    result.Branch = (await _branchService.GetBranchById(result.BranchId)).Data;
                }
                return new ResponseModel { Success = true, StatusCode = StatusCodes.Status200OK, Data = result };
            }
            else
            {
                return new ResponseModel { Success = false, StatusCode = StatusCodes.Status404NotFound, Message = "User does not exists." };
            }
        }

        public async Task<ResponseModel> UpdateUser(User updateEntity)
        {
            var entityResult = await GetUserById(updateEntity.UserId);

            if (!entityResult.Success) { return entityResult; }

            var entity = entityResult.Data as User;
            entity.StaffId = updateEntity.StaffId;
            entity.FullName = updateEntity.FullName;
            entity.BranchId = updateEntity.BranchId;
            entity.RoleTypeId = updateEntity.RoleTypeId;
            entity.Email = updateEntity.Email;
            entity.Password = updateEntity.Password;
            entity.Salt = updateEntity.Salt;
            entity.MobileNo = updateEntity.MobileNo;
            entity.IsActive = updateEntity.IsActive;
            entity.UpdatedDate = CommonUtils.GetDefaultDateTime();
            entity.UpdatedBy = _userProviderService.UserClaim.UserId;

            var userRoles = new List<UserRole>();
            foreach (var roleId in updateEntity.SelectedRoles)
            {
                var userRole = entity.UserRoles.FirstOrDefault(ur => ur.RoleId == roleId);
                if (userRole != null)
                {
                    userRoles.Add(userRole);
                }
                else
                {
                    userRoles.Add(new UserRole { RoleId = roleId });
                }
            }
            entity.UserRoles = userRoles;

            if (entity.RoleTypeId == RoleTypes.CPU.GetHashCode() || entity.RoleTypeId == RoleTypes.SA.GetHashCode())
            {
                if (updateEntity.IsUpdatePassword)
                {
                    if (string.IsNullOrEmpty(entity.Salt))
                    {
                        entity.Salt = PasswordUtility.GenerateSalt();
                    }
                    entity.Password = PasswordUtility.GenerateHash(updateEntity.Password, entity.Salt);
                }
            }
            else
            {
                entity.Salt = string.Empty;
                entity.Password = string.Empty;
            }


            var result = await _userRepository.SaveChangesAsync();

            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "User updated successfully.",
                    Data = entity
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "User does not updated." };
        }

        public async Task<ResponseModel> AuthenticateUser(LoginModel model)
        {
            ResponseModel entityResult;
            User user = null;
            if (model.IsSystemUser)
            {
                entityResult = await AuthenticateSystemUser(model);

                if (!entityResult.Success) { return entityResult; }

                user = entityResult.Data as User;

                if (user.RoleTypeId != RoleTypes.CPU.GetHashCode() && user.RoleTypeId != RoleTypes.SA.GetHashCode())
                {
                    return new ResponseModel { Success = false, StatusCode = StatusCodes.Status405MethodNotAllowed, Message = "User does not allow" };
                }

                var branch = (await _branchService.GetDropdwon(id: user.BranchId)).FirstOrDefault();
                user.BranchCode = branch != null ? branch.BranchCode : "";
            }
            else
            {
                //var authReponse = await AuthenticateBfilUser(model);
                //if (!authReponse.Success)
                //{
                //    return authReponse;
                //}

                //var bfilUser = authReponse.Data as BfilAuthResult;

                //if (bfilUser.EMP_ISACTIVE != "1")
                //{
                //    return new ResponseModel { Success = false, StatusCode = StatusCodes.Status405MethodNotAllowed, Message = "User does not active" };
                //}

                entityResult = await GetUserByStaffId(model.StaffId);

                if (!entityResult.Success) { return entityResult; }

                user = entityResult.Data as User;

                var branch = (await _branchService.GetDropdwon(id: user.BranchId)).FirstOrDefault();
                user.BranchCode = branch != null ? branch.BranchCode : "";

                //if (bfilUser.BranchCode != user.BranchCode)
                //{
                //    return new ResponseModel { Success = false, StatusCode = StatusCodes.Status405MethodNotAllowed, Message = "User Branch did not match" };
                //}

                if ((model.RoleTypeId.HasValue && model.RoleTypeId != user.RoleTypeId) ||
                     user.RoleTypeId == RoleTypes.CPU.GetHashCode() || user.RoleTypeId == RoleTypes.SA.GetHashCode())
                {
                    return new ResponseModel { Success = false, StatusCode = StatusCodes.Status405MethodNotAllowed, Message = "User does not allow" };
                }
            }

            return new ResponseModel
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Message = "User Login successfully.",
                Data = new
                {
                    Token = _tokenBuilder.BuildToken(
                        new UserClaim
                        {
                            UserId = user.UserId,
                            Name = user.FullName,
                            BranchId = user.BranchId,
                            BranchCode = user.BranchCode,
                            RoleTypeId = user.RoleTypeId,
                            RoleIds = user.UserRoles.Select(r => r.RoleId).ToList()
                        }),
                    User = user,
                }
            };
        }

        public async Task<ResponseModel> AuthenticateBfilUser(LoginModel model)
        {
            try
            {
                var restClient = new RestClient(_bfilAuthSettings.LoginUrl);
                var request = new RestRequest(Method.POST)
                {
                    Resource = "api/Login/Login/{value}",
                    RequestFormat = DataFormat.Json
                }
                .AddJsonBody(new BfilAuthRequest
                {
                    Request = CommonUtils.Encryption($"[{{'USERNAME':'{model.StaffId}','PASSWORD':'{model.Password}'}}]", _bfilAuthSettings.SecretKey)
                })
                .AddHeader(_bfilAuthSettings.ReqHeaderKey, _bfilAuthSettings.ReqHeaderValue);

                var response = await restClient.ExecuteAsync<dynamic>(request);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var result = JsonConvert.DeserializeObject<BfilAuthResponse>(response.Content);
                    var users = JsonConvert.DeserializeObject<List<BfilAuthResult>>(CommonUtils.Decryption(result.Response, _bfilAuthSettings.SecretKey));
                    var user = users.FirstOrDefault();

                    return new ResponseModel
                    {
                        Success = user != null && Convert.ToInt32(user.RESPONSE_CODE) == StatusCodes.Status200OK,
                        StatusCode = Convert.ToInt32(user.RESPONSE_CODE),
                        Message = user.RESPONSE_MESSAGE,
                        Data = user
                    };
                }
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, ex.Message);
            }

            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "User Validation Error" };
        }

        public async Task<List<DropdownModel>> GetDropdwon(long? id = null, long? branchId = null, long? roleTypeId = null, bool? isActive = null)
        {
            return (await _userRepository.GetDropdwon(id, branchId, roleTypeId, isActive)).Data;
        }

        private async Task<ResponseModel> AuthenticateSystemUser(LoginModel model)
        {
            var entityResult = await GetUserByStaffId(model.StaffId);

            if (!entityResult.Success) { return entityResult; }

            var entity = entityResult.Data as User;

            if (entity.IsDeleted)
            {
                return new ResponseModel { Success = false, StatusCode = StatusCodes.Status404NotFound, Message = "User does not exists." };
            }
            if (!entity.IsActive)
            {
                return new ResponseModel { Success = false, StatusCode = StatusCodes.Status405MethodNotAllowed, Message = "User does not active." };
            }

            var hash = PasswordUtility.GenerateHash(model.Password, entity.Salt);
            //if (hash != entity.Password)
            //{
            //    return new ResponseModel { Success = false, StatusCode = StatusCodes.Status406NotAcceptable, Message = "Password did not match." };
            //}

            return new ResponseModel
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Message = "User Login successfully.",
                Data = entity
            };
        }
    }
}
