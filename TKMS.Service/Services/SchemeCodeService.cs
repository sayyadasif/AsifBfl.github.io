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
    public class SchemeCodeService : ISchemeCodeService
    {
        private readonly ISchemeCodeRepository _schemeCodeRepository;
        private readonly IUserProviderService _userProviderService;

        public SchemeCodeService(
            ISchemeCodeRepository schemeCodeRepository,
            IUserProviderService userProviderService
        )
        {
            _schemeCodeRepository = schemeCodeRepository;
            _userProviderService = userProviderService;
        }

        public async Task<ResponseModel> CreateSchemeCode(SchemeCode entity)
        {
            var existEntity = await GetSchemeCodeById(entity.SchemeCodeId);
            if (existEntity.Success)
            {
                return new ResponseModel
                {
                    Success = false,
                    StatusCode = StatusCodes.Status409Conflict,
                    Message = "Scheme Code already exists.",
                };
            }

            entity.CreatedBy = _userProviderService.UserClaim.UserId;
            entity.UpdatedBy = _userProviderService.UserClaim.UserId;

            foreach (var c5CodeId in entity.SelectedC5Codes)
            {
                entity.SchemeC5Codes.Add(new SchemeC5Code { C5CodeId = c5CodeId });
            }

            await _schemeCodeRepository.AddAsync(entity);
            var result = await _schemeCodeRepository.SaveChangesAsync();
            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Scheme Code created successfully.",
                    Data = entity
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "Scheme Code does not created." };
        }

        public async Task<ResponseModel> DeleteSchemeCode(long id)
        {
            var entityResult = await GetSchemeCodeById(id);

            if (!entityResult.Success) { return entityResult; }

            var entity = entityResult.Data as SchemeCode;
            entity.UpdatedBy = _userProviderService.UserClaim.UserId;
            entity.IsDeleted = true;
            var result = await _schemeCodeRepository.SaveChangesAsync();

            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Scheme Code deleted successfully.",
                    Data = entity
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "Scheme Code does not deleted." };
        }

        public async Task<ResponseModel> GetAllSchemeCode(bool? isActive = null)
        {
            return new ResponseModel
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Data = await _schemeCodeRepository.Find(a => !a.IsDeleted && (!isActive.HasValue || a.IsActive == isActive), b => b.SchemeC5Codes)
            };
        }

        public async Task<ResponseModel> GetSchemeCodePaged(Pagination pagination)
        {
            return new ResponseModel
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Data = await _schemeCodeRepository.GetSchemeCodePaged(pagination)
            };
        }

        public async Task<ResponseModel> GetSchemeCodeById(long id)
        {
            var result = await _schemeCodeRepository.SingleOrDefaultAsync(a => a.SchemeCodeId == id, b => b.SchemeC5Codes);
            if (result != null)
            {
                return new ResponseModel { Success = true, StatusCode = StatusCodes.Status200OK, Data = result };
            }
            else
            {
                return new ResponseModel { Success = false, StatusCode = StatusCodes.Status404NotFound, Message = "Scheme Code does not exists." };
            }
        }

        public async Task<ResponseModel> UpdateSchemeCode(SchemeCode updateEntity)
        {
            var entityResult = await GetSchemeCodeById(updateEntity.SchemeCodeId);

            if (!entityResult.Success) { return entityResult; }

            var entity = entityResult.Data as SchemeCode;
            entity.SchemeCodeName = updateEntity.SchemeCodeName;
            entity.SortOrder = updateEntity.SortOrder;
            entity.IsActive = updateEntity.IsActive;
            entity.UpdatedDate = CommonUtils.GetDefaultDateTime();
            entity.UpdatedBy = _userProviderService.UserClaim.UserId;

            var schemeC5Code = new List<SchemeC5Code>();
            foreach (var c5CodeId in updateEntity.SelectedC5Codes)
            {
                var schemeCode = entity.SchemeC5Codes.FirstOrDefault(ur => ur.C5CodeId == c5CodeId);
                if (schemeCode != null)
                {
                    schemeC5Code.Add(schemeCode);
                }
                else
                {
                    schemeC5Code.Add(new SchemeC5Code { C5CodeId = c5CodeId });
                }
            }
            entity.SchemeC5Codes = schemeC5Code;

            var result = await _schemeCodeRepository.SaveChangesAsync();

            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Scheme Code updated successfully.",
                    Data = entity
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "Scheme Code does not updated." };
        }

        public async Task<List<DropdownModel>> GetDropdwon(long? id = null, bool? isActive = null)
        {
            return (await _schemeCodeRepository.GetDropdwon(id, isActive)).Data;
        }
    }
}
