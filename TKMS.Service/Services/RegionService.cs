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
    public class RegionService : IRegionService
    {
        private readonly IRegionRepository _regionRepository;
        private readonly IUserProviderService _userProviderService;
        private readonly IAddressService _addressService;

        public RegionService(
            IRegionRepository regionRepository,
            IUserProviderService userProviderService,
            IAddressService addressService
        )
        {
            _regionRepository = regionRepository;
            _userProviderService = userProviderService;
            _addressService = addressService;
        }

        public async Task<ResponseModel> CreateRegion(Region entity)
        {
            var existEntity = await _regionRepository.SingleOrDefaultAsync(a => a.RegionName == entity.RegionName);
            if (existEntity != null)
            {
                return new ResponseModel
                {
                    Success = false,
                    StatusCode = StatusCodes.Status409Conflict,
                    Message = "Region Name already exists.",
                };
            }

            entity.CreatedBy = _userProviderService.UserClaim.UserId;
            entity.UpdatedBy = _userProviderService.UserClaim.UserId;

            entity.Address.CreatedBy = _userProviderService.UserClaim.UserId;
            entity.Address.UpdatedBy = _userProviderService.UserClaim.UserId;

            await _regionRepository.AddAsync(entity);
            var result = await _regionRepository.SaveChangesAsync();
            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Region created successfully.",
                    Data = entity
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "Region does not created." };
        }

        public async Task<ResponseModel> CreateRegions(List<Region> entities)
        {
            foreach (var entity in entities)
            {
                entity.CreatedBy = _userProviderService.UserClaim.UserId;
                entity.UpdatedBy = _userProviderService.UserClaim.UserId;
                entity.Address.CreatedBy = _userProviderService.UserClaim.UserId;
                entity.Address.UpdatedBy = _userProviderService.UserClaim.UserId;
                await _regionRepository.AddAsync(entity);
            }
            var result = await _regionRepository.SaveChangesAsync();
            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Regions created successfully."
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "Regions does not created." };
        }

        public async Task<ResponseModel> DeleteRegion(long id)
        {
            var entityResult = await GetRegionById(id);

            if (!entityResult.Success) { return entityResult; }

            var entity = entityResult.Data as Region;
            entity.UpdatedBy = _userProviderService.UserClaim.UserId;
            entity.IsDeleted = true;
            var result = await _regionRepository.SaveChangesAsync();

            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Region deleted successfully.",
                    Data = entity
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "Region does not deleted." };
        }

        public async Task<ResponseModel> GetAllRegion(bool? isActive = null)
        {
            return new ResponseModel
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Data = await _regionRepository.Find(a => a.IsDeleted == false && (!isActive.HasValue || a.IsActive == isActive))
            };
        }

        public async Task<ResponseModel> GetRegionPaged(Pagination pagination)
        {
            return new ResponseModel
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Data = await _regionRepository.GetRegionPaged(pagination)
            };
        }

        public async Task<ResponseModel> GetRegionById(long id)
        {
            var result = await _regionRepository.SingleOrDefaultAsync(a => a.RegionId == id);
            if (result != null)
            {
                if (result.AddressId > 0)
                {
                    result.Address = (await _addressService.GetAddressById(result.AddressId)).Data;
                }
                return new ResponseModel { Success = true, StatusCode = StatusCodes.Status200OK, Data = result };
            }
            else
            {
                return new ResponseModel { Success = false, StatusCode = StatusCodes.Status404NotFound, Message = "Region does not exists." };
            }
        }

        public async Task<ResponseModel> UpdateRegion(Region updateEntity)
        {
            var entityResult = await GetRegionById(updateEntity.RegionId);

            if (!entityResult.Success) { return entityResult; }

            var entity = entityResult.Data as Region;
            entity.SystemRoId = updateEntity.SystemRoId;
            entity.RegionName = updateEntity.RegionName;
            entity.SortOrder = updateEntity.SortOrder;
            entity.IsActive = updateEntity.IsActive;
            entity.UpdatedDate = CommonUtils.GetDefaultDateTime();
            entity.UpdatedBy = _userProviderService.UserClaim.UserId;

            entity.Address = updateEntity.Address;
            entity.Address.UpdatedDate = CommonUtils.GetDefaultDateTime();
            entity.Address.UpdatedBy = _userProviderService.UserClaim.UserId;

            var result = await _regionRepository.SaveChangesAsync();

            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Region updated successfully.",
                    Data = entity
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "Region does not updated." };
        }

        public async Task<List<DropdownModel>> GetDropdwon(long? id = null, bool? isActive = null)
        {
            return (await _regionRepository.GetDropdwon(id, isActive)).Data;
        }
    }
}
