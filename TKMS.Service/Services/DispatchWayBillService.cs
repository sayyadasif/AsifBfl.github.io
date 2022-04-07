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
    public class DispatchWayBillService : IDispatchWayBillService
    {
        private readonly IDispatchWayBillRepository _dispatchWayBillRepository;
        private readonly IUserProviderService _userProviderService;

        public DispatchWayBillService(
            IDispatchWayBillRepository dispatchWayBillRepository,
            IUserProviderService userProviderService
        )
        {
            _dispatchWayBillRepository = dispatchWayBillRepository;
            _userProviderService = userProviderService;
        }

        public async Task<ResponseModel> CreateDispatchWayBill(DispatchWayBill entity)
        {
            var existEntity = await GetDispatchWayBillById(entity.DispatchWayBillId);
            if (existEntity.Success)
            {
                return new ResponseModel
                {
                    Success = false,
                    StatusCode = StatusCodes.Status409Conflict,
                    Message = "DispatchWayBill already exists.",
                };
            }

            entity.CreatedBy = _userProviderService.UserClaim.UserId;
            entity.UpdatedBy = _userProviderService.UserClaim.UserId;
            await _dispatchWayBillRepository.AddAsync(entity);
            var result = await _dispatchWayBillRepository.SaveChangesAsync();
            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "DispatchWayBill created successfully.",
                    Data = entity
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "DispatchWayBill does not created." };
        }

        public async Task<ResponseModel> DeleteDispatchWayBill(long id)
        {
            var entityResult = await GetDispatchWayBillById(id);

            if (!entityResult.Success) { return entityResult; }

            var entity = entityResult.Data as DispatchWayBill;
            entity.UpdatedBy = _userProviderService.UserClaim.UserId;
            entity.IsDeleted = true;
            var result = await _dispatchWayBillRepository.SaveChangesAsync();

            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "DispatchWayBill deleted successfully.",
                    Data = entity
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "DispatchWayBill does not deleted." };
        }

        public async Task<ResponseModel> GetAllDispatchWayBill(bool? isActive = null)
        {
            return new ResponseModel
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Data = await _dispatchWayBillRepository.Find(a => a.IsDeleted == false && (!isActive.HasValue || a.IsActive == isActive))
            };
        }

        public async Task<ResponseModel> GetDispatchWayBillPaged(Pagination pagination)
        {
            return new ResponseModel
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Data = await _dispatchWayBillRepository.GetDispatchWayBillPaged(pagination)
            };
        }

        public async Task<ResponseModel> GetDispatchWayBillById(long id)
        {
            var result = await _dispatchWayBillRepository.SingleOrDefaultAsync(a => a.DispatchWayBillId == id);
            if (result != null)
            {
                return new ResponseModel { Success = true, StatusCode = StatusCodes.Status200OK, Data = result };
            }
            else
            {
                return new ResponseModel { Success = false, StatusCode = StatusCodes.Status404NotFound, Message = "DispatchWayBill does not exists." };
            }
        }

        public async Task<ResponseModel> UpdateDispatchWayBill(DispatchWayBill updateEntity)
        {
            var entityResult = await GetDispatchWayBillById(updateEntity.DispatchWayBillId);

            if (!entityResult.Success) { return entityResult; }

            var entity = entityResult.Data as DispatchWayBill;
            entity.DispatchId = updateEntity.DispatchId;
            entity.WayBillNo = updateEntity.WayBillNo;
            entity.DispatchDate = updateEntity.DispatchDate;
            entity.DeliveryDate = updateEntity.DeliveryDate;
            entity.CourierStatusId = updateEntity.CourierStatusId;
            entity.ReceiveBy = updateEntity.ReceiveBy;
            entity.ReceiveStatusId = updateEntity.ReceiveStatusId;
            entity.DispatchStatusId = updateEntity.DispatchStatusId;
            entity.CourierPartner = updateEntity.CourierPartner;
            entity.Remarks = updateEntity.Remarks;
            entity.IsActive = updateEntity.IsActive;
            entity.UpdatedDate = CommonUtils.GetDefaultDateTime();
            entity.UpdatedBy = _userProviderService.UserClaim.UserId;

            var result = await _dispatchWayBillRepository.SaveChangesAsync();

            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "DispatchWayBill updated successfully.",
                    Data = entity
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "DispatchWayBill does not updated." };
        }
    }
}
