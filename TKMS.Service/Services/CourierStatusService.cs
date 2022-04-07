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
    public class CourierStatusService : ICourierStatusService
    {
        private readonly ICourierStatusRepository _courierStatusRepository;
        private readonly IUserProviderService _userProviderService;

        public CourierStatusService(
            ICourierStatusRepository courierStatusRepository,
            IUserProviderService userProviderService
        )
        {
            _courierStatusRepository = courierStatusRepository;
            _userProviderService = userProviderService;
        }

        public async Task<ResponseModel> CreateCourierStatus(CourierStatus entity)
        {
            var existEntity = await GetCourierStatusById(entity.CourierStatusId);
            if (existEntity.Success)
            {
                return new ResponseModel
                {
                    Success = false,
                    StatusCode = StatusCodes.Status409Conflict,
                    Message = "CourierStatus already exists.",
                };
            }

            entity.CreatedBy = _userProviderService.UserClaim.UserId;
            entity.UpdatedBy = _userProviderService.UserClaim.UserId;
            await _courierStatusRepository.AddAsync(entity);
            var result = await _courierStatusRepository.SaveChangesAsync();
            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "CourierStatus created successfully.",
                    Data = entity
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "CourierStatus does not created." };
        }

        public async Task<ResponseModel> DeleteCourierStatus(long id)
        {
            var entityResult = await GetCourierStatusById(id);

            if (!entityResult.Success) { return entityResult; }

            var entity = entityResult.Data as CourierStatus;
            entity.UpdatedBy = _userProviderService.UserClaim.UserId;
            entity.IsDeleted = true;
            var result = await _courierStatusRepository.SaveChangesAsync();

            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "CourierStatus deleted successfully.",
                    Data = entity
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "CourierStatus does not deleted." };
        }

        public async Task<ResponseModel> GetAllCourierStatus(bool? isActive = null)
        {
            return new ResponseModel
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Data = await _courierStatusRepository.Find(a => a.IsDeleted == false && (!isActive.HasValue || a.IsActive == isActive))
            };
        }

        public async Task<ResponseModel> GetCourierStatusPaged(Pagination pagination)
        {
            return new ResponseModel
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Data = await _courierStatusRepository.GetCourierStatusPaged(pagination)
            };
        }

        public async Task<ResponseModel> GetCourierStatusById(long id)
        {
            var result = await _courierStatusRepository.SingleOrDefaultAsync(a => a.CourierStatusId == id);
            if (result != null)
            {
                return new ResponseModel { Success = true, StatusCode = StatusCodes.Status200OK, Data = result };
            }
            else
            {
                return new ResponseModel { Success = false, StatusCode = StatusCodes.Status404NotFound, Message = "CourierStatus does not exists." };
            }
        }

        public async Task<ResponseModel> UpdateCourierStatus(CourierStatus updateEntity)
        {
            var entityResult = await GetCourierStatusById(updateEntity.CourierStatusId);

            if (!entityResult.Success) { return entityResult; }

            var entity = entityResult.Data as CourierStatus;
            entity.StatusName = updateEntity.StatusName;
            entity.SortOrder = updateEntity.SortOrder;
            entity.IsActive = updateEntity.IsActive;
            entity.UpdatedDate = CommonUtils.GetDefaultDateTime();
            entity.UpdatedBy = _userProviderService.UserClaim.UserId;

            var result = await _courierStatusRepository.SaveChangesAsync();

            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "CourierStatus updated successfully.",
                    Data = entity
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "CourierStatus does not updated." };
        }

        public async Task<List<DropdownModel>> GetDropdwon(long? id = null, bool? isActive = null)
        {
            return (await _courierStatusRepository.GetDropdwon(id, isActive)).Data;
        }
    }
}
