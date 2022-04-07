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
    public class AddressService : IAddressService
    {
        private readonly IAddressRepository _addressRepository;
        private readonly IUserProviderService _userProviderService;

        public AddressService(
            IAddressRepository addressRepository,
            IUserProviderService userProviderService
        )
        {
            _addressRepository = addressRepository;
            _userProviderService = userProviderService;
        }

        public async Task<ResponseModel> CreateAddress(Address entity)
        {
            entity.CreatedBy = _userProviderService.UserClaim.UserId;
            entity.UpdatedBy = _userProviderService.UserClaim.UserId;
            await _addressRepository.AddAsync(entity);
            var result = await _addressRepository.SaveChangesAsync();
            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Address created successfully.",
                    Data = entity
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "Address does not created." };
        }

        public async Task<ResponseModel> DeleteAddress(long id)
        {
            var entityResult = await GetAddressById(id);

            if (!entityResult.Success) { return entityResult; }

            var entity = entityResult.Data as Address;
            entity.UpdatedBy = _userProviderService.UserClaim.UserId;
            entity.IsDeleted = true;
            var result = await _addressRepository.SaveChangesAsync();

            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Address deleted successfully.",
                    Data = entity
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "Address does not deleted." };
        }

        public async Task<ResponseModel> GetAllAddress(bool? isActive = null)
        {
            return new ResponseModel
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Data = await _addressRepository.Find(a => a.IsDeleted == false && (!isActive.HasValue || a.IsActive == isActive))
            };
        }

        public async Task<ResponseModel> GetAddressPaged(Pagination pagination)
        {
            return new ResponseModel
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Data = await _addressRepository.GetAddressPaged(pagination)
            };
        }

        public async Task<ResponseModel> GetAddressById(long id)
        {
            var result = await _addressRepository.SingleOrDefaultAsync(a => a.AddressId == id);
            if (result != null)
            {
                return new ResponseModel { Success = true, StatusCode = StatusCodes.Status200OK, Data = result };
            }
            else
            {
                return new ResponseModel { Success = false, StatusCode = StatusCodes.Status404NotFound, Message = "Address does not exists." };
            }
        }

        public async Task<ResponseModel> UpdateAddress(Address updateEntity)
        {
            var entityResult = await GetAddressById(updateEntity.AddressId);

            if (!entityResult.Success) { return entityResult; }

            var entity = entityResult.Data as Address;
            entity.AddressDetail = updateEntity.AddressDetail;
            entity.Region = updateEntity.Region;
            entity.Zone = updateEntity.Zone;
            entity.District = updateEntity.District;
            entity.State = updateEntity.State;
            entity.PinCode = updateEntity.PinCode;
            entity.IsActive = updateEntity.IsActive;
            entity.UpdatedDate = CommonUtils.GetDefaultDateTime();
            entity.UpdatedBy = _userProviderService.UserClaim.UserId;

            var result = await _addressRepository.SaveChangesAsync();

            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Address updated successfully.",
                    Data = entity
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "Address does not updated." };
        }
    }
}
