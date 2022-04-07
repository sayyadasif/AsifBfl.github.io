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
    public class CardTypeService : ICardTypeService
    {
        private readonly ICardTypeRepository _cardTypeRepository;
        private readonly IUserProviderService _userProviderService;

        public CardTypeService(
            ICardTypeRepository cardTypeRepository,
            IUserProviderService userProviderService
        )
        {
            _cardTypeRepository = cardTypeRepository;
            _userProviderService = userProviderService;
        }

        public async Task<ResponseModel> CreateCardType(CardType entity)
        {
            var existEntity = await GetCardTypeByName(entity.CardTypeName);
            if (existEntity.Success)
            {
                return new ResponseModel
                {
                    Success = false,
                    StatusCode = StatusCodes.Status409Conflict,
                    Message = "Card Type already exists.",
                };
            }

            entity.CreatedBy = _userProviderService.UserClaim.UserId;
            entity.UpdatedBy = _userProviderService.UserClaim.UserId;
            await _cardTypeRepository.AddAsync(entity);
            var result = await _cardTypeRepository.SaveChangesAsync();
            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Card Type created successfully.",
                    Data = entity
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "Card Type does not created." };
        }

        public async Task<ResponseModel> DeleteCardType(long id)
        {
            var entityResult = await GetCardTypeById(id);

            if (!entityResult.Success) { return entityResult; }

            var entity = entityResult.Data as CardType;
            entity.UpdatedBy = _userProviderService.UserClaim.UserId;
            entity.IsDeleted = true;
            var result = await _cardTypeRepository.SaveChangesAsync();

            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Card Type deleted successfully.",
                    Data = entity
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "Card Type does not deleted." };
        }

        public async Task<ResponseModel> GetAllCardType(bool? isActive = null)
        {
            return new ResponseModel
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Data = await _cardTypeRepository.Find(a => a.IsDeleted == false && (!isActive.HasValue || a.IsActive == isActive))
            };
        }

        public async Task<ResponseModel> GetCardTypePaged(Pagination pagination)
        {
            return new ResponseModel
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Data = await _cardTypeRepository.GetCardTypePaged(pagination)
            };
        }

        public async Task<ResponseModel> GetCardTypeById(long id)
        {
            var result = await _cardTypeRepository.SingleOrDefaultAsync(a => a.CardTypeId == id);
            if (result != null)
            {
                return new ResponseModel { Success = true, StatusCode = StatusCodes.Status200OK, Data = result };
            }
            else
            {
                return new ResponseModel { Success = false, StatusCode = StatusCodes.Status404NotFound, Message = "Card Type does not exists." };
            }
        }

        public async Task<ResponseModel> GetCardTypeByName(string cardTypeName)
        {
            var result = await _cardTypeRepository.SingleOrDefaultAsync(a => a.CardTypeName == cardTypeName);
            if (result != null)
            {
                return new ResponseModel { Success = true, StatusCode = StatusCodes.Status200OK, Data = result };
            }
            else
            {
                return new ResponseModel { Success = false, StatusCode = StatusCodes.Status404NotFound, Message = "Card Type does not exists." };
            }
        }


        public async Task<ResponseModel> UpdateCardType(CardType updateEntity)
        {
            var entityResult = await GetCardTypeById(updateEntity.CardTypeId);

            if (!entityResult.Success) { return entityResult; }

            var entity = entityResult.Data as CardType;
            entity.CardTypeName = updateEntity.CardTypeName;
            entity.SortOrder = updateEntity.SortOrder;
            entity.IsActive = updateEntity.IsActive;
            entity.UpdatedDate = CommonUtils.GetDefaultDateTime();
            entity.UpdatedBy = _userProviderService.UserClaim.UserId;

            var result = await _cardTypeRepository.SaveChangesAsync();

            if (result > 0)
            {
                return new ResponseModel
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Card Type updated successfully.",
                    Data = entity
                };
            }
            return new ResponseModel { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Message = "Card Type does not updated." };
        }

        public async Task<List<DropdownModel>> GetDropdwon(long? id = null, long? c5CodeId = null)
        {
            return (await _cardTypeRepository.GetDropdwon(id, c5CodeId)).Data;
        }
    }
}
