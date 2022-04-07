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
    public interface IKitAuditService
    {
        Task<ResponseModel> GetKitAuditById(long id);
        Task<ResponseModel> GetAllKitAudit(bool? isActive = null);
        Task<ResponseModel> GetKitAuditPaged(Pagination pagination);
        Task<ResponseModel> CreateKitAudit(KitAudit entity);
        Task<ResponseModel> UpdateKitAudit(KitAudit updateEntity);
        Task<ResponseModel> DeleteKitAudit(long id);
        Task<List<DropdownModel>> GetDropdwon(long? id = null, bool? isActive = null);
        Task<KitAudit> GetKitLastDestruction(long kitId);
    }
}
