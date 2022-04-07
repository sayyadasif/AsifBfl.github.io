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
    public class C5CodeService : IC5CodeService
    {
        private readonly IC5CodeRepository _c5CodeRepository;
        private readonly IUserProviderService _userProviderService;

        public C5CodeService(
            IC5CodeRepository c5CodeRepository,
            IUserProviderService userProviderService
        )
        {
            _c5CodeRepository = c5CodeRepository;
            _userProviderService = userProviderService;
        }

        public async Task<ResponseModel> CreateC5Code(C5Code entity)
        {
            var existEntity = await GetC5CodeByName(entity.C5CodeName);
            if (existEntity.Success)
            {
                return new ResponseModel
                {
                    Success = false,
                    StatusCode = StatusCodes.Status409Conflict,
                    Message = "C5 Code already exists.",
                };
            }

            existEntity = await GetC5CodeByCardType(entity.CardTypeId);
            if (existEntity.Success)
            {
                return new ResponseModel
                {
                    Success = false,
                    StatusCode = StatusCodes.Status409Conflict,
                    Message = "Card Type already mapped with other C5 Code.",
                };
            }

            entity.CreatedBy = _userProviderService.UserClaim.UserId;
            entity.UpdatedBy = _userProviderService.UserClaim.UserId;
            await _c5CodeRepository.AddAsync(entity);
            var result = await _c5CodeRepository.SaveChangesAsync();
            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "C5 Code created successfully.",
                    Data = entity
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "C5 Code does not created." };
        }

        public async Task<ResponseModel> DeleteC5Code(long id)
        {
            var entityResult = await GetC5CodeById(id);

            if (!entityResult.Success) { return entityResult; }

            var entity = entityResult.Data as C5Code;
            entity.UpdatedBy = _userProviderService.UserClaim.UserId;
            entity.IsDeleted = true;
            var result = await _c5CodeRepository.SaveChangesAsync();

            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "C5 Code deleted successfully.",
                    Data = entity
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "C5 Code does not deleted." };
        }

        public async Task<ResponseModel> GetAllC5Code(bool? isActive = null)
        {
            return new ResponseModel
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Data = await _c5CodeRepository.Find(a => !a.IsDeleted && (!isActive.HasValue || a.IsActive == isActive))
            };
        }

        public async Task<ResponseModel> GetC5CodePaged(Pagination pagination)
        {
            return new ResponseModel
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Data = await _c5CodeRepository.GetC5CodePaged(pagination)
            };
        }

        public async Task<ResponseModel> GetC5CodeById(long id)
        {
            var result = await _c5CodeRepository.SingleOrDefaultAsync(a => a.C5CodeId == id);
            if (result != null)
            {
                return new ResponseModel { Success = true, StatusCode = StatusCodes.Status200OK, Data = result };
            }
            else
            {
                return new ResponseModel { Success = false, StatusCode = StatusCodes.Status404NotFound, Message = "C5 Code does not exists." };
            }
        }


        public async Task<ResponseModel> GetC5CodeByName(string c5CodeName)
        {
            var result = await _c5CodeRepository.SingleOrDefaultAsync(a => a.C5CodeName == c5CodeName);
            if (result != null)
            {
                return new ResponseModel { Success = true, StatusCode = StatusCodes.Status200OK, Data = result };
            }
            else
            {
                return new ResponseModel { Success = false, StatusCode = StatusCodes.Status404NotFound, Message = "C5 Code does not exists." };
            }
        }


        public async Task<ResponseModel> GetC5CodeByCardType(long cardTypeId)
        {
            var result = await _c5CodeRepository.SingleOrDefaultAsync(a => a.CardTypeId == cardTypeId);
            if (result != null)
            {
                return new ResponseModel { Success = true, StatusCode = StatusCodes.Status200OK, Data = result };
            }
            else
            {
                return new ResponseModel { Success = false, StatusCode = StatusCodes.Status404NotFound, Message = "Card Type does not exists." };
            }
        }


        public async Task<ResponseModel> UpdateC5Code(C5Code updateEntity)
        {
            var entityResult = await GetC5CodeById(updateEntity.C5CodeId);

            var existEntity = await GetC5CodeByName(updateEntity.C5CodeName);
            if (existEntity.Success && (existEntity.Data as C5Code).C5CodeId != updateEntity.C5CodeId)
            {
                return new ResponseModel
                {
                    Success = false,
                    StatusCode = StatusCodes.Status409Conflict,
                    Message = "C5 Code already exists.",
                };
            }

            existEntity = await GetC5CodeByCardType(updateEntity.CardTypeId);
            if (existEntity.Success && (existEntity.Data as C5Code).C5CodeId != updateEntity.C5CodeId)
            {
                return new ResponseModel
                {
                    Success = false,
                    StatusCode = StatusCodes.Status409Conflict,
                    Message = "Card Type already mapped with other C5 Code.",
                };
            }

            if (!entityResult.Success) { return entityResult; }

            var entity = entityResult.Data as C5Code;
            entity.C5CodeName = updateEntity.C5CodeName;
            entity.CardTypeId = updateEntity.CardTypeId;
            entity.SortOrder = updateEntity.SortOrder;
            entity.IsActive = updateEntity.IsActive;
            entity.UpdatedDate = CommonUtils.GetDefaultDateTime();
            entity.UpdatedBy = _userProviderService.UserClaim.UserId;

            var result = await _c5CodeRepository.SaveChangesAsync();

            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "C5 Code updated successfully.",
                    Data = entity
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "C5 Code does not updated." };
        }

        public async Task<List<C5CodeDropdownModel>> GetDropdwon(long? id = null, long? schemeCodeId = null)
        {
            return (await _c5CodeRepository.GetDropdwon(id, schemeCodeId)).Data;
        }
    }
}
