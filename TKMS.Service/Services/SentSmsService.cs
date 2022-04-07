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
    public class SentSmsService : ISentSmsService
    {
        private readonly ISentSmsRepository _sentSmsRepository;
        private readonly IUserProviderService _userProviderService;

        public SentSmsService(
            ISentSmsRepository sentSmsRepository,
            IUserProviderService userProviderService
        )
        {
            _sentSmsRepository = sentSmsRepository;
            _userProviderService = userProviderService;
        }

        public async Task<ResponseModel> CreateSentSms(SentSms entity)
        {
            entity.CreatedBy = _userProviderService.UserClaim.UserId;
            entity.UpdatedBy = _userProviderService.UserClaim.UserId;
            await _sentSmsRepository.AddAsync(entity);
            var result = await _sentSmsRepository.SaveChangesAsync();
            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "SentSms created successfully.",
                    Data = entity
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "SentSms does not created." };
        }


        public async Task<ResponseModel> CreateSentSmses(List<SentSms> entities)
        {
            foreach (var entity in entities)
            {
                entity.CreatedBy = _userProviderService.UserClaim.UserId;
                entity.UpdatedBy = _userProviderService.UserClaim.UserId;
                await _sentSmsRepository.AddAsync(entity);
            }
            var result = await _sentSmsRepository.SaveChangesAsync();
            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "SentSms created successfully."
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "SentSms does not created." };
        }

        public async Task<ResponseModel> DeleteSentSms(long id)
        {
            var entityResult = await GetSentSmsById(id);

            if (!entityResult.Success) { return entityResult; }

            var entity = entityResult.Data as SentSms;
            entity.UpdatedBy = _userProviderService.UserClaim.UserId;
            entity.IsDeleted = true;
            var result = await _sentSmsRepository.SaveChangesAsync();

            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "SentSms deleted successfully.",
                    Data = entity
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "SentSms does not deleted." };
        }

        public async Task<ResponseModel> GetAllSentSms(bool? isActive = null)
        {
            return new ResponseModel
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Data = await _sentSmsRepository.Find(a => a.IsDeleted == false && (!isActive.HasValue || a.IsActive == isActive))
            };
        }

        public async Task<ResponseModel> GetSentSmsPaged(Pagination pagination)
        {
            return new ResponseModel
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Data = await _sentSmsRepository.GetSentSmsPaged(pagination)
            };
        }

        public async Task<ResponseModel> GetSentSmsById(long id)
        {
            var result = await _sentSmsRepository.SingleOrDefaultAsync(a => a.SentSmsId == id);
            if (result != null)
            {
                return new ResponseModel { Success = true, StatusCode = StatusCodes.Status200OK, Data = result };
            }
            else
            {
                return new ResponseModel { Success = false, StatusCode = StatusCodes.Status404NotFound, Message = "SentSms does not exists." };
            }
        }

        public async Task<ResponseModel> UpdateSentSms(SentSms updateEntity)
        {
            var entityResult = await GetSentSmsById(updateEntity.SentSmsId);

            if (!entityResult.Success) { return entityResult; }

            var entity = entityResult.Data as SentSms;
            entity.IsActive = updateEntity.IsActive;
            entity.UpdatedDate = CommonUtils.GetDefaultDateTime();
            entity.UpdatedBy = _userProviderService.UserClaim.UserId;

            var result = await _sentSmsRepository.SaveChangesAsync();

            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "SentSms updated successfully.",
                    Data = entity
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "SentSms does not updated." };
        }
    }
}
