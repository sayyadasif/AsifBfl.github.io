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
    public class DispatchAuditService : IDispatchAuditService
    {
        private readonly IDispatchAuditRepository _dispatchAuditRepository;
        private readonly IUserProviderService _userProviderService;

        public DispatchAuditService(
            IDispatchAuditRepository dispatchAuditRepository,
            IUserProviderService userProviderService
        )
        {
            _dispatchAuditRepository = dispatchAuditRepository;
            _userProviderService = userProviderService;
        }

        public async Task<ResponseModel> CreateDispatchAudit(DispatchAudit entity)
        {
            entity.CreatedBy = _userProviderService.UserClaim.UserId;
            entity.UpdatedBy = _userProviderService.UserClaim.UserId;
            await _dispatchAuditRepository.AddAsync(entity);
            var result = await _dispatchAuditRepository.SaveChangesAsync();
            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Dispatch Audit created successfully.",
                    Data = entity
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "Dispatch Audit does not created." };
        }

        public async Task<ResponseModel> DeleteDispatchAudit(long id)
        {
            var entityResult = await GetDispatchAuditById(id);

            if (!entityResult.Success) { return entityResult; }

            var entity = entityResult.Data as DispatchAudit;
            entity.UpdatedBy = _userProviderService.UserClaim.UserId;
            entity.IsDeleted = true;
            var result = await _dispatchAuditRepository.SaveChangesAsync();

            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Dispatch Audit deleted successfully.",
                    Data = entity
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "Dispatch Audit does not deleted." };
        }

        public async Task<ResponseModel> GetAllDispatchAudit(bool? isActive = null)
        {
            return new ResponseModel
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Data = await _dispatchAuditRepository.Find(a => a.IsDeleted == false && (!isActive.HasValue || a.IsActive == isActive))
            };
        }

        public async Task<ResponseModel> GetDispatchAuditPaged(Pagination pagination)
        {
            return new ResponseModel
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Data = await _dispatchAuditRepository.GetDispatchAuditPaged(pagination)
            };
        }

        public async Task<ResponseModel> GetDispatchAuditById(long id)
        {
            var result = await _dispatchAuditRepository.SingleOrDefaultAsync(a => a.DispatchAuditId == id);
            if (result != null)
            {
                return new ResponseModel { Success = true, StatusCode = StatusCodes.Status200OK, Data = result };
            }
            else
            {
                return new ResponseModel { Success = false, StatusCode = StatusCodes.Status404NotFound, Message = "Dispatch Audit does not exists." };
            }
        }

        public async Task<ResponseModel> UpdateDispatchAudit(DispatchAudit updateEntity)
        {
            var entityResult = await GetDispatchAuditById(updateEntity.DispatchAuditId);

            if (!entityResult.Success) { return entityResult; }

            var entity = entityResult.Data as DispatchAudit;
            entity.UpdatedDate = CommonUtils.GetDefaultDateTime();
            entity.UpdatedBy = _userProviderService.UserClaim.UserId;

            var result = await _dispatchAuditRepository.SaveChangesAsync();

            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Dispatch Audit updated successfully.",
                    Data = entity
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "Dispatch Audit does not updated." };
        }
    }
}
