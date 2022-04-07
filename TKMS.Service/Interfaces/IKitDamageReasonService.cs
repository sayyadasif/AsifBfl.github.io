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
    public interface IKitDamageReasonService
    {
        Task<ResponseModel> GetKitDamageReasonById(long id);
        Task<ResponseModel> GetAllKitDamageReason(bool? isActive = null);
        Task<ResponseModel> GetKitDamageReasonPaged(Pagination pagination);
        Task<ResponseModel> CreateKitDamageReason(KitDamageReason entity);
        Task<ResponseModel> UpdateKitDamageReason(KitDamageReason updateEntity);
        Task<ResponseModel> DeleteKitDamageReason(long id);
        Task<List<DropdownModel>> GetDropdwon(long? id = null, bool? isActive = null);
    }
}
