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
using TKMS.Abstraction.Models;
using TKMS.Repository.Interfaces;
using TKMS.Service.Interfaces;

namespace TKMS.Service.Services
{
    public class IWorksKitService : IIWorksKitService
    {
        private readonly IIWorksKitRepository _iWorksKitRepository;
        private readonly IUserProviderService _userProviderService;
        private readonly IWorksSettings _iWorksSettings;

        public IWorksKitService(
            ITokenBuilder tokenBuilder,
            IIWorksKitRepository iWorksKitRepository,
            IUserProviderService userProviderService,
            IOptions<IWorksSettings> iWorksSettings
        )
        {
            _iWorksKitRepository = iWorksKitRepository;
            _userProviderService = userProviderService;
            _iWorksSettings = iWorksSettings.Value;
        }

        public async Task<ResponseModel> CreateIWorksKit(IWorksKit entity)
        {
            entity.CreatedBy = _userProviderService.UserClaim.UserId;
            entity.UpdatedBy = _userProviderService.UserClaim.UserId;
            await _iWorksKitRepository.AddAsync(entity);
            var result = await _iWorksKitRepository.SaveChangesAsync();
            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "IWorksKit created successfully.",
                    Data = entity
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "IWorksKit does not created." };
        }

        public async Task<ResponseModel> CreateIWorksKits(List<IWorksKit> entities)
        {
            foreach (var entity in entities)
            {
                try
                {
                    entity.CreatedBy = _userProviderService.UserClaim.UserId;
                    entity.UpdatedBy = _userProviderService.UserClaim.UserId;
                }
                catch (Exception)
                {
                    entity.CreatedBy = 1;
                    entity.UpdatedBy = 1;
                }

                await _iWorksKitRepository.AddAsync(entity);
            }
            var result = await _iWorksKitRepository.SaveChangesAsync();
            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "IWorksKit created successfully."
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "IWorksKit does not created." };
        }

        public async Task<ResponseModel> DeleteIWorksKit(long id)
        {
            var entityResult = await GetIWorksKitById(id);

            if (!entityResult.Success) { return entityResult; }

            var entity = entityResult.Data as IWorksKit;
            entity.UpdatedBy = _userProviderService.UserClaim.UserId;
            entity.IsDeleted = true;
            var result = await _iWorksKitRepository.SaveChangesAsync();

            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "IWorksKit deleted successfully.",
                    Data = entity
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "IWorksKit does not deleted." };
        }

        public async Task<ResponseModel> GetAllIWorksKit(bool? isActive = null)
        {
            return new ResponseModel
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Data = await _iWorksKitRepository.Find(a => a.IsDeleted == false && (!isActive.HasValue || a.IsActive == isActive))
            };
        }

        public async Task<List<IWorksKit>> GetIWorksKitPending()
        {
            return await _iWorksKitRepository.GetIWorksKitPending();
        }

        public async Task<ResponseModel> GetIWorksKitById(long id)
        {
            var result = await _iWorksKitRepository.SingleOrDefaultAsync(a => a.IWorksKitId == id);
            if (result != null)
            {
                return new ResponseModel { Success = true, StatusCode = StatusCodes.Status200OK, Data = result };
            }
            else
            {
                return new ResponseModel { Success = false, StatusCode = StatusCodes.Status404NotFound, Message = "IWorksKit does not exists." };
            }
        }

        public async Task<ResponseModel> UpdateIWorksKit(IWorksKit updateEntity)
        {
            var entityResult = await GetIWorksKitById(updateEntity.IWorksKitId);

            if (!entityResult.Success) { return entityResult; }

            var entity = entityResult.Data as IWorksKit;
            entity.IsSuccess = updateEntity.IsSuccess;
            entity.ResponseMessage = updateEntity.ResponseMessage;
            entity.IsActive = updateEntity.IsActive;
            entity.UpdatedDate = CommonUtils.GetDefaultDateTime();
            entity.UpdatedBy = _userProviderService.UserClaim.UserId;

            var result = await _iWorksKitRepository.SaveChangesAsync();

            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "IWorksKit updated successfully.",
                    Data = entity
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "IWorksKit does not updated." };
        }


        public async Task UpdateIWorksStatus(List<Kit> kits, string status)
        {
            var client = new RestClient(_iWorksSettings.BaseUrl);
            var iWorksKits = new List<IWorksKit>();

            foreach (var kit in kits)
            {
                var success = false;
                var message = string.Empty;

                try
                {
                    var request = new RestRequest(string.Format(_iWorksSettings.BaseMethod, kit.AccountNo, status), Method.GET);
                    var response = await client.ExecuteAsync(request);

                    IWorksResult result;
                    try
                    {
                        result = JsonConvert.DeserializeObject<IWorksResult>(response.Content);
                    }
                    catch (Exception)
                    {
                        result = new IWorksResult { errorMessage = response.Content };
                    }

                    success = !string.IsNullOrEmpty(result.successMessage);
                    message = success ? result.successMessage : $"{result.errorMessage}{result.Message}";
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                    Serilog.Log.Error(ex, ex.Message);
                }

                iWorksKits.Add(new IWorksKit
                {
                    KitId = kit.KitId,
                    AccountNo = kit.AccountNo,
                    IWorksStatus = status,
                    ResponseMessage = message,
                    IsSuccess = success
                });
            }

            await CreateIWorksKits(iWorksKits);
        }
    }
}
