using Core.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TKMS.Abstraction.ComplexModels;
using TKMS.Abstraction.Models;

namespace TKMS.Service.Interfaces
{
    public interface ICardTypeService
    {
        Task<ResponseModel> GetCardTypeById(long id);
        Task<ResponseModel> GetCardTypeByName(string cardTypeName);
        Task<ResponseModel> GetAllCardType(bool? isActive = null);
        Task<ResponseModel> GetCardTypePaged(Pagination pagination);
        Task<ResponseModel> CreateCardType(CardType entity);
        Task<ResponseModel> UpdateCardType(CardType updateEntity);
        Task<ResponseModel> DeleteCardType(long id);
        Task<List<DropdownModel>> GetDropdwon(long? id = null, long? c5CodeId = null);
    }
}
