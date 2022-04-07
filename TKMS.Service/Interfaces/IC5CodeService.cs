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
    public interface IC5CodeService
    {
        Task<ResponseModel> GetC5CodeById(long id);
        Task<ResponseModel> GetC5CodeByName(string c5CodeName);
        Task<ResponseModel> GetC5CodeByCardType(long cardTypeId);
        Task<ResponseModel> GetAllC5Code(bool? isActive = null);
        Task<ResponseModel> GetC5CodePaged(Pagination pagination);
        Task<ResponseModel> CreateC5Code(C5Code entity);
        Task<ResponseModel> UpdateC5Code(C5Code updateEntity);
        Task<ResponseModel> DeleteC5Code(long id);
        Task<List<C5CodeDropdownModel>> GetDropdwon(long? id = null, long? schemeCodeId = null);
    }
}
